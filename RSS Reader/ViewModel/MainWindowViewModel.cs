using System.Collections.ObjectModel;
using System.Windows.Input;
using PropertyChanged;
using RSS_Reader.ViewModel.Dto;

namespace RSS_Reader.ViewModel
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        private News _news;
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

    }




}
