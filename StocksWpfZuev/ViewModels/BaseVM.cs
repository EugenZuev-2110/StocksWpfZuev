using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksWpfZuev
{
    public class BaseVM
    {
        private IOperation _operation;
        public BaseVM(IOperation operation)
        {
            _operation = operation;
        }

        private RelayCommand _executeCommand;

        public RelayCommand ExecuteCommand
        {
            get
            {
                return _executeCommand ??
                  (_executeCommand = new RelayCommand(obj =>
                  {
                      _operation.Execute();
                  }
                  ));
            }
        }
    }
}
