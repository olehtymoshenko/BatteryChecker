using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using BatteryChecker.Model.Reports;
using BatteryChecker.Model.BatteryInfo;
using System.IO;

/// <summary>
/// Namespace for viewmodel component of application
/// </summary>
namespace BatteryChecker.ViewModel
{

    /// <summary>
    /// ViewModel for MainWindow 
    /// </summary>
    public class MainWindowViewModel
    {
        /// <summary>
        /// Collection for contain battery information
        /// </summary>
        public ObservableCollection<BatteryProperty> properties;

        /// <summary>
        /// Timer for update battery information
        /// </summary>
        DispatcherTimer timer;

        /// <summary>
        /// Defaul constructor
        /// </summary>
        public MainWindowViewModel()
        {
            properties = new ObservableCollection<BatteryProperty>();
            InitializeProperiesDictionary();
            SetUpTimer();

        }

        /// <summary>
        /// Set up timer (interval, handler and start it)
        /// </summary>
        private void SetUpTimer()
        {
            try
            {
                timer = new DispatcherTimer
                {
                    Interval = new TimeSpan(0, 0, 5)
                };
                timer.Tick += (sender, e) => { UpdateProperties(); };
                timer.Start();
            }
            catch(Exception e)
            {
                DefaultDialogs.ShowMessage("Ошибка запуска таймера для обновления информации"+
                    "информация о батарее не будет обновлятся\n" +
                    "Системное описание ошибки" + e.Message, "Ошибка");
            }
        }

        /// <summary>
        /// Get information about battrey from all available sources
        /// </summary>
        private void InitializeProperiesDictionary()
        {
            try
            {
                properties.Clear();
                BatteryInfo[] batInfo = new BatteryInfo[] {new BatteryInfo_UWP_API(),
                                                       new BatteryInfo_WMI(),
                                                       new BatteryInfo_Win32()};
                foreach (BatteryInfo i in batInfo)
                {
                    foreach (KeyValuePair<string, string> pair in i.GetBatteryInfo())
                    {
                        properties.Add(new BatteryProperty(pair.Key, pair.Value));
                    }
                }
            }
            catch (NullReferenceException e)
            {
                DefaultDialogs.ShowMessage("Не удалось обновить данные о батарее, возможно она больше не доступна\n" +
                    "Системное описание ошибки" + e.Message, "Ошибка");
            }
            catch (Exception e)
            {
                DefaultDialogs.ShowMessage("Не опознанная ошибка получения информации о батарее!\n" +
                     "Системное описание ошибки" + e.Message, "Ошибка");
            }
        }

        /// <summary>
        /// Update information in observable collection
        /// </summary>
        private void UpdateProperties()
        {
            try
            {
                BatteryInfo[] batInfo = new BatteryInfo[] {new BatteryInfo_UWP_API(),
                                                       new BatteryInfo_WMI(),
                                                       new BatteryInfo_Win32()};
                foreach (BatteryInfo i in batInfo)
                {
                    foreach (KeyValuePair<string, string> pair in i.GetBatteryInfo())
                    {
                        int index = properties.IndexOf(properties.FirstOrDefault(x => x.Name == pair.Key && x.Value != pair.Value));
                        if (index >= 0)
                        {
                            properties.RemoveAt(index);
                            properties.Insert(index, new BatteryProperty(pair.Key, pair.Value));
                        }
                    }
                }
            }
            catch (NullReferenceException e)
            {
                DefaultDialogs.ShowMessage("Не удалось обновить данные о батарее, возможно она больше не доступна\n" +
                    "Системное описание ошибки" + e.Message, "Ошибка");
            }
            catch (Exception e)
            {
                DefaultDialogs.ShowMessage("Не опознанная ошибка получения информации о батарее!\n" +
                     "Системное описание ошибки" + e.Message, "Ошибка");
            }
        }

        /// <summary>
        /// Create report with showing SaveFileDialog
        /// </summary>
        /// <param name="format">type of creating report</param>
        public void CreateReport(DefaultDialogs.TargetFileType format)
        {
            try
            {
                DefaultDialogs dialogs = new DefaultDialogs();

                if (dialogs.SaveFileDialog(format) == true)
                {
                    IReportCreator creator = GetIReportCreatorFromReportFormat(format);
                    creator.CreateReport(dialogs.FilePath, properties.ToList());
                }
            }
            catch (NullReferenceException e)
            {
                DefaultDialogs.ShowMessage("Не удалось создать отчет, возможно не достаточно свободной памяти\n" +
                    "Системное описание ошибки" + e.Message, "Ошибка");
            }
            catch(IOException e)
            {
                DefaultDialogs.ShowMessage(e.Message, "Ошибка");
            }
            catch (Exception e)
            {
                DefaultDialogs.ShowMessage("Не опознанная ошибка!\n" +
                     "Системное описание ошибки" + e.Message, "Ошибка");
            }
        }

        /// <summary>
        /// Create report without any dialogs
        /// </summary>
        /// <param name="path">path to file</param>
        /// <param name="format">type of creating file</param>
        public void CreateReport(string path, DefaultDialogs.TargetFileType format)
        {
            try
            {
                IReportCreator creator = GetIReportCreatorFromReportFormat(format);
                creator.CreateReport(path, properties.ToList());
            }
            catch(NullReferenceException e)
            {
                DefaultDialogs.ShowMessage("Не удалось создать отчет, возможно не достаточно свободной памяти\n" +
                    "Системное описание ошибки" + e.Message, "Ошибка");
            }
            catch (IOException e)
            {
                DefaultDialogs.ShowMessage(e.Message, "Ошибка");
            }
            catch (Exception e)
            {
                DefaultDialogs.ShowMessage("Не опознанная ошибка!\n" +
                     "Системное описание ошибки" + e.Message, "Ошибка");
            }
        }

        /// <summary>
        /// From enum format get appropriate object for creating report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private IReportCreator GetIReportCreatorFromReportFormat(DefaultDialogs.TargetFileType format)
        {
            try
            {
                switch (format)
                {
                    case DefaultDialogs.TargetFileType.DOC_DOCX: return new DocReportCreator();
                    case DefaultDialogs.TargetFileType.PDF: return new PdfReportCreator();
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Не удалось создать объект для создания отчета\n Системное описание ошибки:" + e.Message);
            }
        }

        /// <summary>
        /// Insert battery information (table) into existing file with showing OpenFileDialog
        /// </summary>
        public void InsertTableInTemplateDOC()
        {
            try
            {
                DefaultDialogs dialogs = new DefaultDialogs();
                if (dialogs.OpenFileDialog(DefaultDialogs.TargetFileType.DOC_DOCX) == true)
                {
                    DocReportCreator creatorDOC = new DocReportCreator();
                    creatorDOC.InsertBatteryInfoIntoTemplate(dialogs.FilePath, properties.ToList());
                }
            }
            catch (Exception e)
            {
                DefaultDialogs.ShowMessage(e.Message, "Ошибка");
            }
        }

        /// <summary>
        /// Insert battery into existing file without any dialogs
        /// </summary>
        /// <param name="path">path to existing template</param>
        public void InsertTableInTemplateDOC(string path)
        {
            try
            {
                DocReportCreator creatorDOC = new DocReportCreator();
                creatorDOC.InsertBatteryInfoIntoTemplate(path, properties.ToList());
            }
            catch(Exception e)
            {
                DefaultDialogs.ShowMessage(e.Message, "Ошибка");
            }
        }

        /// <summary>
        /// Show dialog with information about app
        /// </summary>
        public void ShowInfAboutApp()
        {
            DefaultDialogs.ShowMessage("Программа предназначена для просмотра детальной\nинформации о батарее\n\n"+
                "Приложение является курсовым проектом 2020\nстудента 535Б группы Тимошенко Олега\n\n"+
                "E-mail: o.tymoshenko@student.csn.khai.edu", "Информация о программе");
        }
    }

    /// <summary>
    /// Struct which represent one battery property (name and value)
    /// </summary>
    public struct BatteryProperty
    {
        /// <summary>
        /// Battery property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Battery porperty value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="_name">Property name</param>
        /// <param name="_value">Property value</param>
        public BatteryProperty(string _name, string _value)
        {
            Name = _name;
            Value = _value;
        }

        /// <summary>
        /// Indexer for getting property by number
        /// </summary>
        /// <param name="i">Index for getting porperty: 0 - get Name, 1 - get Value</param>
        /// <returns>Depending on the index will return name or value</returns>
        public string this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return Name;
                    case 1: return Value;
                    default: return null;
                }
            }
        }

    }
}
