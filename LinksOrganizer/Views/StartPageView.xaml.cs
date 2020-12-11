using dotMorten.Xamarin.Forms;
using LinksOrganizer.Models;
using LinksOrganizer.ViewModels;
using System.Linq;
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
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var vm = this.BindingContext as StartPageViewModel;
                vm.SetSearchedLinkItemsCommand.Execute(sender.Text);
            }
        }

        //private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        //{
        //    sender.Text = (args.SelectedItem as LinkItem).Name;
        //}

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                var vm = this.BindingContext as StartPageViewModel;
                vm.LoadLinkItemCommand.Execute(args.ChosenSuggestion as LinkItem);             
            }
        }
    }
}