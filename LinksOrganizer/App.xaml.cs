using LinksOrganizer.Data;
using LinksOrganizer.Services.Navigation;
using LinksOrganizer.ViewModels;
using LinksOrganizer.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LinksOrganizer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            if (Device.RuntimePlatform != Device.UWP)
            {
                await InitNavigation();
            }
            
            base.OnResume();
        }

        private Task InitNavigation()
        {
            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            return navigationService.InitializeAsync();
        }
    }
}

