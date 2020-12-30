using LinksOrganizer.Data;
using LinksOrganizer.Services.Navigation;
using LinksOrganizer.Utils.ClipboardInfo;
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

        protected ILinkItemDatabase Database => database;
        protected IMemoryCache Cache => cache;
        protected INavigationService NavigationService => navigationService;
        protected IClipboardInfo ClipboardInfo => clipboardInfo;

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


        public ViewModelBase(INavigationService navigationService, IMemoryCache memoryCache, ILinkItemDatabase linkItemDatabase, IClipboardInfo clipboardInfo)
        {
            this.navigationService = navigationService;
            cache = memoryCache;
            database = linkItemDatabase;
            this.clipboardInfo = clipboardInfo;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}