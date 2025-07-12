using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Accounts;
using Pos_WebApp.Services.AccountsManagement.AccountsServices;

namespace Pos_WebApp.Areas.AccountsManagement.ViewComponents
{
    public class JournalEntryRow : ViewComponent
    {
        private readonly IAccountsService _accountsService;
        public JournalEntryRow(IAccountsService accountsService) => _accountsService = accountsService;
        public async Task<IViewComponentResult> InvokeAsync(Tuple<int, AccTransactionDetailDto> data)
        {
            var token = HttpContext.Session.GetString("token");

            if (token != null)
                ViewBag.Accounts = await _accountsService.GetAccountsSelectList(token, selectForManualTransactions: true);
            return View(data);
        }
    }
}
