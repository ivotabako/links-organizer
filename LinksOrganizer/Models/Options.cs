using LinksOrganizer.Themes;
using SQLite;
using System;

namespace LinksOrganizer.Models
{
    public class Options
    {
        [PrimaryKey]
        public int ID { get; set; }

        [NotNull]
        public bool IsOrderedByRank { get; set; }

        [NotNull]
        public Theme Theme { get; set; }

        [NotNull]
        public bool UseSecureLinksOnly { get; set; }

        [NotNull]
        public bool CanUseClipboard { get; set; }
    }
}

