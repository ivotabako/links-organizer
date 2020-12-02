using Kri.Solutions;
using LinksOrganizer.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace LinksOrganizer.ViewModels
{
    public class LinkItemViewModel : ViewModelBase
    {
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


        //      public ICommand DeleteLinkItemCommand => new Command(async () => await FilterAsync());

        //public ICommand CancelCommand => new Command(async () => await ClearFilterAsync());

        //public override async Task InitializeAsync(object navigationData)
        //{
        //    IsBusy = true;



        //    IsBusy = false;
        //}

        async private void SaveLinkItem()
        {
            var linkItem = new LinkItem()
            {
                Info = this.Info,
                Link = this.Link,
                Name = this.Name
            };
            await App.Database.SaveItemAsync(linkItem);
            await NavigationService.NavigateToAsync<StartPageViewModel>();
        }
    }
}