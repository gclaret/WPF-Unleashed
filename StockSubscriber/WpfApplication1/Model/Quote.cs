using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model
{
    public enum Tick { 
        Up,
        Down,
        NoChange
    };

    public class Quote
    {
        private string _symbol;
        private double _price;
        private int _volume;
        private DateTime _date;
        private Tick _tick;

        public Quote() 
        {
        }

        public Quote(string symbol, double price, int volume, DateTime date, Tick tick)
        {
            _symbol = symbol;
            _price = price;
            _volume = volume;
            _date = date;
            _tick = tick;
        }

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public int Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public Tick Tick
        {
            get { return _tick; }
            set { _tick = value; }
        }
    }
}
