namespace Vote.Common.ViewModels
{
    using System.Windows.Input;
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Services;
    using Vote.Common.Helpers;

    public class ChangePasswordViewModel : MvxViewModel
    {
        private readonly IApiService apiService;
        private readonly IMvxNavigationService navigationService;
        private readonly IDialogService dialogService;
        private MvxCommand changePasswordCommand;
        private bool isLoading;

        public ChangePasswordViewModel(
            IMvxNavigationService navigationService,
            IApiService apiService,
            IDialogService dialogService)
        {
            this.apiService = apiService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
        }

        public ICommand ChangePasswordCommand
        {
            get
            {
                this.changePasswordCommand = this.changePasswordCommand ?? new MvxCommand(this.ChangePasswordPassword);
                return this.changePasswordCommand;
            }
        }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string PasswordConfirm { get; set; }

        private async void ChangePasswordPassword()
        {
            if (string.IsNullOrEmpty(this.CurrentPassword))
            {
                this.dialogService.Alert("Error", "You must enter your old password.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.NewPassword))
            {
                this.dialogService.Alert("Error", "You must enter the new password.", "Accept");
                return;
            }

            if (!this.NewPassword.Equals(this.PasswordConfirm))
            {
                this.dialogService.Alert("Error", "The passwords are not the same.", "Accept");
                return;
            }
            this.IsLoading = true;

            var request = new ChangePasswordRequest
            {
                Email = Settings.UserEmail,
                NewPassword = this.NewPassword,
                OldPassword = this.CurrentPassword
            };

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var response = await this.apiService.ChangePasswordAsync(
                "https://camilovoting.azurewebsites.net",
                "/api",
                "/Account/ChangePassword",
                request,
                "bearer",
                token.Token);

            this.IsLoading = false;

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", response.Message, "Accept");
                return;
            }
            else
            {
                this.dialogService.Alert("Done",
                                     "Your password was change succesfuly.",
                                     "Accept",
                                     () => { this.navigationService.Close(this); });
            }
        }
    }
}
