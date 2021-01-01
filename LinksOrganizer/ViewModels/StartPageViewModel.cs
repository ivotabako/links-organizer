using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LinksOrganizer.Data;
using LinksOrganizer.Models;
using LinksOrganizer.Utils;
using LinksOrganizer.Services.Navigation;
using LinksOrganizer.Utils.ClipboardInfo;
using Microsoft.Extensions.Caching.Memory;
using Xamarin.Forms;
using LinksOrganizer.Themes;
using LinksOrganizer.Utils.ResourcesProvider;

namespace LinksOrganizer.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly IResourcesProvider resourcesProvider;

        public ICommand AddLinkItemCommand => new Command(async () => await AddLinkItemAsync());

        public ICommand LoadLinkItemCommand => new Command<LinkItem>(async (item) => await LoadLinkItemAsync(item));

        public ICommand SetFavoriteLinksItemsCommand => new Command<string>(async (item) => await SetFavoriteLinksItemsCommandAsync(item));

        public List<LinkItem> FavoriteLinks { get; private set; }

        public bool IsOrderedByRank { get; private set; }

        public Theme Theme { get; private set; }

        public StartPageViewModel(
            IClipboardInfo clipboardInfo,
            INavigationService navigationService,
            IMemoryCache memoryCache,
            ILinkItemDatabase linkItemDatabase,
            IResourcesProvider resourcesProvider)
            : base(navigationService, memoryCache, linkItemDatabase, clipboardInfo)
        {
            this.resourcesProvider = resourcesProvider;
        }

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
            if (ClipboardInfo.HasText)
            {
                string uriName = await ClipboardInfo.GetTextAsync();
                bool result = Uri.TryCreate(uriName, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                return (result, uriName);
            }

            return (false, string.Empty);
        }

        private async Task LoadLinkItemAsync(LinkItem item)
        {
            await NavigationService.NavigateToAsync<LinkItemViewModel>(item);
        }

        private async Task SetFavoriteLinksItemsCommandAsync(string searchedText)
        {
            var items = await Database.GetItemsAsync();
            if (string.IsNullOrWhiteSpace(searchedText))
            {
                FavoriteLinks = items.ToList();
                RaisePropertyChanged(() => FavoriteLinks);
                return;
            }

            FavoriteLinks = items.Where(item =>
               item.Name.ToUpperInvariant().Contains(searchedText.ToUpperInvariant()) ||
               item.Info.ToUpperInvariant().Contains(searchedText.ToUpperInvariant())
            ).ToList();
            RaisePropertyChanged(() => FavoriteLinks);
        }

        public async override Task InitializeAsync(object navigationData)
        {
            await RefreshFavoriteLinks(navigationData);

            ChangeTheme(navigationData);
        }

        private void ChangeTheme(object navigationData)
        {
            if (!(navigationData is ValueTuple<Theme, ChangeEvents> toggleTupple 
                && toggleTupple.Item2 == ChangeEvents.ThemeChanged))
                return;

            StoreThemeInCache(toggleTupple);
            AddThemeToResourceDictionary(toggleTupple);
            UpdateTheme();
        }

        private void StoreThemeInCache((Theme, ChangeEvents) toggleTupple)
        {
            using var key = Cache.CreateEntry(ChangeEvents.ThemeChanged);
            key.Value = toggleTupple.Item1;
        }

        private void AddThemeToResourceDictionary((Theme, ChangeEvents) toggleTupple)
        {
            ICollection<ResourceDictionary> mergedDictionaries = resourcesProvider.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                if (toggleTupple.Item1 == Theme.DarkTheme)
                    mergedDictionaries.Add(new DarkTheme());
                else
                    mergedDictionaries.Add(new LightTheme());
            }
        }

        private void UpdateTheme()
        {
            if (!(Cache.TryGetValue(ChangeEvents.ThemeChanged, out object theme) && theme is Theme))
                return;

            Theme = (Theme)theme;
            RaisePropertyChanged(() => Theme);
        }

        private async Task RefreshFavoriteLinks(object navigationData)
        {
            if (navigationData is null)
            {
                await InitialiseLinks();
                return;
            }

            if (!(navigationData is ValueTuple<bool, ChangeEvents> toggleTupple && toggleTupple.Item2 == ChangeEvents.OrderChanged))
            {
                return;
            }

            StoreOrderInCache(toggleTupple);
            await UpdateFavouriteLinks();
        }

        private async Task UpdateFavouriteLinks()
        {
            if (!Cache.TryGetValue(ChangeEvents.OrderChanged, out object isOrderedByRank))
                return;

            var items = await Database.GetItemsAsync();
            IsOrderedByRank = (bool)isOrderedByRank;

            if (IsOrderedByRank)
                FavoriteLinks = items.OrderByDescending(link => link.Rank).ToList();
            else
                FavoriteLinks = items.OrderByDescending(link => link.LastUpdatedOn.Ticks).ToList();

            RaisePropertyChanged(() => IsOrderedByRank);
            RaisePropertyChanged(() => FavoriteLinks);
        }

        private void StoreOrderInCache((bool, ChangeEvents) toggleTupple)
        {
            using var key = Cache.CreateEntry(ChangeEvents.OrderChanged);
            key.Value = toggleTupple.Item1;
        }

        private async Task InitialiseLinks()
        {
            var items = await Database.GetItemsAsync();
            IsOrderedByRank = false;
            FavoriteLinks = items.OrderByDescending(link => link.LastUpdatedOn.Ticks).ToList();

            RaisePropertyChanged(() => IsOrderedByRank);
            RaisePropertyChanged(() => FavoriteLinks);
        }
    }
}