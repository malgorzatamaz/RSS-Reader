﻿using System;
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
using RSS_Reader.ViewModel;
using RSS_Reader.ViewModel.Dto;

namespace RSS_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public List<Category> Categories { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            Categories = GetCategories();
            CbxCategory.DataContext = Categories;
            DataContext = new MainWindowViewModel();
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
