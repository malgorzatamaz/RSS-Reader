using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSS_Reader.Model;

namespace RSS_Reader.DAL
{
    /// <summary>
    /// Klasa implementująca metody interface'u IRSSrepo
    /// </summary>
    class RSSrepo : IRSSrepo
    {
        private RSSContext _rssContext = new RSSContext();
        
        /// <summary>
        /// Funkcja zapisująca wybraną wiadomość do bazy danych
        /// </summary>
        /// <param name="archiveListCategories">Lista kategorii dostępnych w bazie danych</param>
        /// <param name="news">Wiadomość do zapisania</param>
        public void AddSelectedArticle(ObservableCollection<Category> archiveListCategories, News news)
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

                if (!archiveListCategories.Any(n => n.Name == news.Category))
                    UpdateArchiveCategory(archiveListCategories, news.Category, true);
            }            
        }

        /// <summary>
        /// Funkcja kasująca wybraną wiadomość z bazy danych
        /// </summary>
        /// <param name="archiveListCategories">Lista kategorii dostępnych w bazie danych</param>
        /// <param name="id">Id wybranej wiadomości</param>
        public void DeleteSelectedArticle(ObservableCollection<Category> archiveListCategories, string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                string selectedCategory = _rssContext.News.Where(n => n.Id == id).Select(n => n.Category).Single();

                _rssContext.News.Remove(_rssContext.News.FirstOrDefault(n => n.Id == id));
                _rssContext.SaveChanges();

                if (!_rssContext.News.Any(n => n.Category == selectedCategory))
                    UpdateArchiveCategory(archiveListCategories, selectedCategory, false);
            }

        }

        /// <summary>
        /// Funkcja pobierająca listę kategorii z bazy danych 
        /// </summary>
        /// <param name="archiveListCategories">Lista kategorii dostępnych w bazie danych</param>
        public void GetListArchiveCategories(ObservableCollection<Category> archiveListCategories)
        {

            var articles = _rssContext.News.GroupBy(n => n.Category).Select(n => n.FirstOrDefault()).ToList();
            foreach (var a in articles)
            {
                archiveListCategories.Add(new Category
                {
                    Name = a.Category,
                    Url = string.Empty
                });
            }

        }

        /// <summary>
        /// Funkcja pobierająca zapisane wiadomości (o danej kategorii) z bazy danych
        /// </summary>
        /// <param name="archiveListCategories">Lista kategorii dostępnych w bazie danych</param>
        /// <param name="category">kategoria</param>
        public void GetSavedNews(ObservableCollection<News> lineNews, ObservableCollection<Category> archiveListCategories, string category)
        {
            if (archiveListCategories.Any())
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

        /// <summary>
        /// Funkcja aktualizująca (dodaje lub usuwa) listę kategorii w bazie danych
        /// </summary>
        /// <param name="archiveListCategories">Lista kategorii dostępnych w bazie danych</param>
        /// <param name="selectedCategory">kategoria</param>
        /// <param name="toAdd">zmienna warunkująca dodawanie bądż usuwanie kategorii</param>
        public void UpdateArchiveCategory(ObservableCollection<Category> archiveListCategories, string selectedCategory, bool toAdd)
        {
            if (toAdd)
            {
                archiveListCategories.Add(new Category
                {
                    Name = selectedCategory,
                    Url = string.Empty
                });
            }
            else if (!toAdd)
            {
                foreach (var category in archiveListCategories.ToList())
                {
                    if (category.Name == selectedCategory)
                    {
                        archiveListCategories.Remove(category);
                        break;
                    }

                }
            }
        }



    }
}
