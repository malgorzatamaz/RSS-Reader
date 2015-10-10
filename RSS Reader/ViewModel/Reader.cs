using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        public void ParseXml(ObservableCollection<News> lineNews)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(@"http://interia.pl.feedsportal.com/c/34004/f/618045/index.rss"))
                {
                    var formatter = new Rss20FeedFormatter();
                    formatter.ReadFrom(reader);
                    foreach (var item in formatter.Feed.Items)
                    {
                        lineNews.Add(new News
                        {
                            Title = item.Title.Text,
                            Date = item.PublishDate.DateTime.ToString(),
                            LinkNews = item.Links.First().Uri.ToString(),
                            Description = item.Summary.Text,
                            Id = item.Id
                        });

                        if (item.Links.Count > 1 && item.Links.Any(n => n.Uri.ToString().Contains(".jpg")))
                            lineNews.Last().LinkImage = item.Links[1].Uri.ToString();
                        foreach (var category in item.Categories)
                        {
                            lineNews.Last().Category = category.Name;
                        }
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
            HttpUtility.HtmlDecode(news.Description, myWriter);
            string decode = myWriter.ToString();
            string searchId = "</a>";
            string end = "</p>";
            int index = decode.IndexOf(searchId);
            if (index > -1)
            {
                for (int i = index + searchId.Length; i < decode.Length; i++)
                {
                    if (i + end.Length < decode.Length)
                    {
                        string tmp = decode.Substring(i, end.Length);
                        if (tmp != end)
                            description += decode[i];
                        else
                        {
                            if (description[0] == '-')
                            {
                                news.Description = description.Substring(1);
                            }
                            else
                            {
                                news.Description = description;
                            }
                            return;
                        }
                    }
                }
            }
        }

        private void ParseId(News news)
        {
            string ipLong = news.Id;
            string searchId = ",nId,";
            int index = ipLong.IndexOf(searchId);
            if (index > -1 && ipLong.Length > index + searchId.Length + 7)
                news.Id = ipLong.Substring(index + searchId.Length, 7);
            else
                news.Id = string.Empty;
        }

        public void ReadBase(ObservableCollection<News> lineNews)
        {

        }
    }
}
