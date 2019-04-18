namespace Vote.UIForms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;

    public class CandidatesViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;
        private List<Candidate> myCandidates;
        private ObservableCollection<CandidateItemViewModel> candidates;
        private bool isRefreshing;

        public ObservableCollection<CandidateItemViewModel> Candidates
        {
            get => this.candidates;
            set => this.SetValue(ref this.candidates, value);
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
        
        public CandidatesViewModel(VoteEvent voteEvent)
        {
            this.VoteEvent = voteEvent;
            this.apiService = new ApiService();
            this.LoadCandidates();
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

            this.IsRunning = false;
            this.IsEnabled = true;

            this.myCandidates = (List<Candidate>)response.Result;
            this.RefreshCandidatesList();
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