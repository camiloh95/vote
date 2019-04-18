namespace Vote.UIForms.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Newtonsoft.Json;
    using Common.Helpers;
    using UIForms.Views;
    using Xamarin.Forms;

    public class ProfileViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private bool isRunning;
        private bool isEnabled;
        private ObservableCollection<Country> countries;
        private Country country;
        private ObservableCollection<City> cities;
        private City city;
        private User user;
        private List<Country> myCountries;

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

        public User User
        {
            get => this.user;
            set => this.SetValue(ref this.user, value);
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
        public Gender Gender { get; set; }

        public Stratum Stratum { get; set; }

        public List<Gender> Genders { get; set; }

        public List<Stratum> Stratums { get; set; }

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand ModifyPasswordCommand => new RelayCommand(this.ModifyPassword);

        public ProfileViewModel()
        {
            this.apiService = new ApiService();
            this.User = MainViewModel.GetInstance().User;
            this.IsEnabled = true;
            this.Genders = this.GetGenders();
            this.Stratums = this.GetStratums();
            this.SetStratum();
            this.SetGender();
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
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            this.myCountries = (List<Country>)response.Result;
            this.Countries = new ObservableCollection<Country>(myCountries);
            this.SetCountryAndCity();
        }

        private void SetCountryAndCity()
        {
            foreach (var country in this.myCountries)
            {
                var city = country.Cities.Where(c => c.Id == this.User.CityId).FirstOrDefault();
                if (city != null)
                {
                    this.Country = country;
                    this.City = city;
                    return;
                }
            }
        }

        private void SetGender()
        {
            foreach (var gender in this.Genders)
            {
                if(gender.Id == this.User.Gender)
                {
                    this.Gender = gender;
                    return;
                }
            }
        }

        private void SetStratum()
        {
            foreach (var stratum in this.Stratums)
            {
                if (stratum.Id == this.User.Stratum)
                {
                    this.Stratum = stratum;
                    return;
                }
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.User.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter the first name.",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.User.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter the last name.",
                    "Accept");
                return;
            }

            if (this.Country == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must select a country.",
                    "Accept");
                return;
            }

            if (this.City == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must select a city.",
                    "Accept");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PutAsync(
                url,
                "/api",
                "/Account",
                this.User,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            MainViewModel.GetInstance().User = this.User;
            Settings.User = JsonConvert.SerializeObject(this.User);

            await Application.Current.MainPage.DisplayAlert(
                "Ok",
                "User updated!",
                "Accept");
            await App.Navigator.PopAsync();
        }

        private async void ModifyPassword()
        {
            MainViewModel.GetInstance().ChangePassword = new ChangePasswordViewModel();
            await App.Navigator.PushAsync(new ChangePasswordPage());
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
