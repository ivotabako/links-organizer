using LinksOrganizer.Data;
using LinksOrganizer.Services.Navigation;
using LinksOrganizer.Utils.ClipboardInfo;
using LinksOrganizer.Utils.ResourcesProvider;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace LinksOrganizer.ViewModels
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        private readonly ILinkItemDatabase database;
        private readonly IMemoryCache cache;
        private readonly INavigationService navigationService;
        private readonly IClipboardInfo clipboardInfo;
        private readonly IResourcesProvider resourcesProvider;

        protected ILinkItemDatabase Database => database;
        protected IMemoryCache Cache => cache;
        protected INavigationService NavigationService => navigationService;
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
            ILinkItemDatabase linkItemDatabase, 
            IClipboardInfo clipboardInfo,
            IResourcesProvider resourcesProvider)
        {
            this.navigationService = navigationService;
            this.cache = memoryCache;
            this.database = linkItemDatabase;
            this.clipboardInfo = clipboardInfo;
            this.resourcesProvider = resourcesProvider;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}