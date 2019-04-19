namespace Vote.UIForms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using System;
    using Xamarin.Forms;

    public class VoteViewModel : BaseViewModel
    {
        private readonly ApiService apiService;

        public SaveVoteRequest SaveVoteRequest { get; set; }

        public VoteViewModel(Candidate candidate)
        {
            this.apiService = new ApiService();
            this.SaveVoteRequest = new SaveVoteRequest
            {
                CandidateId = candidate.Id,
                UserId = MainViewModel.GetInstance().User.Id
            };
            this.SaveVote();
        }

        private async void SaveVote()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PostAsync(
                url,
                "/api",
                "/VoteEvents/SaveVote",
                this.SaveVoteRequest,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }
        }
    }
}
