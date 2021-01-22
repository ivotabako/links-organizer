using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinksOrganizer.Data;
using LinksOrganizer.Models;
using LinksOrganizer.Services.Navigation;
using LinksOrganizer.Themes;
using LinksOrganizer.Utils;
using LinksOrganizer.Utils.ClipboardInfo;
using LinksOrganizer.Utils.ResourcesProvider;
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
                var options = await Options.GetOptionsAsync(1);
                _theme = options != null ? options.Theme : Theme.LightTheme;
                _isOrderedByRank = options != null && options.IsOrderedByRank;
                RaisePropertyChanged(() => Theme);
                RaisePropertyChanged(() => IsOrderedByRank);
            }

            await GetOrderTypeFromNavigationData(navigationData);
            await GetThemeFromNavigationData(navigationData);
        }

        private async Task GetThemeFromNavigationData(object navigationData)
        {
            if (navigationData is ValueTuple<Theme, ChangeEvents> toggleTupple
                && toggleTupple.Item2 == ChangeEvents.ThemeChanged)
            {
                await Options.SaveAsync(new Options() { IsOrderedByRank = _isOrderedByRank, Theme = toggleTupple.Item1, ID = 1 });
                AddThemeToResourceDictionary(toggleTupple.Item1);
            }
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

        private async Task GetOrderTypeFromNavigationData(object navigationData)
        {
            if (navigationData is ValueTuple<bool, ChangeEvents> toggleTupple && toggleTupple.Item2 == ChangeEvents.OrderChanged)
            {
                await Options.SaveAsync(new Models.Options() { IsOrderedByRank = toggleTupple.Item1, Theme = _theme, ID = 1 });
            }
        }
    }
}