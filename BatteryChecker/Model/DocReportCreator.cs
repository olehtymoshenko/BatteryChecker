using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatteryChecker.ViewModel;
using System.Drawing;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System.Drawing.Text;
using System.IO;

namespace BatteryChecker.Model
{
    class DocReportCreator : IReportCreator
    {
        private readonly string[] NAME_HEADERS_COLUMN = new string[] { "Свойство", "Значение" };
        private readonly string SPECIAL_STRING_TO_REPLACE_WITH_TABLE = @"$tableBatteryInfo$";

        public void CreateReport(string path, List<BatteryProperty> batteryInfo)
        {
            using (Document doc = new Document())
            {
                Section s = doc.AddSection();

                TextRange tr;
                tr = InsertNewParagraph("Отчет о состоянии батареи\n", s, HorizontalAlignment.Center);
                tr.CharacterFormat.FontName = "Arial";
                tr.CharacterFormat.FontSize = 22;
                tr.CharacterFormat.CharacterSpacing = 2;

                tr = InsertNewParagraph(DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"+'\n'), s, HorizontalAlignment.Center);
                tr.CharacterFormat.FontName = "Arial";
                tr.CharacterFormat.FontSize = 16;

                Table table = s.AddTable(true);
                FillOutTable(table, batteryInfo);

                doc.SaveToFile(path, FileFormat.Auto);
            }
            System.Diagnostics.Process.Start(path);
        }

        public void InsertTableIntoTemplate(string path, List<BatteryProperty> batteryInfo)
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

        private void FillOutTable(Table table, List<BatteryProperty> batteryInfo)
        {


            table.TableFormat.HorizontalAlignment = RowAlignment.Center;
            table.PreferredWidth = new PreferredWidth(WidthType.Percentage, 90);
            table.ResetCells(batteryInfo.Count, NAME_HEADERS_COLUMN.Length);
            //table.PreferredWidth = new PreferredWidth(WidthType.Auto, 0);

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

                int countProp = batteryInfo[i].GetType().GetProperties().Length;

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
            
                
                
            table.AutoFit(AutoFitBehaviorType.AutoFitToContents);
        }

        private TextRange InsertNewParagraph(string text, Section s, HorizontalAlignment hAlign)
        {
            Paragraph p = s.AddParagraph();
            p.Format.HorizontalAlignment = hAlign;
            TextRange tr = p.AppendText(text);
            return tr;
        }
    }
}
