namespace Vote.UIForms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Vote.Common.Helpers;
    using Vote.UIForms.Views;
    using Xamarin.Forms;

    public class CandidatesViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;
        private List<Candidate> myCandidates;
        private ObservableCollection<CandidateItemViewModel> candidates;
        private bool isRefreshing;
        private bool alreadyVoted;
        private Candidate candidate;

        public ObservableCollection<CandidateItemViewModel> Candidates
        {
            get => this.candidates;
            set => this.SetValue(ref this.candidates, value);
        }

        public Candidate Candidate
        {
            get => this.candidate;
            set => this.SetValue(ref this.candidate, value);
        }

        public bool AlreadyVoted
        {
            get => this.alreadyVoted;
            set => this.SetValue(ref this.alreadyVoted, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
        }

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }

        public VoteEvent VoteEvent { get; set; }

        public AlreadyVotedRequest alreadyVotedRequest { get; set; }
        
        public CandidatesViewModel(VoteEvent voteEvent)
        {
            this.VoteEvent = voteEvent;
            this.apiService = new ApiService();
            this.ValidationPath();
        }

        private async void ValidationPath()
        {
            if (this.VoteEvent.EndDate <= DateTime.Today)
            {
                this.LoadResults();
            }
            else
            {
                var validation = await this.AlreadyVotedAsync();
                if (validation)
                {
                    this.LoadVotedCandidate();
                }
                else
                {
                    this.LoadCandidates();
                }
            }
        }

        private async void LoadResults()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetVotedCandidateAsync(
                url,
                "/api",
                "/VoteEvents/GetVotedCandidate",
                this.alreadyVotedRequest,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            this.IsRunning = false;

            this.Candidate = (Candidate)response.Result;
            await App.Navigator.PushAsync(new VotedCandidatePage());
        }

        private async void LoadVotedCandidate()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetVotedCandidateAsync(
                url,
                "/api",
                "/VoteEvents/GetVotedCandidate",
                this.alreadyVotedRequest,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            this.IsRunning = false;

            this.Candidate = (Candidate)response.Result;
            await App.Navigator.PushAsync(new VotedCandidatePage());
        }

        private async Task<bool> AlreadyVotedAsync()
        {
            this.IsRefreshing = true;

            this.AlreadyVoted = false;

            this.alreadyVotedRequest = new AlreadyVotedRequest
            {
                Email = Settings.UserEmail,
                VoteEventId = this.VoteEvent.Id
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.AlreadyVotedAsync(
                url,
                "/api",
                "/VoteEvents/GetAlreadyVoted",
                this.alreadyVotedRequest,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return false;
            }

            this.IsRunning = false;
            this.IsEnabled = true;

            return (bool)response.Result;
        }

        private async void LoadCandidates()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetCandidatesAsync(
                url,
                "/api",
                "/VoteEvents/GetCandidatesById",
                this.VoteEvent,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            this.IsRefreshing = false;
            this.IsRunning = false;
            this.IsEnabled = true;

            this.myCandidates = (List<Candidate>)response.Result;
            this.RefreshCandidatesList();
            await App.Navigator.PushAsync(new CandidatesPage());
        }

        private void RefreshCandidatesList()
        {
            this.Candidates = new ObservableCollection<CandidateItemViewModel>(
                this.myCandidates.Select(c => new CandidateItemViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageFullPath = c.ImageFullPath,
                    Proposal = c.Proposal
                })
            .OrderBy(c => c.Name)
            .ToList());
        }
    }
}