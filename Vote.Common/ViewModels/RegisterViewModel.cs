namespace Vote.Common.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Services;

    public class RegisterViewModel : MvxViewModel
    {
        private readonly IApiService apiService;
        private readonly IMvxNavigationService navigationService;
        private readonly IDialogService dialogService;
        private List<Country> countries;
        private List<City> cities;
        private List<Gender> genders;
        private List<Stratum> stratums;
        private Country selectedCountry;
        private City selectedCity;
        private Gender selectedGender;
        private Stratum selectedStratum;
        private MvxCommand registerCommand;
        private string firstName;
        private string lastName;
        private string email;
        private string phone;
        private string occupation;
        private string password;
        private string confirmPassword;

        public RegisterViewModel(
            IMvxNavigationService navigationService,
            IApiService apiService,
            IDialogService dialogService)
        {
            this.apiService = apiService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.LoadCountries();
        }

        public ICommand RegisterCommand
        {
            get
            {
                this.registerCommand = this.registerCommand ?? new MvxCommand(this.RegisterUser);
                return this.registerCommand;
            }
        }

        public string FirstName
        {
            get => this.firstName;
            set => this.SetProperty(ref this.firstName, value);
        }

        public string LastName
        {
            get => this.lastName;
            set => this.SetProperty(ref this.lastName, value);
        }

        public string Email
        {
            get => this.email;
            set => this.SetProperty(ref this.email, value);
        }

        public string Phone
        {
            get => this.phone;
            set => this.SetProperty(ref this.phone, value);
        }

        public string Password
        {
            get => this.password;
            set => this.SetProperty(ref this.password, value);
        }

        public string ConfirmPassword
        {
            get => this.confirmPassword;
            set => this.SetProperty(ref this.confirmPassword, value);
        }

        public string Occupation
        {
            get => this.occupation;
            set => this.SetProperty(ref this.occupation, value);
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
            get => selectedGender;
            set
            {
                selectedGender = value;
                RaisePropertyChanged(() => selectedGender);
            }
        }

        public Stratum SelectedStratum
        {
            get => selectedStratum;
            set
            {
                selectedStratum = value;
                RaisePropertyChanged(() => selectedStratum);
            }
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

        private async void RegisterUser()
        {
            var request = new NewUserRequest
            {
                CityId = this.SelectedCity.Id,
                Email = this.Email,
                Occupation = this.Occupation,
                Gender = this.SelectedGender.Id,
                Stratum = this.SelectedStratum.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Password = this.Password,
                Phone = this.Phone
            };

            var response = await this.apiService.RegisterUserAsync(
                "https://camilovoting.azurewebsites.net",
                "/api",
                "/Account",
                request);

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", "There where a problem trying to register the user", "Accept");
                return;
            }

            this.dialogService.Alert("Ok", "The user was created succesfully, you must " +
                "confirm your user by the email sent to you and then you could login with " +
                "the email and password entered.", "Accept"); 

            await this.navigationService.Close(this);
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            this.Genders = this.GetGenders();
            this.Stratums = this.GetStratums();
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
