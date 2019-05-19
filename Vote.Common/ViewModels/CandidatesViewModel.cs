namespace Vote.Common.ViewModels
{
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
        private Candidate candidate;
        private bool isLoading;
        private MvxCommand updateCommand;

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

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public Candidate Candidate
        {
            get => this.candidate;
            set => this.SetProperty(ref this.candidate, value);
        }

        public ICommand UpdateCommand
        {
            get
            {
                this.updateCommand = this.updateCommand ?? new MvxCommand(this.Update);
                return this.updateCommand;
            }
        }

        private async void Update()
        {
            if (string.IsNullOrEmpty(this.Candidate.Name))
            {
                this.dialogService.Alert("Error", "You must enter a candidate name.", "Accept");
                return;
            }

            this.IsLoading = true;

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await this.apiService.PutAsync(
                "https://camilovoting.azurewebsites.net",
                "/api",
                "/Candidates",
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
            this.candidate = parameter.Candidate;
        }
    }
}
