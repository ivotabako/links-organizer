using SQLite;

namespace Kri.Solutions
{
    public class LinkItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Link { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

