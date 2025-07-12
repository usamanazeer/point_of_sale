using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.DTO.UserManagement;

namespace Pos_WebApp.Services.GeneralSettings.TaxServices.UtilityServices
{
    public interface IUtilityService
    {
        Task<IList<PrinterInfo>> GetPrinters(string token);
        Task<Response> GetPrintersResponse(string token);
    }
}
