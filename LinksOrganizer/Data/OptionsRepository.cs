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
                    try
                    {
                        await Database.CreateTablesAsync(CreateFlags.None, typeof(Options)).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        await Database.DropTableAsync<Options>().ConfigureAwait(false);
                        await Database.CreateTablesAsync(CreateFlags.None, typeof(Options)).ConfigureAwait(false);
                    }
                }
                initialized = true;
            }
        }

        public async Task<Options> GetOptionsAsync()
        {
            var options = await Database.Table<Options>().ToListAsync();
            return options.FirstOrDefault() ?? new Options() { ID = 1, IsOrderedByRank = false, Theme = Themes.Theme.LightTheme, UseSecureLinksOnly = false, CanUseClipboard = false };
        }

        public async Task<int> SaveAsync(Options options)
        {
            var exists = await Database.Table<Options>().ToListAsync();

            if (exists.Any())
            {
                return await Database.UpdateAsync(options, typeof(Options));
            }
            else
            {
                return await Database.InsertAsync(options, typeof(Options));
            }
        }
    }
}

