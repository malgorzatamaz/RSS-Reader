using System.Data.Entity;
using RSS_Reader.Model;

namespace RSS_Reader.DAL
{
    public class RSSContext : DbContext
    {
        public DbSet<News> News { get; set; }

        public RSSContext()
        {
            Database.SetInitializer<RSSContext>(new DropCreateDatabaseAlways<RSSContext>());
        }
    }
}
