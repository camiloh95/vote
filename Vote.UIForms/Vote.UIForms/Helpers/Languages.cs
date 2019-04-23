namespace Vote.UIForms.Helpers
{
    using Interfaces;
    using Resources;
    using Xamarin.Forms;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept => Resource.Accept;

        public static string Error => Resource.Error;

        public static string EmailError => Resource.EmailError;

        public static string PasswordError => Resource.PasswordError;

        public static string LoginError => Resource.LoginError;

        public static string Login => Resource.Login;

        public static string Email => Resource.Email;

        public static string EmailPlaceHolder => Resource.EmailPlaceHolder;

        public static string Password => Resource.Password;

        public static string PasswordPlaceHolder => Resource.PasswordPlaceHolder;

        public static string Remember => Resource.Remember;

        public static string RegisterNewUser => Resource.RegisterNewUser;

        public static string Forgot => Resource.Forgot;

        public static string Occupation => Resource.Occupation;

        public static string VoteQuestion => Resource.VoteQuestion;

        public static string VoteCandidate => Resource.VoteCandidate;

        public static string Yes => Resource.Yes;

        public static string No => Resource.No;

        public static string Congratulations => Resource.Congratulations;

        public static string CongratulationsMessage => Resource.CongratulationsMessage;

        public static string CurrentPassword => Resource.CurrentPassword;

        public static string PasswordIncorrect => Resource.PasswordIncorrect;

        public static string MustNewPassword => Resource.MustNewPassword;

        public static string PasswordAtLeastSix => Resource.PasswordAtLeastSix;

        public static string PasswordNotMatch => Resource.PasswordNotMatch;

        public static string PasswordConfirm => Resource.PasswordConfirm;

        public static string UserUpdated => Resource.UserUpdated;

        public static string SelectStratum => Resource.SelectStratum;

        public static string SelectGender => Resource.SelectGender;

        public static string SelectCity => Resource.SelectCity;
                
        public static string SelectCountry => Resource.SelectCountry;

        public static string EnterFirstName => Resource.EnterFirstName;

        public static string EnterLastName => Resource.EnterLastName;

        public static string EmailInvalid => Resource.EmailInvalid;

        public static string EnterPhone => Resource.EnterPhone;

        public static string Candidates => Resource.Candidates;

        public static string CurrentPasswordName => Resource.CurrentPasswordName;

        public static string NewPassword => Resource.NewPassword;

        public static string EnterNewPassword => Resource.EnterNewPassword;

        public static string ConfirmNewPassword => Resource.ConfirmNewPassword;

        public static string EnterConfirmNewPassword => Resource.EnterConfirmNewPassword;

        public static string ModifyUser => Resource.ModifyUser;

        public static string FirstName => Resource.FirstName;

        public static string PlaceholderFirstName => Resource.PlaceholderFirstName;

        public static string LastName => Resource.LastName;

        public static string PlaceholderLastName => Resource.PlaceholderLastName;

        public static string PlaceholderOccupation => Resource.PlaceholderOccupation;

        public static string Gender => Resource.Gender;

        public static string PlaceholderGender => Resource.PlaceholderGender;

        public static string Stratum => Resource.Stratum;

        public static string PlaceholderStratum => Resource.PlaceholderStratum;

        public static string Country => Resource.Country;

        public static string PlaceholderCountry => Resource.PlaceholderCountry;

        public static string City => Resource.City;

        public static string PlaceholderCity => Resource.PlaceholderCity;

        public static string Save => Resource.Save;

        public static string ModifyPassword => Resource.ModifyPassword;

        public static string Telephone => Resource.Telephone;

        public static string EnterTelephone => Resource.EnterTelephone;

        public static string RegisterNewUserName => Resource.RegisterNewUserName;

        public static string RecoverPassword => Resource.RecoverPassword;

        public static string VotedCandidate => Resource.VotedCandidate;

        public static string Menu => Resource.Menu;

        public static string Profile => Resource.Profile;

        public static string VotingEvents => Resource.VotingEvents;

        public static string VoteResults => Resource.VoteResults;

        public static string TotalVotes => Resource.TotalVotes;

        public static string About => Resource.About;

        public static string Setup => Resource.Setup;

        public static string CloseSession => Resource.CloseSession;
    }
}
