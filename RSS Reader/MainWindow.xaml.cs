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
using RSS_Reader.DAL;
using System.Xml;
using System.Text;
using System.Windows.Media.Imaging;
using RSS_Reader.ViewModel;
using RSS_Reader.ViewModel.Dto;


namespace RSS_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //private string _newsUrl { get; set; }
        //private News _news { get; set; }
        //private Category _selectedCategory { get; set; }
        //private byte[] _newsPhoto { get; set; }

        public List<Category> Categories { get; set; }

        public MainWindow()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
            InitializeComponent();
            Categories = GetCategories();
            CbxCategory.DataContext = Categories;
            DataContext = new MainWindowViewModel();
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
            //provider.Source = new Uri(Categories[CbxCategory.SelectedIndex].Url);
        }

        
    }


}
