using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows.Input;
using HtmlAgilityPack;
using PropertyChanged;
using RSS_Reader.ViewModel.Dto;

namespace RSS_Reader.ViewModel
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        private int _selectedIndexListBoxNews;
        private int _selectedIndexCategories;
        public ObservableCollection<News> LineNews { get; set; }
        public ObservableCollection<Category> ListCategories { get; set; }
        public News News { get; set; }
        public Reader Reader { get; set; }
        public ICommand OpenWebsiteCommand { get; set; }

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
            LineNews = new ObservableCollection<News>();
            Reader.ParseXml(LineNews, ListCategories[SelectedIndexCategories].Url);
            SelectedIndexListBoxNews = 0;
            ShowDescription();
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
            GetCategories();

            SelectedIndexCategories = 0;
            ReadNews();        
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
            if (LineNews.Count > 0)
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
    }
}
