using LinksOrganizer.Data;
using LinksOrganizer.Services.Navigation;
using LinksOrganizer.Utils;
using LinksOrganizer.Utils.ClipboardInfo;
using LinksOrganizer.Utils.ResourcesProvider;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace LinksOrganizer.ViewModels
{
    public abstract class ViewModelBase : ExtendedBindableObject, IBackButtonHandler
    {
        private readonly ILinkItemRepository database;
        private readonly IOptionsRepository options;
        private readonly IMemoryCache cache;
        private readonly INavigationService navigationService;
        private readonly IClipboardInfo clipboardInfo;
        private readonly IResourcesProvider resourcesProvider;

        protected ILinkItemRepository Database => database;
        protected IOptionsRepository Options => options;
        protected IMemoryCache Cache => cache;
        public INavigationService NavigationService => navigationService;
        protected IClipboardInfo ClipboardInfo => clipboardInfo;
        protected IResourcesProvider ResourcesProvider => resourcesProvider;

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }


        public ViewModelBase(
            INavigationService navigationService, 
            IMemoryCache memoryCache, 
            ILinkItemRepository linkItemDatabase,
            IOptionsRepository optionsDatabase,
            IClipboardInfo clipboardInfo,
            IResourcesProvider resourcesProvider)
        {
            this.navigationService = navigationService;
            this.cache = memoryCache;
            this.database = linkItemDatabase;
            this.options = optionsDatabase;
            this.clipboardInfo = clipboardInfo;
            this.resourcesProvider = resourcesProvider;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        public async Task<bool> HandleBackButton(ViewModelBase viewModel)
        {
            if (viewModel is StartPageViewModel )
            {
                await NavigationService.NavigateToAsync<StartPageViewModel>();
            }
            if (viewModel is OptionsViewModel)
            {
                // if we were before in options do not return again to it, instead go to start page
                await NavigationService.NavigateToAsync<StartPageViewModel>();
            }
            if (viewModel is LinkItemViewModel)
            {
                await NavigationService.NavigateToAsync<LinkItemViewModel>();
            }
            return true;
        }
    }
}