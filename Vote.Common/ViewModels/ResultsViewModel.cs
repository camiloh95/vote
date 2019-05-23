namespace Vote.Common.ViewModels
{
    using System.Collections.Generic;
    using Models;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;

    public class ResultsViewModel : MvxViewModel<NavigationArgs>
    {
        private List<Candidate> candidates;
        private bool isLoading;

        public ResultsViewModel(
            IMvxNavigationService navigationService)
        {
            this.IsLoading = false;
        }
        public SaveVoteRequest SaveVoteRequest { get; set; }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public List<Candidate> Candidates
        {
            get => this.candidates;
            set => this.SetProperty(ref this.candidates, value);
        }

        public override void Prepare(NavigationArgs parameter)
        {
            this.candidates = parameter.VoteEvent.Candidates;
        }
    }
}