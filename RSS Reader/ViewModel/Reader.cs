using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web;
using System.Windows;
using System.Xml;
using RSS_Reader.ViewModel.Dto;

namespace RSS_Reader.ViewModel
{
    public class Reader
    {
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

        private void ParseDescription(News news)
        {
            StringWriter myWriter = new StringWriter();
            string description = string.Empty;
            string start = ResourceRss.StartDescription;
            HttpUtility.HtmlDecode(news.Description, myWriter);
            string decode = myWriter.ToString();
            if (news.UrlImage == null)
                start = ResourceRss.StartDescriptionWithoutImage;
            
            int index = decode.IndexOf(start);
            if (index > -1)
            {
                for (int i = index + start.Length; i < decode.Length; i++)
                {
                    if (i + ResourceRss.EndDescription.Length < decode.Length)
                    {
                        string tmp = decode.Substring(i, ResourceRss.EndDescription.Length);
                        if (tmp != ResourceRss.EndDescription)
                            description += decode[i];
                        else
                        {
                            news.Description = description[0] == '-' ? description.Substring(1) : description;
                            return;
                        }
                    }
                }
            }
        }

        public void ParseId(News news)
        {
            string ipLong = news.Id;
            int index = ipLong.IndexOf(ResourceRss.SearchId);
            if (index > -1 && ipLong.Length > index + ResourceRss.SearchId.Length + Convert.ToInt32(ResourceRss.LengthId))
                news.Id = ipLong.Substring(index + ResourceRss.SearchId.Length, Convert.ToInt32(ResourceRss.LengthId));
            else
                news.Id = string.Empty;
        }

        public void ReadBase(ObservableCollection<News> lineNews)
        {

        }

  

    }
}
