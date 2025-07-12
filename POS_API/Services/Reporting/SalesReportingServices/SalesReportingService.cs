using Models;
using Models.DTO.Reporting.Sales;
using Models.Enums;
using POS_API.Repositories.Reporting.SalesReportingRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_API.Services.Reporting.SalesReportingServices
{
    public class SalesReportingService : ISalesReportingService, IService
    {
        private readonly ISalesReportingRepository _salesReportingRepository;
        public SalesReportingService(ISalesReportingRepository salesReportingRepository) => _salesReportingRepository = salesReportingRepository;
        
        public async Task<Response> GetItemSales(RptSalesSalesReportDto reportFormat)
        {
            var response = new Response();
            var resData = await _salesReportingRepository.GetItemSales(reportFormat);
            if (resData.Any())
            {
                
                response.Model = FormatReport(resData,reportFormat);
                response.ResponseCode = StatusCodes.OK.ToInt();
            }
            else
            {
                response.Model = resData;
                response.ResponseCode = StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "No Sales Found.";
            }
            return response;
        }
        public async Task<Response> GetItemSales_ByItems(RptSalesSalesReportDto reportFormat)
        {
            var response = new Response();
            var resData = await _salesReportingRepository.GetItemSales_ByItems(reportFormat);
            if (resData.Any())
            {
                //var formattedData = FormatReport(resData, reportFormat);
                //formattedData.SalesDataList;
                response.Model = FormatReport(resData, reportFormat);
                response.ResponseCode = StatusCodes.OK.ToInt();
            }
            else
            {
                response.Model = resData;
                response.ResponseCode = StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "No Sales Found.";
            }
            return response;
        }


        public async Task<Response> GetSalesAmount(RptSalesSalesReportDto filters)
        {
            var response = new Response();
            var resData = await _salesReportingRepository.GetSalesAmount(filters);
            response.Model = resData;
            response.ResponseCode = StatusCodes.OK.ToInt();
            return response;
        }


        public async Task<Response> GetSales_ByDeliveryServices(RptSalesSalesReportDto filters)
        {
            var response = new Response();
            var resData = await _salesReportingRepository.GetSales_ByDeliveryServices(filters);
            if (resData.Count > 0)
            {

                response.Model = FormatReport(resData, filters);
                response.ResponseCode = StatusCodes.OK.ToInt();
            }
            else
            {
                response.Model = resData;
                response.ResponseCode = StatusCodes.Not_Found.ToInt();
                response.ResponseMessage = "No Sales Found.";
            }
            return response;
        }


        //public async Task<Response> GetTaxCollectedAmount(RptSalesSalesReportDto filters)
        //{
        //    var response = new Response();
        //    var resData = await _salesReportingRepository.GetTaxCollectedAmount(filters);
        //    response.Model = resData;
        //    response.ResponseCode = StatusCodes.OK.ToInt();
        //    return response;
        //}


        private RptSalesSalesReportDto FormatReport(List<RptSalesSalesReportRowDto> salesDataList, RptSalesSalesReportDto reportFormat)
        {
            salesDataList ??= new List<RptSalesSalesReportRowDto>();
            reportFormat.Labels = GetReportLabels(reportFormat);
            var startdate = reportFormat.StartDate;
            var enddate = reportFormat.EndDate;
            //var currentdate = new DateTime(startdate.Year, startdate.Month, startdate.Day);

            reportFormat.SalesDataList = new List<List<RptSalesSalesReportRowDto>>();
            var dataSets = salesDataList.GroupBy(x => x.ItemId).Select(x=>x.ToList()).ToList();
            foreach (var data in dataSets)
            {
                var startData = data[0];
                var currentdate = new DateTime(startdate.Year, startdate.Month, startdate.Day);
                var whileFlag = true;
                while (whileFlag)
                {
                    var newRow = new RptSalesSalesReportRowDto() 
                    {
                        ItemId = startData.ItemId,
                        ItemName = startData.ItemName,
                        //DeliveryServiceId = StartData.DeliveryServiceId,
                        //DeliveryServiceName = StartData.DeliveryServiceName,
                        
                        TotalSales = 0,
                        SalesDate = new DateTime(currentdate.Year, currentdate.Month, day: currentdate.Day)
                    };
                    switch (reportFormat.DateGroupByFilter.ToLower())
                    {
                        case "day":
                            newRow.FormattedDate = currentdate.ToString("dd-MMM-yyyy");
                            if (data.Count(x => x.SalesDate.ToString("dd-MM-yyyy") == currentdate.ToString("dd-MM-yyyy")) == 0)
                                data.Add(newRow);
                            currentdate = currentdate.AddDays(1);
                            if (currentdate.Date > enddate.Date)
                            {
                                reportFormat.SalesDataList.Add(data.OrderBy(x => x.SalesDate).ToList());
                                whileFlag = false;
                            }
                            break;
                        case "month":
                            newRow.FormattedDate = currentdate.ToString("MMM-yyyy");
                            if (data.Count(x => x.SalesDate.ToString("MMM-yyyy") == currentdate.ToString("MMM-yyyy")) == 0)
                                data.Add(newRow);
                            currentdate = currentdate.AddMonths(1);
                            if ((currentdate.Date.Year >= enddate.Date.Year && currentdate.Date.Month > enddate.Date.Month) || (currentdate.Date.Year > enddate.Date.Year))
                            {
                                reportFormat.SalesDataList.Add(data.OrderBy(x => x.SalesDate).ToList());
                                whileFlag = false;
                            }
                            break;
                        case "year":
                            newRow.FormattedDate = currentdate.ToString("yyyy");
                            if (data.Count(x => x.SalesDate.ToString("yyyy") == currentdate.ToString("yyyy")) == 0)
                            {
                                data.Add(newRow);
                            }
                            currentdate = currentdate.AddYears(1);
                            if (currentdate.Date.Year > enddate.Date.Year)
                            {
                                reportFormat.SalesDataList.Add(data.OrderBy(x => x.SalesDate).ToList());
                                whileFlag = false;
                            }
                            break;
                    }
                }
            }
            return reportFormat;
        }
        private List<string> GetReportLabels(RptSalesSalesReportDto reportFormat)
        {
            var startdate = reportFormat.StartDate;
            var enddate = reportFormat.EndDate;
            var labels = new List<string>();
            var currentdate = new DateTime(startdate.Year, startdate.Month, startdate.Day);
            var whileFlag = true;
            while (whileFlag)
            {
                switch (reportFormat.DateGroupByFilter.ToLower())
                {
                    case "day":
                        labels.Add(currentdate.ToString("dd-MMM-yyyy"));
                        currentdate = currentdate.AddDays(1);
                        if (currentdate.Date > enddate.Date) whileFlag = false;
                        break;
                    case "month":
                        labels.Add(currentdate.ToString("MMM-yyyy"));
                        currentdate = currentdate.AddMonths(1);
                        if ((currentdate.Date.Year >= enddate.Date.Year && currentdate.Date.Month > enddate.Date.Month) || (currentdate.Date.Year > enddate.Date.Year))
                            whileFlag = false;
                        break;
                    case "year":
                        labels.Add(currentdate.ToString("yyyy"));
                        currentdate = currentdate.AddYears(1);
                        if (currentdate.Date.Year > enddate.Date.Year)
                            whileFlag = false;
                        break;
                }
            }
            return labels;
        }
    }
}
