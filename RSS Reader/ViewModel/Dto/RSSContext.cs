using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Reader.ViewModel.Dto
{
    public class RSSContext : DbContext
    {
        public DbSet<News> News { get; set; }

        public RSSContext()
        {
            Database.SetInitializer<RSSContext>(new DropCreateDatabaseIfModelChanges<RSSContext>());
        }
    }
}
