namespace Vote.UIForms.Infrastructure
{
    using Vote.UIForms.ViewModels;

    class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}
