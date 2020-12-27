using LinksOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LinksOrganizer.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        public ICommand AddLinkItemCommand => new Command(async () => await AddLinkItemAsync());

        public ICommand LoadLinkItemCommand => new Command<LinkItem>(async (item) => await LoadLinkItemAsync(item));

        public ICommand SetSearchedLinkItemsCommand => new Command<string>(async (item) => await SetSearchedLinkItemNamesCommandAsync(item));

        public List<LinkItem> SearchedLinks { get; private set; }

        public List<LinkItem> FavoriteLinks { get; private set; }

        public bool IsOrderedByRank { get; private set; }

        private async Task AddLinkItemAsync()
        {
            var newLink = new LinkItem();

            (bool isUrl, string url) = await CheckClipboard();
            if (isUrl)
            {
                newLink.Link = url;
            }

            await NavigationService.NavigateToAsync<LinkItemViewModel>(newLink);
        }

        private async Task<(bool isUrl, string url)> CheckClipboard()
        {
            if (Clipboard.HasText)
            {
                string uriName = await Clipboard.GetTextAsync();
                bool result = Uri.TryCreate(uriName, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                return (result, uriName);
            }

            return (false, string.Empty);
        }

        private async Task LoadLinkItemAsync(LinkItem item)
        {
            await NavigationService.NavigateToAsync<LinkItemViewModel>(item);
        }

        private async Task SetSearchedLinkItemNamesCommandAsync(string searchedText)
        {
            if (string.IsNullOrWhiteSpace(searchedText))
            {
                SearchedLinks = null;
                RaisePropertyChanged(() => SearchedLinks);
                return;
            }

            var items = await App.Database.GetItemsAsync();
            SearchedLinks = items.Where(item =>
               item.Name.ToUpperInvariant().Contains(searchedText.ToUpperInvariant()) ||
               item.Info.ToUpperInvariant().Contains(searchedText.ToUpperInvariant())
            ).ToList();
            RaisePropertyChanged(() => SearchedLinks);
        }

        public async override Task InitializeAsync(object navigationData)
        {
            var items = await App.Database.GetItemsAsync();

            if (navigationData is bool isToggled)
            {
                using (var key = Cache.CreateEntry("IsToggled"))
                {
                    key.Value = isToggled;
                }
            }

            if(Cache.TryGetValue("IsToggled", out object isToggledFromCache) && (bool)isToggledFromCache == true )
            {
                IsOrderedByRank = (bool)isToggledFromCache;
                RaisePropertyChanged(() => IsOrderedByRank);

                this.FavoriteLinks = items.OrderByDescending(link => link.Rank).ToList();
            }
            else
            {
                IsOrderedByRank = false;
                RaisePropertyChanged(() => IsOrderedByRank);

                this.FavoriteLinks = items.OrderByDescending(link => link.CreateDate.Ticks).ToList();
            }

            RaisePropertyChanged(() => FavoriteLinks);
        }
    }
}