using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfApplication1.Model;

namespace StockSubscriber.ViewModel
{
    public class MainViewVm : DependencyObject
    {
        public ObservableCollection<Quote> Quotes { get; set; }
        private IQuoteSource _source;
        private readonly Dispatcher _currentDispatcher; 

        public string Symbol { get; set; }
        public string LastSymbol
        {
            get { return (string)GetValue(LastSymbolProperty); }
            set { SetValue(LastSymbolProperty, value); }
        }

        public static readonly DependencyProperty LastSymbolProperty = DependencyProperty.Register("LastSymbol", typeof(string), typeof(MainViewVm), new UIPropertyMetadata(""));


        public MainViewVm(IQuoteSource source)
        {
            Quotes = new ObservableCollection<Quote>();
            _source = source;
            _currentDispatcher = Dispatcher.CurrentDispatcher;
            _source.QuoteArrived += new Action<Quote>(_source_QuoteArrived);

        }

        void _source_QuoteArrived(Quote obj)
        {
            Action dispatchAction = () => Quotes.Insert(0, obj);
            _currentDispatcher.BeginInvoke(dispatchAction);
        }

        public void Subscribe()
        {
            _source.Subscribe(Symbol);
            LastSymbol = Symbol;
        }

    }
}
