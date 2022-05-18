using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksWpfZuev
{
    public class StringFinderOperation : OperationBase, IOperation
    {
        public async void Execute()
        {
            try
            {
                MenuOperations.OpenFileDialog_1();//Открываем диалоговое окно для чтения первого файла
                MenuOperations.OpenFileDialog_2();//Открываем диалоговое окно для чтения второго файла          
                var document1 = await Task.Run(() => Model.Read(Model.OpenDialogPath_1));//Производим чтение первого файла и запись строк в переменную document1
                var document2 = await Task.Run(() => Model.Read(Model.OpenDialogPath_2));//Производим чтение второго файла и запись строк в переменную document2
                var sortedDocument1 = await Task.Run(() => DocumentDateParser(document1));//Производим сортировку document1 по датам и сохраняем результат в переменную sortedDocument1
                var sortedDocument2 = await Task.Run(() => DocumentDateParser(document2));//Производим сортировку document2 по датам и сохраняем результат в переменную sortedDocument2
                var dictList1 = await Task.Run(() => StringFinderGeneral(sortedDocument1, sortedDocument2));//Производим поиск строк, кототрые есть в sortedDocument1 и нет в sortedDocument2
                MenuOperations.SaveFileDialog();//Открываем диалоговое окно для сохранения dictList1 в файл
                await Task.Run(() => Model.ToTxt(dictList1));//Сохраняем dictList1 в файл
                var dictList2 = await Task.Run(() => StringFinderGeneral(sortedDocument2, sortedDocument1));//Производим поиск строк, кототрые есть в sortedDocument2 и нет в sortedDocument1
                MenuOperations.SaveFileDialog();//Открываем диалоговое окно для сохранения dictList2 в файл
                await Task.Run(() => Model.ToTxt(dictList2));//Сохраняем dictList2 в файл
            }
            catch { }
        }
    }
}
