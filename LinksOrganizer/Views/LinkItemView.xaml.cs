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

        //private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var linkItem = (LinkItem)BindingContext;
            
        //    this.SaveButtonRumi.IsEnabled = !string.IsNullOrWhiteSpace(linkItem.Link) && !string.IsNullOrWhiteSpace(linkItem.Name);
            
        //}
    }
}
