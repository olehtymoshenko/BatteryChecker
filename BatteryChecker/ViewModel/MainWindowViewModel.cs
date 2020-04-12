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
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 5);
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
    }



    struct BatteryProperty
    {
        public string name { get; set; }
        public string value { get; set; }

        public BatteryProperty(string _name, string _value)
        {
            name = _name;
            value = _value;
        }
    }
}
