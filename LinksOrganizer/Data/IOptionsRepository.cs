using System.Collections.Generic;
using System.Threading.Tasks;
using LinksOrganizer.Models;

namespace LinksOrganizer.Data
{
    public interface IOptionsRepository
    {
        Task<Options> GetOptionsAsync();
        Task<int> SaveAsync(Options options);
    }
}