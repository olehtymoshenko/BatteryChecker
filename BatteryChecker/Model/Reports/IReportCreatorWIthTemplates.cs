using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatteryChecker.Model.Reports
{
    interface IReportCreatorWIthTemplates:IReportCreator
    {
        string SPECIAL_STRING_TO_REPLACE_WITH_TABLE { get; set; }
        void InsertTableIntoTemplate(string path, List<ViewModel.BatteryProperty> batteryInfo);

    }
}
