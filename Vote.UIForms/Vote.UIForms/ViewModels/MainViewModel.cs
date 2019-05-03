namespace Vote.UIForms.ViewModels
{
    using Common.Models;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Vote.UIForms.Helpers;

    public class MainViewModel
    {
        private static MainViewModel instance;

        public User User { get; set; }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        public TokenResponse Token { get; set; }

        public string UserEmail { get; set; }

        public string UserPassword { get; set; }

        public LoginViewModel Login { get; set; }

        public RegisterViewModel Register { get; set; }

        public RememberPasswordViewModel RememberPassword { get; set; }

        public ProfileViewModel Profile { get; set; }

        public VoteEventsViewModel VoteEvents { get; set; }

        public VoteViewModel Vote { get; set; }

        public CandidatesViewModel Candidates { get; set; }

        public VotedCandidateViewModel VotedCandidate { get; set; }
        
        public ChangePasswordViewModel ChangePassword { get; set; }

        public MainViewModel()
        {
            instance = this;
            this.LoadMenus();
        }

        private void LoadMenus()
        {
            var menus = new List<Menu>
        {
            new Menu
            {
                Icon = "ic_info",
                PageName = "AboutPage",
                Title = Languages.About
            },

            new Menu
            {
                Icon = "ic_person",
                PageName = "ProfilePage",
                Title = Languages.ModifyUser
            },

            new Menu
            {
                Icon = "ic_phonelink_setup",
                PageName = "SetupPage",
                Title = Languages.Setup
            },

            new Menu
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = Languages.CloseSession
            }
        };

            this.Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
    }
}