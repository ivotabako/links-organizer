using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinksOrganizer.Models;
using SQLite;

namespace LinksOrganizer.Data
{
    public class OptionsRepository : IOptionsRepository
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public OptionsRepository()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Options).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Options)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public async Task<Options> GetOptionsAsync(int id)
        {
            var options = await Database.Table<Options>().Where(i => i.ID == id).FirstOrDefaultAsync();
            return options ?? new Options() { ID = id, IsOrderedByRank = false, Theme = Themes.Theme.LightTheme };
        }

        public Task<int> SaveAsync(Options options)
        {
            if (options.ID != 0)
            {
                return Database.UpdateAsync(options);
            }
            else
            {
                return Database.InsertAsync(options);
            }
        }
    }
}

