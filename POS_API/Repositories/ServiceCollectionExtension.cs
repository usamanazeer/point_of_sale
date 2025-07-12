//using Microsoft.Extensions.DependencyInjection;
//using POS_API.Repositories.AccountsManagement.AccountRepositories;
//using POS_API.Repositories.AccountsManagement.AccountTransactionRepositories;
//using POS_API.Repositories.AccountsManagement.BillsRepositories;
//using POS_API.Repositories.AccountsManagement.FiscalYearRepositories;
//using POS_API.Repositories.DeliveryService.DeliveryBoyRepos;
//using POS_API.Repositories.DeliveryService.DeliveryServiceVendorRepos;
//using POS_API.Repositories.GeneralSettings.TaxRepos;
//using POS_API.Repositories.InventoryManagement.BrandRepos;
//using POS_API.Repositories.InventoryManagement.CategoryRepos;
//using POS_API.Repositories.InventoryManagement.ColorRepos;
//using POS_API.Repositories.InventoryManagement.GoodsReceivedNoteRepos;
//using POS_API.Repositories.InventoryManagement.GoodsReturnNoteRepos;
//using POS_API.Repositories.InventoryManagement.ItemBarCodeRepos;
//using POS_API.Repositories.InventoryManagement.ItemRepos;
//using POS_API.Repositories.InventoryManagement.ModifierRepos;
//using POS_API.Repositories.InventoryManagement.PhysicalInventoryRepos;
//using POS_API.Repositories.InventoryManagement.PurchaseOrderRepos;
//using POS_API.Repositories.InventoryManagement.PurchaseRepositories;
//using POS_API.Repositories.InventoryManagement.SizeRepos;
//using POS_API.Repositories.InventoryManagement.UnitRepos;
//using POS_API.Repositories.InventoryManagement.VendorRepos;
//using POS_API.Repositories.MemoryCache;
//using POS_API.Repositories.NotificationsManagement;
//using POS_API.Repositories.Reporting.AccountsReportingRepos;
//using POS_API.Repositories.Reporting.SalesReportingRepositories;
//using POS_API.Repositories.RestaurantManagement.DiningTableRepos;
//using POS_API.Repositories.RestaurantManagement.RestaurantFloorsRepos;
//using POS_API.Repositories.RestaurantManagement.WaitersRepos;
//using POS_API.Repositories.SalesManagement.OrderRepos;
//using POS_API.Repositories.UserManagement.BranchRepos;
//using POS_API.Repositories.UserManagement.CompanyRepos;
//using POS_API.Repositories.UserManagement.RightsRepos;
//using POS_API.Repositories.UserManagement.RoleRepos;
//using POS_API.Repositories.UserManagement.UserRepos;
////using ReportingUnit = POS_API.Repositories.Reporting.SalesReportingRepositories.ReportingUnit;

//namespace POS_API.Repositories
//{
//    public static class ServiceCollectionExtension
//    {
//        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
//        {

            
            

//            //services
//            //     //cache repo
//            //    .AddTransient<IMemoryCacheUtil, MemoryCacheUtil>()



//            //    //notification management
//            //    .AddTransient<INotificationRepository, NotificationRepository>()
                

//            //    //general settings repos
//            //    .AddTransient<ITaxRepository, TaxRepository>()

//            //    //User Repos
//            //    .AddTransient<IUserRepository, UserRepository>()
//            //    .AddTransient<IBranchRepository, BranchRepository>()
//            //    .AddTransient<IRoleRepository, RoleRepository>()
//            //    .AddTransient<ICompanyRepository, CompanyRepository>()
//            //    .AddTransient<IRightsRepository, RightsRepository>()

//            //    //Inventory Repos
//            //    .AddTransient<IVendorRepository, VendorRepository>()
//            //    .AddTransient<IMainCategoryRepository, MainCategoryRepository>()
//            //    .AddTransient<ISubCategoryRepository, SubCategoryRepository>()

//            //    .AddTransient<IBrandRepository, BrandRepository>()
//            //    .AddTransient<IColorRepository, ColorRepository>()
//            //    .AddTransient<ISizeRepository, SizeRepository>()
//            //    .AddTransient<IUnitRepository, UnitRepository>()

//            //    .AddTransient<IItemRepository, ItemRepository>()
//            //    .AddTransient<IItemBarCodeRepository, ItemBarCodeRepository>()
//            //    .AddTransient<IModifierRepository, ModifierRepository>()

//            //    .AddTransient<IPhysicalInventoryRepository, PhysicalInventoryRepository>()

//            //    .AddTransient<IPORepository, PORepository>()
//            //    .AddTransient<IGrnRepository, GrnRepository>()
//            //    .AddTransient<IGrrnRepository, GrrnRepository>()
//            //    .AddTransient<IPurchaseRepository, PurchaseRepository>()
                

//            //    //restaurant management
//            //    .AddTransient<IRestaurantFloorsRepository, RestaurantFloorsRepository>()
//            //    .AddTransient<IDiningTableRepository,DiningTableRepository>()
//            //    .AddTransient<IWaitersRepository, WaitersRepository>()



//            //    //sales management
//            //    .AddTransient<IOrderRepository, OrderRepository>()



//            //    //reporting
//            //    .AddTransient<ISalesReportingRepository, SalesReportingRepository>()
//            //    .AddTransient<IAccountsReportingRepository, AccountsReportingRepository>()

//            //    //delivery service
//            //    .AddTransient<IDeliveryServiceVendorRepository, DeliveryServiceVendorRepository>()
//            //    .AddTransient<IDeliveryBoyRepository, DeliveryBoyRepository>()

//            //    //AccountsManagement
//            //    .AddTransient<IAccountRepository, AccountRepository>()
//            //    .AddTransient<IAccountTransactionRepository, AccountTransactionRepository>()
//            //    .AddTransient<IFiscalYearRepository, FiscalYearRepository>()
//            //    .AddTransient<IBillsRepository, BillsRepository>()


//            //    //units
//            //    //.AddTransient<IUserUnit, UserUnit>()
//            //    //.AddTransient<IInventoryUnit, InventoryUnit>()
//            //    //.AddTransient<IGeneralUnit, GeneralUnit>()
//            //    //.AddTransient<IRestaurantUnit, RestaurantUnit>()
//            //    //.AddTransient<ISalesUnit, SalesUnit>()
//            //    //.AddTransient<IReportingUnit, ReportingUnit>()
//            //    //.AddTransient<IDeliveryServiceUnit, DeliveryServiceUnit>()
//            //    //.AddTransient<IAccountsUnit, AccountsUnit>()
//            //    ;
//            return services;
//        }
//    }
//}