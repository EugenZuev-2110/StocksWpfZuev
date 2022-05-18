using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksWpfZuev
{
    public class MenuVM
    {
        private RelayCommand _openFileCommand;
        private RelayCommand _saveFileCommand;
        public RelayCommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ??
                  (_openFileCommand = new RelayCommand(obj =>
                  {
                      MenuOperations.OpenFileDialog_1();
                  }
                  ));
            }
        }

        public RelayCommand SaveFileCommand
        {
            get
            {
                return _saveFileCommand ??
                  (_saveFileCommand = new RelayCommand(obj =>
                  {
                      MenuOperations.SaveFileDialog();
                  }
                  ));
            }
        }
    }
}
