using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Management;
using Models;

namespace POS_API.Utilities.ReceiptPrinterUtilities
{
    public class ReceiptPrinterUtility : IReceiptPrinterUtility
    {
        private ReceiptFormat _receipt;


        //public bool IsPrinterInstalled()
        //{

        //    var ps = PrinterSettings.InstalledPrinters;
        //    return true;
        //}
        public void PrintReceipt(ReceiptFormat receiptFormat, string printerName, short printQuantity = 1)
        {
            _receipt = receiptFormat;
            var printDoc = new PrintDocument { PrinterSettings = { PrinterName = printerName, Copies = printQuantity } };
            var printController = new StandardPrintController();
            printDoc.PrintController = printController;
            if (!printDoc.PrinterSettings.IsValid)
                throw new Exception(message: "Error: cannot find the default printer.");
            if (!string.IsNullOrEmpty(printerName))
            {
                printDoc.PrintPage += PrintPage;
                printDoc.Print();
            }
            
        }


        public IList<PrinterInfo> GetPrintersList()
        {
            var printersList = new List<PrinterInfo>();
            var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
            foreach (var printer in printerQuery.Get())
            {
                var name = printer.GetPropertyValue("Name");
                var status = printer.GetPropertyValue("Status");
                var isDefault = printer.GetPropertyValue("Default");
                var isNetworkPrinter = printer.GetPropertyValue("Network");
                printersList.Add(new PrinterInfo((string)name, (string)status, (bool)isDefault, (bool)isNetworkPrinter));
            }
            return printersList;
        }


        


        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            var currentHeight = 8;
            try
            {
                var graphics = e.Graphics;

                var stringFormatCenter = new StringFormat
                                         {
                                             LineAlignment = StringAlignment.Center,
                                             Alignment = StringAlignment.Center
                                         };

                var stringFormatLeft = new StringFormat
                                       {
                                           LineAlignment = StringAlignment.Center,
                                           Alignment = StringAlignment.Near
                                       };

                var stringFormatRight = new StringFormat
                                        {
                                            LineAlignment = StringAlignment.Center,
                                            Alignment = StringAlignment.Far
                                        };

                var fontSizeRegular7 = new Font(family: FontFamily.GenericSansSerif,
                                                emSize: 7.0f,
                                                style: FontStyle.Regular);

                //var fontSizeBold9 = new Font(family: FontFamily.GenericSansSerif,
                //                             emSize: 9.0f,
                //                             style: FontStyle.Regular);

                //var fontSizeBold12 = new Font(family: FontFamily.GenericSansSerif,
                //                              emSize: 12.0f,
                //                              style: FontStyle.Bold);

                //var fontSizeBold30 = new Font(family: FontFamily.GenericSansSerif,
                //                              emSize: 30.0f,
                //                              style: FontStyle.Bold);

                var defaultFont = fontSizeRegular7;

                foreach (var row in _receipt.ReceiptRows)
                {
                    currentHeight += row.PaddingTop;
                    if (row.IsImageRow)
                    {
                        graphics.DrawImage(image: row.Image,
                                           x: GetXForCentre(printPageEventArgs: e,image: row.Image),
                                           y: currentHeight,
                                           width: row.Width ?? row.Image.Width,
                                           height: row.Height ?? row.Image.Height);

                        currentHeight += row.Height ?? row.Image.Height;
                    }
                    else
                    {
                        row.Font ??= defaultFont;
                        if (!row.IsMultiTextRow)
                            graphics.DrawString(s: row.Text,
                                                font: row.Font ?? defaultFont,
                                                brush: row.Brush ?? Brushes.Black,
                                                layoutRectangle: new Rectangle(x: row.PaddingLeft,
                                                                               y: currentHeight,
                                                                               width: e.PageBounds.Width,
                                                                               height: 0),
                                                format: row.AlignmentBIT == null ? stringFormatCenter :
                                                row.AlignmentBIT == false ? stringFormatLeft : stringFormatRight
                                               );
                        else
                            for (var i = 0; i < row.RowTexts.Count; i++)
                            {
                                if (i == row.RowTexts.Count - 1)
                                {
                                    graphics.DrawString(s: row.RowTexts[index: i],
                                                        font: row.Font,
                                                        brush: Brushes.Black,
                                                        layoutRectangle: new Rectangle(x: 0,
                                                                                       y: currentHeight,
                                                                                       width: e.PageBounds.Width,
                                                                                       height: 0),
                                                        format: stringFormatRight);
                                    break;
                                }

                                graphics.DrawString(s: row.RowTexts[index: i],
                                                    font: row.Font,
                                                    brush: Brushes.Black,
                                                    layoutRectangle: new
                                                        Rectangle(x: row.RowTextLeftPaddings[index: i] ?? 0,
                                                                  y: currentHeight,
                                                                  width: e.PageBounds.Width,
                                                                  height: 0),
                                                    format: stringFormatLeft);
                            }

                        //graphics.DrawString(s: row.Text1,
                        //            font: row.Font,
                        //            brush: Brushes.Black,
                        //            layoutRectangle: new Rectangle(x: row.PaddingLeft,
                        //                                           y: currentHeight,
                        //                                           width: e.PageBounds.Width,
                        //                                           height: 0),
                        //            format: stringFormatLeft);

                        //graphics.DrawString(s: row.Text2,
                        //           font: row.Font,
                        //           brush: Brushes.Black,
                        //           layoutRectangle: new Rectangle(x: -row.PaddingRight,
                        //                                          y: currentHeight,
                        //                                          width: e.PageBounds.Width,
                        //                                          height: 0),
                        //           format: stringFormatRight);

                        currentHeight += Convert.ToInt32(value: Math.Ceiling(a: row.Font.GetHeight()));
                    }

                    currentHeight += row.PaddingBottum;
                }

                //fontSizeRegular7.Dispose();
                //fontSizeBold9.Dispose();
                defaultFont.Dispose();
                //fontSizeBold30.Dispose();

                // Check to see if more pages are to be printed.
                e.HasMorePages = false;
            }
            catch (Exception)
            {
                // ignored
            }

            static float GetXForCentre(PrintPageEventArgs printPageEventArgs, Image image) 
                => (Convert.ToSingle(value: printPageEventArgs.PageBounds.Width) - Convert.ToSingle(value: image.Width)) / 2;
        }
    }
}