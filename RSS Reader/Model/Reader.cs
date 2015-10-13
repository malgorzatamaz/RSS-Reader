using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web;
using System.Windows;
using System.Xml;
using RSS_Reader.DAL;

namespace RSS_Reader.Model
{
    /// <summary>
    /// Klasa służąca odczytu danych wiadomości z plików xml i html
    /// </summary>
    public class Reader
    {
        /// <summary>
        /// Wybiera stronę na podstawie adresu url i zapisuje wszystkie wiadomości w kolekcji
        /// </summary>
        /// <param name="lineNews">Lista wiadomości</param>
        /// <param name="category">Kategoria źródłowa do odczytu</param>
        public void ParseXml(ObservableCollection<News> lineNews, Category category)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(category.Url))
                {
                    var formatter = new Rss20FeedFormatter();
                    formatter.ReadFrom(reader);
                    foreach (var item in formatter.Feed.Items)
                    {
                        lineNews.Add(new News
                        {
                            Title = item.Title.Text,
                            Date = item.PublishDate.DateTime.ToString(),
                            UrlNews = item.Links.First().Uri.ToString(),
                            Description = item.Summary.Text,
                            Category = category.Name,
                            Id = item.Id
                        });

                        if (item.Links.Count > 1 && item.Links.Any(n => n.Uri.ToString().Contains(".jpg")))
                            lineNews.Last().UrlImage = item.Links[1].Uri.ToString();

                        ParseId(lineNews.Last());
                        ParseDescription(lineNews.Last());
                    }
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message, "Syndication Reader");
            }
        }


        /// <summary>
        /// Dekoduje z opisu ze znacznikami HTML do normalnego opisu 
        /// </summary>
        /// <param name="news">Wiadomość do dekodowania</param>
        private void ParseDescription(News news)
        {
            StringWriter myWriter = new StringWriter();
            string description = string.Empty;
            string start = string.Empty;
            string end = ResourceRss.EndDescription;
            int index = 0;
            HttpUtility.HtmlDecode(news.Description, myWriter);
            string decode = myWriter.ToString();

            if (decode[0] == Convert.ToChar(ResourceRss.OneSymbol) && decode[1] == Convert.ToChar(ResourceRss.TwoSymbol))
            {
                start = ResourceRss.StartDescription;
                if (news.UrlImage == null && decode.Contains(ResourceRss.CheckedTwice) == false)
                    start = ResourceRss.StartDescriptionWithoutImage;
                index = decode.IndexOf(start);
            }

            if (!decode.Contains(end))
                end = ResourceRss.EndDescriptionBr;

            if (index > -1)
            {
                for (int i = index + start.Length; i < decode.Length; i++)
                {
                    if (i + end.Length < decode.Length)
                    {
                        string tmp = decode.Substring(i, end.Length);
                        if (tmp != end)
                            description += decode[i];
                        else
                        {
                            if (description != string.Empty)
                                news.Description = description[0] == '-' ? description.Substring(1) : description;
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Dekoduje z id ze znacznikami HTML do normalnego id 
        /// </summary>
        /// <param name="news">Wiadomość do dekodowania</param>
        public void ParseId(News news)
        {
            string ipLong = news.Id;
            int index = ipLong.IndexOf(ResourceRss.SearchId);
            news.Id = ipLong.Substring(index + ResourceRss.SearchId.Length, Convert.ToInt32(ResourceRss.LengthId));      
        }

        /// <summary>
        /// Odczytuje dane z bazy na podstawie kategorii i kopiuje do podanej kolekcji
        /// </summary>
        /// <param name="lineNews">Kolekcja do wczytania wiadomości</param>
        /// <param name="ArchiveListCategories">Kolkcja do wczytania wszystkich kategorii w archiwum</param>
        /// <param name="category">Docelowa kategoria</param>
        public void ReadBase(ObservableCollection<News> lineNews, ObservableCollection<Category> ArchiveListCategories, Category category)
        {
            IRSSrepo rssRepo = new RSSrepo();
            rssRepo.GetSavedNews(lineNews, ArchiveListCategories, category.Name);
        }
    }
}
