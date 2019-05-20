namespace Vote.Common.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Helpers;
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Services;
    
    public class CandidatesViewModel : MvxViewModel<NavigationArgs>
    {
        private readonly IApiService apiService;
        private readonly IDialogService dialogService;
        private readonly IMvxNavigationService navigationService;
        private List<Candidate> candidates;
        private MvxCommand<Candidate> itemClickCommand;
        private bool isLoading;
        private User user;

        public CandidatesViewModel(
            IApiService apiService,
            IDialogService dialogService,
            IMvxNavigationService navigationService)
        {
            this.apiService = apiService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.IsLoading = false;
        }
        public SaveVoteRequest SaveVoteRequest { get; set; }

        public ICommand ItemClickCommand
        {
            get
            {
                this.itemClickCommand = new MvxCommand<Candidate>(this.OnItemClickCommand);
                return itemClickCommand;
            }
        }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public User User
        {
            get => this.user;
            set => this.SetProperty(ref this.user, value);
        }

        public List<Candidate> Candidates
        {
            get => this.candidates;
            set => this.SetProperty(ref this.candidates, value);
        }

        private void OnItemClickCommand(Candidate candidate)
        {
            this.SaveVoteRequest = new SaveVoteRequest
            {
                CandidateId = candidate.Id,
                UserId = this.User.Id
            };
            this.Vote(candidate);
        }

        private async void getUserId()
        {
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var response = await this.apiService.GetUserByEmailAsync(
                "https://camilovoting.azurewebsites.net",
                "/api",
                "/Account/GetUserByEmail",
                Settings.UserEmail,
                "bearer",
                token.Token);

            if (!response.IsSuccess)
            {
                this.IsLoading = false;
                this.dialogService.Alert("Error", "Error en la consulta", "Accept");
                return;
            }

            this.User = (User)response.Result;
        }

        private async void Vote(Candidate candidate)
        {
            this.IsLoading = true;

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var response = await this.apiService.PutAsync(
                "https://camilovoting.azurewebsites.net",
                "/api",
                "/VoteEvents/SaveVote",
                candidate.Id,
                candidate,
                "bearer",
                token.Token);

            this.IsLoading = false;

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", response.Message, "Accept");
                return;
            }

            await this.navigationService.Close(this);
        }
        
        public override void Prepare(NavigationArgs parameter)
        {
            this.getUserId();
            this.candidates = parameter.VoteEvent.Candidates;
        }
    }
}
