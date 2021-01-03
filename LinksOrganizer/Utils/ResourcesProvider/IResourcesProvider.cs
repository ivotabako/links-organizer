using System.Threading.Tasks;
using Xamarin.Forms;
namespace LinksOrganizer.Utils.ResourcesProvider
{
    public interface IResourcesProvider
    {
        ResourceDictionary Resources { get; }
        Task<bool> DisplayAlert(string title, string question, string yesAnswer, string noAnswer);
    }
}
