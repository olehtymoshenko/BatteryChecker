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
            if(dialogs.SaveFileDialog()==true)
            {
                PdfReportCreator creatorPDF = new PdfReportCreator();
                creatorPDF.CreateReport(dialogs.FilePath, properties.ToList());
            }
            else
            {
                dialogs.ShowMessage("Создание отчета отменено");
            }
        }

        public void CreateReportPDF(string path)
        {
            PdfReportCreator creatorPDF = new PdfReportCreator();
            creatorPDF.CreateReport(path, properties.ToList());
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
    }
}
