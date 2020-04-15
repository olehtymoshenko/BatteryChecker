using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatteryChecker.Model.Translators
{
    public class EnToRusTranslator
    {
        private static EnToRusTranslator instance; // singleton

        private readonly Dictionary<string, string> en_rus_Dictionary;

        private EnToRusTranslator()
        {
            en_rus_Dictionary = new Dictionary<string, string>();
            InitializeDictionary();
        }

        public static EnToRusTranslator GetInstance()
        {
            if(instance == null)
            {
                instance = new EnToRusTranslator();
            }
            return instance;
        }

        private void InitializeDictionary()
        {
            en_rus_Dictionary.Add("ChargeRateInMilliwatts", "Темп зарядки/розрядки, мВт");
            en_rus_Dictionary.Add("DesignCapacityInMilliwattHours", "Заводская макс. емкость, мВтЧ");
            en_rus_Dictionary.Add("FullChargeCapacityInMilliwattHours", "Текущая макс. емкость, мВтЧ");
            en_rus_Dictionary.Add("RemainingCapacityInMilliwattHours", "Оставшаяся емкость, мВтЧ");
            en_rus_Dictionary.Add("Status", "Статус батареи");
            en_rus_Dictionary.Add("NotPresent", "Отсутстует");
            en_rus_Dictionary.Add("Discharging", "Разряжается");
            en_rus_Dictionary.Add("Idle", "Не используется");
            en_rus_Dictionary.Add("Charging", "Заряжается");
            en_rus_Dictionary.Add("EnergySaverStatus", "Режим сбережения энергии");
            en_rus_Dictionary.Add("Disabled", "Отключен");
            en_rus_Dictionary.Add("Off", "Выключен");
            en_rus_Dictionary.Add("On", "Включен");
            en_rus_Dictionary.Add("PowerSupplyStatus", "Статут источника питания");
            en_rus_Dictionary.Add("Inadequate", "Ненормальный, теряется заряд");
            en_rus_Dictionary.Add("Adequate", "Нормальный");
            en_rus_Dictionary.Add("RemainingChargePercent", "Осташаяся емкость, %");
            en_rus_Dictionary.Add("RemainingDischargeTime", "Оставшееся время работы");
            en_rus_Dictionary.Add("Caption", "Краткое описание батареи");
            en_rus_Dictionary.Add("DesignVoltage", "Напряжение, мВ");
            en_rus_Dictionary.Add("DeviceID", "ID батареи");
            en_rus_Dictionary.Add("Name", "Модель батареи:");
            en_rus_Dictionary.Add("SystemName", "Текущая система:");
            en_rus_Dictionary.Add("Chemistry", "Химический состав:");
            en_rus_Dictionary.Add("PbAc", "Свинцово-кислотный");
            en_rus_Dictionary.Add("LION", "Литий-ионный");
            en_rus_Dictionary.Add("Lion", "Литий-ионный");
            en_rus_Dictionary.Add("Li-I", "Литий-ионный");
            en_rus_Dictionary.Add("LiP", "Литий полимерный");
            en_rus_Dictionary.Add("NiCd", "Никель кадмий");
            en_rus_Dictionary.Add("NiMH", "Никель металлогидрид");
            en_rus_Dictionary.Add("NiZn", "Никель цинк");
            en_rus_Dictionary.Add("RAM", "Щелочно-марганцевый");
            en_rus_Dictionary.Add("DefaultAlert1", "Емкость для предупреждения 1, мВт");
            en_rus_Dictionary.Add("DefaultAlert2", "Емкость для предупреждения 2, мВт");
            en_rus_Dictionary.Add("CriticalBias", "Смещение емкости:");
        }

        public string Translate(string value)
        {
            en_rus_Dictionary.TryGetValue(value, out string valueTranslated);
            return valueTranslated;
        }
    }
}
