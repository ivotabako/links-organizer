using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinksOrganizer.Models;
using SQLite;

namespace LinksOrganizer.Data
{
    public class LinkItemDatabase : ILinkItemDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public LinkItemDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(LinkItem).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(LinkItem)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<List<LinkItem>> GetItemsAsync()
        {
            return Database.Table<LinkItem>().ToListAsync();
        }

        public Task<List<LinkItem>> GetItemsNotDoneAsync()
        {
            return Database.QueryAsync<LinkItem>("SELECT * FROM [LinkItem] WHERE [Done] = 0");
        }

        public Task<LinkItem> GetItemAsync(int id)
        {
            return Database.Table<LinkItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(LinkItem item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(LinkItem item)
        {
            return Database.DeleteAsync(item);
        }
    }
}

