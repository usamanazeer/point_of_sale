namespace Models
{
    public class PrinterInfo
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public bool IsDefault { get; set; }
        public bool IsNetworkPrinter { get; set; }
        public PrinterInfo() { }


        public PrinterInfo(string name, string status = "", bool isDefault = false, bool isNetworkPrinter = false)
        {
            Name = name;
            Status = status;
            IsDefault = isDefault;
            IsNetworkPrinter = isNetworkPrinter;
        }
    }
}
