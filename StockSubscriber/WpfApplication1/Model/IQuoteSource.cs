using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model
{
    public interface IQuoteSource
    {
        void Subscribe(string symbol);

        // Fired each time a Quote arrives
        event Action<Quote> QuoteArrived;

    }
}
