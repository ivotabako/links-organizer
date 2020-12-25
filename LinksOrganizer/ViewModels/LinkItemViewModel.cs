using LinksOrganizer.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LinksOrganizer.ViewModels
{
    public class LinkItemViewModel : ViewModelBase
    {
        public bool CanSave
        {
            get { return !string.IsNullOrWhiteSpace(this.Link) && !string.IsNullOrWhiteSpace(this.Name) && IsUrl(this.Link); }
        }

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

        private int _rank;
        public int Rank
        {
            get { return _rank; }
            set
            {
                _rank = value;
                RaisePropertyChanged(() => Rank);
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
                RaisePropertyChanged(() => CanSave);
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
                RaisePropertyChanged(() => CanSave);
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
                ID = this.Id,
                Rank = this.Rank
            };
            await App.Database.SaveItemAsync(linkItem);
            await NavigationService.NavigateToAsync<StartPageViewModel>();
        }

        private bool IsUrl(string uriName)
        {
            return Uri.TryCreate(uriName, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        async private void DeleteLinkItem()
        {
            var linkItem = new LinkItem()
            {
                Info = this.Info,
                Link = this.Link,
                Name = this.Name,
                ID = this.Id,
                Rank = this.Rank
            };
            await App.Database.DeleteItemAsync(linkItem);
            await NavigationService.NavigateToAsync<StartPageViewModel>();
        }

        async private void CancelLinkItem()
        {
            await NavigationService.NavigateToAsync<StartPageViewModel>();
        }

        public async override Task InitializeAsync(object navigationData)
        {
            if (navigationData is LinkItem data)
            {
                if (data.ID > 0)
                {
                    data.Rank++;
                    await App.Database.SaveItemAsync(data);
                }

                this.Name = data.Name;
                this.Link = data.Link;
                this.Info = data.Info;
                this.Id = data.ID;
                this.Rank = data.Rank;
            }
        }
    }
}