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
        private List<Candidate> myCandidates;
        private ObservableCollection<Candidate> candidates;
        private bool isRefreshing;

        public ObservableCollection<Candidate> Candidates
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
            this.myCandidates = voteEvent.Candidates;
            this.RefreshCandidatesList();
        }

        private void RefreshCandidatesList()
        {
            this.Candidates = new ObservableCollection<Candidate>(
                this.myCandidates.Select(p => new Candidate
                {
                    Id = p.Id,
                    Name = p.Name,
                    Proposal = p.Proposal
                })
            .OrderBy(p => p.Name)
            .ToList());
        }
    }
}
