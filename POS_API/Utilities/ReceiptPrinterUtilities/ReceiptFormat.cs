using System.Collections.Generic;

namespace POS_API.Utilities.ReceiptPrinterUtilities
{
    public class ReceiptFormat
    {
        private readonly List<ReceiptRow> _receiptRows;

        public List<ReceiptRow> ReceiptRows => _receiptRows;

        public ReceiptFormat() => _receiptRows = new List<ReceiptRow>();


        public void AddRow(ReceiptRow receipt) => _receiptRows.Add(receipt);


        public void AddRowRange(List<ReceiptRow> receipt) => _receiptRows.AddRange(receipt);


        public void ClearRows(List<ReceiptRow> receipt) => _receiptRows.Clear();
    }
}
