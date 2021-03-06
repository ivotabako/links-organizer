using System;
using System.Threading.Tasks;
using System.Windows.Input;
using LinksOrganizer.Data;
using LinksOrganizer.Models;
using LinksOrganizer.Resx;
using LinksOrganizer.Services.Navigation;
using LinksOrganizer.Utils.ClipboardInfo;
using LinksOrganizer.Utils.ResourcesProvider;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LinksOrganizer.ViewModels
{
    public class LinkItemViewModel : ViewModelBase
    {
        private bool _canSave;
        public bool CanSave
        {
            get { return _canSave; }
            private set
            {
                if (_canSave == value)
                    return;

                _canSave = value;
                RaisePropertyChanged(() => CanSave);
            }
        }

        private bool _canDelete;
        public bool CanDelete
        {
            get { return _canDelete; }
            private set
            {
                if (_canDelete == value)
                    return;

                _canDelete = value;
                RaisePropertyChanged(() => CanDelete);
            }
        }

        public int Id { get; private set; }

        public int Rank { get; private set; }

        public DateTime LastUpdatedOn { get; private set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                _name = value;
                RaisePropertyChanged(() => Name);
                UpdateCanSave();
            }
        }

        private string _link;
        public string Link
        {
            get { return _link; }
            set
            {
                if (_link == value)
                    return;

                _link = value;
                RaisePropertyChanged(() => Link);
                UpdateCanSave();
            }
        }

        private string _info;
        private bool _useSecureLinksOnly;

        public string Info
        {
            get { return _info; }
            set
            {
                if (_info == value)
                    return;

                _info = value;
                RaisePropertyChanged(() => Info);
            }
        }

        public ICommand OptionsCommand => new Command(async () => await OptionsAsync());

        public ICommand SaveLinkItemCommand => new Command(SaveLinkItem);

        public ICommand DeleteLinkItemCommand => new Command(DeleteLinkItem);

        public ICommand CopyLinkItemCommand => new Command(async () => await CopyLinkItem());

        public ICommand OpenLinkItemCommand => new Command(async () => await OpenLinkItem());

        public ICommand ShareLinkItemCommand => new Command(async () => await ShareLinkItem());

        public LinkItemViewModel(
            INavigationService navigationService,
            ILinkItemRepository linkItemDatabase,
            IOptionsRepository optionsRepository,
            IClipboardInfo clipboardInfo,
            IResourcesProvider resourcesProvider)
            : base(navigationService, linkItemDatabase, optionsRepository, clipboardInfo, resourcesProvider)
        {

        }

        async private void SaveLinkItem()
        {
            var linkItem = new LinkItem()
            {
                Info = this.Info,
                Link = this.Link,
                Name = this.Name,
                ID = this.Id,
                Rank = this.Rank,
                LastUpdatedOn = DateTime.UtcNow
            };

            await Database.SaveItemAsync(linkItem);
            await NavigationService.NavigateToAsync<StartPageViewModel>();
        }

        private bool IsUrl(string uriName)
        {
            if (_useSecureLinksOnly)
                return Uri.TryCreate(uriName, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeHttps;

            return true;
        }

        async private void DeleteLinkItem()
        {
            var flag = await ResourcesProvider.DisplayAlert(
                AppResources.DeleteDialogTitle,
                AppResources.DeleteDialogQuestion,
                AppResources.DeleteDialogYesAnswer,
                AppResources.DeleteDialogNoAnswer);

            if (!flag)
                return;

            var linkItem = new LinkItem()
            {
                Info = this.Info,
                Link = this.Link,
                Name = this.Name,
                ID = this.Id,
                Rank = this.Rank,
                LastUpdatedOn = this.LastUpdatedOn
            };

            await Database.DeleteItemAsync(linkItem);
            await NavigationService.NavigateToAsync<StartPageViewModel>();
        }

        private async Task CopyLinkItem()
        {
            await ClipboardInfo.SetTextAsync(this.Link);
        }

        private async Task ShareLinkItem()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = this.Link,
                Title = "Share Web Link"
            });           
        }

        private async Task OpenLinkItem()
        {
            await Browser.OpenAsync(this.Link, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.AliceBlue,
                PreferredControlColor = Color.Violet
            });
        }

        public async override Task InitializeAsync(object navigationData)
        {
            var options = await Options.GetOptionsAsync();
            _useSecureLinksOnly = options.UseSecureLinksOnly;

            InitialiseBindings();

            if (navigationData is not LinkItem data)
                return;

            if (data.ID > 0)
            {
                data.Rank++;
                await Database.SaveItemAsync(data);
                CanDelete = true;
            }
            else
            {
                data.LastUpdatedOn = DateTime.UtcNow;
            }

            Name = data.Name;
            Link = data.Link;
            Info = data.Info;
            Id = data.ID;
            Rank = data.Rank;
            LastUpdatedOn = data.LastUpdatedOn;
        }

        private void UpdateCanSave()
        {
            var canSave = !string.IsNullOrWhiteSpace(this.Link) && !string.IsNullOrWhiteSpace(this.Name) && IsUrl(this.Link);
            CanSave = canSave;
        }

        private void InitialiseBindings()
        {
            RaisePropertyChanged(() => CanSave);
            RaisePropertyChanged(() => CanDelete);
        }

        private async Task OptionsAsync()
        {
            await NavigationService.NavigateToAsync<OptionsViewModel>();
        }
    }
}