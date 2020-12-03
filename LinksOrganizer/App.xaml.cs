using LinksOrganizer.Data;
using LinksOrganizer.Services.Navigation;
using LinksOrganizer.ViewModels;
using LinksOrganizer.Views;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LinksOrganizer
{
    public partial class App : Application
    {
        static LinkItemDatabase database;

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

        public static LinkItemDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new LinkItemDatabase();
                }
                return database;
            }
        }
    }
}

