using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StocksWpfZuev
{
    public class MinMaxDayVM : BaseVM
    {
        public MinMaxDayVM() : base(new MinMaxDayOperation())
        { }       
    }
}
