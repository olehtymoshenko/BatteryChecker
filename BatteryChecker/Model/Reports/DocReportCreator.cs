using System;
using System.Collections.Generic;
using System.IO;
using BatteryChecker.ViewModel;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;

/// <summary>
/// Namespace for creating reports with battery information
/// </summary>
namespace BatteryChecker.Model.Reports
{
    /// <summary>
    /// Class for creating reports in doc/docx formats
    /// </summary>
    class DocReportCreator : IReportCreatorWIthTemplates
    {
        /// <summary>
        /// Name table header with battery information
        /// </summary>
        private readonly string[] NAME_HEADERS_COLUMN = new string[] { "Свойство", "Значение" };
        /// <summary>
        /// Special string which will be replaced by battery information
        /// </summary>
        public string SPECIAL_STRING_TO_REPLACE_WITH_TABLE { get; set; } = "$tableBatteryInfo$";

        /// <summary>
        /// Create report in doc/docx format
        /// </summary>
        /// <param name="path">path to file</param>
        /// <param name="batteryInfo">battery information</param>
        public void CreateReport(string path, List<BatteryProperty> batteryInfo)
        {
            try
            {
                using (Document doc = new Document())
                {
                    Section s = doc.AddSection();
                    TextRange tr;
                    tr = InsertNewParagraph("Отчет о состоянии батареи\n", s, HorizontalAlignment.Center);
                    tr.CharacterFormat.FontName = "Arial";
                    tr.CharacterFormat.FontSize = 22;
                    tr.CharacterFormat.CharacterSpacing = 2;

                    tr = InsertNewParagraph(DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss" + '\n'), s, HorizontalAlignment.Center);
                    tr.CharacterFormat.FontName = "Arial";
                    tr.CharacterFormat.FontSize = 16;

                    Table table = s.AddTable(true);
                    FillOutTable(table, batteryInfo);

                    doc.SaveToFile(path, FileFormat.Auto);
                }
            }
            catch (IOException)
            {
                throw new IOException("Не удалось получить доступ к файлу, возможно он открыт в другом приложении\n");
            }
            catch (Exception e)
            {
                throw new Exception("Неопознання ошибка! \n Системное описание ошибки:" + e.Message);
            }
        }

        /// <summary>
        /// Insert battery information (table) in template
        /// </summary>
        /// <param name="path">path to template</param>
        /// <param name="batteryInfo">battery information</param>
        public void InsertBatteryInfoIntoTemplate(string path, List<BatteryProperty> batteryInfo)
        {
            try
            {
                using (Document doc = new Document())
                {
                    doc.LoadFromFile(path);
                    TextSelection selection = doc.FindString(SPECIAL_STRING_TO_REPLACE_WITH_TABLE, true, true);
                    if (selection != null)
                    {
                        TextRange tr = selection.GetAsOneRange();
                        Paragraph p = tr.OwnerParagraph;
                        Body body = p.OwnerTextBody;
                        int index = body.ChildObjects.IndexOf(p);
                        body.ChildObjects.Remove(p);
                        Table table = body.AddTable(true);
                        FillOutTable(table, batteryInfo);
                        body.ChildObjects.Insert(index, table);

                        doc.SaveToFile(path, FileFormat.Auto);
                    }
                }
            }
            catch (IOException)
            {
                throw new IOException("Не удалось получить доступ к файлу, возможно он открыт в другом приложении\n");
            }
            catch (Exception e)
            {
                throw new Exception("Неопознання ошибка! \n Системное описание ошибки:" + e.Message);
            }
        }

        /// <summary>
        /// Insert battery information in the table
        /// </summary>
        /// <param name="table">table, which will be filled</param>
        /// <param name="batteryInfo"></param>
        private void FillOutTable(Table table, List<BatteryProperty> batteryInfo)
        {
            table.TableFormat.HorizontalAlignment = RowAlignment.Center;
            table.PreferredWidth = new PreferredWidth(WidthType.Percentage, 90);
            table.ResetCells(batteryInfo.Count, NAME_HEADERS_COLUMN.Length);
         
            TableRow rowHead = table.Rows[0];
            rowHead.IsHeader = true;
            // Header row
            for(int i=0; i < NAME_HEADERS_COLUMN.Length; i++)
            {
                Paragraph p = rowHead.Cells[i].AddParagraph();
                p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                TextRange tr = p.AppendText(NAME_HEADERS_COLUMN[i]);
                tr.CharacterFormat.FontName = "Arial";
                tr.CharacterFormat.FontSize = 16;
                tr.CharacterFormat.Bold = true;
            }

            // Data row
            for(int i = 1; i < batteryInfo.Count;i++)
            {
                TableRow rowBody = table.Rows[i];

                for(int j = 0; j < NAME_HEADERS_COLUMN.Length; j++)
                {
                    Paragraph p = rowBody.Cells[j].AddParagraph();
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    TextRange tr = p.AppendText(batteryInfo[i][j]);
                    tr.CharacterFormat.FontName = "Arial";
                    tr.CharacterFormat.FontSize = 12;
                }
            }
            // keep together table
            foreach(TableRow row in table.Rows)
                foreach(TableCell cell in row.Cells)
                    foreach (Paragraph p in cell.Paragraphs)
                        p.Format.KeepFollow = true;
        }

        /// <summary>
        /// Insert paragraph in section
        /// </summary>
        /// <param name="text">text, which will be inserted in paragpaph</param>
        /// <param name="s">Section into which paragraph will be inserted</param>
        /// <param name="hAlign">Paragraph horizontal alignment</param>
        /// <returns>Text Range which represent inserted text</returns>
        private TextRange InsertNewParagraph(string text, Section s, HorizontalAlignment hAlign)
        {
            Paragraph p = s.AddParagraph();
            p.Format.HorizontalAlignment = hAlign;
            TextRange tr = p.AppendText(text);
            return tr;
        }
    }
}
