// See https://aka.ms/new-console-template for more information
using TestTask;

/* Программа может принимать один, несколько или 0 параметров:
 * "C:\Users\Kirill\Desktop>TestTask.exe"
 * "C:\Users\Kirill\Desktop>TestTask.exe -h"
 * "C:\Users\Kirill\Desktop>TestTask.exe -o C:\Users\Kirill\Desktop\save" - сохранить результат на рабочем столе в файл save.txt
 * 
 * "C:\Users\Kirill\Desktop>TestTask.exe -p C:\Program Files -o C:\Users\Kirill\Desktop\save -q -h" - перебор файлов и папок
 *                              начиная с Program Files, сохранить в файл на рабочем столе в человекочитаемом виде, не выводить в консоль.
 */
string[] str = Environment.GetCommandLineArgs();

try
{
    CatalogView cv = new CatalogView(str);
    cv.View();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
//Console.WriteLine("Программа завершила свое выполнение");
//Console.ReadLine();



