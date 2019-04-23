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
    }
}
