namespace Vote.Common.ViewModels
{
    using System.Windows.Input;
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Services;

    public class RememberViewModel : MvxViewModel
    {
        private readonly IApiService apiService;
        private readonly IMvxNavigationService navigationService;
        private readonly IDialogService dialogService;
        private MvxCommand rememberCommand;
        private string email;
        private bool isLoading;

        public RememberViewModel(
            IMvxNavigationService navigationService,
            IApiService apiService,
            IDialogService dialogService)
        {
            this.apiService = apiService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
        }

        public ICommand RememberCommand
        {
            get
            {
                this.rememberCommand = this.rememberCommand ?? new MvxCommand(this.RememberPassword);
                return this.rememberCommand;
            }
        }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public string Email
        {
            get => this.email;
            set => this.SetProperty(ref this.email, value);
        }

        private async void RememberPassword()
        {
            if(string.IsNullOrEmpty(this.Email))
            {
                this.dialogService.Alert("Error", "You must enter an email.", "Accept");
                return;
            }

            this.IsLoading = true;

            var request = new RecoverPasswordRequest
            {
                Email = this.Email
            };

            var response = await this.apiService.RecoverPasswordAsync(
                "https://camilovoting.azurewebsites.net",
                "/api",
                "/Account/RecoverPassword",
                request);

            this.IsLoading = false;

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", response.Message, "Accept");
                return;
            }
            else
            {
                this.dialogService.Alert("Password sent...",
                                     "The email to recover your password was send to your email, " +
                                     "please check your inbox.",
                                     "Accept",
                                     () => { this.navigationService.Close(this); });
            }
        }
    }
}
