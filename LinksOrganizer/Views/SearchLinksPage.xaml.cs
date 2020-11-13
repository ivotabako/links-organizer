using dotMorten.Xamarin.Forms;
using Kri.Solutions;
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
    public partial class SearchLinksPage : ContentPage
    {
        public SearchLinksPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing, 
            // otherwise assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var items = await App.Database.GetItemsAsync();

                //List<string> names = new List<string>();
                //foreach (var item in items)
                //{
                //    if (item.Name.Contains(sender.Text) )
                //    {
                //        names.Add(item.Name);
                //    }
                //}
                //sender.ItemsSource = names;

                sender.ItemsSource = items
                    //.Select(t => t.Name)
                    .Where(item => item.Name.Contains(sender.Text) && !string.IsNullOrWhiteSpace(sender.Text))
                    .ToList();
                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = dataset;
            }
        }


        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = (args.SelectedItem as LinkItem).Name;
            // Set sender.Text. You can use args.SelectedItem to build your text string.
        }


        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                await Navigation.PushAsync(new LinkItemPage
                {
                    BindingContext = args.ChosenSuggestion as LinkItem
                });
                // User selected an item from the suggestion list, take an action on it here.
            }
            else
            {
                // User hit Enter from the search box. Use args.QueryText to determine what to do.
            }
        }
    }
}