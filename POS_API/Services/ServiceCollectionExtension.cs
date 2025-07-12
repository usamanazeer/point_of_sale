//using Microsoft.Extensions.DependencyInjection;
//using POS_API.Services.AccountsManagement.AccountServices;
//using POS_API.Services.AccountsManagement.BillsServices;
//using POS_API.Services.AccountsManagement.FiscalYearServices;
//using POS_API.Services.AccountsManagement.JournalServices;
//using POS_API.Services.DeliveryService.DeliveryBoyServices;
//using POS_API.Services.DeliveryService.DeliveryServiceVendorServices;
//using POS_API.Services.GeneralSettings.TaxServices;
//using POS_API.Services.InventoryManagement.BrandServices;
//using POS_API.Services.InventoryManagement.CategoryServices;
//using POS_API.Services.InventoryManagement.ColorServices;
//using POS_API.Services.InventoryManagement.GoodsReceivedNoteServices;
//using POS_API.Services.InventoryManagement.GoodsReturnNoteServices;
//using POS_API.Services.InventoryManagement.ItemBarCodeServices;
//using POS_API.Services.InventoryManagement.ItemServices;
//using POS_API.Services.InventoryManagement.ModifierServices;
//using POS_API.Services.InventoryManagement.PhysicalInventory;
//using POS_API.Services.InventoryManagement.PhysicalInventory.PhysicalInventoryServices;
//using POS_API.Services.InventoryManagement.PurchaseOrderServices;
//using POS_API.Services.InventoryManagement.PurchaseServices;
//using POS_API.Services.InventoryManagement.SizeServices;
//using POS_API.Services.InventoryManagement.UnitServices;
//using POS_API.Services.InventoryManagement.VendorServices;
//using POS_API.Services.NotificationsManagement;
//using POS_API.Services.Reporting.AccountsReportingServices;
//using POS_API.Services.Reporting.SalesReportingServices;
//using POS_API.Services.RestaurantManagement.DiningTableServices;
//using POS_API.Services.RestaurantManagement.RestaurantFloorsServices;
//using POS_API.Services.RestaurantManagement.WaitersServices;
//using POS_API.Services.SalesManagement.OrderServices;
//using POS_API.Services.SalesManagement.OrderServices.OrderReceiptServices;
//using POS_API.Services.SalesManagement.PosServices;
//using POS_API.Services.UserManagement.BranchServices;
//using POS_API.Services.UserManagement.CompanyServices;
//using POS_API.Services.UserManagement.RightsServices;
//using POS_API.Services.UserManagement.RolesServices;
//using POS_API.Services.UserManagement.UserServices;

//namespace POS_API.Services
//{
//    public static class ServiceCollectionExtension
//    {
//        public static IServiceCollection AddServices(this IServiceCollection services)
//        {


//            //services.Scan(x => x.FromAssemblyOf<IService>().AddClasses(@classes => @classes.AssignableTo<IService>()).AsImplementedInterfaces().WithTransientLifetime());
//            //services

//            //    //notification management services
//            //    .AddTransient<INotificationService, NotificationService>()



//            //    //general settings services
//            //    .AddTransient<ITaxService, TaxService>()




//            //    //user services
//            //    //.AddTransient<IUserService, UserService>()
//            //    //.AddTransient<IRolesService, RolesService>()
//            //    //.AddTransient<ICompanyService, CompanyService>()
//            //    .AddTransient<IRightsService, RightsService>()
//            //    .AddTransient<IBranchService, BranchService>()




//            //    //inventory services
//            //    .AddTransient<IVendorService, VendorService>()
//            //    .AddTransient<IMainCategoryService, MainCategoryService>()
//            //    .AddTransient<ISubCategoryService, SubCategoryService>()

//            //    .AddTransient<IBrandService, BrandService>() 
//            //    .AddTransient<IColorService, ColorService>()
//            //    .AddTransient<ISizeService, SizeService>()
//            //    .AddTransient<IUnitService, UnitService>()

//            //    .AddTransient<IItemService, ItemService>()
//            //    .AddTransient<IItemBarCodeService, ItemBarCodeService>()
//            //    .AddTransient<IModifierService, ModifierService>()

//            //    .AddTransient<IPhysicalInventoryService, PhysicalInventoryService>()
//            //    .AddTransient<IStockNotificationManager, StockNotificationManager>()

//            //    .AddTransient<IPOService, POService>()
//            //    .AddTransient<IGrnService, GrnService>()
//            //    .AddTransient<IGrrnService, GrrnService>()
//            //    .AddTransient<IPosService, PosService>()

//            //    .AddTransient<IPurchaseService, PurchaseService>()





//            //    //Restaurant Services
//            //    .AddTransient<IRestaurantFloorsService, RestaurantFloorsService>()
//            //    .AddTransient<IDiningTableService, DiningTableService>()
//            //    .AddTransient<IWaitersService, WaitersService>()


//            //    //sales services
//            //    .AddTransient<IOrderService,OrderService>()
//            //    .AddTransient<IOrderReceiptService, OrderReceiptService>()



//            //    //reporting
//            //    .AddTransient<ISalesReportingService, SalesReportingService>()
//            //    .AddTransient<IAccountsReportingService, AccountsReportingService>()


//            //    //delivery services
//            //    .AddTransient<IDeliveryServiceVendorService, DeliveryServiceVendorService>()
//            //    .AddTransient<IDeliveryBoyService, DeliveryBoyService>()



//            //    //AccountsManagement Services
//            //    .AddTransient<IAccountService, AccountService>()
//            //    .AddTransient<IJournalService, JournalService>()
//            //    .AddTransient<IFiscalYearService, FiscalYearService>()
//            //    .AddTransient<IBillsService, BillsService>()



//                ;
//            return services;
//        }
//    }
//}
