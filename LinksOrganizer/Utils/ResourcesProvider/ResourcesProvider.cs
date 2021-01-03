using System.Threading.Tasks;
using Xamarin.Forms;
namespace LinksOrganizer.Utils.ResourcesProvider
{
    public class ResourcesProvider : IResourcesProvider
    {
        public ResourceDictionary Resources => Application.Current.Resources;

        public async Task<bool> DisplayAlert(string title, string question, string yesAnswer, string noAnswer)
        {
            return await Application.Current.MainPage.DisplayAlert(title, question, yesAnswer, noAnswer);          
        }
    }
}
