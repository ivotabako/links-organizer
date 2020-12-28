using System.Collections.Generic;
using System.Threading.Tasks;
using LinksOrganizer.Models;

namespace LinksOrganizer.Data
{
    public interface ILinkItemDatabase
    {
        Task<int> DeleteItemAsync(LinkItem item);
        Task<LinkItem> GetItemAsync(int id);
        Task<List<LinkItem>> GetItemsAsync();
        Task<List<LinkItem>> GetItemsNotDoneAsync();
        Task<int> SaveItemAsync(LinkItem item);
    }
}