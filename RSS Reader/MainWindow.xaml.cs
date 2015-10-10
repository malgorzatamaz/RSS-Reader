using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using HtmlAgilityPack;
using MahApps.Metro.Controls;
using RSS_Reader.Models;
using RSS_Reader.DAL;
using System.Xml;
using System.Text;
using System.Windows.Media.Imaging;

namespace RSS_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private string _newsUrl { get; set; }
        private News _news { get; set; }
        private Category _selectedCategory { get; set; }
        private byte[] _newsPhoto { get; set; }

        public List<Category> Categories { get; set; }

        public MainWindow()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
            InitializeComponent();
            Categories = GetCategories();
            CbxCategory.DataContext = Categories;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            News entry = new News();
            entry.Title = TxbTitle.Text;
            entry.Date = TxbDate.Text;
            entry.Description = TxbDescription.Text;
            StringWriter myWriter = new StringWriter();
            HttpUtility.HtmlDecode(entry.Description, myWriter);
            string decode = myWriter.ToString(); // potrzebny nam tekst mamy w <p>...</p>         

        }
        private List<Category> GetCategories()
        {
            List<Category> list = new List<Category>();
            var listTitles = new List<string>();
            var listUrls = new List<string>();

            WebClient webClient = new WebClient();
            string page = webClient.DownloadString("http://rss.interia.pl/");
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);

            var titles = from node in doc.DocumentNode.SelectNodes("//td[@class = 'tdPolecane01']")
                         select node.InnerText.ToString();

            listTitles = titles.AsQueryable().ToList();

            var urls = from node in doc.DocumentNode.SelectNodes("//td[@class = 'tdPolecane02']")
                       select node.InnerText.ToString();

            listUrls = urls.AsQueryable().ToList();

            for (int i = 0; i < listTitles.Count(); i++)
            {
                list.Add(new Category { Name = listTitles[i], Url = listUrls[i] });
            }
            return list;
        }

        private void CbxCategory_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var provider = (XmlDataProvider)this.Resources["rssData"];
            provider.Source = new Uri(Categories[CbxCategory.SelectedIndex].Url);
        }

        private void BtnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            RSSrepo rssRepo = new RSSrepo();

        }

        private void BtnSaveSelected_Click(object sender, RoutedEventArgs e)
        {
            GetSelectedArticleDetails();

            RSSrepo rssRepo = new RSSrepo();
            rssRepo.AddSelectedArticle(_news);

        }

        private void GetSelectedArticleDetails()
        {
            ParseArticleDetails();
            _news = new News();
            _news.Date = TxbDate.Text;
            _news.Description = TxbDescription.Text; //temporary; use description created from convert function (made by Ania)
            _news.Title = TxbTitle.Text;
            _news.Url = _newsUrl;
            _news.Photo = _newsPhoto;
            _news.CategoryName = _selectedCategory.Name;

        }

        private void ParseArticleDetails()
        {
            string selectedCategory = CbxCategory.Text;
            string categoryUrl = Categories.Where(n => n.Name == selectedCategory).Select(n => n.Url).FirstOrDefault();

            _selectedCategory = new Category();
            _selectedCategory.Name = selectedCategory;
            _selectedCategory.Url = categoryUrl;

            XmlDocument rssXmlDoc = new XmlDocument();
            rssXmlDoc.Load(categoryUrl);
            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

            string photoUrl = string.Empty;
            foreach (XmlNode rssNode in rssNodes)
            {
                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                if (rssSubNode.InnerText == TxbTitle.Text)
                {
                    rssSubNode = rssNode.SelectSingleNode("link");
                    _newsUrl = rssSubNode != null ? rssSubNode.InnerText : "";

                    rssSubNode = rssNode.SelectSingleNode("enclosure/@url");
                    photoUrl = rssSubNode != null ? rssSubNode.InnerText : "";

                    break;
                }

            }

            if (!string.IsNullOrEmpty(photoUrl))
            {
                WebClient webClient = new WebClient();
                _newsPhoto = webClient.DownloadData(photoUrl);
            }


        }

        private void ParseArticleID()
        {
            //TO DO create a unique id umber in order to check if the same article already exists in the database
        }
    }


}
