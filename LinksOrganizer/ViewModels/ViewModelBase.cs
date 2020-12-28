using LinksOrganizer.Services.Navigation;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace LinksOrganizer.ViewModels
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        protected readonly IMemoryCache Cache;
        protected readonly INavigationService NavigationService;

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

        public ViewModelBase(INavigationService navigationService, IMemoryCache memoryCache)
        {
            NavigationService = navigationService;
            Cache = memoryCache;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}