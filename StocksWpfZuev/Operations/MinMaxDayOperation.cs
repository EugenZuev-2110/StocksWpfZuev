using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksWpfZuev
{
    public class MinMaxDayOperation : Operation, IOperation
    {
        public MinMaxDayOperation() { }
        public async void Execute()
        {
            try
            {
                MenuOperations.OpenFileDialog_1();//Открываем диалоговое окно для чтения файла
                var document = await Task.Run(() => Model.Read(Model.OpenDialogPath_1));//Производим чтение первого файла и запись строк в переменную document
                var days = await Task.Run(() => MinMaxOrder(document));//Производит поиск минимального и максимального значений каждого дня
                MenuOperations.SaveFileDialog();//Открываем диалоговое окно для сохранения days в файл
                await Task.Run(() => Model.ToTxt(days));//Сохраняем days в файл
            }
            catch { }
        }
    }
}
