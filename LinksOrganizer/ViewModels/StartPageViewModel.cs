using LinksOrganizer.Models;
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

        public ICommand SetSearchedLinkItemsCommand => new Command<string> (async (item) => await SetSearchedLinkItemNamesCommandAsync(item));

        public List<LinkItem> SearchedLinks { get; private set; }

        public List<LinkItem> FavoriteLinks { get; private set; }
        
        private async Task AddLinkItemAsync()
        {
            var newLink = new LinkItem();

            if (Clipboard.HasText)
            {
                var text = await Clipboard.GetTextAsync();
                newLink.Link = text;
            }
            await NavigationService.NavigateToAsync<LinkItemViewModel>(newLink);
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
            SearchedLinks =  items.Where(item =>
                item.Name.ToUpperInvariant().Contains(searchedText.ToUpperInvariant()) ||
                item.Info.ToUpperInvariant().Contains(searchedText.ToUpperInvariant())
            ).ToList();
            RaisePropertyChanged(() => SearchedLinks);
        }

        public async override Task InitializeAsync(object navigationData)
        {
            var items = await App.Database.GetItemsAsync();
            this.FavoriteLinks = items.OrderByDescending(link => link.Rank).ToList();
            RaisePropertyChanged(() => FavoriteLinks);
        }
    }
}