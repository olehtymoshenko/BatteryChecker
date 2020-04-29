using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BatteryChecker.Model.Translators;

namespace UnitTests_BatteryChecker
{
    /// <summary>
    /// Test method are named in this way:
    /// [Testing method]_[InputInMethod]_[ExpectedValue]
    /// </summary>
    
    // Class for testing translators 
    [TestClass]
    public class UnitTests_Translators
    {
        /// <summary>
        /// Method for test En to Rus translator
        /// </summary>
        [TestMethod]
        public void Translate_EngWordsInput_TranslationToRusReturned()
        {
            // Arrange 
            EnToRusTranslator translator = EnToRusTranslator.GetInstance();

            // testing words
            List<string> testingWords = new List<string>() { "Status", "Discharging", "Charging", "Disabled",
                "Off", "On", "PowerSupplyStatus", "Inadequate", "Adequate", "DesignVoltage", "DefaultAlert1" };

            // expected translation
            List<string> expectedTranslation = new List<string>() { "Статус батареи", "Разряжается", "Заряжается",
                "Отключен", "Выключен", "Включен", "Статут источника питания", "Ненормальный, теряется заряд",
                "Нормальный", "Напряжение, мВ", "Емкость для предупреждения 1, мВт" };
            expectedTranslation.TrimExcess();

            List<string> recievedTranslation = new List<string>();

            // Act
            foreach (string word in testingWords)
            {
                recievedTranslation.Add(translator.Translate(word));
            }
            recievedTranslation.TrimExcess();

            // Assert
            CollectionAssert.AreEqual(expectedTranslation, recievedTranslation);
        }
    }
}
