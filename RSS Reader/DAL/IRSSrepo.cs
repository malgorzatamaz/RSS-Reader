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
    /// Interface reposytorium do obsługi zapisywania oraz odczytu z bazy danych
    /// </summary>
    interface IRSSrepo
    {
        /// <summary>
        /// Funkcja zapisująca wybraną wiadomość do bazy danych
        /// </summary>
        /// <param name="archiveListCategories">Lista kategorii dostępnych w bazie danych</param>
        /// <param name="news">Wiadomość do zapisania</param>
        void AddSelectedArticle(ObservableCollection<Category> ArchiveListCategories, News news);

        /// <summary>
        /// Funkcja kasująca wybraną wiadomość z bazy danych
        /// </summary>
        /// <param name="archiveListCategories">Lista kategorii dostępnych w bazie danych</param>
        /// <param name="id">Id wybranej wiadomości</param>
        void DeleteSelectedArticle(ObservableCollection<Category> ArchiveListCategories, string Id);

        /// <summary>
        /// Funkcja pobierająca zapisane wiadomości (o danej kategorii) z bazy danych
        /// </summary>
        /// <param name="archiveListCategories">Lista kategorii dostępnych w bazie danych</param>
        /// <param name="category">kategoria</param>
        void GetSavedNews(ObservableCollection<News> lineNews, ObservableCollection<Category> ArchiveListCategories, string category);

        /// <summary>
        /// Funkcja aktualizująca (dodaje lub usuwa) listę kategorii w bazie danych
        /// </summary>
        /// <param name="archiveListCategories">Lista kategorii dostępnych w bazie danych</param>
        /// <param name="selectedCategory">kategoria</param>
        /// <param name="toAdd">zmienna warunkująca dodawanie bądż usuwanie kategorii</param>
        void UpdateArchiveCategory(ObservableCollection<Category> ArchiveListCategories, string selectedCategory, bool toAdd);

        /// <summary>
        /// Funkcja pobierająca listę kategorii z bazy danych 
        /// </summary>
        /// <param name="archiveListCategories">Lista kategorii dostępnych w bazie danych</param>
        void GetListArchiveCategories(ObservableCollection<Category> ArchiveListCategories);
    }
}
