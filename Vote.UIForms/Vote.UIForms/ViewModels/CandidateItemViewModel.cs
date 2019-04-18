namespace Vote.UIForms.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Common.Models;
    using UIForms.Views;

    public class CandidateItemViewModel : Candidate
    {
        public ICommand SelectCandidateCommand => new RelayCommand(this.SelectCandidate);

        private async void SelectCandidate()
        {

            await App.Navigator.PopAsync();
        }
    }
}
