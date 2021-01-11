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
        public ICommand OptionsCommand => new Command(async () => await OptionsAsync());

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

        public StartPageViewModel(
            IClipboardInfo clipboardInfo,
            INavigationService navigationService,
            IMemoryCache memoryCache,
            ILinkItemRepository linkItemDatabase,
            IOptionsRepository optionsRepository,
            IResourcesProvider resourcesProvider)
            : base(navigationService, memoryCache, linkItemDatabase, optionsRepository, clipboardInfo, resourcesProvider)
        {
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

        private async Task OptionsAsync()
        {        
            await NavigationService.NavigateToAsync<OptionsViewModel>();
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
            await UpdateFavouriteLinks();          
        }
   
        private async Task UpdateFavouriteLinks()
        {
            var items = await Database.GetItemsAsync();
            var options = await Options.GetOptionsAsync(1);

            FavoriteLinks = options.IsOrderedByRank
                ? items.OrderByDescending(link => link.Rank).ToList()
                : items.OrderByDescending(link => link.LastUpdatedOn.Ticks).ToList();
        }
    }
}