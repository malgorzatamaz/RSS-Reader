using RSS_Reader.ViewModel.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            bool uniqueNews = _rssContext.News.Any(n => n.Id == news.Id);
            if (!uniqueNews)
            {
                _rssContext.News.Add(new News
                   {
                       Category = news.Category,
                       Date = news.Date,
                       Description = news.Description,
                       Photo = news.Photo,
                       Title = news.Title,
                       UrlNews = news.UrlNews,
                       UrlImage = news.UrlImage,
                       Id = news.Id
                   });
            }
            _rssContext.SaveChanges();
        }

        public void DeleteSelectedArticle(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                _rssContext.News.Remove(_rssContext.News.FirstOrDefault(n => n.Id == Id));
                _rssContext.SaveChanges();
            }
        }

        public void GetSavedNews(string category)
        { }
    }
}
