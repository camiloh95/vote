namespace Vote.Common.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Services;
    using Vote.Common.Helpers;

    public class ModifyUserViewModel : MvxViewModel
    {
        private readonly IApiService apiService;
        private readonly IMvxNavigationService navigationService;
        private readonly IDialogService dialogService;
        private List<Country> countries;
        private List<City> cities;
        private List<Gender> genders;
        private List<Stratum> stratums;
        private User user;
        private Country selectedCountry;
        private City selectedCity;
        private Gender selectedGender;
        private Stratum selectedStratum;
        private MvxCommand updateCommand;
        private MvxCommand changePasswordCommand;
        private bool isLoading;

        public ModifyUserViewModel(
            IMvxNavigationService navigationService,
            IApiService apiService,
            IDialogService dialogService)
        {
            this.apiService = apiService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.LoadCountries();
        }

        public ICommand UpdateCommand
        {
            get
            {
                this.updateCommand = this.updateCommand ?? new MvxCommand(this.UpdateUser);
                return this.updateCommand;
            }
        }

        public ICommand ChangePasswordCommand
        {
            get
            {
                this.changePasswordCommand = this.changePasswordCommand ?? new MvxCommand(this.ChangePassword);
                return this.changePasswordCommand;
            }
        }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public User User
        {
            get => this.user;
            set => this.SetProperty(ref this.user, value);
        }

        public List<Country> Countries
        {
            get => this.countries;
            set => this.SetProperty(ref this.countries, value);
        }

        public List<City> Cities
        {
            get => this.cities;
            set => this.SetProperty(ref this.cities, value);
        }

        public List<Gender> Genders
        {
            get => this.genders;
            set => this.SetProperty(ref this.genders, value);
        }

        public List<Stratum> Stratums
        {
            get => this.stratums;
            set => this.SetProperty(ref this.stratums, value);
        }

        public Country SelectedCountry
        {
            get => selectedCountry;
            set
            {
                this.selectedCountry = value;
                this.RaisePropertyChanged(() => SelectedCountry);
                this.Cities = SelectedCountry.Cities;
            }
        }

        public City SelectedCity
        {
            get => selectedCity;
            set
            {
                selectedCity = value;
                RaisePropertyChanged(() => SelectedCity);
            }
        }

        public Gender SelectedGender
        {
            get => this.selectedGender;
            set => this.SetProperty(ref this.selectedGender, value);
        }

        public Stratum SelectedStratum
        {
            get => this.selectedStratum;
            set => this.SetProperty(ref this.selectedStratum, value);
        }

        private async void ChangePassword()
        {
            await this.navigationService.Navigate<ChangePasswordViewModel>();
        }

        private async void LoadCountries()
        {
            var response = await this.apiService.GetListAsync<Country>(
                "https://camilovoting.azurewebsites.net",
                "/api",
                "/Countries");

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", response.Message, "Accept");
                return;
            }

            this.Countries = (List<Country>)response.Result;
        }

        private async Task<User> UserRequest()
        {
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var response = await this.apiService.GetUserByEmailAsync(
                "https://camilovoting.azurewebsites.net",
                "/api",
                "/Account/GetUserByEmail",
                Settings.UserEmail,
                "bearer",
                token.Token);

            return (User)response.Result;
        }

        private async void UpdateUser()
        {
            if (string.IsNullOrEmpty(this.User.FirstName))
            {
                this.dialogService.Alert("Error", "You must enter a firstname.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.User.LastName))
            {
                this.dialogService.Alert("Error", "You must enter a lastname.", "Accept");
                return;
            }

            if (this.SelectedCountry == null)
            {
                this.dialogService.Alert("Error", "You must select a country.", "Accept");
                return;
            }

            if (this.SelectedCity == null)
            {
                this.dialogService.Alert("Error", "You must select a city.", "Accept");
                return;
            }

            if (this.SelectedGender == null)
            {
                this.dialogService.Alert("Error", "You must select a gender.", "Accept");
                return;
            }

            if (this.SelectedStratum == null)
            {
                this.dialogService.Alert("Error", "You must select a stratum.", "Accept");
                return;
            }

            this.User.Gender = this.SelectedCountry.Id;
            this.User.Stratum = this.SelectedStratum.Id;
            this.User.CityId = this.SelectedCity.Id;

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var response = await this.apiService.PutAsync(
                "https://camilovoting.azurewebsites.net",
                "/api",
                "/Account",
                this.User,
                "bearer",
                token.Token);

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", response.Message, "Accept");
                return;
            }
            else
            {
                this.dialogService.Alert("¡ Congratulations !",
                                     "The user was updated succesfully.",
                                     "Accept",
                                     () => { this.navigationService.Close(this); });
            }
        }

        public override async void ViewAppeared()
        {
            base.ViewAppeared();
            this.User = await this.UserRequest();
            this.Genders = this.GetGenders();
            this.Stratums = this.GetStratums();
            this.SetCountryAndCity();
            this.SetStratum();
            this.SetGender();
        }

        private void SetGender()
        {
            foreach (var gender in this.Genders)
            {
                if (gender.Id == this.User.Gender)
                {
                    this.SelectedGender = gender;
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
                        this.SelectedStratum = stratum;
                        return;
                }
            }
        }

        private void SetCountryAndCity()
        {
            foreach (var country in this.Countries)
            {
                var city = country.Cities.Find(c => c.Id == this.User.CityId);
                if (city != null)
                {
                    this.SelectedCountry = country;
                    this.SelectedCity = city;
                    return;
                }
            }
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
