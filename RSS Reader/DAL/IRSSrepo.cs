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
        void AddSelectedArticle(News news);
        void DeleteSelectedArticle(string Id);
        void GetSavedNews(ObservableCollection<News> lineNews, string category);
    }
}
