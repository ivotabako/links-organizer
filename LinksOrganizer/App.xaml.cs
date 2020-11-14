using LinksOrganizer.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Kri.Solutions
{
    public partial class App : Application
    {
        static LinkItemDatabase database;

        public App()
        {
            InitializeComponent();

            

            var nav = new NavigationPage(new StartPage() );
            
            MainPage = nav;
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

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}

