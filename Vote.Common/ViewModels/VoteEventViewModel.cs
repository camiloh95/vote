namespace Vote.Common.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Helpers;
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Services;

    public class VoteEventViewModel : MvxViewModel
    {
        private List<VoteEvent> voteEvents;
        private readonly IApiService apiService;
        private readonly IDialogService dialogService;
        private readonly IMvxNavigationService navigationService;
        private MvxCommand<VoteEvent> itemClickCommand;

        public VoteEventViewModel(
            IApiService apiService,
            IDialogService dialogService,
            IMvxNavigationService navigationService)
        {
            this.apiService = apiService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
        }

        public ICommand ItemClickCommand
        {
            get
            {
                this.itemClickCommand = new MvxCommand<VoteEvent>(this.OnItemClickCommand);
                return itemClickCommand;
            }
        }

        public List<VoteEvent> VoteEvents
        {
            get => this.voteEvents;
            set => this.SetProperty(ref this.voteEvents, value);
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            this.LoadVoteEvents();
        }

        private async void OnItemClickCommand(VoteEvent voteEvent)
        {
            await this.navigationService.Navigate<CandidatesViewModel, NavigationArgs>(
                new NavigationArgs { VoteEvent = voteEvent});
        }

        private async void LoadVoteEvents()
        {
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var response = await this.apiService.GetListAsync<VoteEvent>(
                "https://camilovoting.azurewebsites.net",
                "/api",
                "/VoteEvents",
                "bearer",
                token.Token);

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", response.Message, "Accept");
                return;
            }

            this.VoteEvents = (List<VoteEvent>)response.Result;
            this.VoteEvents = this.VoteEvents.OrderBy(p => p.Name).ToList();
        }
    }
}