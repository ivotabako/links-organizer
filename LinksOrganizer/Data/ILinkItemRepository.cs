using System.Collections.Generic;
using System.Threading.Tasks;
using LinksOrganizer.Models;

namespace LinksOrganizer.Data
{
    public interface ILinkItemRepository
    {
        Task<int> DeleteItemAsync(LinkItem item);
        Task<LinkItem> GetItemAsync(int id);
        Task<List<LinkItem>> GetItemsAsync();
    
        Task<int> SaveItemAsync(LinkItem item);
    }
}