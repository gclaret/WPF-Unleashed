using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication1.Model;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Source from which we get our Quote Data
        private readonly RandomQuoteSource _quoteSource = new RandomQuoteSource();

        // All the Quotes that we are subscribed to
        private ObservableCollection<Quote> _quotes = new ObservableCollection<Quote>();

        public MainWindow()
        {
            InitializeComponent();
            _quoteSource.QuoteArrived += new Action<Quote>(_quoteSource_QuoteArrived);
            this.DataContext = _quotes;
        }

        void _quoteSource_QuoteArrived(Quote obj)
        {
            Action dispatchAction = () => _quotes.Insert(0, obj);
            this.Dispatcher.BeginInvoke(dispatchAction);
        }

        // Not a good idea to have all this logic in the code-behind, but still functional
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string symbol = symbolTxt.Text;
            _quoteSource.Subscribe(symbol);
            lastTxt.Content = symbol;
        }
    }
}
