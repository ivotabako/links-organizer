using LinksOrganizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinksOrganizer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OptionsView : ContentPage
    {
        public OptionsView()
        {
            InitializeComponent();
            var optionsViewModel = ViewModelLocator.Resolve<OptionsViewModel>();
            BindingContext = optionsViewModel;

            NavigationPage.SetHasBackButton(this, false);
        }
    }
}