using LinksOrganizer.Data;
using LinksOrganizer.Services.Navigation;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace LinksOrganizer.ViewModels
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        private readonly ILinkItemDatabase database;
        protected readonly IMemoryCache cache;
        protected readonly INavigationService navigationService;

        protected ILinkItemDatabase Database => database;
        protected IMemoryCache Cache => cache;
        protected INavigationService NavigationService => navigationService;

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

        public ViewModelBase(INavigationService navigationService, IMemoryCache memoryCache, ILinkItemDatabase linkItemDatabase)
        {
            this.navigationService = navigationService;
            cache = memoryCache;
            database = linkItemDatabase;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}