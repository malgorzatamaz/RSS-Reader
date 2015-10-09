using System.IO;
using System.Web;
using System.Windows;
using MahApps.Metro.Controls;

namespace RSS_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
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
    }

    public class News
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
    }
}
