using System.Collections.ObjectModel;
using System.Windows.Input;
using PropertyChanged;
using RSS_Reader.ViewModel.Dto;
using RSS_Reader.DAL;
using System.Net;

namespace RSS_Reader.ViewModel
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        //private News _news;
        private int _selectedIndexListBoxNews;
        public ObservableCollection<News> LineNews { get; set; }
        public News News { get; set; }
        public Reader Reader { get; set; }

        public int SelectedIndexListBoxNews
        {
            get { return _selectedIndexListBoxNews; }
            set
            {
                _selectedIndexListBoxNews = value;
                Execute(null);
            }
        }

        public MainWindowViewModel()
        {
            LineNews = new ObservableCollection<News>();
            News = new News();
            Reader = new Reader();
            Reader.ParseXml(LineNews);
            SelectedIndexListBoxNews = 0;
            Execute(null);
        }

        private void Execute(object obj)
        {
            News.Title = LineNews[SelectedIndexListBoxNews].Title;
            News.Date = LineNews[SelectedIndexListBoxNews].Date;
            News.Description = LineNews[SelectedIndexListBoxNews].Description;
            News.LinkNews = LineNews[SelectedIndexListBoxNews].LinkNews;
        }

        public ICommand SaveSelectedToBaseCommand
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    SaveSelectedToBase(News);
                }, (m) => true);
            }
            
        }
        private void SaveSelectedToBase(News news)
        {
            Reader = new Reader();
            //TO DO get category
            News.LinkImage = LineNews[SelectedIndexListBoxNews].LinkImage;
            News.Photo = DownloadImage(News.LinkImage);
            News.Id = LineNews[SelectedIndexListBoxNews].Id;
            //Reader.ParseId(News);

            RSSrepo rssRepo = new RSSrepo();
            rssRepo.AddSelectedArticle(News);
        }

        private byte[] DownloadImage(string LinkImage)
        {
            WebClient webClient = new WebClient();
            return News.Photo = webClient.DownloadData(LinkImage);
        }


        private void SaveAllFromCategoryToBase()
        {
            RSSrepo rssRepo = new RSSrepo();
           // rssRepo.AddAllNews(News);
        }


        //private void ParseArticleDetails()
        //{
        //    string selectedCategory = CbxCategory.Text;
        //    string categoryUrl = Categories.Where(n => n.Name == selectedCategory).Select(n => n.Url).FirstOrDefault();

        //    _selectedCategory = new Category();
        //    _selectedCategory.Name = selectedCategory;
        //    _selectedCategory.Url = categoryUrl;

        //    XmlDocument rssXmlDoc = new XmlDocument();
        //    rssXmlDoc.Load(categoryUrl);
        //    XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

        //    string photoUrl = string.Empty;
        //    foreach (XmlNode rssNode in rssNodes)
        //    {
        //        XmlNode rssSubNode = rssNode.SelectSingleNode("title");
        //        if (rssSubNode.InnerText == TxbTitle.Text)
        //        {
        //            rssSubNode = rssNode.SelectSingleNode("link");
        //            _newsUrl = rssSubNode != null ? rssSubNode.InnerText : "";

        //            rssSubNode = rssNode.SelectSingleNode("enclosure/@url");
        //            photoUrl = rssSubNode != null ? rssSubNode.InnerText : "";

        //            break;
        //        }

        //    }

        //}

    }
}
