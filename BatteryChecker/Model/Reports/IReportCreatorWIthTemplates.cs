using System.Collections.Generic;

/// <summary>
/// Namespace for creating reports with battery information
/// </summary>
namespace BatteryChecker.Model.Reports
{
    /// <summary>
    /// IReportCreator advanced abillity to insert battery information to template 
    /// </summary>
    public interface IReportCreatorWIthTemplates:IReportCreator
    {
        /// <summary>
        /// Special string which will be replaced by battery information
        /// </summary>
        string SPECIAL_STRING_TO_REPLACE_WITH_TABLE { get; set; }
        /// <summary>
        /// Insert battery information to existing file (template)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="batteryInfo"></param>
        void InsertBatteryInfoIntoTemplate(string path, List<ViewModel.BatteryProperty> batteryInfo);
    }
}
