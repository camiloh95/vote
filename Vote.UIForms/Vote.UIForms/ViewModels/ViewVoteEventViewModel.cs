namespace Vote.UIForms.ViewModels
{
    using System.Linq;
    using System.Windows.Input;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class ViewVoteEventViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;

        public VoteEvent VoteEvent { get; set; }

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

        public ICommand SaveCommand => new RelayCommand(this.SaveProduct);

        public ICommand DeleteCommand => new RelayCommand(this.DeleteProduct);

        public ViewVoteEventViewModel(VoteEvent voteEvent)
        {
            this.VoteEvent = voteEvent;
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }

        private async void SaveProduct()
        {
            if (string.IsNullOrEmpty(this.VoteEvent.Name))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a product name.",
                    "Accept");
                return;
            }

            /*if (this.Product.Price <= 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "The price must be a number greather than zero.",
                    "Accept");
                return;
            }*/

            this.IsRunning = true;
            this.IsEnabled = false;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PutAsync(
                url,
                "/api",
                "/VoteEvents",
                this.VoteEvent.Id,
                this.VoteEvent,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var modifiedProduct = (VoteEvent)response.Result;
            MainViewModel.GetInstance().VoteEvents.UpdateProductInList(modifiedProduct);
            await App.Navigator.PopAsync();
        }

        private async void DeleteProduct()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirm",
                "Are you sure to delete the product?",
                "Yes",
                "No");
            if (!confirm)
            {
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.DeleteAsync(
                url,
                "/api",
                "/VoteEvents",
                this.VoteEvent.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            MainViewModel.GetInstance().VoteEvents.DeleteProductInList(this.VoteEvent.Id);
            await App.Navigator.PopAsync();
        }
    }
}
