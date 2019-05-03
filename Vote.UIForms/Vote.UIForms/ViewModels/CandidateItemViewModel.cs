namespace Vote.UIForms.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Common.Models;
    using Common.Services;
    using System.Diagnostics;
    using Xamarin.Forms;
    using Vote.UIForms.Helpers;

    public class CandidateItemViewModel : Candidate
    {
        public ICommand SelectCandidateCommand => new RelayCommand(this.SelectCandidate);

        private async void SelectCandidate()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert(Languages.VoteQuestion, Languages.VoteCandidate, Languages.Yes, Languages.No);
            Debug.WriteLine("Answer: " + answer);
            if (answer == true)
            {
                MainViewModel.GetInstance().Vote = new VoteViewModel((Candidate)this);
                await Application.Current.MainPage.DisplayAlert(Languages.Congratulations, Languages.CongratulationsMessage, "Ok");
                await App.Navigator.PopAsync();
            }
            else
            {
                return;
            }
        }
    }
}
