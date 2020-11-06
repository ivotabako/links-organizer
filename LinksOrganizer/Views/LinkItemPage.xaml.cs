﻿using System;
using Xamarin.Forms;

namespace Kri.Solutions
{
    public partial class LinkItemPage : ContentPage
    {
        public LinkItemPage()
        {
            InitializeComponent();
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            var linkItem = (LinkItem)BindingContext;
            await App.Database.SaveItemAsync(linkItem);
            await Navigation.PopAsync();
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            var linkItem = (LinkItem)BindingContext;
            await App.Database.DeleteItemAsync(linkItem);
            await Navigation.PopAsync();
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}