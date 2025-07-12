using System.Net.NetworkInformation;

namespace Utilities.SystemUtil
{
    public class SystemUtility : ISystemUtility
    {
        public string GetMacAddress()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            var sMacAddress = string.Empty;
            foreach (var adapter in networkInterfaces)
                if (sMacAddress == string.Empty) // only return MAC Address from first card  
                    //IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
            return sMacAddress;
        }
    }

    public interface ISystemUtility
    {
        string GetMacAddress();
    }
}