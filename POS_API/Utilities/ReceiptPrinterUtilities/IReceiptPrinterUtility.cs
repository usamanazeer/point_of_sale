using System.Collections.Generic;
using Models;

namespace POS_API.Utilities.ReceiptPrinterUtilities
{
    public interface IReceiptPrinterUtility
    {
        void PrintReceipt(ReceiptFormat receiptFormat, string printerName, short printQuantity = 1);
        IList<PrinterInfo> GetPrintersList();
    }
}
