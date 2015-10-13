using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSS_Reader.Model;

namespace RSS_Reader.DAL
{
    class RSSrepo : IRSSrepo
    {
        private RSSContext _rssContext = new RSSContext();

        public void AddSelectedArticle(ObservableCollection<Category> ArchiveListCategories, News news)
        {
            bool newsExist = _rssContext.News.Any(n => n.Id == news.Id);
            if (!newsExist)
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

                _rssContext.SaveChanges();

                if (!ArchiveListCategories.Any(n => n.Name == news.Category))
                    UpdateArchiveCategory(ArchiveListCategories, news.Category, true);
            }            
        }

        public void DeleteSelectedArticle(ObservableCollection<Category> ArchiveListCategories, string Id)
        {

            if (!string.IsNullOrEmpty(Id))
            {
                string selectedCategory = _rssContext.News.Where(n => n.Id == Id).Select(n => n.Category).Single();

                _rssContext.News.Remove(_rssContext.News.FirstOrDefault(n => n.Id == Id));
                _rssContext.SaveChanges();

                if (!_rssContext.News.Any(n => n.Category == selectedCategory))
                    UpdateArchiveCategory(ArchiveListCategories, selectedCategory, false);
            }

        }

        public void GetListArchiveCategories(ObservableCollection<Category> ArchiveListCategories)
        {

            var articles = _rssContext.News.GroupBy(n => n.Category).Select(n => n.FirstOrDefault()).ToList();
            foreach (var a in articles)
            {
                ArchiveListCategories.Add(new Category
                {
                    Name = a.Category,
                    Url = string.Empty
                });
            }

        }

        public void GetSavedNews(ObservableCollection<News> lineNews, ObservableCollection<Category> ArchiveListCategories, string category)
        {
            if (ArchiveListCategories.Any())
            {
                var savedNews = _rssContext.News.Where(n => n.Category == category);
                
                foreach (var news in savedNews)
                    {
                        lineNews.Add(new News
                        {
                            Category = news.Category,
                            Date = news.Date,
                            Description = news.Description,
                            Id = news.Id,
                            Photo = news.Photo,
                            Title = news.Title,
                            UrlImage = news.UrlImage,
                            UrlNews = news.UrlNews
                        });
                    }
            }
        }


        public void UpdateArchiveCategory(ObservableCollection<Category> ArchiveListCategories, string selectedCategory, bool toAdd)
        {
            if (toAdd)
            {
                ArchiveListCategories.Add(new Category
                {
                    Name = selectedCategory,
                    Url = string.Empty
                });
            }
            else if (!toAdd)
            {
                foreach (var category in ArchiveListCategories.ToList())
                {
                    if (category.Name == selectedCategory)
                    {
                        ArchiveListCategories.Remove(category);
                        break;
                    }

                }
            }
        }



    }
}
