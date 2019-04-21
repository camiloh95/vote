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
                this.myCandidates = await this.LoadCandidates();
                this.RefreshCandidatesList();
                await App.Navigator.PushAsync(new VoteResultsPage());
            }
            else
            {
                this.Candidate = await this.GetAlreadyVotedAsync();
                if (this.Candidate != null)
                {
                    await App.Navigator.PushAsync(new VotedCandidatePage());
                }
                else
                {
                    this.myCandidates = await this.LoadCandidates();
                    this.RefreshCandidatesList();
                    await App.Navigator.PushAsync(new CandidatesPage());
                }
            }
        }

        private async Task<Candidate> GetAlreadyVotedAsync()
        {
            this.IsRefreshing = true;

            this.alreadyVotedRequest = new AlreadyVotedRequest
            {
                Email = Settings.UserEmail,
                VoteEventId = this.VoteEvent.Id
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetAlreadyVotedAsync(
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
                return null;
            }

            this.IsRunning = false;
            this.IsEnabled = true;

            return (Candidate)response.Result;
        }

        private async Task<List<Candidate>> LoadCandidates()
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
                return null;
            }

            this.IsRefreshing = false;
            this.IsRunning = false;
            this.IsEnabled = true;

            return (List<Candidate>)response.Result;
        }

        private void RefreshCandidatesList()
        {
            this.Candidates = new ObservableCollection<CandidateItemViewModel>(
                this.myCandidates.Select(c => new CandidateItemViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageFullPath = c.ImageFullPath,
                    Proposal = c.Proposal,
                    VoteResult = c.VoteResult
                })
            .OrderBy(c => c.Name)
            .ToList());
        }
    }
}