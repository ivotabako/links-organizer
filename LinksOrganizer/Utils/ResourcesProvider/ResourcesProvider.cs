using Xamarin.Forms;
namespace LinksOrganizer.Utils.ResourcesProvider
{
    public class ResourcesProvider : IResourcesProvider
    {
        public ResourceDictionary Resources => Application.Current.Resources;
    }
}
