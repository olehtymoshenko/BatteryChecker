using System.Collections.Generic;

/// <summary>
/// Namespace for creating reports with battery information
/// </summary>
namespace BatteryChecker.Model.Reports
{
    /// <summary>
    /// Interface for creating reports with battery info
    /// </summary>
    public interface IReportCreator
    {
        /// <summary>
        /// Create report
        /// </summary>
        /// <param name="path">path to file</param>
        /// <param name="batteryInfo">Battery information</param>
        void CreateReport(string path, List<ViewModel.BatteryProperty> batteryInfo);
    }
}
