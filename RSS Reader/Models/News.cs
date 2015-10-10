using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Reader.Models
{
    public class News
    {
        public int NewsId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string PhotoPath { get; set; }
    }
}
