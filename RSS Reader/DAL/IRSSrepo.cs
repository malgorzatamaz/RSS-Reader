using RSS_Reader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Reader.DAL
{
    interface IRSSrepo
    {
        void AddSelectedArticle(News news);
        void AddAllAnews();
        void DeleteSelectedArticle(News news);
        
    }
}
