using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows.Input;
using HtmlAgilityPack;

using PropertyChanged;
using RSS_Reader.ViewModel.Dto;
using RSS_Reader.DAL;


namespace RSS_Reader.ViewModel
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        private int _selectedIndexListBoxNews;
        private int _selectedIndexCategories;
        private bool _isSelectedListBox;
        private int _selectedIndexTab;
        public ObservableCollection<News> LineNews { get; set; }
        public ObservableCollection<Category> ListCategories { get; set; }
        public News News { get; set; }
        public Reader Reader { get; set; }

        public int SelectedIndexTab
        {
            get { return _selectedIndexTab; }
            set
            {
                _selectedIndexTab = value;
                ReadNews();
            }
        }

        public bool IsSelectedListBox
        {
            get { return _isSelectedListBox; }
            set
            {
                _isSelectedListBox = value; 
                
            }
        }

        public ICommand OpenWebsiteCommand { get; set; }
        public ICommand SaveAllCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public int SelectedIndexCategories
        {
            get { return _selectedIndexCategories; }
            set 
            { 
                _selectedIndexCategories = value;
                ReadNews();
            }
        }

        private void ReadNews()
        {
            if (SelectedIndexTab == 0)
            {
                LineNews = new ObservableCollection<News>();
                Reader.ParseXml(LineNews, ListCategories[SelectedIndexCategories]);
                SelectedIndexListBoxNews = 0;
                ShowDescription();
            }
            else
            {
                SelectedIndexListBoxNews = 0;
                SelectedIndexCategories = 0;
                LineNews = new ObservableCollection<News>();
                Reader.ReadBase(LineNews, ListCategories[SelectedIndexCategories]);
                ShowDescription();
            }
           
        }

        public int SelectedIndexListBoxNews
        {
            get { return _selectedIndexListBoxNews; }
            set
            {
                _selectedIndexListBoxNews = value;
                ShowDescription();
            }
        }

        public MainWindowViewModel()
        {
            ListCategories = new ObservableCollection<Category>();
            News = new News();
            Reader = new Reader();
            OpenWebsiteCommand = new RelayCommand(OpenWebsite, (m) => true);
            SaveAllCommand = new RelayCommand(SaveAll, (m) => true);
            SaveCommand = new RelayCommand(Save, (m) => true);
            DeleteCommand = new RelayCommand(Delete, (m) => true);
            GetCategories();
            SelectedIndexTab = 0;
            SelectedIndexCategories = 0;
            ReadNews();        
        }

        private void Delete(object obj)
        {
            int index = SelectedIndexListBoxNews;
        }

        private void Save(object obj)
        {
            News newsSave=new News();
            int index = SelectedIndexListBoxNews;
            if (obj is Int32)
                index = (int)obj;
            newsSave.Title = LineNews[index].Title;
            newsSave.Description = LineNews[index].Description;
            newsSave.Id = LineNews[index].Id;
            newsSave.UrlImage = LineNews[index].UrlImage;
            newsSave.Category = LineNews[index].Category;
            newsSave.UrlNews = LineNews[index].UrlNews;
            newsSave.Date = LineNews[index].Date;
            newsSave.Photo = GetImageAsByte(newsSave.UrlImage);

            RSSrepo rssRepo = new RSSrepo();
            rssRepo.AddSelectedArticle(newsSave);
        }

        private void SaveAll(object obj)
        {
            for (int i = 0; i < LineNews.Count; i++)
            {
                Save(i);
            }         
        }

        private void OpenWebsite(object obj)
        {
            if (string.IsNullOrEmpty(LineNews[SelectedIndexListBoxNews].UrlNews) == false)
            {
                System.Diagnostics.Process.Start(LineNews[SelectedIndexListBoxNews].UrlNews);
            }
        }

        private void ShowDescription()
        {
            if (LineNews.Count > 0 && SelectedIndexListBoxNews>-1)
            {
                News.Title = LineNews[SelectedIndexListBoxNews].Title;
                News.Date = LineNews[SelectedIndexListBoxNews].Date;
                News.Description = LineNews[SelectedIndexListBoxNews].Description;
                News.UrlNews = LineNews[SelectedIndexListBoxNews].UrlNews;
            }
            
        }

        private void GetCategories()
        {
            var listTitles = new List<string>();
            var listUrls = new List<string>();

            WebClient webClient = new WebClient();
            string page = webClient.DownloadString(ResourceRss.UrlWebsite);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);
            var titles = from node in doc.DocumentNode.SelectNodes("//td[@class = 'tdPolecane01']")
                         select node.InnerText.ToString();


            listTitles = titles.AsQueryable().ToList();

            var urls = from node in doc.DocumentNode.SelectNodes("//td[@class = 'tdPolecane02']")
                       select node.InnerText.ToString();

            listUrls = urls.AsQueryable().ToList();

            for (int i = 0; i < listTitles.Count(); i++)
            {
                ListCategories.Add(new Category { Name = listTitles[i], Url = listUrls[i] });
            }

        }

        private byte[] GetImageAsByte(string LinkImage)
        {
            if (!string.IsNullOrWhiteSpace(LinkImage))
            {
                WebClient webClient = new WebClient();
                return News.Photo = webClient.DownloadData(LinkImage);
            }
            else
            {
                byte[] c = new byte[] { 0 };
                return c;
            }
        }
    }
}
