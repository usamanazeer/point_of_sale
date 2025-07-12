using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace POS_API.Data
{
    public partial class PosDB_Context : DbContext
    {
        public PosDB_Context()
        {
        }

        public PosDB_Context(DbContextOptions<PosDB_Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AccAccount> AccAccount { get; set; }
        public virtual DbSet<AccAccountType> AccAccountType { get; set; }
        public virtual DbSet<AccBillPayment> AccBillPayment { get; set; }
        public virtual DbSet<AccBillPaymentType> AccBillPaymentType { get; set; }
        public virtual DbSet<AccBillStatus> AccBillStatus { get; set; }
        public virtual DbSet<AccFiscalYear> AccFiscalYear { get; set; }
        public virtual DbSet<AccLedgerPosting> AccLedgerPosting { get; set; }
        public virtual DbSet<AccTransactionDetail> AccTransactionDetail { get; set; }
        public virtual DbSet<AccTransactionMaster> AccTransactionMaster { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<BusinessType> BusinessType { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CompanyModules> CompanyModules { get; set; }
        public virtual DbSet<DeliDeliveryBoy> DeliDeliveryBoy { get; set; }
        public virtual DbSet<DeliDeliveryServiceVendor> DeliDeliveryServiceVendor { get; set; }
        public virtual DbSet<InvBrand> InvBrand { get; set; }
        public virtual DbSet<InvCategory> InvCategory { get; set; }
        public virtual DbSet<InvColor> InvColor { get; set; }
        public virtual DbSet<InvGrnDetails> InvGrnDetails { get; set; }
        public virtual DbSet<InvGrnMaster> InvGrnMaster { get; set; }
        public virtual DbSet<InvGrrnDetails> InvGrrnDetails { get; set; }
        public virtual DbSet<InvGrrnMaster> InvGrrnMaster { get; set; }
        public virtual DbSet<InvItem> InvItem { get; set; }
        public virtual DbSet<InvItemBarCode> InvItemBarCode { get; set; }
        public virtual DbSet<InvItemModifiers> InvItemModifiers { get; set; }
        public virtual DbSet<InvItemRecipe> InvItemRecipe { get; set; }
        public virtual DbSet<InvItemView> InvItemView { get; set; }
        public virtual DbSet<InvLatestItemBarCodeView> InvLatestItemBarCodeView { get; set; }
        public virtual DbSet<InvModifier> InvModifier { get; set; }
        public virtual DbSet<InvModifierItems> InvModifierItems { get; set; }
        public virtual DbSet<InvNegativeInventory> InvNegativeInventory { get; set; }
        public virtual DbSet<InvPhysicalInventory> InvPhysicalInventory { get; set; }
        public virtual DbSet<InvPhysicalInventoryItem> InvPhysicalInventoryItem { get; set; }
        public virtual DbSet<InvPhysicalInventoryView> InvPhysicalInventoryView { get; set; }
        public virtual DbSet<InvPoMaster> InvPoMaster { get; set; }
        public virtual DbSet<InvPodetails> InvPodetails { get; set; }
        public virtual DbSet<InvPurchaseDetail> InvPurchaseDetail { get; set; }
        public virtual DbSet<InvPurchaseMaster> InvPurchaseMaster { get; set; }
        public virtual DbSet<InvSize> InvSize { get; set; }
        public virtual DbSet<InvSubCategory> InvSubCategory { get; set; }
        public virtual DbSet<InvUnit> InvUnit { get; set; }
        public virtual DbSet<InvVendor> InvVendor { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<NotiNotification> NotiNotification { get; set; }
        public virtual DbSet<NotiNotificationRecipient> NotiNotificationRecipient { get; set; }
        public virtual DbSet<NotiNotificationType> NotiNotificationType { get; set; }
        public virtual DbSet<NotiRoleNotification> NotiRoleNotification { get; set; }
        public virtual DbSet<NotiUserNotificationsView> NotiUserNotificationsView { get; set; }
        public virtual DbSet<RestDiningTable> RestDiningTable { get; set; }
        public virtual DbSet<RestRestaurantFloors> RestRestaurantFloors { get; set; }
        public virtual DbSet<RestWaiter> RestWaiter { get; set; }
        public virtual DbSet<Rights> Rights { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleRights> RoleRights { get; set; }
        public virtual DbSet<SalesOrderBilling> SalesOrderBilling { get; set; }
        public virtual DbSet<SalesOrderDetails> SalesOrderDetails { get; set; }
        public virtual DbSet<SalesOrderInventoryTracker> SalesOrderInventoryTracker { get; set; }
        public virtual DbSet<SalesOrderItemModifiers> SalesOrderItemModifiers { get; set; }
        public virtual DbSet<SalesOrderMaster> SalesOrderMaster { get; set; }
        public virtual DbSet<SalesOrderStatus> SalesOrderStatus { get; set; }
        public virtual DbSet<SalesOrderType> SalesOrderType { get; set; }
        public virtual DbSet<StatusType> StatusType { get; set; }
        public virtual DbSet<Tax> Tax { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccAccount>(entity =>
            {
                entity.ToTable("Acc_Account");

                entity.Property(e => e.AccNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.HasOne(d => d.AccountType)
                    .WithMany(p => p.AccAccount)
                    .HasForeignKey(d => d.AccountTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acc_Account_Acc_AccountType");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Acc_Account_Acc_Account");
            });

            modelBuilder.Entity<AccAccountType>(entity =>
            {
                entity.ToTable("Acc_AccountType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AccBillPayment>(entity =>
            {
                entity.ToTable("Acc_BillPayment");

                entity.Property(e => e.ChequeNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PaymentDate).HasColumnType("date");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.AccBillPayment)
                    .HasForeignKey(d => d.BillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acc_BillPayment_Inv_PurchaseMaster");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.AccBillPayment)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acc_BillPayment_Acc_BillPaymentType");
            });

            modelBuilder.Entity<AccBillPaymentType>(entity =>
            {
                entity.ToTable("Acc_BillPaymentType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AccBillStatus>(entity =>
            {
                entity.ToTable("Acc_BillStatus");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AccFiscalYear>(entity =>
            {
                entity.ToTable("Acc_FiscalYear");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<AccLedgerPosting>(entity =>
            {
                entity.ToTable("Acc_LedgerPosting");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PostedOn).HasColumnType("datetime");

                entity.Property(e => e.TransactionDate).HasColumnType("date");

                entity.Property(e => e.VoucherNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccLedgerPosting)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acc_LedgerPosting_Acc_Account");

                entity.HasOne(d => d.TransactionDetail)
                    .WithMany(p => p.AccLedgerPosting)
                    .HasForeignKey(d => d.TransactionDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acc_LedgerPosting_Acc_TransactionDetail");

                entity.HasOne(d => d.TransactionMaster)
                    .WithMany(p => p.AccLedgerPosting)
                    .HasForeignKey(d => d.TransactionMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acc_LedgerPosting_Acc_TransactionMaster");
            });

            modelBuilder.Entity<AccTransactionDetail>(entity =>
            {
                entity.ToTable("Acc_TransactionDetail");

                entity.Property(e => e.Statement)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccTransactionDetail)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acc_TransactionDetail_Acc_Account");

                entity.HasOne(d => d.TransactionMaster)
                    .WithMany(p => p.AccTransactionDetail)
                    .HasForeignKey(d => d.TransactionMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acc_TransactionDetail_Acc_TransactionMaster");
            });

            modelBuilder.Entity<AccTransactionMaster>(entity =>
            {
                entity.ToTable("Acc_TransactionMaster");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ReferenceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionDate).HasColumnType("date");

                entity.Property(e => e.TransactionId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Branch)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Branch_Company");
            });

            modelBuilder.Entity<BusinessType>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Logo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OffDeskPrinter)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OnDeskPrinter)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.BusinessType)
                    .WithMany(p => p.Company)
                    .HasForeignKey(d => d.BusinessTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Company_BusinessType");
            });

            modelBuilder.Entity<CompanyModules>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyModules)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyModules_Company");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.CompanyModules)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyModules_Module");
            });

            modelBuilder.Entity<DeliDeliveryBoy>(entity =>
            {
                entity.ToTable("Deli_DeliveryBoy");

                entity.Property(e => e.BikeNo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Cnic)
                    .HasColumnName("CNIC")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.ContactNo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DeliDeliveryServiceVendor>(entity =>
            {
                entity.ToTable("Deli_DeliveryServiceVendor");

                entity.Property(e => e.AccountNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvBrand>(entity =>
            {
                entity.ToTable("Inv_Brand");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<InvCategory>(entity =>
            {
                entity.ToTable("Inv_Category");

                entity.Property(e => e.CategoryCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvColor>(entity =>
            {
                entity.ToTable("Inv_Color");

                entity.Property(e => e.ColorValue)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvGrnDetails>(entity =>
            {
                entity.ToTable("Inv_GrnDetails");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Grn)
                    .WithMany(p => p.InvGrnDetails)
                    .HasForeignKey(d => d.GrnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_GrnDetails_Inv_GrnMaster");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InvGrnDetails)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_GrnDetails_Inv_Item");

                entity.HasOne(d => d.Po)
                    .WithMany(p => p.InvGrnDetails)
                    .HasForeignKey(d => d.PoId)
                    .HasConstraintName("FK_Inv_GrnDetails_Inv_PoMaster");
            });

            modelBuilder.Entity<InvGrnMaster>(entity =>
            {
                entity.ToTable("Inv_GrnMaster");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.GrnDate).HasColumnType("date");

                entity.Property(e => e.GrnNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.InvGrnMaster)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_GrnMaster_Inv_Vendor");
            });

            modelBuilder.Entity<InvGrrnDetails>(entity =>
            {
                entity.ToTable("Inv_GrrnDetails");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Grrn)
                    .WithMany(p => p.InvGrrnDetails)
                    .HasForeignKey(d => d.GrrnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_GrrnDetails_Inv_GrrnMaster");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InvGrrnDetails)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_GrrnDetails_Inv_Item");
            });

            modelBuilder.Entity<InvGrrnMaster>(entity =>
            {
                entity.ToTable("Inv_GrrnMaster");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.GrrnDate).HasColumnType("date");

                entity.Property(e => e.GrrnNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.InvGrrnMaster)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_GrrnMaster_Inv_Vendor");
            });

            modelBuilder.Entity<InvItem>(entity =>
            {
                entity.ToTable("Inv_Item");

                entity.Property(e => e.AccountNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FinalSalesRate).HasComputedColumnSql("([dbo].[Inv_Item__FinalSalesRate]([Id]))");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ItemCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Measurement)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.InvItem)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Inv_Item_Inv_Brand");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.InvItem)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Inv_Item_Inv_Category");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.InvItem)
                    .HasForeignKey(d => d.ColorId)
                    .HasConstraintName("FK_Inv_Item_Inv_Color");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.InvItem)
                    .HasForeignKey(d => d.SizeId)
                    .HasConstraintName("FK_Inv_Item_Inv_Size");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.InvItem)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("FK_Inv_Item_Inv_SubCategory");

                entity.HasOne(d => d.Tax)
                    .WithMany(p => p.InvItem)
                    .HasForeignKey(d => d.TaxId)
                    .HasConstraintName("FK_Inv_Item_Tax");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.InvItem)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_Inv_Item_Inv_Unit");
            });

            modelBuilder.Entity<InvItemBarCode>(entity =>
            {
                entity.ToTable("Inv_ItemBarCode");

                entity.Property(e => e.BarCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InvItemBarCode)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_Inv_ItemBarCode_Inv_Item");
            });

            modelBuilder.Entity<InvItemModifiers>(entity =>
            {
                entity.ToTable("Inv_ItemModifiers");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InvItemModifiers)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_ItemModifiers_Inv_Item");

                entity.HasOne(d => d.Modifier)
                    .WithMany(p => p.InvItemModifiers)
                    .HasForeignKey(d => d.ModifierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_ItemModifiers_Inv_Modifier1");
            });

            modelBuilder.Entity<InvItemRecipe>(entity =>
            {
                entity.ToTable("Inv_ItemRecipe");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.BarCode)
                    .WithMany(p => p.InvItemRecipe)
                    .HasForeignKey(d => d.BarCodeId)
                    .HasConstraintName("FK_Inv_ItemRecipe_Inv_ItemBarCode");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InvItemRecipeItem)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_ItemRecipe_Inv_Item1");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InvItemRecipeParent)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_ItemRecipe_Inv_Item");
            });

            modelBuilder.Entity<InvItemView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Inv_Item_View");

                entity.Property(e => e.BarCode)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BrandName).HasMaxLength(50);

                entity.Property(e => e.CategoryCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryImageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ColorName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ColorValue)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByFirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByLastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FullName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ItemCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ItemTypeName)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Measurement)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedByFirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedByLastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SizeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubCategoryCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubCategoryImageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.SubCategoryName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaxName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UnitDescription).HasMaxLength(250);

                entity.Property(e => e.UnitName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvLatestItemBarCodeView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Inv_Latest_ItemBarCode_View");

                entity.Property(e => e.BarCode)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<InvModifier>(entity =>
            {
                entity.ToTable("Inv_Modifier");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<InvModifierItems>(entity =>
            {
                entity.ToTable("Inv_ModifierItems");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.BarCode)
                    .WithMany(p => p.InvModifierItems)
                    .HasForeignKey(d => d.BarCodeId)
                    .HasConstraintName("FK_Inv_ModifierItems_Inv_ItemBarCode");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InvModifierItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_ModifierItems_Inv_Item");

                entity.HasOne(d => d.Modifier)
                    .WithMany(p => p.InvModifierItems)
                    .HasForeignKey(d => d.ModifierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_ModifierItems_Inv_Modifier");
            });

            modelBuilder.Entity<InvNegativeInventory>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.ToTable("Inv_NegativeInventory");

                entity.Property(e => e.ItemId).ValueGeneratedNever();

                entity.HasOne(d => d.Item)
                    .WithOne(p => p.InvNegativeInventory)
                    .HasForeignKey<InvNegativeInventory>(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_NegativeInventory_Inv_Item");
            });

            modelBuilder.Entity<InvPhysicalInventory>(entity =>
            {
                entity.ToTable("Inv_PhysicalInventory");

                entity.Property(e => e.BillDate).HasColumnType("date");

                entity.Property(e => e.BillNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<InvPhysicalInventoryItem>(entity =>
            {
                entity.ToTable("Inv_PhysicalInventoryItem");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.BarCode)
                    .WithMany(p => p.InvPhysicalInventoryItem)
                    .HasForeignKey(d => d.BarCodeId)
                    .HasConstraintName("FK_Inv_PhysicalInventoryItem_Inv_ItemBarCode");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InvPhysicalInventoryItem)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_Inv_PhysicalInventoryItem_Inv_Item");

                entity.HasOne(d => d.PhysicalInventory)
                    .WithMany(p => p.InvPhysicalInventoryItem)
                    .HasForeignKey(d => d.PhysicalInventoryId)
                    .HasConstraintName("FK_Inv_PhysicalInventoryItem_Inv_PhysicalInventory");

                entity.HasOne(d => d.Tax)
                    .WithMany(p => p.InvPhysicalInventoryItem)
                    .HasForeignKey(d => d.TaxId)
                    .HasConstraintName("FK_Inv_PhysicalInventoryItem_Tax");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.InvPhysicalInventoryItem)
                    .HasForeignKey(d => d.VendorId)
                    .HasConstraintName("FK_Inv_PhysicalInventoryItem_Inv_Vendor");
            });

            modelBuilder.Entity<InvPhysicalInventoryView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Inv_PhysicalInventory_View");

                entity.Property(e => e.BillDate).HasColumnType("date");

                entity.Property(e => e.BillNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BrandName).HasMaxLength(50);

                entity.Property(e => e.CategoryCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryImageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ColorName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ColorValue)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByName)
                    .IsRequired()
                    .HasMaxLength(101)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.ItemBarCode)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ItemFullName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ItemImageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ItemMeasurement)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ItemTypeName)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedByUserName)
                    .IsRequired()
                    .HasMaxLength(101)
                    .IsUnicode(false);

                entity.Property(e => e.SizeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubCategoryCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubCategoryImageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.SubCategoryName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaxName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UnitDescription).HasMaxLength(250);

                entity.Property(e => e.UnitName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VendorName).HasMaxLength(50);
            });

            modelBuilder.Entity<InvPoMaster>(entity =>
            {
                entity.ToTable("Inv_PoMaster");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Podate)
                    .HasColumnName("PODate")
                    .HasColumnType("date");

                entity.Property(e => e.Pono)
                    .IsRequired()
                    .HasColumnName("PONo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.InvPoMaster)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_PoMaster_Inv_Vendor");
            });

            modelBuilder.Entity<InvPodetails>(entity =>
            {
                entity.ToTable("Inv_PODetails");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InvPodetails)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_PODetails_Inv_Item");

                entity.HasOne(d => d.Po)
                    .WithMany(p => p.InvPodetails)
                    .HasForeignKey(d => d.PoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_PODetails_Inv_PoMaster");
            });

            modelBuilder.Entity<InvPurchaseDetail>(entity =>
            {
                entity.ToTable("Inv_PurchaseDetail");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.BarCode)
                    .WithMany(p => p.InvPurchaseDetail)
                    .HasForeignKey(d => d.BarCodeId)
                    .HasConstraintName("FK_Inv_PurchaseDetails_Inv_ItemBarCode");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InvPurchaseDetail)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_PurchaseDetails_Inv_Item");

                entity.HasOne(d => d.PurchaseMaster)
                    .WithMany(p => p.InvPurchaseDetail)
                    .HasForeignKey(d => d.PurchaseMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_PurchaseDetails_Inv_PurchaseMaster");
            });

            modelBuilder.Entity<InvPurchaseMaster>(entity =>
            {
                entity.ToTable("Inv_PurchaseMaster");

                entity.Property(e => e.BillDueDate).HasColumnType("date");

                entity.Property(e => e.BillNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.PurchaseDate).HasColumnType("date");

                entity.HasOne(d => d.BillStatus)
                    .WithMany(p => p.InvPurchaseMaster)
                    .HasForeignKey(d => d.BillStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_PurchaseMaster_Acc_BillStatus");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.InvPurchaseMaster)
                    .HasForeignKey(d => d.VendorId)
                    .HasConstraintName("FK_Inv_PurchaseMaster_Inv_Vendor");
            });

            modelBuilder.Entity<InvSize>(entity =>
            {
                entity.ToTable("Inv_Size");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvSubCategory>(entity =>
            {
                entity.ToTable("Inv_SubCategory");

                entity.Property(e => e.CategoryCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.InvSubCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inv_SubCategory_Inv_Category");
            });

            modelBuilder.Entity<InvUnit>(entity =>
            {
                entity.ToTable("Inv_Unit");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvVendor>(entity =>
            {
                entity.ToTable("Inv_Vendor");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactName).HasMaxLength(50);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OtherEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VendorCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CraetedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NotiNotification>(entity =>
            {
                entity.ToTable("Noti_Notification");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Message)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.NotiNotification)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Noti_Nitificaions_Noti_NotificationTypes");
            });

            modelBuilder.Entity<NotiNotificationRecipient>(entity =>
            {
                entity.ToTable("Noti_NotificationRecipient");

                entity.Property(e => e.SeenTime).HasColumnType("datetime");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.NotiNotificationRecipient)
                    .HasForeignKey(d => d.NotificationId)
                    .HasConstraintName("FK_Noti_NotificationRecipients_Noti_Nitificaions");

                entity.HasOne(d => d.Recipient)
                    .WithMany(p => p.NotiNotificationRecipient)
                    .HasForeignKey(d => d.RecipientId)
                    .HasConstraintName("FK_Noti_NotificationRecipients_User");
            });

            modelBuilder.Entity<NotiNotificationType>(entity =>
            {
                entity.ToTable("Noti_NotificationType");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CssClasses)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Icon)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceColumn)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceTable)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NotiRoleNotification>(entity =>
            {
                entity.ToTable("Noti_RoleNotification");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.NotificationType)
                    .WithMany(p => p.NotiRoleNotification)
                    .HasForeignKey(d => d.NotificationTypeId)
                    .HasConstraintName("FK_Noti_RoleNotifications_Noti_NotificationTypes");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.NotiRoleNotification)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Noti_RoleNotifications_Role");
            });

            modelBuilder.Entity<NotiUserNotificationsView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Noti_UserNotifications_View");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.NotificationColor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NotificationCssClasses)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NotificationIcon)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NotificationMessage)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NotificationSeenTime).HasColumnType("datetime");

                entity.Property(e => e.NotificationTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NotificationTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NotificationUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.RecipientFirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecipientLastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecipientOtherEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RecipientPrimaryEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RecipientUserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceColumn)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceTable)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleDescription)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RestDiningTable>(entity =>
            {
                entity.ToTable("Rest_DiningTable");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Floor)
                    .WithMany(p => p.RestDiningTable)
                    .HasForeignKey(d => d.FloorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rast_DiningTable_Rast_RastaurantFloors");
            });

            modelBuilder.Entity<RestRestaurantFloors>(entity =>
            {
                entity.ToTable("Rest_RestaurantFloors");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RestWaiter>(entity =>
            {
                entity.ToTable("Rest_Waiter");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Rights>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Action)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Area)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Controller)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CssClasses)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName).HasMaxLength(50);

                entity.Property(e => e.LiCssclasses)
                    .HasColumnName("LiCSSClasses")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.Rights)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("FK_Rights_Module");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Rights_Rights");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RoleRights>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Right)
                    .WithMany(p => p.RoleRights)
                    .HasForeignKey(d => d.RightId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleRights_Rights");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleRights)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleRights_Role1");
            });

            modelBuilder.Entity<SalesOrderBilling>(entity =>
            {
                entity.ToTable("Sales_OrderBilling");

                entity.HasIndex(e => e.OrderId)
                    .HasName("UQ__Sales_Or__C3905BCEB45E7CCE")
                    .IsUnique();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithOne(p => p.SalesOrderBilling)
                    .HasForeignKey<SalesOrderBilling>(d => d.OrderId)
                    .HasConstraintName("FK_Sales_OrderBilling_Sales_OrderMaster");

                entity.HasOne(d => d.Tax)
                    .WithMany(p => p.SalesOrderBilling)
                    .HasForeignKey(d => d.TaxId)
                    .HasConstraintName("FK_Sales_OrderBilling_Tax");
            });

            modelBuilder.Entity<SalesOrderDetails>(entity =>
            {
                entity.ToTable("Sales_OrderDetails");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.SalesOrderDetails)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sales_OrderDetails_Inv_Item");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.SalesOrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sales_OrderDetails_Sales_OrderMaster");
            });

            modelBuilder.Entity<SalesOrderInventoryTracker>(entity =>
            {
                entity.ToTable("Sales_OrderInventoryTracker");
            });

            modelBuilder.Entity<SalesOrderItemModifiers>(entity =>
            {
                entity.ToTable("Sales_OrderItemModifiers");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Modifier)
                    .WithMany(p => p.SalesOrderItemModifiers)
                    .HasForeignKey(d => d.ModifierId)
                    .HasConstraintName("FK_Sales_OrderItemModifiers_Inv_Modifier");

                entity.HasOne(d => d.OrderItem)
                    .WithMany(p => p.SalesOrderItemModifiers)
                    .HasForeignKey(d => d.OrderItemId)
                    .HasConstraintName("FK_Sales_OrderItemModifiers_Sales_OrderDetails");
            });

            modelBuilder.Entity<SalesOrderMaster>(entity =>
            {
                entity.ToTable("Sales_OrderMaster");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeliveryServiceReferenceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OrderNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DeliveryBoy)
                    .WithMany(p => p.SalesOrderMaster)
                    .HasForeignKey(d => d.DeliveryBoyId)
                    .HasConstraintName("FK_Sales_OrderMaster_DeliDeliveryBoy");

                entity.HasOne(d => d.DeliveryServiceVendor)
                    .WithMany(p => p.SalesOrderMaster)
                    .HasForeignKey(d => d.DeliveryServiceVendorId)
                    .HasConstraintName("FK_Sales_OrderMaster_Deli_DeliveryServiceVendor");

                entity.HasOne(d => d.DiningTable)
                    .WithMany(p => p.SalesOrderMaster)
                    .HasForeignKey(d => d.DiningTableId)
                    .HasConstraintName("FK_Sales_OrderMaster_Rast_DiningTable");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.SalesOrderMaster)
                    .HasForeignKey(d => d.OrderStatusId)
                    .HasConstraintName("FK_Sales_OrderMaster_Sales_OrderStatus");

                entity.HasOne(d => d.OrderType)
                    .WithMany(p => p.SalesOrderMaster)
                    .HasForeignKey(d => d.OrderTypeId)
                    .HasConstraintName("FK_Sales_OrderMaster_Sales_OrderType");

                entity.HasOne(d => d.Waiter)
                    .WithMany(p => p.SalesOrderMaster)
                    .HasForeignKey(d => d.WaiterId)
                    .HasConstraintName("FK_Sales_OrderMaster_Rast_Waiter");
            });

            modelBuilder.Entity<SalesOrderStatus>(entity =>
            {
                entity.ToTable("Sales_OrderStatus");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SalesOrderType>(entity =>
            {
                entity.ToTable("Sales_OrderType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StatusType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.StatusType1)
                    .IsRequired()
                    .HasColumnName("StatusType")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Tax>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EnableForPos).HasColumnName("EnableForPOS");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OtherEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_User_Branch");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Company");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
