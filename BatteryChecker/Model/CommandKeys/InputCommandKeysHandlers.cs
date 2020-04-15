using System;
using System.Linq;
using BatteryChecker.ViewModel;
using System.Runtime.InteropServices;
using BatteryChecker.Model.Reports;

/// <summary>
/// Namespace for processing input programm arguments
/// </summary>
namespace BatteryChecker.Model.CommandKeys
{
    /// <summary>
    /// Class for processing arguments
    /// </summary>
    class InputCommandKeysHandlers
    {
        /// <summary>
        /// Input arguments
        /// </summary>
        public string[] CommandParams { get; private set; }

        /// <summary>
        /// Constructor with setting inputed arguments
        /// </summary>
        /// <param name="args">Input arguments</param>
        public InputCommandKeysHandlers(string[] args)
        {
            CommandParams = args;
        }

        /// <summary>
        /// Native function, using for connect current application with other process
        /// console
        /// </summary>
        /// <param name="processId">process id, which console try to get</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        private static extern bool AttachConsole(int processId);

        /// <summary>
        /// Free console, which connected by AttachConsole
        /// </summary>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        private static extern bool FreeConsole();

        /// <summary>
        /// Main entry point for processing arguments
        /// </summary>
        public void ExecuteCommandKey()
        {
            switch (CommandParams[0]) // detect key 
            {
                case "-c": // print to console
                    {
                        PrintTableToConsole();
                    }
                    break;
                case "-d": // create doc
                    {
                        CreateNewReport(new DocReportCreator());
                    }
                    break;
                case "-p": // create pdf
                    {
                        CreateNewReport(new PdfReportCreator());
                    }
                    break;
                case "-t": // insert table in existing doc tempalate 
                    {
                        InsertTableInTemplate(new DocReportCreator());
                    }
                    break;
            }
        }

        /// <summary>
        /// Print battery info to parrent process console
        /// </summary>
        private void PrintTableToConsole()
        {
            try
            {
                if(!AttachConsole(-1)) // connect parrent procces console to this app
                {
                    throw new Exception("Не удалось получить доступ к консоли");
                }
                MainWindowViewModel vm = new MainWindowViewModel();
                Console.WriteLine("\n|" + new string('-', 35) + "|" + new string('-', 35) + "|");
                Console.WriteLine("|{0,-35}" + "|" + "{1,-35}" + "|", "Свойство", "Значение");
                Console.WriteLine("|" + new string('-', 35) + "|" + new string('-', 35) + "|");
                foreach (BatteryProperty prop in vm.properties.ToList())
                {
                    Console.WriteLine("|{0,-35}|{1,-35}|", prop.Name, prop.Value);
                    Console.WriteLine("|" + new string('-', 35) + "|" + new string('-', 35) + "|");
                }
                FreeConsole();
            }
            catch(Exception e)
            {
                Environment.FailFast(e.Message, e);
            }
        }

        /// <summary>
        /// Create report with battery information
        /// </summary>
        /// <param name="creator">object, which can create report (implement required interface)</param>
        private void CreateNewReport(IReportCreator creator)
        {
            try
            {
                if (CommandParams[1] != null)
                {
                    MainWindowViewModel vm = new MainWindowViewModel();
                    creator.CreateReport(CommandParams[1], vm.properties.ToList());
                }
                else throw new Exception("Ошибка! Путь для файла не передан");
            }
            catch(Exception e)
            {
                try
                {
                    if(!AttachConsole(-1)) throw new Exception("\nНе удалось получить доступ к консоли");
                    Console.WriteLine("Ошибка, не удалось создать вставить таблицу в существующий шаблон\n" +
                     "проверьте передаваемые параметры, в случае если не поможет,\n то пишите на почту o.tymoshenko@student.csn.khai.edu");
                    FreeConsole();
                    Environment.Exit(1);
                }
                catch(Exception innerE)
                {
                    Environment.FailFast(e.Message, innerE);
                }
            }
        }

        /// <summary>
        /// Insert battery information (table) to existing template
        /// </summary>
        /// <param name="creatorTemplates">object, which can insert table to template(implement required interface)</param>
        private void InsertTableInTemplate(IReportCreatorWIthTemplates creatorTemplates)
        {
            try
            {
                if (CommandParams[1] != null)
                {
                    MainWindowViewModel vm = new MainWindowViewModel();
                    creatorTemplates.InsertTableIntoTemplate(CommandParams[1], vm.properties.ToList());
                }
                else throw new Exception("Ошибка! Путь для файла не передан");
            }
            catch (Exception e)
            {
                try
                {
                    if (!AttachConsole(-1)) throw new Exception("\nНе удалось получить доступ к консоли");
                    Console.WriteLine("Ошибка, не удалось создать вставить таблицу в существующий шаблон\n"+
                        "проверьте передаваемые параметры, в случае если не поможет,\n то пишите на почту o.tymoshenko@student.csn.khai.edu");
                    FreeConsole();
                    Environment.Exit(1);
                }
                catch (Exception innerE)
                {
                    Environment.FailFast(e.Message, innerE);
                }
            }
        }
    }
}
