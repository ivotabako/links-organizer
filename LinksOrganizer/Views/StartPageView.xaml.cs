using LinksOrganizer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinksOrganizer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPageView : ContentPage
    {
        public StartPageView()
        {
            InitializeComponent();
            var startPageViewModel = ViewModelLocator.Resolve<StartPageViewModel>();
            BindingContext = startPageViewModel;

            NavigationPage.SetHasBackButton(this, false);
        }
    }
}