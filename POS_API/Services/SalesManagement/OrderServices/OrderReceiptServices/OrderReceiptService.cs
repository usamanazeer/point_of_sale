using Models;
using Models.DTO.SalesManagement;
using POS_API.Services.UserManagement.CompanyServices;
using POS_API.Utilities.ReceiptPrinterUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
namespace POS_API.Services.SalesManagement.OrderServices.OrderReceiptServices
{
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    public class OrderReceiptService:IOrderReceiptService, IService
    {
        private readonly IReceiptPrinterUtility _receiptPrinterUtility;
        private readonly ICompanyService _companyService;
        public OrderReceiptService(IReceiptPrinterUtility receiptPrinterUtility, ICompanyService companyService)
        {
            _receiptPrinterUtility = receiptPrinterUtility;
            _companyService = companyService;
        }

        public async Task<bool> PrintOrder(SalesOrderMasterDto salesOrderMasterDto, short printQuantity = 1)
        {
            try
            {
                var company = await _companyService.GetCompanyById(salesOrderMasterDto.CompanyId/*, fromCache: true*/);
                var receipt = new ReceiptFormat();

                receipt.AddRow(new ReceiptRow(image: Image.FromFile(company.LogoPhysicalPath), paddingBottum: 20).AlignCentre());
                receipt.AddRow(new ReceiptRow($"OrderNo:", font: new Font(family: FontFamily.GenericMonospace,
                    emSize: 13.0f, style: FontStyle.Bold)).AlignCentre());
                receipt.AddRow(new ReceiptRow($"{salesOrderMasterDto.OrderNo}", font: new Font(family: FontFamily.GenericMonospace,
                    emSize: 15.0f, style: FontStyle.Bold)).AlignCentre());
                receipt.AddRow(new ReceiptRow(new List<string> { $"Order Type:", $"{salesOrderMasterDto.OrderTypeText}" }, 
                                              new List<int?>{ 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,
                    emSize: 8.0f, style: FontStyle.Bold)));
                if (salesOrderMasterDto.DiningTableId.HasValue)
                {
                    receipt.AddRow(new ReceiptRow(
                        new List<string>
                        {
                            $"Table:", $"{salesOrderMasterDto.DiningTable?.TableNo} {(salesOrderMasterDto.DiningTable != null ? $"({salesOrderMasterDto.DiningTable.Floor.Name})" : "")}"
                        }, new List<int?> { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace, emSize: 8.0f, style: FontStyle.Bold)));
                }

                if (salesOrderMasterDto.WaiterId.HasValue)
                {
                    receipt.AddRow(
                                   new ReceiptRow(new List<string>{ $"Waiter:", $"{(salesOrderMasterDto.Waiter != null ? salesOrderMasterDto.Waiter.Name : "")}" }, 
                                   new List<int?> { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,emSize: 8.0f,style: FontStyle.Bold), paddingBottum: 8));
                }

                receipt.AddRow(
                               new ReceiptRow(new List<string>{ $"Items ", $"qty." }, new List<int?>{ 0, 0 }, 
                                font: new Font(family: FontFamily.GenericMonospace, emSize: 9.0f,style: FontStyle.Bold), paddingBottum: 2)
                              );

                foreach (var item in salesOrderMasterDto.SalesOrderDetails)
                {
                    receipt.AddRow(
                                   new ReceiptRow(new List<string>{ $"{item.Item.Name}", $"{item.Quantity}" }, new List<int?>() { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,
                                                                                                                                                            emSize: 9.0f,
                                                                                                                                                            style: FontStyle.Regular))
                                  );
                    foreach (var modifier in item.SalesOrderItemModifiers)
                    {
                        receipt.AddRow(
                                       new ReceiptRow(new List<string> { $"  {modifier.Modifier.Name}", $"{modifier.Quantity}" }, new List<int?>() { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,
                                                                                                                                                                        emSize: 7.0f,
                                                                                                                                                                        style: FontStyle.Regular))
                                      );
                    }
                    receipt.AddRow(
                                   new ReceiptRow($"----------------------------------------------------------------------------------------------------------------------------------------", font: new Font(family: FontFamily.GenericMonospace,
                                                                                                   emSize: 2.0f,
                                                                                                   style: FontStyle.Bold), paddingBottum: 5)
                                  );
                }
                _receiptPrinterUtility.PrintReceipt(receiptFormat: receipt, company.OffDeskPrinter, printQuantity: printQuantity);
                return true;
            }
            catch (Exception )
            {
                return false;
            }
            
        }

        public async Task<bool> PrintSalesReceipt(SalesOrderMasterDto salesOrderMasterDto, short printQuantity = 1)
        {
            try
            {
                var dateTimeFormatWithDay = "ddd, dd-MMM-yyyy  hh:mm tt";
                var dateTimeFormat = "dd-MMM-yyyy  hh:mm tt";
                var company = await _companyService.GetCompanyById(salesOrderMasterDto.CompanyId/*,fromCache:true*/);
                var receipt = new ReceiptFormat();
                //header start
                receipt.AddRow(
                    new ReceiptRow(image: Image.FromFile(company.LogoPhysicalPath), paddingBottum: 20).AlignCentre()
                    );
                receipt.AddRow(
                       new ReceiptRow($"{company.Name}", font: new Font(family: FontFamily.GenericMonospace,
                                            emSize: 13.0f,
                                            style: FontStyle.Bold)).AlignCentre()
                   );
                
                if (company.MainBranch!=null)
                {
                    receipt.AddRow(
                                   new ReceiptRow($"{company.MainBranch.Address}", font: new Font(family: FontFamily.GenericMonospace,
                                                                                                  emSize: 7.0f,
                                                                                                  style: FontStyle.Regular)).AlignCentre()
                                  );
                    if (company.MainBranch.ShowPhoneOnBill)
                    {
                        receipt.AddRow(
                                       new ReceiptRow($"Phone: {company.MainBranch.Phone}", font: new Font(family: FontFamily.GenericMonospace,
                                                                                                 emSize: 7.0f,
                                                                                                 style: FontStyle.Regular)).AlignCentre()
                                      );
                    }
                    if (company.MainBranch.ShowMobileOnBill)
                    {
                        receipt.AddRow(
                                       new ReceiptRow($"Mobile: {company.MainBranch.Mobile}", 
                                       font: new Font(family: FontFamily.GenericMonospace, emSize: 7.0f, style: FontStyle.Regular)).AlignCentre()
                                      );
                    }
                }
                
                //header end
                receipt.AddRow(
                        new ReceiptRow($"-----------------------------", font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 10.0f,
                                             style: FontStyle.Bold),paddingBottum: 17)
                    );

                //receipt title
                receipt.AddRow(
                       new ReceiptRow($"Sales Receipt", font: new Font(family: FontFamily.GenericMonospace,
                                            emSize: 12.0f,
                                            style: FontStyle.Bold), paddingTop:7, paddingBottum: 7).AlignCentre()
                   );
                receipt.AddRow(
                        new ReceiptRow(new List<string> { $"Invoice #", $"{salesOrderMasterDto.OrderNo}" },
                        new List<int?>() { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 7.0f,
                                             style: FontStyle.Bold)).AlignCentre()
                    );
               
                receipt.AddRow(
                        new ReceiptRow(new List<string> { $"Operator Name:", $"{salesOrderMasterDto.CreatedByUser.FirstName} {salesOrderMasterDto.CreatedByUser.LastName}" }, new List<int?> { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 7.0f,
                                             style: FontStyle.Bold)).AlignCentre()
                    );
                receipt.AddRow(
                        new ReceiptRow(new List<string>{ $"Inv Date:", $"{ salesOrderMasterDto.SalesOrderBilling.CreatedOn.ToString(dateTimeFormatWithDay)}"}, new List<int?> { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 7.0f,
                                             style: FontStyle.Bold)).AlignCentre()
                    );
                receipt.AddRow(new ReceiptRow(new List<string> { $"Order Type:", $"{ salesOrderMasterDto.OrderTypeText}" }, new List<int?> { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,
                                                                                                                                                                                                                    emSize: 7.0f,
                                                                                                                                                                                                                    style: FontStyle.Bold)).AlignCentre()
                              );
                if (salesOrderMasterDto.DiningTableId.HasValue)
                {
                    receipt.AddRow(new ReceiptRow(new List<string> { $"Table No:", $"{ salesOrderMasterDto.DiningTable?.TableNo} ({ salesOrderMasterDto.DiningTable?.Floor?.Name})" }, new List<int?> { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,
                                                                                                                                                                            emSize: 7.0f,
                                                                                                                                                                            style: FontStyle.Bold)).AlignCentre()
                                  );
                }
                
                receipt.AddRow(
                        new ReceiptRow(new List<string> { $"Item", "Price", "Qty.", $"Amt." }, new List<int?> { 0,160, 210, 0 }, font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 7.0f,
                                             style: FontStyle.Bold), paddingBottum: 2, paddingTop: 18)
                    );
                foreach (var item in salesOrderMasterDto.SalesOrderDetails)
                {
                    receipt.AddRow(
                        new ReceiptRow(new List<string> { $"{item.Item.Name}", $"{item.Item.FinalSalesRate}", $"{item.Quantity}", $"{item.FinalSalesRate * item.Quantity}" }, new List<int?> { 0, 160, 210, 0 },
                        font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 7.0f,
                                             style: FontStyle.Bold), paddingBottum: 2)
                    );

                    foreach (var modifier in item.SalesOrderItemModifiers)
                    {
                        receipt.AddRow(
                                       new ReceiptRow(new List<string> { $"   {modifier.Modifier.Name}", $"{modifier.Modifier.ModifierCharges}", $"{modifier.Quantity}", $"{modifier.Charges * modifier.Quantity}" }, new List<int?> { 0, 160, 210, 0 },
                                                      font: new Font(family: FontFamily.GenericMonospace,emSize: 7.0f,style: FontStyle.Bold))
                                      );

                    }
                    receipt.AddRow( new ReceiptRow($"----------------------------------------------------------------------------------------------------------------------------------------", 
                                   font: new Font(family: FontFamily.GenericMonospace,emSize: 2.0f,style: FontStyle.Bold), paddingBottum: 5));
                }
                receipt.AddRow(
                        new ReceiptRow(new List<string> { $"Sub Total", $"{salesOrderMasterDto.GetOrderAmount()}" }, new List<int?>() { 0, 0 }, 
                        font: new Font(family: FontFamily.GenericMonospace,emSize: 9.0f,style: FontStyle.Bold), paddingTop: 15)
                    );
                if (salesOrderMasterDto.DiscountAmount is > 0 ) {
                    receipt.AddRow(
                        new ReceiptRow(
                           new List<string> { 
                                $"Discount {(salesOrderMasterDto.IsDiscountInPercent == true ? "(" + salesOrderMasterDto.DiscountAmount + "%)" : "")}", 
                                $"-{salesOrderMasterDto.GetDiscount().ToNDecimalPlaces(2)}"
                            }, 
                           new List<int?>{ 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,emSize: 9.0f,style: FontStyle.Bold)));
                }
                receipt.AddRow(
                    new ReceiptRow(
                        new List<string>{ $"Net Amount", $"{salesOrderMasterDto.GetOrderAmountWithDiscount().ToNDecimalPlaces(2) }" },
                        new List<int?> { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace, emSize: 9.0f, style: FontStyle.Bold))
                              );
                if (salesOrderMasterDto.SalesOrderBilling.Tax!=null)
                {
                    receipt.AddRow(
                   new ReceiptRow(new List<string>
                  {
                      $"{salesOrderMasterDto.SalesOrderBilling.Tax.Name} {(salesOrderMasterDto.SalesOrderBilling.IsTaxInPercent == true? $"({salesOrderMasterDto.SalesOrderBilling.Tax.Amount}%)" : "")}",
                      $"{salesOrderMasterDto.SalesOrderBilling.TaxAmount}"
                  }, new List<int?>{ 0, 0 }, font: new Font(family: FontFamily.GenericMonospace, emSize: 9.0f, style: FontStyle.Bold), paddingTop: 15)
                  );
                }
                else
                {
                    receipt.AddRow(
                                   new ReceiptRow(new List<string> { $"Tax", $"0.00" }, new List<int?> { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,
                                         emSize: 9.0f,style: FontStyle.Bold), paddingTop: 15)
                                  );
                }

                receipt.AddRow(
                        new ReceiptRow(new List<string> { $"Total", $"{salesOrderMasterDto.GetOrderAmountAfterTax()}" }, new List<int?> { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 9.0f,
                                             style: FontStyle.Bold))
                    );


                receipt.AddRow(
                        new ReceiptRow(new List<string> { $"Cash Paid", $"{(salesOrderMasterDto.SalesOrderBilling.CashReceived)??0}" }, new List<int?> { 0, 0 }, font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 9.0f,
                                             style: FontStyle.Bold)/*, paddingTop: 15*/)
                    );
                receipt.AddRow(
                        new ReceiptRow(new List<string>{ $"Cash Back", $"{salesOrderMasterDto.SalesOrderBilling.CashReturn ?? 0}" },
                        new List<int?> { 0, 0 },
                        font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 10.0f,
                                             style: FontStyle.Bold))
                    );



                receipt.AddRow(
                        new ReceiptRow($"Data Entry Date: {salesOrderMasterDto.CreatedOn.ToString(dateTimeFormat)}", font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 7.0f,
                                             style: FontStyle.Regular), paddingTop: 20).AlignCentre()
                    );
                receipt.AddRow(
                        new ReceiptRow($"Print Date: {DateTime.Now.ToString(dateTimeFormat)}", font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 7.0f,
                                             style: FontStyle.Regular)).AlignCentre()
                    );
                receipt.AddRow(
                        new ReceiptRow($"Thanks to visit {company.Name}.", font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 7.0f,
                                             style: FontStyle.Bold)).AlignCentre()
                    );
                receipt.AddRow(
                        new ReceiptRow($"-----------------------------", font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 10.0f,
                                             style: FontStyle.Bold), paddingTop: 10)
                    );
                receipt.AddRow(
                        new ReceiptRow($"Powered By: {Info.PoweredBy}", font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 7.0f,
                                             style: FontStyle.Regular)).AlignCentre()
                    );
                receipt.AddRow(
                        new ReceiptRow($"{Info.Website}", font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 7.0f,
                                             style: FontStyle.Regular)).AlignCentre()
                    );
                receipt.AddRow(
                        new ReceiptRow($"Contact: {Info.Mobile}", font: new Font(family: FontFamily.GenericMonospace,
                                             emSize: 7.0f,
                                             style: FontStyle.Regular)).AlignCentre()
                    );
                receipt.AddRow(
                               new ReceiptRow($"UAN: {Info.UAN}", font: new Font(family: FontFamily.GenericMonospace,
                                                                                          emSize: 7.0f,
                                                                                          style: FontStyle.Regular)).AlignCentre()
                              );
                _receiptPrinterUtility.PrintReceipt(receipt, company.OnDeskPrinter, printQuantity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
