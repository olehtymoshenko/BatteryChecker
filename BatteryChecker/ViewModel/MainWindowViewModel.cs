using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatteryChecker.Model;
using System.Windows.Threading;

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

        public void CreateReportPDF()
        {
            DefaultDialogs dialogs = new DefaultDialogs();
            if(dialogs.SaveFileDialog(DefaultDialogs.TargetFileType.PDF)==true)
            {
                PdfReportCreator creatorPDF = new PdfReportCreator();
                creatorPDF.CreateReport(dialogs.FilePath, properties.ToList());
            }
        }

        public void CreateReportPDF(string path)
        {
            PdfReportCreator creatorPDF = new PdfReportCreator();
            creatorPDF.CreateReport(path, properties.ToList());
        }


        public void CreateReportDOC()
        {
            DefaultDialogs dialogs = new DefaultDialogs();
            if (dialogs.SaveFileDialog(DefaultDialogs.TargetFileType.DOC_DOCX) == true)
            {
                DocReportCreator creatorDOC = new DocReportCreator();
                creatorDOC.CreateReport(dialogs.FilePath, properties.ToList());
            }
        }

        public void CreateReportDOC(string path)
        {
            DocReportCreator creatorDOC = new DocReportCreator();
            creatorDOC.CreateReport(path, properties.ToList());
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
