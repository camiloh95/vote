namespace Vote.UIForms.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Common.Models;
    using Common.Services;
    using System.Diagnostics;
    using Xamarin.Forms;

    public class CandidateItemViewModel : Candidate
    {
        public ICommand SelectCandidateCommand => new RelayCommand(this.SelectCandidate);

        private async void SelectCandidate()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Vote Question", "Would you like to vote for this candidate ?", "Yes", "No");
            Debug.WriteLine("Answer: " + answer);
            if (answer == true)
            {
                MainViewModel.GetInstance().Vote = new VoteViewModel((Candidate)this);
                await Application.Current.MainPage.DisplayAlert("Congratulations", "Your vote has been save successfully !", "OK");
                await App.Navigator.PopAsync();
            }
            else
            {
                return;
            }
        }
    }
}
