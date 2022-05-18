using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksWpfZuev
{
    public static class Model
    {
        private static string headerDocumentString = @"""Symbol""" + "," + @"""Description""" + "," + @"""Date""" + "," + @"""Time""" + "," + @"""Open""" + ","
                    + @"""High""" + "," + @"""Low""" + "," + @"""Close""" + "," + @"""TotalVolume""";

        public static string OpenDialogPath_1 { get; set; }
        public static string OpenDialogPath_2 { get; set; }
        public static string SaveDialogPath{ get; set; }

        public static List<Bar> Read(string path)
        {
            List<Bar> bars = new List<Bar>();
            try
            {
                StreamReader reader = new StreamReader(path);
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var bar = new Bar();
                    string[] items = line.Split(',');
                    try
                    {
                        bar.Symbol = items[0];
                        bar.Description = items[1];
                        bar.Date = DateTime.Parse(items[2]);
                        bar.Time = Convert.ToDateTime(items[3]).TimeOfDay;
                        bar.Open = Convert.ToDouble(items[4], CultureInfo.InvariantCulture);
                        bar.High = Convert.ToDouble(items[5], CultureInfo.InvariantCulture);
                        bar.Low = Convert.ToDouble(items[6], CultureInfo.InvariantCulture);
                        bar.Close = Convert.ToDouble(items[7], CultureInfo.InvariantCulture);
                        bar.TotalVolume = Convert.ToInt32(items[8]);
                        bars.Add(bar);
                    }
                    catch { }

                }   
            }
            catch { }
            return bars;
        }

        /// <summary>
        /// Write a dictionary of bars to text file
        /// </summary>
        /// <param name="orderList"></param>
        public static void ToTxt(Dictionary<DateTime, List<Bar>> orderList)
        {
            try
            {
                using (TextWriter writer = new StreamWriter(SaveDialogPath))
                {
                    writer.Write(headerDocumentString);
                    writer.WriteLine();
                    foreach (var dd in orderList)
                    {
                        var date = dd.Key;
                        foreach (var d in dd.Value)
                        {
                            var bar = d;
                            bar.Date = date;
                            WriteStringToFile(writer, bar);
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Write a dictionary of bars to text file
        /// </summary>
        /// <param name="orderList"></param>
        public static void ToTxt(Dictionary<DateTime, Bar> orderList)
        {
            try
            {
                using (TextWriter writer = new StreamWriter(SaveDialogPath))
                {
                    writer.Write(headerDocumentString);
                    writer.WriteLine();
                    foreach (var d in orderList)
                    {
                        var bar = d.Value;
                        bar.Date = d.Key;
                        WriteStringToFile(writer, bar);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Write a dictionary of bars to text file
        /// </summary>
        /// <param name="orderList"></param>
        public static void ToTxt(List<Bar> orderList)
        {
            try
            {
                using (TextWriter writer = new StreamWriter(SaveDialogPath))
                {
                    writer.Write(headerDocumentString);
                    writer.WriteLine();
                    foreach (var d in orderList)
                    {
                        WriteStringToFile(writer, d);
                    }
                }
            }
            catch { }
        }

        public static void WriteStringToFile(TextWriter writer, Bar bar)
        {
            string writeString = $"{bar.Symbol},{bar.Description},{bar.Date.ToString("dd.MM.yyyy")},{bar.Time}," +
                        $"{bar.Open.ToString().Replace(',', '.')}," +
                        $"{bar.High.ToString().Replace(',', '.')},{bar.Low.ToString().Replace(',', '.')}," +
                        $"{bar.Close.ToString().Replace(',', '.')},{bar.TotalVolume}";

            writer.Write(writeString);
            writer.WriteLine();
        }
    }
}
