using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    public class CatalogView
    {
        private int _intLevel;             // Уровень вложенности в каталоге.
        private DirectoryInfo _root;       // Корневой каталог.
        private StringBuilder _logString;  // Строка для записи в лог.
        private string _pathSave;          // Путь сохранения лога.

        bool _inConsole;                   // true - вывод в консоль и в файл; false - вывод только в файл.
        bool _isHumanread;                 // Человекочитаемый вид.

        public CatalogView(string[] args)
        {
            _intLevel = 0;
            _logString = new StringBuilder("");
            _root = new DirectoryInfo(Directory.GetCurrentDirectory());
            _pathSave = $"Sizes-{DateOnly.FromDateTime(DateTime.Now).ToString("yyyy.MM.dd")}.txt";
            _inConsole = true;

            // В зависимости от переданных параметров, включаем опции отображения.
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-q") || args[i].Equals("--quite"))
                {
                    _inConsole = false;
                }
                else if (args[i].Equals("-p") || args[i].Equals("--path"))
                {
                    _root = new DirectoryInfo(args[i + 1]);
                    if (!_root.Exists)
                        throw new ArgumentException("Несуществующий путь", args[i + 1]);
                }
                else if (args[i].Equals("-o") || args[i].Equals("--output"))
                {
                    _pathSave = args[i + 1] + ".txt";
                    using (File.Create(_pathSave))
                        if (!File.Exists(_pathSave))
                        {
                            throw new ArgumentException("Несуществующий путь", args[i + 1]);
                        }
                }
                else if (args[i].Equals("-h") || args[i].Equals("--humanread"))
                {
                    _isHumanread = true;
                }
                
            }
        }

        // Публичный метод для вывода данных в зависимости от переданных параметров
        public void View()
        {
            if (!_inConsole)
                SaveLog();
            else
            {
                SaveLog();
                ConsoleView(_root, _intLevel);
            }
        }

        // Метод выводит данные в консоль.
        private void ConsoleView(DirectoryInfo root, int intLevel)
        {
            FileInfo[] files = root.GetFiles();

            // Выводим название каталога.
            for (int i = 0; i < intLevel; i++)
                Console.Write("   ");

            long sum = Summ(root);
            if (_isHumanread)
            {
                if (sum < 1024)
                    Console.WriteLine($"{root.Name} ({sum} bytes):");
                else if (sum < 1024 * 1024)
                    Console.WriteLine($"{root.Name} ({Math.Round((double)sum / 1024, 2)} Kb):");
                else if (sum < Math.Pow(1024, 3))
                    Console.WriteLine($"{root.Name} ({Math.Round(sum / Math.Pow(1024, 2), 2)} Mb):");
                else if (sum < Math.Pow(1024, 4))
                    Console.WriteLine($"{root.Name} ({Math.Round(sum / Math.Pow(1024, 3), 2)} GB):");
            }
            else
                Console.WriteLine($"{root.Name} ({sum} bytes):");

            // Выводим названия файлов.
            foreach (var file in files)
            {
                for (int i = 0; i < intLevel + 1; i++)
                    Console.Write("   ");
                if (_isHumanread)
                {
                    if (file.Length < 1024)
                        Console.WriteLine($"{file.Name,-50} ({file.Length} bytes)");
                    else if (file.Length < 1024 * 1024)
                        Console.WriteLine($"{file.Name,-50} ({Math.Round((double)file.Length / 1024, 2)} Kb)");
                    else if (file.Length < Math.Pow(1024, 3))
                        Console.WriteLine($"{file.Name,-50} ({Math.Round(file.Length / Math.Pow(1024, 2), 2)} Mb)");
                    else if (file.Length < Math.Pow(1024, 4))
                        Console.WriteLine($"{file.Name,-50} ({Math.Round(file.Length / Math.Pow(1024, 3), 2)} GB)");
                }
                else
                    Console.WriteLine($"{file.Name,-50} ({file.Length} bytes)");
            }

            DirectoryInfo[] subRoots = root.GetDirectories();
            if (subRoots.Length > 0)
                foreach (var subRoot in subRoots)
                {
                    ConsoleView(subRoot, intLevel + 1);
                }
        }

        #region Запись в файл

        // Сначала вызывается метод обхода дерева каталогов, затем информация сохраняется в файл.
        private void SaveLog()
        {
            LogStringCreate(_root, _intLevel);
            using (StreamWriter sw = new StreamWriter(_pathSave))
            {
                sw.Write(_logString.ToString());
            }
        }

        // Метод формирует строку _logString для сохранения.
        private void LogStringCreate(DirectoryInfo root, int intLevel)
        {
            FileInfo[] files = root.GetFiles();

            // Выводим название каталога.
            for (int i = 0; i < intLevel; i++)
            {
                _logString.Append("   ");
            }

            long sum = Summ(root);
            if (_isHumanread)
            {
                if (sum < 1024)
                    _logString.Append($"{root.Name} ({sum} bytes):\n");
                else if (sum < 1024 * 1024)
                    _logString.Append($"{root.Name} ({Math.Round((double)sum / 1024, 2)} Kb):\n");
                else if (sum < Math.Pow(1024, 3))
                    _logString.Append($"{root.Name} ({Math.Round(sum / Math.Pow(1024, 2), 2)} Mb):\n");
                else if (sum < Math.Pow(1024, 4))
                    _logString.Append($"{root.Name} ({Math.Round(sum / Math.Pow(1024, 3), 2)} GB):\n");
            }
            else
                _logString.Append($"{root.Name} ({sum} bytes):\n");

            // Выводим названия файлов.
            foreach (var file in files)
            {
                for (int i = 0; i < intLevel + 1; i++)
                {
                    _logString.Append("   ");
                }

                if (_isHumanread)
                {
                    if (file.Length < 1024)
                        _logString.Append($"{file.Name,-50} ({file.Length} bytes)\n");
                    else if (file.Length < 1024 * 1024)
                        _logString.Append($"{file.Name,-50} ({Math.Round((double)file.Length / 1024, 2)} Kb)\n");
                    else if (file.Length < Math.Pow(1024, 3))
                        _logString.Append($"{file.Name,-50} ({Math.Round(file.Length / Math.Pow(1024, 2), 2)} Mb)\n");
                    else if (file.Length < Math.Pow(1024, 4))
                        _logString.Append($"{file.Name,-50} ({Math.Round(file.Length / Math.Pow(1024, 3), 2)} GB)\n");
                }
                else
                    _logString.Append($"{file.Name,-50} ({file.Length} bytes)\n");
            }

            DirectoryInfo[] subRoots = root.GetDirectories();
            if (subRoots.Length > 0)
                foreach (var subRoot in subRoots)
                {
                    LogStringCreate(subRoot, intLevel + 1);
                }
        }
        #endregion

        // Подсчет размера файлов и папок.
        private long Summ(DirectoryInfo root)
        {
            long size = 0;

            FileInfo[] files = root.GetFiles();
            size += files?.Sum(f => f.Length) ?? 0;

            DirectoryInfo[] subDir = root.GetDirectories();
            if (subDir.Length > 0)
            {
                foreach (DirectoryInfo dir in subDir)
                {
                    size += Summ(dir);
                }
            }
            return size;
        }


    }
}
