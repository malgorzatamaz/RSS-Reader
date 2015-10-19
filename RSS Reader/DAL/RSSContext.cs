using System.Data.Entity;
using RSS_Reader.Model;

namespace RSS_Reader.DAL
{
    /// <summary>
    /// Klasa definująca model bazy danych
    /// </summary>
    public class RSSContext : DbContext
    {
        public DbSet<News> News { get; set; }

        /// <summary>
        /// Konstruktor obsługujący tworzenie bazy danych
        /// </summary>
        public RSSContext()
        {
            Database.SetInitializer<RSSContext>(new DropCreateDatabaseAlways<RSSContext>());
        }
    }
}
