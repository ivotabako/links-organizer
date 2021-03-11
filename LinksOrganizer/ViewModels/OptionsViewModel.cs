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
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LinksOrganizer.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
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
       
        private bool _canUseClipboard;
        public bool CanUseClipboard
        {
            get => _canUseClipboard;
            private set
            {
                _canUseClipboard = value;

                RaisePropertyChanged(() => CanUseClipboard);
            }
        }

        private bool _useSecureLinksOnly;
        public bool UseSecureLinksOnly
        {
            get => _useSecureLinksOnly;
            private set
            {
                _useSecureLinksOnly = value;

                RaisePropertyChanged(() => UseSecureLinksOnly);
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

        public ICommand ExportCommand => new Command(async () => await ExportAsync());

        private async Task ExportAsync()
        {
            var data = await Database.GetItemsAsync();
            string output = JsonConvert.SerializeObject(data.Select(d=> new { d.Name, d.Link }));

            await Share.RequestAsync(new ShareTextRequest(output));
        }

        public OptionsViewModel(
            IClipboardInfo clipboardInfo,
            INavigationService navigationService,
            ILinkItemRepository linkItemDatabase,
            IOptionsRepository optionsRepository,
            IResourcesProvider resourcesProvider)
            : base(navigationService, linkItemDatabase, optionsRepository, clipboardInfo, resourcesProvider)
        {
        }

        public async override Task InitializeAsync(object navigationData)
        {
            if (navigationData is null)
            {
                var options = await Options.GetOptionsAsync();
                _theme = options != null ? options.Theme : Theme.LightTheme;
                _isOrderedByRank = options != null && options.IsOrderedByRank;
                _useSecureLinksOnly = options != null && options.UseSecureLinksOnly;
                _canUseClipboard = options != null && options.CanUseClipboard;
                RaisePropertyChanged(() => Theme);
                RaisePropertyChanged(() => IsOrderedByRank);
                RaisePropertyChanged(() => UseSecureLinksOnly);
                RaisePropertyChanged(() => CanUseClipboard);
            }
            if(navigationData is ValueTuple<bool,ChangeEvents> pair)
                await SaveFromNavigationData(pair);          
        }       

        private void AddThemeToResourceDictionary(Theme theme)
        {
            ICollection<ResourceDictionary> mergedDictionaries = ResourcesProvider.Resources.MergedDictionaries;
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

        private async Task SaveFromNavigationData(ValueTuple<bool,ChangeEvents> navigationData)
        {
            switch (navigationData.Item2)
            {
                case ChangeEvents.OrderChanged:
                    _isOrderedByRank = navigationData.Item1;
                    break;
                case ChangeEvents.ThemeChanged:
                    _theme = navigationData.Item1 ? Theme.DarkTheme : Theme.LightTheme;
                    AddThemeToResourceDictionary(_theme);
                    break;
                case ChangeEvents.SecureLinksChanged:
                    _useSecureLinksOnly = navigationData.Item1;
                    break;
                case ChangeEvents.UseClipboardChanged:
                    _canUseClipboard = navigationData.Item1;
                    break;
            }
            
            await Options.SaveAsync(new Models.Options() { IsOrderedByRank = _isOrderedByRank, Theme = _theme, ID = 1, UseSecureLinksOnly = _useSecureLinksOnly, CanUseClipboard = _canUseClipboard });
        }
    }
}