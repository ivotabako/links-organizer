using LinksOrganizer.ViewModels;
using Xamarin.Forms;

namespace LinksOrganizer.Views
{
    public partial class LinkItemView : ContentPage
    {
        public LinkItemView()
        {
            InitializeComponent();
            var linkItemViewModel = ViewModelLocator.Resolve<LinkItemViewModel>();
            BindingContext = linkItemViewModel;
        }
    }
}
