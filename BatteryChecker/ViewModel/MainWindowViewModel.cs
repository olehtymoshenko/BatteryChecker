using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using BatteryChecker.Model.Reports;
using BatteryChecker.Model.BatteryInfo;

namespace BatteryChecker.ViewModel
{
    class MainWindowViewModel
    {
        public ObservableCollection<BatteryProperty> properties;

        DispatcherTimer timer;

        public MainWindowViewModel()
        {
            properties = new ObservableCollection<BatteryProperty>();

            SetUpTimer();

            InitializeProperiesDictionary();
        }

        private void SetUpTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 5)
            };
            timer.Tick += (sender, e) => { InitializeProperiesDictionary(); };
            timer.Start();
        }

        private void InitializeProperiesDictionary()
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

        public void CreateReport(DefaultDialogs.TargetFileType format)
        {
            DefaultDialogs dialogs = new DefaultDialogs();
            
            if (dialogs.SaveFileDialog(format) == true)
            {
                IReportCreator creator = GetIReportCreatorFromReportFormat(format);
                creator.CreateReport(dialogs.FilePath, properties.ToList());
            }
        }

        public void CreateReport(string path, DefaultDialogs.TargetFileType format)
        {
            IReportCreator creator = GetIReportCreatorFromReportFormat(format);
            creator.CreateReport(path, properties.ToList());
        }


        private IReportCreator GetIReportCreatorFromReportFormat(DefaultDialogs.TargetFileType format)
        {
            switch (format)
            {
                case DefaultDialogs.TargetFileType.DOC_DOCX: return new DocReportCreator() as IReportCreator;
                case DefaultDialogs.TargetFileType.PDF: return new PdfReportCreator() as IReportCreator;
            }
            return null;
        }


        public void InsertTableInTemplateDOC()
        {
            DefaultDialogs dialogs = new DefaultDialogs();
            if (dialogs.OpenFileDialog(DefaultDialogs.TargetFileType.DOC_DOCX) == true)
            {
                DocReportCreator creatorDOC = new DocReportCreator();
                creatorDOC.InsertTableIntoTemplate(dialogs.FilePath, properties.ToList());
            }
        }

        public void InsertTableInTemplateDOC(string path)
        {
            DocReportCreator creatorDOC = new DocReportCreator();
            creatorDOC.InsertTableIntoTemplate(path, properties.ToList());
        }

        public void ShowInfAboutApp()
        {
            DefaultDialogs dialogs = new DefaultDialogs();
            dialogs.ShowMessage("Программа предназначена для просмотра детальной\nинформации о батарее\n\n"+
                "Приложение является курсовым проектом 2020\nстудента 535Б группы Тимошенко Олега\n\n"+
                "E-mail: o.tymoshenko@student.csn.khai.edu", "Информация о программе");
        }
    }

    struct BatteryProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public BatteryProperty(string _name, string _value)
        {
            Name = _name;
            Value = _value;
        }

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
