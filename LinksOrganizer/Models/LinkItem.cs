using SQLite;

namespace LinksOrganizer.Models
{
    public class LinkItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public string Name { get; set; } = string.Empty;

        public string Info { get; set; } = string.Empty;

        public int Rank { get; set; }

        [NotNull]
        public string Link { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

