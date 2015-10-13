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
    /// Primary reader class for decode news from Xml
    /// </summary>
    public class Reader
    {
        /// <summary>
        /// Select page by category url and writes all news to observable collection 
        /// </summary>
        /// <param name="lineNews">Bind-able colection for news</param>
        /// <param name="category">Source category for reading</param>
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
        /// Decoding from HTML description to normal text description 
        /// </summary>
        /// <param name="news">Target news for decoding description</param>
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
        /// Decoding from HTML id to normal text id. 
        /// </summary>
        /// <param name="news">Target news for decoding id.</param>
        public void ParseId(News news)
        {
            string ipLong = news.Id;
            int index = ipLong.IndexOf(ResourceRss.SearchId);
            news.Id = ipLong.Substring(index + ResourceRss.SearchId.Length, Convert.ToInt32(ResourceRss.LengthId));      
        }

        /// <summary>
        /// Read data from database by category and copy to observable collections
        /// </summary>
        /// <param name="lineNews">Bind-able colection for news</param>
        /// <param name="ArchiveListCategories">Bind-able colection for categories</param>
        /// <param name="category">Source category</param>
        public void ReadBase(ObservableCollection<News> lineNews, ObservableCollection<Category> ArchiveListCategories, Category category)
        {
            IRSSrepo rssRepo = new RSSrepo();
            rssRepo.GetSavedNews(lineNews, ArchiveListCategories, category.Name);
        }
    }
}
