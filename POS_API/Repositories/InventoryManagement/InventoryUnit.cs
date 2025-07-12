//using Models.DTO.InventoryManagement;
//using Models.DTO.InventoryManagement.ViewDTO;
//using Models.DTO.InventoryManagement.ViewDTO.PhysicalInventory;
//using Models.DTO.ViewModels.SelectList.InventoryManagement;
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
//using POS_API.Repositories.InventoryManagement.SizeRepos;
//using POS_API.Repositories.InventoryManagement.UnitRepos;
//using POS_API.Repositories.InventoryManagement.VendorRepos;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using POS_API.Data.TVPs;
//using POS_API.Repositories.InventoryManagement.PurchaseRepositories;

//namespace POS_API.Repositories.InventoryManagement
//{
//    public class InventoryUnit : IInventoryUnit
//    {
//        private readonly IVendorRepository _vendorRepository;
//        private readonly IMainCategoryRepository _mainCategoryRepository;
//        private readonly ISubCategoryRepository _subCategoryRepository;
//        private readonly IBrandRepository _brandRepository;
//        private readonly IColorRepository _colorRepository;
//        private readonly ISizeRepository _sizeRepository;
//        private readonly IUnitRepository _unitRepository;
//        private readonly IItemRepository _itemRepository;
//        private readonly IItemBarCodeRepository _itemBarCodeRepository;
//        private readonly IModifierRepository _modifierRepository;
//        private readonly IPhysicalInventoryRepository _physicalInventoryRepository;
//        private readonly IPORepository _pORepository;
//        private readonly IGrnRepository _grnRepository;
//        private readonly IGrrnRepository _grrnRepository;
//        private readonly IPurchaseRepository _purchaseRepository;
//        public InventoryUnit(
//            IVendorRepository vendorRepository, 
//            IMainCategoryRepository mainCategoryRepository, ISubCategoryRepository subCategoryRepository,
//            IBrandRepository brandRepository, IColorRepository colorRepository, ISizeRepository sizeRepository, IUnitRepository unitRepository,
//            IItemRepository itemRepository, IItemBarCodeRepository itemBarCodeRepository, IModifierRepository modifierRepository
//            , IPhysicalInventoryRepository physicalInventoryRepository
//            , IPORepository pORepository, IGrnRepository grnRepository, IGrrnRepository grrnRepository
//        ,IPurchaseRepository purchaseRepository
//            ) 
//        {
//            _vendorRepository = vendorRepository;
//            _mainCategoryRepository = mainCategoryRepository;
//            _subCategoryRepository = subCategoryRepository;
//            _brandRepository = brandRepository;
//            _colorRepository = colorRepository;
//            _sizeRepository = sizeRepository;
//            _unitRepository = unitRepository;
//            _itemRepository = itemRepository;
//            _itemBarCodeRepository = itemBarCodeRepository;
//            _modifierRepository = modifierRepository;
//            _physicalInventoryRepository = physicalInventoryRepository;
//            _pORepository = pORepository;
//            _grnRepository = grnRepository;
//            _grrnRepository = grrnRepository;
//            _purchaseRepository = purchaseRepository;
//        }

//        #region Vendor


//        public async Task<InvVendorDto> GetVendorDetails(InvVendorDto model)
//        {
//            return await _vendorRepository.GetDetails(model);
//        }

//        public async Task<bool> ChangeVendorStatus(InvVendorDto model)
//        {
//            return await _vendorRepository.ChangeStatus(model);
//        }

//        public async Task<InvVendorDto> CreateVendor(InvVendorDto model)
//        {
//            return await _vendorRepository.Create(model);
//        }

//        public async Task<bool> DeleteVendor(InvVendorDto model)
//        {
//            return await _vendorRepository.Delete(model);
//        }

//        public async Task<InvVendorDto> EditVendor(InvVendorDto model)
//        {
//            return await _vendorRepository.Edit(model);
//        }

//        public async Task<List<InvVendorDto>> GetAllVendors(InvVendorDto model)
//        {
//            return await _vendorRepository.GetAll(model);
//        }

//        public async Task<bool> IsVendorExist(InvVendorDto model)
//        {
//            return await _vendorRepository.IsExist(model);
//        }

//        public async Task<IList<InvVendor_SLM>> GetVendorsSelectList(InvVendorDto model)
//        {
//            return await _vendorRepository.GetSelectList(model);
//        }
//        #endregion Vendor

//        #region MainCategory
//        public async Task<List<InvCategoryDto>> GetAllMainCategories(InvCategoryDto model)
//        {
//            return await _mainCategoryRepository.GetAll(model);
//        }

//        public async Task<InvCategoryDto> CreateMainCategory(InvCategoryDto model)
//        {
//            return await _mainCategoryRepository.Create(model);
//        }

//        public async Task<bool> IsMainCategoryExist(InvCategoryDto model)
//        {
//            return await _mainCategoryRepository.IsExist(model);
//        }

//        public async Task<InvCategoryDto> EditMainCategory(InvCategoryDto model)
//        {
//            return await _mainCategoryRepository.Edit(model);
//        }

//        public async Task<bool> DeleteMainCategory(InvCategoryDto model)
//        {
//            return await _mainCategoryRepository.Delete(model);
//        }
//        #endregion

//        #region SubCategory
//        public async Task<List<InvSubCategoryDto>> GetAllSubCategories(InvSubCategoryDto model)
//        {
//            return await _subCategoryRepository.GetAll(model);
//        }

//        public async Task<InvSubCategoryDto> CreateSubCategory(InvSubCategoryDto model)
//        {
//            return await _subCategoryRepository.Create(model);
//        }

//        public async Task<bool> IsSubCategoryExist(InvSubCategoryDto model)
//        {
//            return await _subCategoryRepository.IsExist(model);
//        }

//        public async Task<InvSubCategoryDto> EditSubCategory(InvSubCategoryDto model)
//        {
//            return await _subCategoryRepository.Edit(model);
//        }

//        public async Task<bool> DeleteSubCategory(InvSubCategoryDto model)
//        {
//            return await _subCategoryRepository.Delete(model);
//        }

//        public async Task<IList<InvSubCategory_SLM>> GetSubCategorySelectList(InvSubCategoryDto model, bool forPos = false)
//        {
//            return await _subCategoryRepository.GetSelectList(model);
//        }

//        #endregion

//        #region Brand
//        public async Task<List<InvBrandDto>> GetAllBrands(InvBrandDto model)
//        {
//            return await _brandRepository.GetAll(model);
//        }

//        public async Task<bool> IsBrandExist(InvBrandDto model)
//        {
//            return await _brandRepository.IsExist(model);
//        }

//        public async Task<InvBrandDto> CreateBrand(InvBrandDto model)
//        {
//            return await _brandRepository.Create(model);
//        }

//        public async Task<InvBrandDto> EditBrand(InvBrandDto model)
//        {
//            return await _brandRepository.Edit(model);
//        }

//        public async Task<bool> DeleteBrand(InvBrandDto model)
//        {
//            return await _brandRepository.Delete(model);
//        }
//        public async Task<InvBrandDto> GetBrandDetails(InvBrandDto model)
//        {
//            return await _brandRepository.GetDetails(model);
//        }
//        #endregion

//        #region Color
//        public async Task<List<InvColorDto>> GetAllColors(InvColorDto model)
//        {
//            return await _colorRepository.GetAll(model);
//        }

//        public async Task<bool> IsColorExist(InvColorDto model)
//        {
//            return await _colorRepository.IsExist(model);
//        }

//        public async Task<InvColorDto> CreateColor(InvColorDto model)
//        {
//            return await _colorRepository.Create(model);
//        }

//        public async Task<InvColorDto> EditColor(InvColorDto model)
//        {
//            return await _colorRepository.Edit(model);
//        }

//        public async Task<bool> DeleteColor(InvColorDto model)
//        {
//            return await _colorRepository.Delete(model);
//        }

//        public async Task<InvColorDto> GetColorDetails(InvColorDto model)
//        {
//            return await _colorRepository.GetDetails(model);
//        }
//        #endregion

//        #region Size
//        public async Task<InvSizeDto> CreateSize(InvSizeDto model)
//        {
//            return await _sizeRepository.Create(model);
//        }

//        public async Task<bool> DeleteSize(InvSizeDto model)
//        {
//            return await _sizeRepository.Delete(model);
//        }

//        public async Task<InvSizeDto> EditSize(InvSizeDto model)
//        {
//            return await _sizeRepository.Edit(model);
//        }

//        public async Task<List<InvSizeDto>> GetAllSizes(InvSizeDto model)
//        {
//            return await _sizeRepository.GetAll(model);
//        }

//        public async Task<bool> IsSizeExist(InvSizeDto model)
//        {
//            return await _sizeRepository.IsExist(model);
//        }
//        public async Task<InvSizeDto> GetSizeDetails(InvSizeDto model)
//        {
//            return await _sizeRepository.GetDetails(model);
//        }
//        #endregion

//        #region Unit
//        public async Task<InvUnitDto> CreateUnit(InvUnitDto model)
//        {
//            return await _unitRepository.Create(model);
//        }

//        public async Task<bool> DeleteUnit(InvUnitDto model)
//        {
//            return await _unitRepository.Delete(model);
//        }

//        public async Task<InvUnitDto> EditUnit(InvUnitDto model)
//        {
//            return await _unitRepository.Edit(model);
//        }

//        public async Task<List<InvUnitDto>> GetAllUnits(InvUnitDto model)
//        {
//            return await _unitRepository.GetAll(model);
//        }

//        public async Task<bool> IsUnitExist(InvUnitDto model)
//        {
//            return await _unitRepository.IsExist(model);
//        }

//        public async Task<InvUnitDto> GetUnitDetails(InvUnitDto model)
//        {
//            return await _unitRepository.GetDetails(model);
//        }


//        #endregion

//        #region Item

//        public async Task<InvItemDto> CreateItem(InvItemDto model)
//        {
//            return await _itemRepository.Create(model);
//        }

//        public async Task<bool> DeleteItem(InvItemDto model)
//        {
//            return await _itemRepository.Delete(model);
//        }

//        public async Task<InvItemDto> EditItem(InvItemDto model)
//        {
//            return await _itemRepository.Edit(model);
//        }

//        public async Task<List<InvItemViewDto>> GetAllItems(InvItemDto model)
//        {
//            return await _itemRepository.GetAll(model);
//        }
//        public async Task<List<InvItemViewDto>> GetAllItemsWithModifiers(InvItemDto model, bool fromCache = false)
//        {
//            return await _itemRepository.GetAllWithModifiers(model,fromCache);
//        }
//        public async Task<bool> IsItemExist(InvItemDto model)
//        {
//            return await _itemRepository.IsExist(model);
//        }

//        public async Task<InvItemDto> GetItemDetails(InvItemDto model)
//        {
//            return await _itemRepository.GetDetails(model);
//        }

//        public async Task<IList<InvItem_SLM>> GetItemsSelectList(InvItemDto model)
//        {
//            return await _itemRepository.GetSelectList(model);
//        }

//        public async Task<bool> UpdateItemImagePath(InvItemDto model)
//        {
//            return await _itemRepository.UpdateImagePath(model);
//        }


//        public async Task<IList<BulkUploadItemsResponse>> ItemsBulkUpload(InvItemDto model, List<BulkUploadItemsTvp> data)
//        {
//            return await _itemRepository.ItemsBulkUpload(model, data);
//        }

//        #endregion

//        #region ItemBarCode

//        public async Task<InvItemBarCodeDto> CreateItemBarCode(InvItemBarCodeDto model)
//        {
//            return await _itemBarCodeRepository.Create(model);
//        }

//        public async Task<bool> DeleteItemBarCode(InvItemBarCodeDto model)
//        {
//            return await _itemBarCodeRepository.Delete(model);
//        }

//        public async Task<InvItemBarCodeDto> EditItemBarCode(InvItemBarCodeDto model)
//        {
//            return await _itemBarCodeRepository.Edit(model);
//        }

//        public async Task<List<InvItemBarCodeDto>> GetAllItemBarCodes(InvItemBarCodeDto model)
//        {
//            return await _itemBarCodeRepository.GetAll(model);
//        }
//        public async Task<bool> IsItemBarCodeExist(InvItemBarCodeDto model)
//        {
//            return await _itemBarCodeRepository.IsExist(model);
//        }

//        public async Task<InvItemBarCodeDto> GetItemBarCodeDetails(InvItemBarCodeDto model)
//        {
//            return await _itemBarCodeRepository.GetDetails(model);
//        }
//        public async Task<IList<InvItemBarCode_SLM>> GetItemBarCodesSelectList(InvItemBarCodeDto model)
//        {
//            return await _itemBarCodeRepository.GetSelectList(model);
//        }
//        #endregion

//        #region Item Modifier

//        public async Task<InvModifierDto> CreateModifier(InvModifierDto model)
//        {
//            return await _modifierRepository.Create(model);
//        }

//        public async Task<bool> DeleteModifier(InvModifierDto model)
//        {
//            return await _modifierRepository.Delete(model);
//        }

//        public async Task<InvModifierDto> EditModifier(InvModifierDto model)
//        {
//            return await _modifierRepository.Edit(model);
//        }

//        public async Task<List<InvModifierDto>> GetAllModifiers(InvModifierDto model)
//        {
//            return await _modifierRepository.GetAll(model);
//        }

//        public async Task<bool> IsModifierExist(InvModifierDto model)
//        {
//            return await _modifierRepository.IsExist(model);
//        }

//        public async Task<InvModifierDto> GetModifierDetails(InvModifierDto model)
//        {
//            return await _modifierRepository.GetDetails(model);
//        }

//        public async Task<IList<InvModifier_SLM>> GetModifiersSelectList(InvModifierDto model)
//        {
//            return await _modifierRepository.GetSelectList(model);
//        }
//        #endregion

//        #region PhysicalInventory
//        public async Task<InvPhysicalInventoryDto> AddPhysicalInventory(InvPhysicalInventoryDto model)
//        {
//            return await _physicalInventoryRepository.AddPhysicalInventory(model);
//        }

//        public async Task<List<InvPhysicalInventoryDto>> GetAllPhysicalInventories(InvPhysicalInventoryDto model)
//        {
//            return await _physicalInventoryRepository.GetAll(model);
//        }

//        public async Task<List<InvPhysicalInventoryViewDto>> GetBillDetails(PhysicalInventoryViewFilter filter)
//        {
//            return await _physicalInventoryRepository.GetPhysicalInventory_View(filter);
//        }

//        public async Task<List<InvPhysicalInventoryViewDto>> GetLowInventory(PhysicalInventoryViewFilter filters = null)
//        {
//            return await _physicalInventoryRepository.GetLowInventory(filters);
//        }

//        public async Task<IList<InvPhysicalInventoryViewDto>> GetPhysicalInventory_View(PhysicalInventoryViewFilter filters = null)
//        {
//            return await _physicalInventoryRepository.GetPhysicalInventory_View(filters);
//        }

//        public async Task<bool> IsPhysicalInventoryExists(InvPhysicalInventoryDto model)
//        {
//            return await _physicalInventoryRepository.IsPhysicalInventoryExists(model);
//        }
//        #endregion

//        #region Purchase Order
//        public async Task<InvPoMasterDto> CreatePo(InvPoMasterDto model)
//        {
//            return await _pORepository.Create(model);
//        }

//        public async Task<bool> DeletePo(InvPoMasterDto model)
//        {
//            return await _pORepository.Delete(model);
//        }

//        public async Task<InvPoMasterDto> EditPo(InvPoMasterDto model)
//        {
//            return await _pORepository.Edit(model);
//        }

//        public async Task<List<InvPoMasterDto>> GetAllPOs(InvPoMasterDto model)
//        {
//            return await _pORepository.GetAll(model);
//        }

        
//        //public async Task<bool> IsPOExist(InvPoMasterDTO model)
//        //{
//        //    return await _pORepository.IsExist(model);
//        //}

//        public async Task<InvPoMasterDto> GetPoDetails(InvPoMasterDto model)
//        {
//            return await _pORepository.GetDetails(model);
//        }

//        public async Task<IList<InvPoMaster_SLM>> GetPOsSelectList(InvPoMasterDto model)
//        {
//            return await _pORepository.GetSelectList(model);
//        }

//        #endregion

//        #region Goods Received Note
//        public async Task<InvGrnMasterDto> CreateGRN(InvGrnMasterDto model)
//        {
//            return await _grnRepository.Create(model);
//        }

//        public async Task<bool> DeleteGRN(InvGrnMasterDto model)
//        {
//            return await _grnRepository.Delete(model);
//        }

//        public async Task<InvGrnMasterDto> EditGRN(InvGrnMasterDto model)
//        {
//            return await _grnRepository.Edit(model);
//        }

//        public async Task<List<InvGrnMasterDto>> GetAllGRNs(InvGrnMasterDto model)
//        {
//            return await _grnRepository.GetAll(model);
//        }

//        public async Task<InvGrnMasterDto> GetGRNDetails(InvGrnMasterDto model)
//        {
//            return await _grnRepository.GetDetails(model);
//        }

//        public async Task<IList<InvGrnMaster_SLM>> GetGRNsSelectList(InvGrnMasterDto model)
//        {
//            return await _grnRepository.GetSelectList(model);
//        }

//        #endregion

//        #region Goods Return Note
//        public async Task<InvGrrnMasterDto> CreateGRRN(InvGrrnMasterDto model)
//        {
//            return await _grrnRepository.Create(model);
//        }

//        public async Task<bool> DeleteGRRN(InvGrrnMasterDto model)
//        {
//            return await _grrnRepository.Delete(model);
//        }

//        public async Task<InvGrrnMasterDto> EditGRRN(InvGrrnMasterDto model)
//        {
//            return await _grrnRepository.Edit(model);
//        }

//        public async Task<List<InvGrrnMasterDto>> GetAllGRRNs(InvGrrnMasterDto model)
//        {
//            return await _grrnRepository.GetAll(model);
//        }

//        public async Task<InvGrrnMasterDto> GetGRRNDetails(InvGrrnMasterDto model)
//        {
//            return await _grrnRepository.GetDetails(model);
//        }

//        public async Task<IList<InvGrrnMaster_SLM>> GetGRRNsSelectList(InvGrrnMasterDto model)
//        {
//            return await _grrnRepository.GetSelectList(model);
//        }


//        #endregion


//        #region Purchase
//        public async Task<InvPurchaseMasterDto> CreatePurchase(InvPurchaseMasterDto purchaseMasterDto)
//        {
//            return await _purchaseRepository.Create(purchaseMasterDto);
//        }


//        public async Task<bool> IsPurchaseExists(InvPurchaseMasterDto purchaseMasterDto)
//        {
//            return await _purchaseRepository.IsExist(purchaseMasterDto);
//        }


//        public async Task<List<InvPurchaseMasterDto>> GetAllPurchases(InvPurchaseMasterDto purchaseMasterDto)
//        {
//            return await _purchaseRepository.GetAll(purchaseMasterDto);
//        }


//        public async  Task<InvPurchaseMasterDto> GetPurchaseDetails(InvPurchaseMasterDto purchaseMasterDto)
//        {
//            return await _purchaseRepository.GetDetails(purchaseMasterDto);
//        }

//        #endregion
//    }
//}