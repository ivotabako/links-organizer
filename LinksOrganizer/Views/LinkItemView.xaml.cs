using LinksOrganizer.Models;
using System;
using Xamarin.Forms;

namespace LinksOrganizer.Views
{
    public partial class LinkItemView : ContentPage
    {
        public LinkItemView()
        {
            InitializeComponent();
        }

        //async void OnDeleteClicked(object sender, EventArgs e)
        //{
        //    var linkItem = (LinkItem)BindingContext;
        //    await App.Database.DeleteItemAsync(linkItem);
        //    await Navigation.PopAsync();
        //}

        //async void OnCancelClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PopAsync();
        //}

        //private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var linkItem = (LinkItem)BindingContext;
            
        //    this.SaveButtonRumi.IsEnabled = !string.IsNullOrWhiteSpace(linkItem.Link) && !string.IsNullOrWhiteSpace(linkItem.Name);
            
        //}
    }
}
