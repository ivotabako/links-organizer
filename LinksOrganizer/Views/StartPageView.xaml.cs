using dotMorten.Xamarin.Forms;
using Kri.Solutions;
using LinksOrganizer.Models;
using LinksOrganizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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

            //NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var items = await App.Database.GetItemsAsync();

                sender.ItemsSource = items
                    .Where(item => item
                        .Name
                        .ToUpperInvariant()
                        .Contains(sender.Text.ToUpperInvariant() ) && !string.IsNullOrWhiteSpace(sender.Text))
                    .ToList();
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = (args.SelectedItem as LinkItem).Name;
        }

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