using LinksOrganizer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinksOrganizer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinkItemView : ContentPage
    {
        public LinkItemView()
        {
            InitializeComponent();
            var linkItemViewModel = ViewModelLocator.Resolve<LinkItemViewModel>();
            BindingContext = linkItemViewModel;

            NavigationPage.SetHasBackButton(this, false);
        }
    }
}
