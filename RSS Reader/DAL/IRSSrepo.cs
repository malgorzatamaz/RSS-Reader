using RSS_Reader.ViewModel.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Reader.DAL
{
    interface IRSSrepo
    {
        void AddSelectedArticle(ObservableCollection<Category> ArchiveListCategories, News news);
        void DeleteSelectedArticle(ObservableCollection<Category> ArchiveListCategories, string Id);
        void GetSavedNews(ObservableCollection<News> lineNews, ObservableCollection<Category> ArchiveListCategories, string category);
        void UpdateArchiveCategory(ObservableCollection<Category> ArchiveListCategories, string selectedCategory, bool toAdd);
    }
}
