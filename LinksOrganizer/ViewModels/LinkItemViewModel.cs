using Kri.Solutions;
using LinksOrganizer.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LinksOrganizer.ViewModels
{
    public class LinkItemViewModel : ViewModelBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private string _link;
        public string Link
        {
            get { return _link; }
            set
            {
                _link = value;
                RaisePropertyChanged(() => Link);
            }
        }

        private string _info;
        public string Info
        {
            get { return _info; }
            set
            {
                _info = value;
                RaisePropertyChanged(() => Info);
            }
        }

        public ICommand SaveLinkItemCommand => new Command(SaveLinkItem);

        public ICommand DeleteLinkItemCommand => new Command(DeleteLinkItem);

        public ICommand CancelLinkItemCommand => new Command(CancelLinkItem);

        async private void SaveLinkItem()
        {
            var linkItem = new LinkItem()
            {
                Info = this.Info,
                Link = this.Link,
                Name = this.Name,
                ID = this.Id
            };
            await App.Database.SaveItemAsync(linkItem);
            await NavigationService.NavigateToAsync<StartPageViewModel>();
        }

        async private void DeleteLinkItem()
        {
            var linkItem = new LinkItem()
            {
                Info = this.Info,
                Link = this.Link,
                Name = this.Name,
                ID = this.Id
            };
            await App.Database.DeleteItemAsync(linkItem);
            await NavigationService.NavigateToAsync<StartPageViewModel>();
        }

        async private void CancelLinkItem()
        {
            await NavigationService.NavigateToAsync<StartPageViewModel>();
        }

        public override Task InitializeAsync(object navigationData)
        {
            var data = navigationData as LinkItem;
            this.Name = data.Name;
            this.Link = data.Link;
            this.Info = data.Info;
            this.Id = data.ID;

            return Task.FromResult(false);
        }
    }
}