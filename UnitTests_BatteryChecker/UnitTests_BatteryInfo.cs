using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BatteryChecker.Model.BatteryInfo;

namespace UnitTests_BatteryChecker
{
    /// <summary>
    /// Test method are named in this way:
    /// [Testing method]_[InputInMethod]_[ExpectedValue]
    /// </summary>
    
    // Class for testing battery info sources 
    [TestClass]
    public class UnitTests_BatteryInfo
    {
        /// <summary>
        /// Method for testing UWP api
        /// </summary>
        [TestMethod]
        public void BatteryInfo_UWP_GetBatteryInfo_NoneInput_9initializedFieldsReturned()
        {
            // Arrange
            BatteryInfo_UWP_API biUWP = new BatteryInfo_UWP_API();
            Dictionary<string, string> expectedFields = new Dictionary<string, string> // expected translation
            {
                {"Темп зарядки/розрядки, мВт", ""},   {"Заводская макс. емкость, мВтЧ", "" },
                {"Текущая макс. емкость, мВтЧ", "" }, {"Оставшаяся емкость, мВтЧ", "" },
                {"Статус батареи","" },               {"Режим сбережения энергии", "" },
                {"Статут источника питания","" },     {"Осташаяся емкость, %", "" },
                {"Оставшееся время работы", "" }
            };
            Dictionary<string, string> receivedFields;

            // Act
            receivedFields = biUWP.GetBatteryInfo();

            // Assert
            foreach (KeyValuePair<string, string> pair in receivedFields)
            {
                // is field is not null and with right translation, than true 
                Assert.IsTrue((expectedFields.ContainsKey(pair.Key)) &&
                    (pair.Value != null));
            }
            Assert.IsTrue(receivedFields.Count == expectedFields.Count); // check for equal count of fields
        }

        /// <summary>
        /// Method for testing WMI 
        /// </summary>
        [TestMethod]
        public void BatteryInfo_WMI_GetBatteryInfo_NoneInput_5initializedFieldsReturned()
        {
            // Arrange
            BatteryInfo_WMI biWMI = new BatteryInfo_WMI();
            Dictionary<string, string> expectedFields = new Dictionary<string, string> // expected translation
            {
                {"Краткое описание батареи", ""}, {"Напряжение, мВ", "" }, 
                {"ID батареи", "" },              {"Модель батареи", "" },
                {"Текущая система","" }
            };
            Dictionary<string, string> receivedFields;

            // Act
            receivedFields = biWMI.GetBatteryInfo();

            // Assert
            foreach (KeyValuePair<string, string> pair in receivedFields)
            {
                // is field is not null and with right translation, than true 
                Assert.IsTrue((expectedFields.ContainsKey(pair.Key)) &&
                    (pair.Value != null));
            }
            Assert.IsTrue(receivedFields.Count == expectedFields.Count); // check for equal count of fields
        }

        /// <summary>
        /// Method for testing Win32 API
        /// </summary>
        [TestMethod]
        public void BatteryInfo_Win32_GetBatteryInfo_NoneInput_4initializedFieldsReturned()
        {
            // Arrange
            BatteryInfo_Win32 biWin32 = new BatteryInfo_Win32();
            Dictionary<string, string> expectedFields = new Dictionary<string, string> // expected translation
            {
                {"Химический состав", ""}, {"Емкость для предупреждения 1, мВт", ""},
                {"Емкость для предупреждения 2, мВт", ""}, {"Смещение емкости", ""},
            };
            Dictionary<string, string> receivedFields;

            // Act
            receivedFields = biWin32.GetBatteryInfo();

            // Assert
            foreach (KeyValuePair<string, string> pair in receivedFields)
            {
                // is field is not null and with right translation, than true 
                Assert.IsTrue((expectedFields.ContainsKey(pair.Key)) && 
                    (pair.Value != null));
            }
            Assert.IsTrue(receivedFields.Count == expectedFields.Count); // check for equal count of fields
        }
    }
}
