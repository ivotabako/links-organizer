using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LinksOrganizer.Utils.ClipboardInfo
{
    public class ClipboardInfo : IClipboardInfo
    {
        public bool HasText => Clipboard.HasText;

        public Task<string> GetTextAsync() => Clipboard.GetTextAsync();

    }
}
