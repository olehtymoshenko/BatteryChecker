using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using BatteryChecker.Model.BatteryInfo;
using BatteryChecker.Model.Reports;
using BatteryChecker.ViewModel;

namespace UnitTests_BatteryChecker
{
    /// <summary>
    /// Test method are named in this way:
    /// [Testing method]__[InputInMethod]__[ExpectedValue]
    /// </summary>
    /// 


    // Class fore testing report creators
    [TestClass]
    public class UnitTests_Reports
    {
        /// <summary>
        /// Check if a file with required format is created, DOC format testing
        /// </summary>
        [TestMethod]
        public void DocReportCreator_CreateReport__PathToFileInput__NewDocFile()
        {
            // Arrange 
            bool expectedFile_Existing_Method_Result = true;
            string pathToFile = @"TestCreating.doc";
            DocReportCreator reportCreator = new BatteryChecker.Model.Reports.DocReportCreator();
            List<BatteryProperty> testBatteryInfoRequiredFormat = new List<BatteryProperty>();

            BatteryInfo_UWP_API testBatteryInfo = new BatteryInfo_UWP_API();
            Dictionary<string, string> testBatteryInfoSourceFormat = testBatteryInfo.GetBatteryInfo();

            foreach (KeyValuePair<string, string> bp in testBatteryInfoSourceFormat)
            {
                testBatteryInfoRequiredFormat.Add(new BatteryProperty(bp.Key, bp.Value));
            }

            // Act
            reportCreator.CreateReport(pathToFile, testBatteryInfoRequiredFormat);

            // Assert
            Assert.AreEqual(expectedFile_Existing_Method_Result, File.Exists(pathToFile));
        }

        /// <summary>
        /// Check if a file with required format is created, PDF format testing
        /// </summary>
        [TestMethod]
        public void PDFReportCreator_CreateReport__PathToFileInput__NewPdfFile()
        {
            // Arrange 
            bool expectedFile_Existing_Method_Result = true;
            string pathToFile = @"TestCreating.pdf";
            PdfReportCreator reportCreator = new BatteryChecker.Model.Reports.PdfReportCreator();
            List<BatteryProperty> testBatteryInfoRequiredFormat = new List<BatteryProperty>();

            BatteryInfo_UWP_API testBatteryInfo = new BatteryInfo_UWP_API();
            Dictionary<string, string> testBatteryInfoSourceFormat = testBatteryInfo.GetBatteryInfo();

            foreach (KeyValuePair<string, string> bp in testBatteryInfoSourceFormat)
            {
                testBatteryInfoRequiredFormat.Add(new BatteryProperty(bp.Key, bp.Value));
            }

            // Act
            reportCreator.CreateReport(pathToFile, testBatteryInfoRequiredFormat);

            // Assert
            Assert.AreEqual(expectedFile_Existing_Method_Result, File.Exists(pathToFile));
        }
    }
}
