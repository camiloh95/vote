namespace Vote.UIForms.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Common.Models;
    using UIForms.Views;

    public class VoteEventItemViewModel : VoteEvent
    {
        public ICommand SelectProductCommand => new RelayCommand(this.SelectProduct);

        private async void SelectProduct()
        {
            MainViewModel.GetInstance().ViewVoteEvent = new ViewVoteEventViewModel((VoteEvent)this);
            await App.Navigator.PushAsync(new ViewVoteEventPage());
        }
    }
}
