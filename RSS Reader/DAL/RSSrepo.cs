using RSS_Reader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Reader.DAL
{
    class RSSrepo : IRSSrepo
    {
        private RSSContext _rssContext = new RSSContext();

        public void AddSelectedArticle(News news)
        {
            _rssContext.News.Add(new News
                {
                    CategoryName = news.CategoryName,
                    Date = news.Date,
                    Description = news.Description,
                    Photo = news.Photo,
                    Title = news.Title,
                    Url = news.Url
                });

            _rssContext.SaveChanges();
        }

        public void AddAllAnews()
        {
            throw new NotImplementedException();
        }

        public void DeleteSelectedArticle(News news)
        {
            throw new NotImplementedException();
        }
    }
}
