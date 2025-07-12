using Models;

namespace Pos_WebApp.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public Response Response { get; set; }
        public string BackURL { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        
    }
}
