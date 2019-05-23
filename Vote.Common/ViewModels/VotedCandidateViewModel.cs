namespace Vote.Common.ViewModels
{
    using Interfaces;
    using Models;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Services;

    public class VotedCandidateViewModel : MvxViewModel<NavigationArgs>
    {
        private bool isLoading;

        public SaveVoteRequest SaveVoteRequest { get; set; }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public Candidate Candidate { get; set; }

        public override void Prepare(NavigationArgs parameter)
        {
            this.Candidate = parameter.Candidate;
        }
    }
}