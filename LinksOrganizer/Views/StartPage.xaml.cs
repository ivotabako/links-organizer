using dotMorten.Xamarin.Forms;
using Kri.Solutions;
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
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var items = await App.Database.GetItemsAsync();

                sender.ItemsSource = items
                    .Where(item => item.Name.Contains(sender.Text) && !string.IsNullOrWhiteSpace(sender.Text))
                    .ToList();
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = (args.SelectedItem as LinkItem).Name;
        }

        private async void OnAddButtonClicked(object sender, EventArgs e)
        {
            var newLink = new LinkItem();

            if (Clipboard.HasText)
            {
                var text = await Clipboard.GetTextAsync();
                newLink.Link = text;
            }
            await Navigation.PushAsync(new LinkItemPage
            {
                BindingContext = newLink
            });
        }

        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                await Navigation.PushAsync(new LinkItemPage
                {
                    BindingContext = args.ChosenSuggestion as LinkItem
                });               
            }
        }
    }
}