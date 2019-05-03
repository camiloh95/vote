namespace Vote.UIForms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;

    public class VotedCandidateViewModel : BaseViewModel
    {
        private readonly ApiService apiService;

        public Candidate Candidate { get; set; }
        
        public VotedCandidateViewModel(Candidate candidate)
        {
            this.Candidate = candidate;
            this.apiService = new ApiService();
        }
    }
}
