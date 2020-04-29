using System;
using System.Collections.Generic;
using System.IO;
using BatteryChecker.ViewModel;
using iText.Layout;
using iText.Kernel.Pdf;
using iText.Kernel.Font;
using iText.Layout.Element;

/// <summary>
/// Namespace for creating reports with battery information
/// </summary>
namespace BatteryChecker.Model.Reports
{
    /// <summary>
    /// Class for creating PDF reports 
    /// </summary>
    public class PdfReportCreator: IReportCreator
    {
        /// <summary>
        /// Count columns in talbe with battery information, which will be inserted into pdf file
        /// </summary>
        private const int COUNT_TABLE_COLUMNS = 2;

        /// <summary>
        /// Create pdf report 
        /// </summary>
        /// <param name="path">path to file</param>
        /// <param name="batteryInfo">battery information</param>
        public void CreateReport(string path, List<BatteryChecker.ViewModel.BatteryProperty> batteryInfo)
        {
            try
            {
                // create low-level abstract object - writer
                using (PdfWriter writer = new PdfWriter(path))
                {
                    // create low-level abstract object - PdfDocument
                    using (PdfDocument pdfDoc = new PdfDocument(writer))
                    {
                        // create high-level abstract object - Document (using for inserting information in pdf file)
                        Document doc = new Document(pdfDoc);

                        //create font from file arial.ttf
                        PdfFont fontText = PdfFontFactory.CreateFont(Path.Combine(Environment.CurrentDirectory, "arial.ttf"), iText.IO.Font.PdfEncodings.IDENTITY_H, true);

                        doc.SetMargins(30, 10, 20, 20);

                        doc.Add(new Paragraph("Отчет о состоянии батареи").SetFont(fontText)
                                                                          .SetFontSize(22)
                                                                          .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                                                          .SetWordSpacing(2)
                                                                          .SetCharacterSpacing(4));
                        doc.Add(new Paragraph(DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")).SetFont(fontText)
                                                                                                   .SetFontSize(16)
                                                                                                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                        Table table = new Table(COUNT_TABLE_COLUMNS);
                        table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                        table.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        table.SetWidth(450);
                        table.IsKeepTogether();

                        table.SetFont(fontText);

                        // add table headers
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Свойство").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                                                                    .SetFont(fontText)
                                                                                    .SetBold()
                                                                                    .SetFontSize(16)));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Значение").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                                                                    .SetFont(fontText)
                                                                                    .SetBold()
                                                                                    .SetFontSize(16)));

                        // fill out table
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
            catch (IOException)
            {
                throw new IOException("Не удалось получить доступ к файлу, возможно он открыт в другом приложении\n");
            }
            catch (Exception e)
            {
                throw new Exception("Неопознання ошибка! \n Системное описание ошибки:" + e.Message);
            }
        }
    }
}
