using System;
using Xamarin.Forms;

namespace Kri.Solutions
{
    public partial class LinkListPage : ContentPage
    {
        public LinkListPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = await App.Database.GetItemsAsync();
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LinkItemPage
            {
                BindingContext = new LinkItem()
            });
        }

        async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            
        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new LinkItemPage
                {
                    BindingContext = e.SelectedItem as LinkItem
                });
            }
        }
    }
}
