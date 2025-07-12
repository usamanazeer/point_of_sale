using AutoMapper;
using POS_API.Data;
using POS_API.Data.TVPs;
using Models.DTO.Accounts;
using Models.DTO.Notifications;
using Models.DTO.UserManagement;
using System.Collections.Generic;
using Models;
using Models.DTO.DeliveryService;
using Models.DTO.GeneralSettings;
using Models.DTO.SalesManagement;
using Models.DTO.InventoryManagement;
using Models.DTO.RestaurantManagement;
using Models.DTO.Notifications.ViewDTO;
using Models.DTO.InventoryManagement.ViewDTO;
using Models.DTO.ViewModels.SelectList.UserManagement;
using Models.DTO.ViewModels.SelectList.DeliveryService;
using Models.DTO.ViewModels.SelectList.SalesManagement;
using Models.DTO.ViewModels.SelectList.AccountsManagement;
using Models.DTO.ViewModels.SelectList.InventoryManagement;
using Models.DTO.ViewModels.SelectList.RestaurantManagement;
using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;

namespace POS_API.Utilities.Mapper
{
    public class GeneralSettingsModuleMappings : Profile
    {
        public GeneralSettingsModuleMappings()
        {
            CreateMap<Tax, TaxDto>()
                .ForMember(dest => dest.InvPhysicalInventoryItem, act => act.MapFrom(src => src.InvPhysicalInventoryItem))
                .ReverseMap();
        }
    }
    public class UserManagementModuleMappings : Profile {
        public UserManagementModuleMappings()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Company, act => act.MapFrom(src => src.Company))
                .ForMember(dest => dest.Role, act => act.MapFrom(src => src.Role))
                .ForMember(dest => dest.NotiNotificationRecipient, act => act.MapFrom(src => src.NotiNotificationRecipient))
                .ReverseMap();

            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.RoleRights, act => act.MapFrom(src => src.RoleRights))
                .ForMember(dest => dest.User, act => act.MapFrom(src => src.User))
                .ForMember(dest => dest.NotiRoleNotification, act => act.MapFrom(src => src.NotiRoleNotification))
                .ReverseMap();

            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.BusinessType, act => act.MapFrom(src => src.BusinessType))
                .ForMember(dest => dest.User, act => act.MapFrom(src => src.User))
                .ForMember(dest => dest.Branch, act => act.MapFrom(src => src.Branch))
                .ReverseMap();
            
            CreateMap<Branch, BranchDto>()
                .ForMember(dest => dest.Company, act => act.MapFrom(src => src.Company))
                .ForMember(dest => dest.User, act => act.MapFrom(src => src.User))
                .ReverseMap();

            CreateMap<Module, ModuleDto>()
                .ForMember(dest => dest.CompanyModules, act => act.MapFrom(src => src.CompanyModules))
                .ForMember(dest => dest.Rights, act => act.MapFrom(src => src.Rights))
                .ReverseMap();


            CreateMap<CompanyModules, CompanyModulesDto>()
                .ForMember(dest => dest.Company, act => act.MapFrom(src => src.Company))
                .ForMember(dest => dest.Module, act => act.MapFrom(src => src.Module))
                .ReverseMap();

            CreateMap<Rights, RightsDto>()
                .ForMember(dest => dest. Module, act => act.MapFrom(src => src.Module))
                .ReverseMap();

            CreateMap<Rights, RightModel>().ReverseMap();





            CreateMap<BusinessType, BusinessTypeDto>()
                .ForMember(dest => dest.Company, act => act.MapFrom(src => src.Company))
                .ReverseMap();

            CreateMap<RoleRights, RoleRightsDto>()
                .ForMember(dest => dest.Role, act => act.MapFrom(src => src.Role))
                .ReverseMap();







            //select list mappings
            CreateMap<BranchDto, Branch_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.Name))
                .ReverseMap();
        }
    }
    public class InventoryManagementModuleMappings : Profile
    {
        public InventoryManagementModuleMappings()
        {
            CreateMap<InvVendor, InvVendorDto>()
                .ForMember(dest => dest.InvPhysicalInventoryItem, act => act.MapFrom(src => src.InvPhysicalInventoryItem))
                .ForMember(dest => dest.InvPoMaster, act => act.MapFrom(src => src.InvPoMaster))
                .ReverseMap();

            CreateMap<InvCategory, InvCategoryDto>()
                .ForMember(dest => dest.InvSubCategory, act => act.MapFrom(src => src.InvSubCategory))
                .ReverseMap();

            CreateMap<InvSubCategory, InvSubCategoryDto>()
                .ForMember(dest => dest.Category, act => act.MapFrom(src => src.Category))
                .ReverseMap();

            CreateMap<InvColor, InvColorDto>()
                .ForMember(dest => dest.InvItem, act => act.MapFrom(src => src.InvItem))
                .ReverseMap();

            CreateMap<InvSize, InvSizeDto>()
                .ForMember(dest => dest.InvItem, act => act.MapFrom(src => src.InvItem))
                .ReverseMap();

            CreateMap<InvBrand, InvBrandDto>()
                .ForMember(dest => dest.InvItem, act => act.MapFrom(src => src.InvItem))
                .ReverseMap();

            CreateMap<InvUnit, InvUnitDto>()
                .ForMember(dest => dest.InvItem, act => act.MapFrom(src => src.InvItem))
                .ReverseMap();

            CreateMap<InvItem, InvItemDto>()
                .ForMember(dest => dest.Brand, act => act.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Category, act => act.MapFrom(src => src.Category))
                .ForMember(dest => dest.SubCategory, act => act.MapFrom(src => src.SubCategory))
                .ForMember(dest => dest.Color, act => act.MapFrom(src => src.Color))
                .ForMember(dest => dest.Size, act => act.MapFrom(src => src.Size))
                .ForMember(dest => dest.Unit, act => act.MapFrom(src => src.Unit))
                .ForMember(dest => dest.InvItemBarCode, act => act.MapFrom(src => src.InvItemBarCode))
                //.ForMember(dest => dest.InvItemRecipeItem, act => act.MapFrom(src => src.InvItemRecipeItem))
                .ForMember(dest => dest.InvItemRecipeChild, act => act.MapFrom(src => src.InvItemRecipeParent))
                .ForMember(dest => dest.InvPhysicalInventoryItem, act => act.MapFrom(src => src.InvPhysicalInventoryItem))
                .ForMember(dest => dest.InvPoDetails, act => act.MapFrom(src => src.InvPodetails))
                .ReverseMap();

            CreateMap<InvItemBarCode, InvItemBarCodeDto>()
                .ForMember(dest => dest.Item, act => act.MapFrom(src => src.Item))
                .ForMember(dest => dest.InvPhysicalInventoryItem, act => act.MapFrom(src => src.InvPhysicalInventoryItem))
                .ForMember(dest => dest.InvItemRecipe, act => act.MapFrom(src => src.InvItemRecipe))
                .ForMember(dest => dest.InvModifierItems, act => act.MapFrom(src => src.InvModifierItems))
                .ReverseMap();

            CreateMap<InvItemRecipe, InvItemRecipeDto>()
                .ForMember(dest => dest.BarCode, act => act.MapFrom(src => src.BarCode))
                .ForMember(dest => dest.Item, act => act.MapFrom(src => src.Item))
                .ForMember(dest => dest.Parent, act => act.MapFrom(src => src.Parent))
                .ReverseMap();


            CreateMap<InvModifier, InvModifierDto>()
                .ForMember(dest => dest.InvModifierItems, act => act.MapFrom(src => src.InvModifierItems))
                .ForMember(dest => dest.InvItemModifiers, act => act.MapFrom(src => src.InvItemModifiers))
                .ReverseMap();

            CreateMap<InvModifierItems, InvModifierItemDto>()
                .ForMember(dest => dest.Item, act => act.MapFrom(src => src.Item))
                .ForMember(dest => dest.Modifier, act => act.MapFrom(src => src.Modifier))
                .ForMember(dest => dest.BarCode, act => act.MapFrom(src => src.BarCode))
                .ReverseMap();

            CreateMap<InvItemModifiers, InvItemModifierDto>()
                .ForMember(dest => dest.Item, act => act.MapFrom(src => src.Item))
                .ForMember(dest => dest.Modifier, act => act.MapFrom(src => src.Modifier))
                .ReverseMap();

            CreateMap<InvPhysicalInventory, InvPhysicalInventoryDto>()
                .ForMember(dest => dest.InvPhysicalInventoryItem, act => act.MapFrom(src => src.InvPhysicalInventoryItem))
                .ReverseMap();

            CreateMap<InvPhysicalInventoryItem, InvPhysicalInventoryItemDto>()
                .ForMember(dest => dest.PhysicalInventory, act => act.MapFrom(src => src.PhysicalInventory))
                .ForMember(dest => dest.Item, act => act.MapFrom(src => src.Item))
                .ForMember(dest => dest.BarCode, act => act.MapFrom(src => src.BarCode))
                .ForMember(dest => dest.Vendor, act => act.MapFrom(src => src.Vendor))
                .ForMember(dest => dest.Tax, act => act.MapFrom(src => src.Tax))
                .ReverseMap();

            CreateMap<InvPoMaster, InvPoMasterDto>()
                .ForMember(dest => dest.PoNo, act => act.MapFrom(src => src.Pono))
                .ForMember(dest => dest.PoDate, act => act.MapFrom(src => src.Podate))
                .ForMember(dest => dest.Vendor, act => act.MapFrom(src => src.Vendor))
                .ForMember(dest => dest.InvPoDetails, act => act.MapFrom(src => src.InvPodetails))
                .ForMember(dest => dest.InvGrnDetails, act => act.MapFrom(src => src.InvGrnDetails))
                .ReverseMap();

            CreateMap<InvPodetails, InvPoDetailsDto>()
                .ForMember(dest => dest.Po, act => act.MapFrom(src => src.Po))
                .ForMember(dest => dest.Item, act => act.MapFrom(src => src.Item))
                .ReverseMap();

            CreateMap<InvGrnMaster, InvGrnMasterDto>()
               .ForMember(dest => dest.Vendor, act => act.MapFrom(src => src.Vendor))
               .ForMember(dest => dest.InvGrnDetails, act => act.MapFrom(src => src.InvGrnDetails))
               .ReverseMap();

            CreateMap<InvGrnDetails, InvGrnDetailsDto>()
                .ForMember(dest => dest.Grn, act => act.MapFrom(src => src.Grn))
                .ForMember(dest => dest.Item, act => act.MapFrom(src => src.Item))
                .ForMember(dest => dest.Po, act => act.MapFrom(src => src.Po))
                .ReverseMap();


            CreateMap<InvGrrnMaster, InvGrrnMasterDto>()
               .ForMember(dest => dest.Vendor, act => act.MapFrom(src => src.Vendor))
               .ForMember(dest => dest.InvGrrnDetails, act => act.MapFrom(src => src.InvGrrnDetails))
               .ReverseMap();

            CreateMap<InvGrrnDetails, InvGrrnDetailsDto>()
                .ForMember(dest => dest.Grrn, act => act.MapFrom(src => src.Grrn))
                .ForMember(dest => dest.Item, act => act.MapFrom(src => src.Item))
                .ReverseMap();

           

            CreateMap<InvPurchaseMaster, InvPhysicalInventory>()
                .ForMember(dest => dest.BillNo,
                           act => act.MapFrom(src => src.BillNo))
                .ForMember(dest => dest.BillDate,
                           act => act.MapFrom(src => src.PurchaseDate))
                .ForMember(dest => dest.InvPhysicalInventoryItem,
                           act => act.MapFrom(src => src.InvPurchaseDetail));


            CreateMap<InvPurchaseDetail, InvPhysicalInventoryItem>()
                .ForMember(dest => dest.PhysicalInventory,
                           act => act.MapFrom(src => src.PurchaseMaster))
                .ForMember(dest => dest.Item,
                           act => act.MapFrom(src => src.Item))
                .ForMember(dest => dest.BarCode,
                           act => act.MapFrom(src => src.BarCode))
                .ForMember(dest => dest.RemainingQuantity,
                           act => act.MapFrom(src => src.Quantity));



            CreateMap<InvPurchaseMasterDto, InvPurchaseMaster>()
                .ForMember(dest => dest.Vendor, act => act.MapFrom(src => src.Vendor))
                .ForMember(dest => dest.InvPurchaseDetail, act => act.MapFrom(src => src.InvPurchaseDetail))
                .ReverseMap();


            CreateMap<InvPurchaseDetailDto, InvPurchaseDetail>()
                .ForMember(dest => dest.BarCode, act => act.MapFrom(src => src.BarCode))
                .ForMember(dest => dest.Item, act => act.MapFrom(src => src.Item))
                .ForMember(dest => dest.PurchaseMaster, act => act.MapFrom(src => src.PurchaseMaster))
                .ReverseMap();


            CreateMap<InvPurchaseMasterDto, BillDto>()
                .ForMember(dest => dest.BillDate,
                           act => act.MapFrom(src => src.PurchaseDate))
                .ForMember(dest => dest.Vendor,
                           act => act.MapFrom(src => src.Vendor))
                .ForMember(dest => dest.InvPurchaseDetail, act => act.MapFrom(src => src.InvPurchaseDetail))
                .ForMember(dest => dest.BillPayments, act => act.MapFrom(src => src.AccBillPayment))
                .ForMember(dest => dest.CreatedByUser, act => act.MapFrom(src => src.CreatedByUser))
                .ForMember(dest => dest.ModifiedByUser, act => act.MapFrom(src => src.ModifiedByUser));


            //VIEWS DTO
            CreateMap<InvLatestItemBarCodeView, InvLatestItemBarCodeViewDto>() 
                .ReverseMap();
            CreateMap<InvItemView, InvItemViewDto>()
                .ReverseMap();
            CreateMap<InvPhysicalInventoryView, InvPhysicalInventoryViewDto>()
                .ReverseMap();


            //VIEWSDTO to TableDTO MAPPINGS
            CreateMap<InvItemViewDto, InvItemDto> ()
                .ForMember(dest => dest.ItemBarCode, act => act.MapFrom(src => src.BarCode))
                .ForMember(dest => dest.InvItemBarCode, act => act.MapFrom(src => new List<InvItemBarCodeDto>() { new InvItemBarCodeDto() { Id = src.BarCodeId, BarCode = src.BarCode } } ))
                .ForMember(dest => dest.Category, act => act.MapFrom(src => src.CategoryId == null? null : new InvCategoryDto() { Id = src.CategoryId.Value, CategoryCode = src.CategoryCode, Name = src.CategoryName, ImageUrl = src.CategoryImageUrl, DisplayOnPos = src.CategoryDisplayOnPos??false, Status = src.CategoryStatus }))
                .ForMember(dest => dest.SubCategory, act => act.MapFrom(src => src.SubCategoryId == null ? null : new InvSubCategoryDto() { Id = src.SubCategoryId.Value, CategoryCode = src.SubCategoryCode, Name = src.SubCategoryName, ImageUrl = src.SubCategoryImageUrl, DisplayOnPos = src.SubCategoryDisplayOnPos ?? false, Status = src.SubCategoryStatus }))
                .ForMember(dest => dest.Unit, act => act.MapFrom(src => src.UnitId == null ? null : new InvUnitDto() { Id = src.UnitId.Value, Name = src.UnitName, Description = src.UnitDescription, Status = src.UnitStatus }))
                .ForMember(dest => dest.Brand, act => act.MapFrom(src => src.BrandId == null ? null : new InvBrandDto() { Id = src.BrandId.Value, Name = src.BrandName, Status = src.UnitStatus }))
                .ForMember(dest => dest.Color, act => act.MapFrom(src => src.ColorId == null ? null : new InvColorDto() { Id = src.ColorId.Value, Name = src.ColorName, ColorValue = src.ColorValue, Status = src.ColorStatus }))
                .ForMember(dest => dest.Size, act => act.MapFrom(src => src.SizeId == null ? null : new InvSizeDto() { Id = src.SizeId.Value, Name = src.SizeName, Status = src.SizeStatus }))
                .ForMember(dest => dest.Tax, act => act.MapFrom(src => src.TaxId == null ? null : new TaxDto() { Id = src.TaxId.Value, Name = src.TaxName, Amount = src.TaxAmount??0, IsInPercent = src.TaxIsInPercent??false, Status = src.SizeStatus }))
                .ForMember(dest => dest.CreatedBy, act => act.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.ModifiedBy, act => act.MapFrom(src => src.ModifiedById))
                .ForMember(dest => dest.totalRecords, act => act.MapFrom(src => src.totalRecords))
                .ForMember(dest => dest.CreatedByUser, act => act.MapFrom(src => src.CreatedById == null ? null : new UserDto() { Id = src.CreatedById, FirstName = src.CreatedByFirstName, LastName = src.CreatedByLastName }))
                .ForMember(dest => dest.ModifiedByUser, act => act.MapFrom(src => src.ModifiedById == null ? null : new UserDto() { Id = src.ModifiedById, FirstName = src.ModifiedByFirstName, LastName = src.ModifiedByLastName }))
                ;


            //select list mappings

            CreateMap<InvVendorDto, InvVendor_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.ContactName))
                .ReverseMap();

            CreateMap<InvSubCategoryDto, InvSubCategory_SLM>()
               .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
               .ForMember(d => d.Text, a => a.MapFrom(s => s.Name))
               .ReverseMap();

            CreateMap<InvItemViewDto, InvItem_SLM>()
                .ForMember(d=> d.Value, a=> a.MapFrom(s=>s.Id))
                .ForMember(d=> d.Text, a=> a.MapFrom(s=>s.Name))
                .ReverseMap();

            CreateMap<InvItemBarCodeDto, InvItemBarCode_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.BarCode))
                .ReverseMap();

            CreateMap<InvModifierDto, InvModifier_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.Name))
                .ReverseMap();

            CreateMap<InvPoMasterDto, InvPoMaster_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.PoNo))
                .ReverseMap();

            CreateMap<InvGrnMasterDto, InvGrnMaster_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.GrnNo))
                .ReverseMap();

            CreateMap<InvGrrnMasterDto, InvGrrnMaster_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.GrrnNo))
                .ReverseMap();
        }
    }
    public class RestaurantManagementModuleMappings : Profile
    {
        public RestaurantManagementModuleMappings()
        {
            CreateMap<RestRestaurantFloors, RestRestaurantFloorsDto>()
                .ForMember(dest => dest.RestDiningTable, act => act.MapFrom(src => src.RestDiningTable))
                .ReverseMap();

            CreateMap<RestDiningTable, RestDiningTableDto>()
                .ForMember(dest => dest.Floor, act => act.MapFrom(src => src.Floor))
                .ReverseMap();

            CreateMap<RestWaiter, RestWaiterDto>()
                .ReverseMap();

            //select list mappings
            CreateMap<RestRestaurantFloorsDto, RestRestaurantFloors_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.Name))
                .ReverseMap();

            CreateMap<RestDiningTableDto, RestDiningTable_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.TableNo))
                .ForMember(d => d.FloorId, a => a.MapFrom(s => s.FloorId))
                .ForMember(d => d.FloorName, a => a.MapFrom(s => s.Floor.Name))
                .ReverseMap();

            CreateMap<RestWaiterDto, RestWaiter_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.Name))
                .ReverseMap();
        }
    }
    public class NotificationsModuleMappings : Profile
    {
        public NotificationsModuleMappings()
        {
            CreateMap<NotiNotification, NotiNotificationDto>()
                .ForMember(dest => dest.NotiNotificationRecipient, act => act.MapFrom(src => src.NotiNotificationRecipient))
                .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type))
                .ReverseMap();
            
            CreateMap<NotiNotificationRecipient, NotiNotificationRecipientDto>()
                .ForMember(dest => dest.Notification, act => act.MapFrom(src => src.Notification))
                .ForMember(dest => dest.Recipient, act => act.MapFrom(src => src.Recipient))
                .ReverseMap();

            CreateMap<NotiNotificationType, NotiNotificationTypeDto>()
                .ForMember(dest => dest.NotiNotification, act => act.MapFrom(src => src.NotiNotification))
                .ForMember(dest => dest.NotiRoleNotification, act => act.MapFrom(src => src.NotiRoleNotification))
                .ReverseMap();

            CreateMap<NotiRoleNotification, NotiRoleNotificationDto>()
                .ForMember(dest => dest.NotificationType, act => act.MapFrom(src => src.NotificationType))
                .ForMember(dest => dest.Role, act => act.MapFrom(src => src.Role))
                .ReverseMap();

            //VIEWS DTO
            CreateMap<NotiUserNotificationsView, NotiUserNotificationsViewDto>()
                .ReverseMap();
        }
    }
    public class SalesManagementModuleMappings : Profile
    {
        public SalesManagementModuleMappings()
        {
            CreateMap<SalesOrderType, SalesOrderTypeDto>()
                .ForMember(dest => dest.SalesOrderMaster, act => act.MapFrom(src => src.SalesOrderMaster))
                .ReverseMap();

            CreateMap<SalesOrderMaster, SalesOrderMasterDto>()
                .ForMember(dest => dest.DiningTable, act => act.MapFrom(src => src.DiningTable))
                .ForMember(dest => dest.OrderType, act => act.MapFrom(src => src.OrderType))
                .ForMember(dest => dest.OrderStatus, act => act.MapFrom(src => src.OrderStatus))
                .ForMember(dest => dest.Waiter, act => act.MapFrom(src => src.Waiter))
                .ForMember(dest => dest.SalesOrderDetails, act => act.MapFrom(src => src.SalesOrderDetails))
                .ReverseMap();

            CreateMap<SalesOrderDetails, SalesOrderDetailsDto>()
                .ForMember(dest => dest.Order, act => act.MapFrom(src => src.Order))
                .ForMember(dest => dest.Item, act => act.MapFrom(src => src.Item))
                .ForMember(dest => dest.SalesOrderItemModifiers, act => act.MapFrom(src => src.SalesOrderItemModifiers))
                .ReverseMap();

            CreateMap<SalesOrderItemModifiers, SalesOrderItemModifiersDto>()
                .ForMember(dest => dest.OrderItem, act => act.MapFrom(src => src.OrderItem))
                .ForMember(dest => dest.Modifier, act => act.MapFrom(src => src.Modifier))
                .ReverseMap();

            CreateMap<SalesOrderBilling, SalesOrderBillingDto>()
                 .ForMember(dest => dest.Order, act => act.MapFrom(src => src.Order))
                 .ForMember(dest => dest.Tax, act => act.MapFrom(src => src.Tax))
                 .ReverseMap();

            CreateMap<SalesOrderStatus, SalesOrderStatusDto>()
                 .ForMember(dest => dest.SalesOrderMaster, act => act.MapFrom(src => src.SalesOrderMaster))
                 .ReverseMap();

            //select list mappings

            CreateMap<SalesOrderMasterDto, SalesOrderMaster_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.OrderNo))
                .ReverseMap();
            
            CreateMap<SalesOrderStatusDto, SalesOrderStatus_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.Name))
                .ReverseMap();
        }
    }
    public class DeliveryServiceModuleMappings : Profile
    {
        public DeliveryServiceModuleMappings()
        {
            CreateMap<DeliDeliveryServiceVendor, DeliDeliveryServiceVendorDto>()
                .ReverseMap();

            CreateMap<DeliDeliveryBoy, DeliDeliveryBoyDto>()
                .ReverseMap();




            //select list mappings

            CreateMap<DeliDeliveryServiceVendorDto, DeliveryServiceVendor_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.Name))
                .ForMember(d => d.ServiceCharges, a => a.MapFrom(s => s.ServiceDiscount))
                .ForMember(d => d.ChargesInPercent, a => a.MapFrom(s => s.IsServiceDiscountInPercent))
                .ReverseMap();

            CreateMap<DeliDeliveryBoyDto, DeliveryBoy_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.Name))
                .ReverseMap();
        }
    }

    public class AccountsManagementModuleMappings : Profile
    {
        public AccountsManagementModuleMappings()
        {
            CreateMap<AccAccountType, AccAccountTypeDto>()
                .ForMember(dest => dest.AccAccount, act => act.MapFrom(src => src.AccAccount))
                .ReverseMap();

            CreateMap<AccAccount, AccAccountDto>()
                .ForMember(dest => dest.AccountType, act => act.MapFrom(src => src.AccountType))
                .ForMember(dest => dest.Parent, act => act.MapFrom(src => src.Parent))
                .ForMember(dest => dest.InverseParent, act => act.MapFrom(src => src.InverseParent))
                .ReverseMap();

            CreateMap<AccTransactionMaster, AccTransactionMasterDto>()
                .ForMember(dest => dest.AccTransactionDetail, act => act.MapFrom(src => src.AccTransactionDetail))
                .ForMember(dest => dest.AccLedgerPosting, act => act.MapFrom(src => src.AccLedgerPosting))
                .ReverseMap();

            CreateMap<AccTransactionDetail, AccTransactionDetailDto>()
                .ForMember(dest => dest.Account, act => act.MapFrom(src => src.Account))
                .ForMember(dest => dest.TransactionMaster, act => act.MapFrom(src => src.TransactionMaster))
                .ForMember(dest => dest.AccLedgerPosting, act => act.MapFrom(src => src.AccLedgerPosting))
                .ReverseMap();
            
            CreateMap<AccLedgerPosting, AccLedgerPostingDto>()
                .ForMember(dest => dest.Account, act => act.MapFrom(src => src.Account))
                .ForMember(dest => dest.TransactionMaster, act => act.MapFrom(src => src.TransactionMaster))
                .ForMember(dest => dest.TransactionDetail, act => act.MapFrom(src => src.TransactionDetail))
                .ReverseMap();

            CreateMap<AccBillPayment, AccBillPaymentDto>()
                .ReverseMap();

            //select list mappings
            CreateMap<AccAccountDto, Account_SLM>()
                .ForMember(d => d.Value, a => a.MapFrom(s => s.Id))
                .ForMember(d => d.Text, a => a.MapFrom(s => s.Title))
                .ForMember(d => d.Code, a => a.MapFrom(s => s.Code))
                .ForMember(d => d.AccNo, a => a.MapFrom(s => s.AccNo))
                .ForMember(d => d.AccountTypeId, a => a.MapFrom(s => s.AccountTypeId))
                .ForMember(d => d.ParentId, a => a.MapFrom(s => s.ParentId))
                .ForMember(d => d.IsEditable, a => a.MapFrom(s => s.IsEditable))
                .ReverseMap();
        }
    }

    public class TvpsMappings : Profile
    {
        public TvpsMappings()
        {
            CreateMap<SalesOrderDetailsDto, OrderItemTVP>();
            CreateMap<SalesOrderItemModifiersDto, OrderItemModifierTVP>();
        }
    }
}