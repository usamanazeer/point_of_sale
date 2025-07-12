//using Models.DTO.InventoryManagement;
//using Models.DTO.InventoryManagement.ViewDTO;
//using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;
//using Models.DTO.ViewModels.SelectList.InventoryManagement;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using POS_API.Data.TVPs;

//namespace POS_API.Repositories.InventoryManagement
//{
//    public interface IInventoryUnit
//    {
//        #region Vendor
//        Task<InvVendorDto> CreateVendor(InvVendorDto model);
//        Task<InvVendorDto> EditVendor(InvVendorDto model);
//        Task<List<InvVendorDto>> GetAllVendors(InvVendorDto model);
//        Task<InvVendorDto> GetVendorDetails(InvVendorDto model);
//        Task<bool> ChangeVendorStatus(InvVendorDto model);
//        Task<bool> IsVendorExist(InvVendorDto model);
//        Task<bool> DeleteVendor(InvVendorDto model);
//        Task<IList<InvVendor_SLM>> GetVendorsSelectList(InvVendorDto model);
//        #endregion

//        #region MainCategory
//        Task<List<InvCategoryDto>> GetAllMainCategories(InvCategoryDto model);
//        Task<InvCategoryDto> CreateMainCategory(InvCategoryDto model);
//        Task<bool> IsMainCategoryExist(InvCategoryDto model);
//        Task<InvCategoryDto> EditMainCategory(InvCategoryDto model);
//        Task<bool> DeleteMainCategory(InvCategoryDto model);
//        #endregion

//        #region SubCategory
//        Task<List<InvSubCategoryDto>> GetAllSubCategories(InvSubCategoryDto model);
//        Task<InvSubCategoryDto> CreateSubCategory(InvSubCategoryDto model);
//        Task<bool> IsSubCategoryExist(InvSubCategoryDto model);
//        Task<InvSubCategoryDto> EditSubCategory(InvSubCategoryDto model);
//        Task<bool> DeleteSubCategory(InvSubCategoryDto model);
//        Task<IList<InvSubCategory_SLM>> GetSubCategorySelectList(InvSubCategoryDto model, bool forPos = false);

//        #endregion

//        #region Brand
//        Task<List<InvBrandDto>> GetAllBrands(InvBrandDto model);
//        Task<bool> IsBrandExist(InvBrandDto model);
//        Task<InvBrandDto> CreateBrand(InvBrandDto model);
//        Task<InvBrandDto> EditBrand(InvBrandDto model);
//        Task<bool> DeleteBrand(InvBrandDto model);
//        Task<InvBrandDto> GetBrandDetails(InvBrandDto model);
//        #endregion

//        #region Color
//        Task<List<InvColorDto>> GetAllColors(InvColorDto model);
//        Task<bool> IsColorExist(InvColorDto model);
//        Task<InvColorDto> CreateColor(InvColorDto model);
//        Task<InvColorDto> EditColor(InvColorDto model);
//        Task<bool> DeleteColor(InvColorDto model); 
//        Task<InvColorDto> GetColorDetails(InvColorDto model);
//        #endregion

//        #region Size
//        Task<InvSizeDto> CreateSize(InvSizeDto model);
//        Task<bool> DeleteSize(InvSizeDto model);
//        Task<InvSizeDto> EditSize(InvSizeDto model);
//        Task<List<InvSizeDto>> GetAllSizes(InvSizeDto model);
//        Task<InvSizeDto> GetSizeDetails(InvSizeDto model);
//        Task<bool> IsSizeExist(InvSizeDto model);
//        #endregion

//        #region Unit
//        Task<InvUnitDto> CreateUnit(InvUnitDto model);
//        Task<bool> DeleteUnit(InvUnitDto model);
//        Task<InvUnitDto> EditUnit(InvUnitDto model);
//        Task<List<InvUnitDto>> GetAllUnits(InvUnitDto model);
//        Task<bool> IsUnitExist(InvUnitDto model);
//        Task<InvUnitDto> GetUnitDetails(InvUnitDto model);
//        #endregion

//        #region Item
//        Task<InvItemDto> CreateItem(InvItemDto model);
//        Task<bool> DeleteItem(InvItemDto model);
//        Task<InvItemDto> EditItem(InvItemDto model);
//        Task<List<InvItemViewDto>> GetAllItems(InvItemDto model);
//        Task<List<InvItemViewDto>> GetAllItemsWithModifiers(InvItemDto model, bool fromCache = false);
//        Task<bool> IsItemExist(InvItemDto model);
//        Task<InvItemDto> GetItemDetails(InvItemDto model);
//        Task<IList<InvItem_SLM>> GetItemsSelectList(InvItemDto model);
//        Task<bool> UpdateItemImagePath(InvItemDto model);
//        Task<IList<BulkUploadItemsResponse>> ItemsBulkUpload(InvItemDto model,List<BulkUploadItemsTvp> data);
//        #endregion

//        #region ItemBarCode

//        Task<InvItemBarCodeDto> CreateItemBarCode(InvItemBarCodeDto model);
//        Task<bool> DeleteItemBarCode(InvItemBarCodeDto model);
//        Task<InvItemBarCodeDto> EditItemBarCode(InvItemBarCodeDto model);
//        Task<List<InvItemBarCodeDto>> GetAllItemBarCodes(InvItemBarCodeDto model);
//        Task<bool> IsItemBarCodeExist(InvItemBarCodeDto model);
//        Task<InvItemBarCodeDto> GetItemBarCodeDetails(InvItemBarCodeDto model);
//        Task<IList<InvItemBarCode_SLM>> GetItemBarCodesSelectList(InvItemBarCodeDto model);

//        #endregion

//        #region Item Modifier
//        Task<InvModifierDto> CreateModifier(InvModifierDto model);
//        Task<bool> DeleteModifier(InvModifierDto model);
//        Task<InvModifierDto> EditModifier(InvModifierDto model);
//        Task<List<InvModifierDto>> GetAllModifiers(InvModifierDto model);
//        Task<bool> IsModifierExist(InvModifierDto model);
//        Task<InvModifierDto> GetModifierDetails(InvModifierDto model);
//        Task<IList<InvModifier_SLM>> GetModifiersSelectList(InvModifierDto model);
//        #endregion

//        #region PhysicalInventory
//        Task<InvPhysicalInventoryDto> AddPhysicalInventory(InvPhysicalInventoryDto model);
//        Task<bool> IsPhysicalInventoryExists(InvPhysicalInventoryDto model);
//        Task<List<InvPhysicalInventoryDto>> GetAllPhysicalInventories(InvPhysicalInventoryDto model);
//        Task<IList<InvPhysicalInventoryViewDto>> GetPhysicalInventory_View(PhysicalInventoryViewFilter filters = null);
//        Task<List<InvPhysicalInventoryViewDto>> GetBillDetails(PhysicalInventoryViewFilter model);
//        Task<List<InvPhysicalInventoryViewDto>> GetLowInventory(PhysicalInventoryViewFilter filters = null);
//        #endregion

//        #region Purchase Order
//        Task<InvPoMasterDto> CreatePo(InvPoMasterDto model);
//        Task<bool> DeletePo(InvPoMasterDto model);
//        Task<InvPoMasterDto> EditPo(InvPoMasterDto model);
//        Task<List<InvPoMasterDto>> GetAllPOs(InvPoMasterDto model);
//        //Task<bool> IsPOExist(InvPoMasterDTO model);
//        Task<InvPoMasterDto> GetPoDetails(InvPoMasterDto model);
//        Task<IList<InvPoMaster_SLM>> GetPOsSelectList(InvPoMasterDto model);
//        #endregion

//        #region Goods Received Note
//        Task<InvGrnMasterDto> CreateGRN(InvGrnMasterDto model);
//        Task<bool> DeleteGRN(InvGrnMasterDto model);
//        Task<InvGrnMasterDto> EditGRN(InvGrnMasterDto model);
//        Task<List<InvGrnMasterDto>> GetAllGRNs(InvGrnMasterDto model);
//        //Task<bool> IsPOExist(InvPoMasterDTO model);
//        Task<InvGrnMasterDto> GetGRNDetails(InvGrnMasterDto model);
//        Task<IList<InvGrnMaster_SLM>> GetGRNsSelectList(InvGrnMasterDto model);
//        #endregion

//        #region Goods Return Note
//        Task<InvGrrnMasterDto> CreateGRRN(InvGrrnMasterDto model);
//        Task<bool> DeleteGRRN(InvGrrnMasterDto model);
//        Task<InvGrrnMasterDto> EditGRRN(InvGrrnMasterDto model);
//        Task<List<InvGrrnMasterDto>> GetAllGRRNs(InvGrrnMasterDto model);
//        Task<InvGrrnMasterDto> GetGRRNDetails(InvGrrnMasterDto model);
//        Task<IList<InvGrrnMaster_SLM>> GetGRRNsSelectList(InvGrrnMasterDto model);

//        #endregion

//        #region Purchase
//        Task<InvPurchaseMasterDto> CreatePurchase(InvPurchaseMasterDto purchaseMasterDto);
//        Task<bool> IsPurchaseExists(InvPurchaseMasterDto purchaseMasterDto);
//        Task<List<InvPurchaseMasterDto>> GetAllPurchases(InvPurchaseMasterDto purchaseMasterDto);
//        Task<InvPurchaseMasterDto> GetPurchaseDetails(InvPurchaseMasterDto model);
//        #endregion

//    }
//}
