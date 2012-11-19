using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApplication1.Model
{
    public class RandomQuoteSource : IQuoteSource
    {
        private const int TIMER_INTERVAL = 150;

        public event Action<Quote> QuoteArrived;

        private readonly List<string> _subscribedSymbols = new List<string>();
        private readonly Dictionary<string, IEnumerator<double>> _symbolToPriceEnumerator = new Dictionary<string, IEnumerator<double>>();

        private readonly Object _symbolsLock = new Object();
        private readonly Object _enumeratorsLock = new Object();

        private readonly Random rand = new Random();

        /// <summary>
        /// Initialize the service with the symbols that it should provide random quotes
        /// for, and then start a background thread to begin generating those quotes.
        /// </summary>
        /// <param name="symbolsToProvide"></param>
        public RandomQuoteSource()
        {
            Thread quoteGenerator = new Thread(new ThreadStart(GenerateQuotes));
            quoteGenerator.IsBackground = true;
            quoteGenerator.Priority = ThreadPriority.Normal;
            quoteGenerator.Start();

        }

        /// <summary>
        /// Initialize this provider with the given symbol by registering it
        /// in the list of subscribed symbols and creating an enumerator for
        /// that symbol.
        /// </summary>
        /// <param name="symbol"></param>
        public void Subscribe(string symbol)
        {

            if (symbol == null)
                throw new ArgumentException("Symbol was null");

            if (_subscribedSymbols.Contains(symbol))
                return;

            int multiplier = rand.Next(50, 100);
            double startVal = rand.NextDouble() * multiplier;

            IEnumerable<double> priceEnumerable = Fractal(3, 0.1, (double)startVal);
            IEnumerator<double> priceEnumerator = priceEnumerable.GetEnumerator();

            priceEnumerator.MoveNext();

            lock (_symbolsLock)
            {
                _subscribedSymbols.Add(symbol);
            }

            lock (_enumeratorsLock)
            {
                _symbolToPriceEnumerator.Add(symbol, priceEnumerator);
            }
        }

        /// <summary>
        /// Main work method of quote publishing thread.  Calls GenerateAndPublishQuotes()
        /// on each iteration, and then sleeps for the configured amount of time.
        /// </summary>
        private void GenerateQuotes()
        {
            while (true)
            {
                GenerateAndPublishQuotes();
                Thread.Sleep(TIMER_INTERVAL);
            }
        }

        /// <summary>
        /// Generate and publish a set of quotes.
        /// </summary>
        private void GenerateAndPublishQuotes()
        {
            // get indexes of symbols we'll pick - quickly acquire lock on _subscribedSymbols list
            // and make a clone of it by adding to a new list.  Then we can release that lock.
            List<string> clonedSubscribedSymbolsList = new List<string>();
            lock (_symbolsLock)
            {
                clonedSubscribedSymbolsList.AddRange(_subscribedSymbols);
            }

            List<string> symbolsToPick = new List<string>();
            for (int i = 0; i < clonedSubscribedSymbolsList.Count; i++)
            {
                int pick = rand.Next(0, 2);
                if (pick == 1)
                {
                    symbolsToPick.Add(clonedSubscribedSymbolsList[i]);
                }
            }

            // for each symbol, get it's last price and it's price enumerator from their dictionaries
            // and use them to generate a new quote for the symbol.
            foreach (var symbol in symbolsToPick)
            {
                IEnumerator<double> priceEnumerator;
                lock (_enumeratorsLock)  // lock here to make sure that a subscribed thread isn't adding an enumerator to the list while we are retrieving.
                {
                    priceEnumerator = _symbolToPriceEnumerator[symbol];
                }

                double lastPrice = priceEnumerator.Current;
                priceEnumerator.MoveNext();
                double newPrice = priceEnumerator.Current;

                int volume = rand.Next(1, 10) * 50;
                DateTime date = DateTime.Now;

                Tick tick;
                if (newPrice < lastPrice)
                    tick = Tick.Down;
                else if (newPrice > lastPrice)
                    tick = Tick.Up;
                else
                    tick = Tick.NoChange;

                Quote newQuote = new Quote(symbol, newPrice, volume, date, tick);
                QuoteArrived(newQuote);

            }
        }



        /// <summary>
        /// If there are subscribers to the QuoteArrived 
        /// event, then notify them.
        /// </summary>
        /// <param name="quote"></param>
        protected virtual void OnQuoteArrived(Quote quote)
        {
            if (QuoteArrived != null)
                QuoteArrived(quote);
        }


        private static IEnumerable<double> Fractal(int levels, double roughness, double scale)
        {
            Random rand = new Random();
            double start = rand.NextDouble() * scale;
            List<double> values = new List<double>(1 << levels);
            while (true)
            {
                double end = rand.NextDouble() * scale;
                values.Clear();
                Subdivide(values, rand, start, end, levels, roughness, 1.0);
                foreach (double x in values) yield return x;
                start = end;
            }
        }

        private static void Subdivide(List<double> values, Random rand, double start, double end, int levels, double roughness, double f)
        {
            if (levels == 0) values.Add(end);
            else
            {
                double mid = ((start + end) / 2) + rand.NextDouble() * f;
                Subdivide(values, rand, start, mid, levels - 1, roughness, f * roughness);
                Subdivide(values, rand, mid, end, levels - 1, roughness, f * roughness);
            }
        }

    }
}
