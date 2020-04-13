﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatteryChecker.Model
{
    interface IReportCreator
    {
        void CreateReport(string path, List<BatteryChecker.ViewModel.BatteryProperty> batteryInfo);
    }
}
