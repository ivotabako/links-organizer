using System.Threading.Tasks;

namespace LinksOrganizer.Utils.ClipboardInfo
{
    public interface IClipboardInfo
    {
        bool HasText { get; }

        Task<string> GetTextAsync();

        Task SetTextAsync(string text);
    }
}
