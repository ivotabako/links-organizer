using Kri.Solutions;
using LinksOrganizer.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LinksOrganizer.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        public ICommand AddLinkItemCommand => new Command(async () => await AddLinkItemAsync());

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
    }
}