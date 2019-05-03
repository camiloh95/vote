namespace Vote.UIForms.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Common.Helpers;
    using Common.Services;
    using Xamarin.Forms;
    using Vote.UIForms.Helpers;

    public class RegisterViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private ObservableCollection<Country> countries;
        private Country country;
        private ObservableCollection<City> cities;
        private City city;
        private readonly ApiService apiService;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Occupation { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string Confirm { get; set; }
        
        public Gender Gender { get; set; }

        public Stratum Stratum { get; set; }

        public List<Gender> Genders { get; set; }

        public List<Stratum> Stratums { get; set; }

        public Country Country
        {
            get => this.country;
            set
            {
                this.SetValue(ref this.country, value);
                this.Cities = new ObservableCollection<City>(this.Country.Cities.OrderBy(c => c.Name));
            }
        }

        public City City
        {
            get => this.city;
            set => this.SetValue(ref this.city, value);
        }

        public ObservableCollection<Country> Countries
        {
            get => this.countries;
            set => this.SetValue(ref this.countries, value);
        }

        public ObservableCollection<City> Cities
        {
            get => this.cities;
            set => this.SetValue(ref this.cities, value);
        }

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

        public ICommand RegisterCommand => new RelayCommand(this.Register);

        public RegisterViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.Genders = this.GetGenders();
            this.Stratums = this.GetStratums();
            this.LoadCountries();
        }

        private async void LoadCountries()
        {
            this.IsRunning = true;
            this.IsEnabled = false;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<Country>(
                url,
                "/api",
                "/Countries");

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var myCountries = (List<Country>)response.Result;
            this.Countries = new ObservableCollection<Country>(myCountries);
        }

        private async void Register()
        {
            if (string.IsNullOrEmpty(this.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EnterFirstName,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EnterLastName,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailError,
                    Languages.Accept);
                return;
            }

            if (!RegexHelper.IsValidEmail(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailInvalid,
                    Languages.Accept);
                return;
            }

            if (this.Country == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.SelectCountry,
                    Languages.Accept);
                return;
            }

            if (this.City == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.SelectCity,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Phone))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EnterPhone,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordError,
                    Languages.Accept);
                return;
            }

            if (this.Password.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordAtLeastSix,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Confirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordConfirm,
                    Languages.Accept);
                return;
            }

            if (!this.Password.Equals(this.Confirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordNotMatch,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var request = new NewUserRequest
            {
                CityId = this.City.Id,
                Email = this.Email,
                FirstName = this.FirstName,
                Occupation = this.Occupation,
                Stratum = this.Stratum.Id,
                Gender = this.Gender.Id,
                LastName = this.LastName,
                Password = this.Password,
                Phone = this.Phone
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.RegisterUserAsync(
                url,
                "/api",
                "/Account",
                request);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                "Ok",
                response.Message,
                Languages.Accept);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public List<Stratum> GetStratums()
        {
            var list = new List<Stratum>();
            list.Insert(0, new Stratum { Id = 0, Name = "1" });
            list.Insert(1, new Stratum { Id = 1, Name = "2" });
            list.Insert(2, new Stratum { Id = 2, Name = "3" });
            list.Insert(3, new Stratum { Id = 3, Name = "4" });
            list.Insert(4, new Stratum { Id = 4, Name = "5" });
            list.Insert(5, new Stratum { Id = 5, Name = "6" });

            return list;
        }

        public List<Gender> GetGenders()
        {
            var list = new List<Gender>();
            list.Insert(0, new Gender { Id = 0, Name = "Male" });
            list.Insert(1, new Gender { Id = 1, Name = "Female" });
            list.Insert(2, new Gender { Id = 2, Name = "Other" });

            return list;
        }
    }
}
