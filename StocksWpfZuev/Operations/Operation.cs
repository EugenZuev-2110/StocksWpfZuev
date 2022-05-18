using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksWpfZuev
{
    public class Operation : OperationBase
    {
        new public Dictionary<DateTime, Bar> MinMaxOrder(List<Bar> document)
        {
            try
            {
                var pairs = base.MinMaxOrder(document);
                var minMaxBars = GetMinMaxBars(pairs);
                return minMaxBars;
            }
            catch 
            {
                return null;
            } 
        }

        /// <summary>
        /// Find min and max values of every days
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns>
        /// Dictionary of date as keys and bar with min and max as values
        /// </returns>
        public Dictionary<DateTime, Bar> GetMinMaxBars(Dictionary<DateTime, List<Bar>> pairs)
        {
            Dictionary<DateTime, Bar> bars = new Dictionary<DateTime, Bar>();

            try
            {
                foreach (var pair in pairs)
                {
                    DateTime date = pair.Key;
                    var min = pair.Value[0].Low;
                    var max = pair.Value[0].High;
                    int minCount = 0;
                    int maxCount = 0;

                    for (var bar = 0; bar < pair.Value.Count; bar++)
                    {
                        if (pair.Value[bar].Low < min)
                        {
                            min = pair.Value[bar].Low;
                            minCount = bar;
                        }

                        if (pair.Value[bar].High > max)
                        {
                            max = pair.Value[bar].High;
                            maxCount = bar;
                        }

                    }

                    var br = new Bar
                    {
                        Date = pair.Value[0].Date,
                        High = pair.Value[maxCount].High,
                        Low = pair.Value[minCount].Low,
                        Symbol = pair.Value[0].Symbol,
                        Description = pair.Value[0].Description,
                        Time = pair.Value[0].Time,
                        Open = pair.Value[0].Open,
                        Close = pair.Value[pair.Value.Count - 1].Close,
                        TotalVolume = pair.Value[0].TotalVolume
                    };

                    bars.Add(date, br);
                }
            }
            catch { }

            return bars;
        }
    }
}
