using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StocksWpfZuev
{
    public abstract class OperationBase
    {
        

        /// <summary>
        /// Find min and max bars in every hour
        /// </summary>
        /// <param name="bars"></param>
        /// <returns>
        /// List of min and max bars by every hour
        /// </returns>
        public List<Bar> GetMinMaxPerHour(Dictionary<int, List<Bar>> bars)
        {
            var minMaxList = new List<Bar>();
            foreach (var bar in bars)
            {
                var minHourValue = bar.Value[0].Low;
                var maxHourValue = bar.Value[0].High;
                var open = bar.Value[0].Open;
                var close = bar.Value[bar.Value.Count - 1].Close;
                var time = new TimeSpan(bar.Key, 0, 0);
                var symbol = bar.Value[0].Symbol;
                var date = bar.Value[0].Date;
                var description = bar.Value[0].Description;
                var totalVolume = bar.Value[0].TotalVolume;

                foreach (var hour in bar.Value)
                {
                    if (hour.Low < minHourValue)
                        minHourValue = hour.Low;
                    if (hour.High > maxHourValue)
                        maxHourValue = hour.High;
                }
                var newBar = new Bar
                {
                    Low = minHourValue,
                    High = maxHourValue,
                    Symbol = symbol,
                    Date = date,
                    TotalVolume = totalVolume,
                    Description = description,
                    Open = open,
                    Close = close,
                    Time = new TimeSpan(bar.Key, 0, 0)
                };
                minMaxList.Add(newBar);
            }
            return minMaxList;
        }

        /// <summary>
        /// Parsing of document by all exist date
        /// </summary>
        /// <param name="document"></param>
        /// <returns>
        /// Dictonary of parsing document by every date
        /// </returns>
        public Dictionary<DateTime, List<Bar>> DocumentDateParser(List<Bar> document)
        {
            Dictionary<DateTime, List<Bar>> documentDateParser = new Dictionary<DateTime, List<Bar>>();
            List<Bar> bars;
            DateTime date;

            for (int i = 0; i < document.Count; i++)
            {
                date = document[i].Date;
                bars = new List<Bar>();
                for (; i < document.Count && document[i].Date == date; i++)
                {
                    bars.Add(document[i]);
                }
                documentDateParser.Add(date, bars);
                i--;
            }

            return documentDateParser;
        }


        /// <summary>
        /// Compose a dictonary of dates as keys and sorted by list of every hour every day bar's list as values
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns>
        /// Dictionary of Date as key and sorted by list of every hour every day bar's list as values
        /// </returns>
        public Dictionary<DateTime, Dictionary<int, List<Bar>>> ComposeHourBarList(Dictionary<DateTime, List<Bar>> dictionary)
        {
            Dictionary<DateTime, Dictionary<int, List<Bar>>> list = new Dictionary<DateTime, Dictionary<int, List<Bar>>>();
            Dictionary<int, List<Bar>> hourBarList = new Dictionary<int, List<Bar>>();

            foreach (var bar in dictionary)
            {
                hourBarList = GetComposeHourBar(bar.Value);
                list.Add(bar.Key, hourBarList);
            }

            return list;
        }

        /// <summary>
        /// Compose all minutes bars of a single day to dictionary when hour represent a keys and minute bar's list represent a values
        /// </summary>
        /// <param name="minuteDayBars"></param>
        /// <returns>
        /// Dictionary of hours as a keys and minute bar's list as a values
        /// </returns>
        public Dictionary<int, List<Bar>> GetComposeHourBar(List<Bar> minuteDayBars)
        {
            var dictionaryBars = new Dictionary<int, List<Bar>>();
            List<Bar> bars;

            for (int i = 0; i < minuteDayBars.Count; i++)
            {
                var currentHour = minuteDayBars[i].Time.Hours;
                bars = new List<Bar>();

                for (; i < minuteDayBars.Count && minuteDayBars[i].Time.Hours == currentHour; i++)
                {
                    bars.Add(minuteDayBars[i]);
                }
                dictionaryBars.Add(currentHour, bars);
                i--;
            }

            return dictionaryBars;
        }

        /// <summary>
        /// Find low and high bar's values per hour of every day
        /// </summary>
        /// <param name="bars"></param>
        /// <returns>
        /// Dictionary of Date as a keys and low and high value's per hour list of every day
        /// </returns>
        public Dictionary<DateTime, List<Bar>> GetMinMaxPerDay(Dictionary<DateTime, Dictionary<int, List<Bar>>> bars)
        {
            Dictionary<DateTime, List<Bar>> listBars = new Dictionary<DateTime, List<Bar>>();

            foreach (var b in bars)
            {
                var un = GetMinMaxPerHour(b.Value);
                listBars.Add(b.Key, un);
            }

            return listBars;
        }

       

        /// <summary>
        /// Start a process of sorting and searching bars
        /// </summary>
        /// <param name="document"></param>
        /// <returns>
        /// Dictionary of date as a keys and list of bars as a values
        /// </returns>
        public Dictionary<DateTime, List<Bar>> MinMaxOrder(List<Bar> document)
        {
            var parsedDocument = DocumentDateParser(document);//Составляев словарь из дат и поминутного списка баров всего дня этих дат
            var units = ComposeHourBarList(parsedDocument);//Составляев словарь из дат и поминутного списка каждого часа этих дат
            var pairs = GetMinMaxPerDay(units);//Производит поиск минимального и максимального значений каждого часа каждого дня
            return pairs;
        }

        /// <summary>
        /// Find unique bars in two date's and list of bars dictionaries
        /// </summary>
        /// <param name="outerBarsMajor"></param>
        /// <param name="outerBarsMinor"></param>
        /// <returns>
        /// List of unique bars
        /// </returns>
        public List<Bar> StringFinderGeneral(Dictionary<DateTime, List<Bar>> outerBarsMajor, Dictionary<DateTime, List<Bar>> outerBarsMinor)
        {
            var bars = new List<Bar>();
            object locker = new object();

            foreach (var majorPair in outerBarsMajor)
            {
                var date = majorPair.Key;
                foreach(var minorPair in outerBarsMinor)
                {
                    if(minorPair.Key == date)
                    {
                        var task = new Task(() =>
                        {
                            var searchedBars = StringFinderUnique(majorPair.Value, minorPair.Value);
                            if (searchedBars.Count > 0)
                            {
                                lock (locker)
                                {
                                    bars.AddRange(searchedBars);
                                }
                            }     
                        });
                        task.Start();
                        task.Wait();
                        break;
                    }
                }
            }

            return bars;
        }

        /// <summary>
        /// Find unique bars in first list
        /// </summary>
        /// <param name="majorBars"></param>
        /// <param name="minorBars"></param>
        /// <returns>
        /// Unique bars in first list
        /// </returns>
        public List<Bar> StringFinderUnique(List<Bar> majorBars, List<Bar> minorBars)
        {
            List<Bar> bars = new List<Bar>();
            bool isExist;

            for (int i = 0; i < majorBars.Count; i++)
            {
                isExist = false;

                    for (int j = 0; j < minorBars.Count; j++)
                    {
                        if (majorBars[i].Equals(minorBars[j]))
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        bars.Add(majorBars[i]);
                    }
            }

            return bars;
        }  
    }
}
