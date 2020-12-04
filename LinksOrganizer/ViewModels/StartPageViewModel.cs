using LinksOrganizer.Models;
using System.Collections.Generic;
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

        public ICommand LoadLinkItemCommand => new Command(async (obj) => await LoadLinkItemAsync(obj));

        public ICommand SetSearchedLinkItemsCommand => new Command(async (obj) => await SetSearchedLinkItemNamesCommandAsync(obj));

        public List<LinkItem> SearchedLinks { get; private set; }

        private async Task AddLinkItemAsync()
        {
            var newLink = new LinkItem();

            if (Clipboard.HasText)
            {
                var text = await Clipboard.GetTextAsync();
                newLink.Link = text;
            }
            await NavigationService.NavigateToAsync<LinkItemViewModel>();
        }

        private async Task LoadLinkItemAsync(object obj)
        {
            await NavigationService.NavigateToAsync<LinkItemViewModel>(obj);
        }

        private async Task SetSearchedLinkItemNamesCommandAsync(object obj)
        {
            if (!(obj is string text) || string.IsNullOrWhiteSpace(text))
            {
                SearchedLinks = null;
                RaisePropertyChanged(() => SearchedLinks);
                return;
            }

            var items = await App.Database.GetItemsAsync();
            SearchedLinks = items.Where(item => item.Name.ToUpperInvariant().Contains(text.ToUpperInvariant())).ToList();
            RaisePropertyChanged(() => SearchedLinks);
        }
    }
}