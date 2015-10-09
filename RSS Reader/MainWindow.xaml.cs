using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Windows;
using HtmlAgilityPack;
using MahApps.Metro.Controls;
using RSS_Reader.Models;

namespace RSS_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public List<Category> Categories { get; set; }
        public string CategoryPath { get; set; }

        public MainWindow()
        {
            Categories = new List<Category>();
            Categories.Add(new Category { Name = "dadsd", Url = "www.sssa.pl" });
            Categories.Add(new Category { Name = "ada", Url = "www.dads.pl" });
            
            InitializeComponent();
            CbxCanals.DataContext = Categories;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            News entry=new News();
            entry.Title = TxbTitle.Text;
            entry.Date = TxbDate.Text;
            entry.Description = TxbDescription.Text;
            StringWriter myWriter = new StringWriter();
            HttpUtility.HtmlDecode(entry.Description, myWriter);
            string decode = myWriter.ToString(); // potrzebny nam tekst mamy w <p>...</p>         
        }
        private void GetCategories()
        {
            WebClient webClient = new WebClient();
            string page = webClient.DownloadString("http://rss.interia.pl/");
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);
            
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {   
                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    Categories.Add( new Category{ Name = row.SelectNodes("td")[0].ToString(), Url = row.SelectNodes("td")[1].ToString()});
                }
            }
        }

    }

   
}
