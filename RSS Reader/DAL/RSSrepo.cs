using RSS_Reader.ViewModel.Dto;
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
                    Category = "cat",
                    Date = news.Date,
                    Description = news.Description,
                    Photo = news.Photo,
                    Title = news.Title,
                    LinkNews = news.LinkNews,
                    LinkImage = news.LinkImage,
                    Id = news.Id
                    
                });

            _rssContext.SaveChanges();
        }

        public void DeleteSelectedArticle(News news)
        {
            throw new NotImplementedException();
        }
    }
}
