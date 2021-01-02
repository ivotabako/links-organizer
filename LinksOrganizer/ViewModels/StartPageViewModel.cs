using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LinksOrganizer.Data;
using LinksOrganizer.Models;
using LinksOrganizer.Services.Navigation;
using LinksOrganizer.Themes;
using LinksOrganizer.Utils;
using LinksOrganizer.Utils.ClipboardInfo;
using LinksOrganizer.Utils.ResourcesProvider;
using Microsoft.Extensions.Caching.Memory;
using Xamarin.Forms;

namespace LinksOrganizer.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly IResourcesProvider resourcesProvider;

        public ICommand AddLinkItemCommand => new Command(async () => await AddLinkItemAsync());

        public ICommand LoadLinkItemCommand => new Command<LinkItem>(async (item) => await LoadLinkItemAsync(item));

        public ICommand SetFavoriteLinksItemsCommand => new Command<string>(async (item) => await SetFavoriteLinksItemsCommandAsync(item));

        private List<LinkItem> _favoriteLinks;
        public List<LinkItem> FavoriteLinks
        {
            get => _favoriteLinks;
            private set
            {
                _favoriteLinks = value;
                RaisePropertyChanged(() => FavoriteLinks);
            }
        }

        private bool _isOrderedByRank;
        public bool IsOrderedByRank
        {
            get => _isOrderedByRank;
            private set
            {
                _isOrderedByRank = value;
                RaisePropertyChanged(() => IsOrderedByRank);
            }
        }

        private Theme _theme;
        public Theme Theme
        {
            get => _theme;
            private set
            {
                _theme = value;
                RaisePropertyChanged(() => Theme);
            } 
        }

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
                return;
            }

            FavoriteLinks = items.Where(item =>
               item.Name.ToUpperInvariant().Contains(searchedText.ToUpperInvariant()) ||
               item.Info.ToUpperInvariant().Contains(searchedText.ToUpperInvariant())
            ).ToList();
        }

        public async override Task InitializeAsync(object navigationData)
        {
            await RefreshFavoriteLinks(navigationData);

            ChangeTheme(navigationData);
        }

        private void ChangeTheme(object navigationData)
        {
            var theme = GetThemeFromNavigationData(navigationData);
            if (!theme.HasValue)
                return;

            UpdateTheme(theme.Value);
        }

        private Theme? GetThemeFromNavigationData(object navigationData)
        {
            Theme? theme = null;
            if (navigationData == null)
            {
                Cache.TryGetValue(ChangeEvents.ThemeChanged, out object result);
                theme = result != null && result is Theme
                    ? (Theme)result
                    : Theme.LightTheme;
            }
            else if (navigationData is ValueTuple<Theme, ChangeEvents> toggleTupple
                && toggleTupple.Item2 == ChangeEvents.ThemeChanged)
            {
                theme = toggleTupple.Item1;
            }

            return theme;
        }

        public void UpdateTheme(Theme theme)
        {
            AddThemeToResourceDictionary(theme);
            StoreInCache(ChangeEvents.ThemeChanged, theme);
            Theme = theme;
        }

        private void AddThemeToResourceDictionary(Theme theme)
        {
            ICollection<ResourceDictionary> mergedDictionaries = resourcesProvider.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (theme)
                {
                    case Theme.DarkTheme:
                        mergedDictionaries.Add(new DarkTheme());
                        return;
                    case Theme.LightTheme:
                    default:
                        mergedDictionaries.Add(new LightTheme());
                        return;
                }
            }
        }

        private async Task RefreshFavoriteLinks(object navigationData)
        {
            bool? isOrderedByRank = GetOrderTypeFromNavigationData(navigationData);
            if (!isOrderedByRank.HasValue)
                return;

            await UpdateFavouriteLinks(isOrderedByRank.Value);
            StoreInCache(ChangeEvents.OrderChanged, isOrderedByRank.Value);
        }

        private bool? GetOrderTypeFromNavigationData(object navigationData)
        {
            bool? isOrderedByRank = null;
            if (navigationData is null)
            {
                Cache.TryGetValue(ChangeEvents.OrderChanged, out object result);
                isOrderedByRank = result == null
                    ? false
                    : (bool)result;
            }
            else if (navigationData is ValueTuple<bool, ChangeEvents> toggleTupple && toggleTupple.Item2 == ChangeEvents.OrderChanged)
            {
                isOrderedByRank = toggleTupple.Item1;
            }

            return isOrderedByRank;
        }

        private async Task UpdateFavouriteLinks(bool isOrderedByRank)
        {
            var items = await Database.GetItemsAsync();
            IsOrderedByRank = isOrderedByRank;

            FavoriteLinks = IsOrderedByRank
                ? items.OrderByDescending(link => link.Rank).ToList()
                : items.OrderByDescending(link => link.LastUpdatedOn.Ticks).ToList();
        }

        private void StoreInCache(object key, object value)
        {
            using var entry = Cache.CreateEntry(key);
            entry.Value = value;
        }
    }
}