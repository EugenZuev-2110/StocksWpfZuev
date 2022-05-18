using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksWpfZuev
{
    public static class MenuOperations
    {
       /// <summary>
       /// Open dialog window for file's selecting
       /// </summary>
        public static void OpenFileDialog_1()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                Model.OpenDialogPath_1 = dialog.FileName;
            }
        }

        /// <summary>
        /// Open dialog window for second file's selecting
        /// </summary>
        public static void OpenFileDialog_2()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                Model.OpenDialogPath_2 = dialog.FileName;
            }
        }

        /// <summary>
        /// Open dialog window for saving file
        /// </summary>
        public static void SaveFileDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();

            if (dialog.ShowDialog() == true)
            {
                Model.SaveDialogPath = dialog.FileName;
            }
        }
    }
}
