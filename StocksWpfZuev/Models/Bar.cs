using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksWpfZuev
{
    public class Bar
    {
        public string Symbol { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public int TotalVolume { get; set; }

        public override bool Equals(object? obj)
        {
            var minorBar = obj as Bar;

            if (minorBar != null)
            {
                return
                    minorBar.Date == this.Date &&
                    minorBar.Time == this.Time &&
                    minorBar.TotalVolume == this.TotalVolume &&
                    minorBar.Open == this.Open &&
                    minorBar.High == this.High &&
                    minorBar.Low == this.Low &&
                    minorBar.Close == this.Close &&
                    minorBar.Symbol == this.Symbol && 
                    minorBar.Description == this.Description ;
            }
            return false;
        }
    }
}
