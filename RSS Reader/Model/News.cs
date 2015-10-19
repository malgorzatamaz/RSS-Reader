using PropertyChanged;

namespace RSS_Reader.Model
{
    /// <summary>
    /// Klasa definiująca wiadomość
    /// </summary>
    [ImplementPropertyChanged]
    public class News
    {
        /// <summary>
        /// Tytuł
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Streszczenie
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Numer Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Link do zdjęcia
        /// </summary>
        public string UrlImage { get; set; }

        /// <summary>
        /// Link do strony z wiadomością
        /// </summary>
        public string UrlNews { get; set; }

        /// <summary>
        /// Kategoria
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Zdjęcie zapisanie jako tablica bajtów
        /// </summary>
        public byte[] Photo { get; set; }
      
    }
}
