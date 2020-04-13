using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using BatteryChecker.ViewModel;
using iText.Layout;
using iText.Kernel.Pdf;
using iText.Kernel.Font;
using iText.Layout.Font;
using iText.Layout.Element;

namespace BatteryChecker.Model
{
    class PdfReportCreator: IReportCreator
    {
        private const int COUNT_TABLE_COLUMNS = 2;

        public PdfReportCreator() { }

        public void CreateReport(string path, List<BatteryChecker.ViewModel.BatteryProperty> batteryInfo)
        {
            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdfDoc = new PdfDocument(writer);
            Document doc = new Document(pdfDoc);
            PdfFont fontText = PdfFontFactory.CreateFont(Path.Combine(Environment.CurrentDirectory, "arial.ttf"), iText.IO.Font.PdfEncodings.IDENTITY_H, true);
            PdfFont fontHeader = PdfFontFactory.CreateFont(Path.Combine(Environment.CurrentDirectory, "arial.ttf"), iText.IO.Font.PdfEncodings.IDENTITY_H, true);

            doc.SetMargins(30, 10, 20, 20);

            doc.Add(new Paragraph("Отчет о состоянии батареи").SetFont(fontText)
                                                              .SetFontSize(22)
                                                              .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                                              .SetWordSpacing(2)
                                                              .SetCharacterSpacing(4));
            doc.Add(new Paragraph(DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")).SetFont(fontText)
                                                                                       .SetFontSize(16)
                                                                                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            Table table = new Table(2);
            table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
            table.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
            table.SetWidth(450);  // iText.Layout.Properties.UnitValue.CreatePercentArray(new float[] { 60f, 40f }));
            table.IsKeepTogether();

            table.SetFont(fontText);
            table.AddHeaderCell(new Cell().Add(new Paragraph("Свойство").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                                                        .SetFont(fontText)
                                                                        .SetBold()
                                                                        .SetFontSize(16)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Значение").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                                                        .SetFont(fontText)
                                                                        .SetBold()
                                                                        .SetFontSize(16)));

            foreach (BatteryProperty bp in batteryInfo)
            {
                Cell cellName = new Cell();
                cellName.SetFont(fontText);
                cellName.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                cellName.Add(new Paragraph(bp.Name).SetFont(fontText));

                Cell cellVal = new Cell();
                cellVal.SetFont(fontText);
                cellVal.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                cellVal.Add(new Paragraph(bp.Value).SetFont(fontText));

                table.AddCell(cellName).SetFont(fontText);
                table.AddCell(cellVal).SetFont(fontText);
            }

            doc.Add(table);
            doc.Close();
        }
    }
}
