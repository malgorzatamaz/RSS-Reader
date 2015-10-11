using System.Collections.ObjectModel;
using System.Windows.Input;
using PropertyChanged;
using RSS_Reader.ViewModel.Dto;

namespace RSS_Reader.ViewModel
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        private int _selectedIndexListBoxNews;
        public ObservableCollection<News> LineNews { get; set; }
        public News News { get; set; }
        public Reader Reader { get; set; }
        public ICommand OpenWebsiteCommand { get; set; }

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
            LineNews = new ObservableCollection<News>();
            News = new News();
            Reader = new Reader();
            OpenWebsiteCommand=new RelayCommand(OpenWebsite, (m)=>true);
            Reader.ParseXml(LineNews);
            SelectedIndexListBoxNews = 0;
            ShowDescription();
        }

        private void OpenWebsite(object obj)
        {
            if (string.IsNullOrEmpty(LineNews[SelectedIndexListBoxNews].LinkNews) == false)
            {
                System.Diagnostics.Process.Start(LineNews[SelectedIndexListBoxNews].LinkNews);
            }
        }

        private void ShowDescription()
        {
            News.Title = LineNews[SelectedIndexListBoxNews].Title;
            News.Date = LineNews[SelectedIndexListBoxNews].Date;
            News.Description = LineNews[SelectedIndexListBoxNews].Description;
            News.LinkNews = LineNews[SelectedIndexListBoxNews].LinkNews;
        }

    }




}
