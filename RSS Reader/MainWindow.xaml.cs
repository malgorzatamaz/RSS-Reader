using MahApps.Metro.Controls;

using RSS_Reader.ViewModel;
using System;


namespace RSS_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

    }


}
