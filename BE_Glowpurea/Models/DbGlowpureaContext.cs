using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BE_Glowpurea.Models;

public partial class DbGlowpureaContext : DbContext
{
    public DbGlowpureaContext()
    {
    }

    public DbGlowpureaContext(DbContextOptions<DbGlowpureaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<BlogCategory> BlogCategories { get; set; }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ReviewBlog> ReviewBlogs { get; set; }

    public virtual DbSet<ReviewBlogReaction> ReviewBlogReactions { get; set; }

    public virtual DbSet<ReviewBlogReply> ReviewBlogReplies { get; set; }

    public virtual DbSet<ReviewProduct> ReviewProducts { get; set; }

    public virtual DbSet<ReviewProductImage> ReviewProductImages { get; set; }

    public virtual DbSet<ReviewProductReaction> ReviewProductReactions { get; set; }

    public virtual DbSet<ReviewProductReply> ReviewProductReplies { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Shape> Shapes { get; set; }

    public virtual DbSet<StatusOrder> StatusOrders { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<EmailOtp> EmailOtps { get; set; }
    public DbSet<Address> Addresses { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-9N0ESV5\\MSSQLSERVER_22; Database=DB_Glowpurea;User ID=sa;Password=admin123456;Trust Server Certificate=True");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-74AQ829\\MSSQLSERVER01;Database=DB_Glowpurea;User ID=sa;Password=12345;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA58630EE5F8B");

            entity.HasIndex(e => e.PhoneNumber, "UQ_Accounts_PhoneNumber_Filtered")
                .IsUnique()
                .HasFilter("([PhoneNumber] IS NOT NULL)");

            entity.HasIndex(e => e.Email, "UQ__Accounts__A9D1053453F81625").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AccountName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Provider).HasMaxLength(255);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Active");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Accounts_Roles");
        });

        modelBuilder.Entity<BlogCategory>(entity =>
        {
            entity.HasKey(e => e.BlogCategoryId).HasName("PK__BlogCate__6BD2DA61ABD754AC");

            entity.HasIndex(e => e.BlogCategoryName, "UQ__BlogCate__06725EA7AA656064").IsUnique();

            entity.Property(e => e.BlogCategoryId).HasColumnName("BlogCategoryID");
            entity.Property(e => e.BlogCategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.BlogPostId).HasName("PK__BlogPost__32174149F15CD456");

            entity.Property(e => e.BlogPostId).HasColumnName("BlogPostID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.BlogThumbnail).HasMaxLength(500);
            entity.Property(e => e.BlogTitle).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.BlogPosts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlogPosts_Accounts");

            entity.HasMany(d => d.BlogCategories).WithMany(p => p.BlogPosts)
                .UsingEntity<Dictionary<string, object>>(
                    "BlogPostCategory",
                    r => r.HasOne<BlogCategory>().WithMany()
                        .HasForeignKey("BlogCategoryId")
                        .HasConstraintName("FK_BlogPostCategories_Categories"),
                    l => l.HasOne<BlogPost>().WithMany()
                        .HasForeignKey("BlogPostId")
                        .HasConstraintName("FK_BlogPostCategories_Posts"),
                    j =>
                    {
                        j.HasKey("BlogPostId", "BlogCategoryId").HasName("PK__BlogPost__34AA6CEFB1A7A93E");
                        j.ToTable("BlogPostCategories");
                        j.IndexerProperty<int>("BlogPostId").HasColumnName("BlogPostID");
                        j.IndexerProperty<int>("BlogCategoryId").HasColumnName("BlogCategoryID");
                    });
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD797DEBEE0AF");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Carts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Cart_Accounts");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B2AC8976898");

            entity.Property(e => e.CartItemId).HasColumnName("CartItemID");
            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.PriceAtThatTime).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK_CartItems_Cart");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Products");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2BBF136F78");

            entity.HasIndex(e => e.CategoryName, "UQ_Categories_CategoryName").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFB423BD5E");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaidByExternalAmount).HasColumnType("decimal(14, 2)");
            entity.Property(e => e.PaidByWalletAmount).HasColumnType("decimal(14, 2)");
            entity.Property(e => e.PaymentCompletedAt).HasColumnType("datetime");
            entity.Property(e => e.RefundStatus)
                .HasMaxLength(15)
                .HasDefaultValue("None");
            entity.Property(e => e.ShippingAddressLine).HasMaxLength(500);
            entity.Property(e => e.ShippingCity).HasMaxLength(100);
            entity.Property(e => e.ShippingMethod).HasMaxLength(100);
            entity.Property(e => e.ShippingName).HasMaxLength(100);
            entity.Property(e => e.ShippingPhone).HasMaxLength(20);
            entity.Property(e => e.ShippingWard).HasMaxLength(100);
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Accounts");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_StatusOrder");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30C2ECE41CB");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Total)
                .HasComputedColumnSql("(([Quantity]*[UnitPrice])*((1)-[Discount]/(100.0)))", true)
                .HasColumnType("numeric(36, 9)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetails_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Products");
        });

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.HasKey(e => e.OrderStatusHistoryId).HasName("PK__OrderSta__D16EDBA313180CB8");

            entity.ToTable("OrderStatusHistory");

            entity.Property(e => e.OrderStatusHistoryId).HasColumnName("OrderStatusHistoryID");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.OrderStatusHistories)
                .HasForeignKey(d => d.ChangedBy)
                .HasConstraintName("FK_OSH_ChangedBy");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderStatusHistories)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OSH_Orders");

            entity.HasOne(d => d.Status).WithMany(p => p.OrderStatusHistories)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_OSH_StatusOrder");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDCFB9A720");

            entity.HasIndex(e => e.Sku, "UQ__Products__CA1ECF0DB43A9995").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.ProductStatus)
                .HasMaxLength(15)
                .HasDefaultValue("Available");
            entity.Property(e => e.ShapesId).HasColumnName("ShapesID");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SKU");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.Shapes).WithMany(p => p.Products)
                .HasForeignKey(d => d.ShapesId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Products_Shapes");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ProductI__7516F4ECC602F4C4");

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductImages_Products");
        });

        modelBuilder.Entity<ReviewBlog>(entity =>
        {
            entity.HasKey(e => e.ReviewBlogId).HasName("PK__ReviewBl__A19536C0D010D49E");

            entity.HasIndex(e => new { e.AccountId, e.BlogPostId }, "UQ_ReviewBlogs_PostAccount").IsUnique();

            entity.Property(e => e.ReviewBlogId).HasColumnName("ReviewBlogID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.BlogPostId).HasColumnName("BlogPostID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.ReviewBlogs)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_ReviewBlogs_Accounts");

            entity.HasOne(d => d.BlogPost).WithMany(p => p.ReviewBlogs)
                .HasForeignKey(d => d.BlogPostId)
                .HasConstraintName("FK_ReviewBlogs_Posts");
        });

        modelBuilder.Entity<ReviewBlogReaction>(entity =>
        {
            entity.HasKey(e => e.ReactionBlogId).HasName("PK__ReviewBl__6A8A0D274E47C44A");

            entity.HasIndex(e => new { e.ReviewBlogId, e.AccountId }, "UQ_ReviewBlogReactions").IsUnique();

            entity.Property(e => e.ReactionBlogId).HasColumnName("ReactionBlogID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReactionType).HasMaxLength(10);
            entity.Property(e => e.ReviewBlogId).HasColumnName("ReviewBlogID");

            entity.HasOne(d => d.Account).WithMany(p => p.ReviewBlogReactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewBlogReactions_Accounts");

            entity.HasOne(d => d.ReviewBlog).WithMany(p => p.ReviewBlogReactions)
                .HasForeignKey(d => d.ReviewBlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewBlogReactions_ReviewBlogs");
        });

        modelBuilder.Entity<ReviewBlogReply>(entity =>
        {
            entity.HasKey(e => e.ReplyBlogId).HasName("PK__ReviewBl__59963641E67E414D");

            entity.Property(e => e.ReplyBlogId).HasColumnName("ReplyBlogID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Content).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReviewBlogId).HasColumnName("ReviewBlogID");

            entity.HasOne(d => d.Account).WithMany(p => p.ReviewBlogReplies)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewBlogReplies_Accounts");

            entity.HasOne(d => d.ReviewBlog).WithMany(p => p.ReviewBlogReplies)
                .HasForeignKey(d => d.ReviewBlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewBlogReplies_ReviewBlogs");
        });

        modelBuilder.Entity<ReviewProduct>(entity =>
        {
            entity.HasKey(e => e.ReviewProductId).HasName("PK__ReviewPr__02A6803AE50728A2");

            entity.HasIndex(e => new { e.AccountId, e.ProductId }, "UQ_Reviews_ProductAccount").IsUnique();

            entity.Property(e => e.ReviewProductId).HasColumnName("ReviewProductID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.ReviewProducts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Reviews_Accounts");

            entity.HasOne(d => d.Product).WithMany(p => p.ReviewProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_Products");
        });

        modelBuilder.Entity<ReviewProductImage>(entity =>
        {
            entity.HasKey(e => e.ReviewProductImageId).HasName("PK__ReviewPr__013E0F1E8841590F");

            entity.Property(e => e.ReviewProductImageId).HasColumnName("ReviewProductImageID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.ReviewProductId).HasColumnName("ReviewProductID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.ReviewProduct).WithMany(p => p.ReviewProductImages)
                .HasForeignKey(d => d.ReviewProductId)
                .HasConstraintName("FK_ReviewProdImages_ReviewProducts");
        });

        modelBuilder.Entity<ReviewProductReaction>(entity =>
        {
            entity.HasKey(e => e.ReactionProductId).HasName("PK__ReviewPr__B56FCAF126CE18CC");

            entity.HasIndex(e => new { e.ReviewProductId, e.AccountId }, "UQ_ReviewProductReactions").IsUnique();

            entity.Property(e => e.ReactionProductId).HasColumnName("ReactionProductID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReactionType).HasMaxLength(10);
            entity.Property(e => e.ReviewProductId).HasColumnName("ReviewProductID");

            entity.HasOne(d => d.Account).WithMany(p => p.ReviewProductReactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewProdReactions_Accounts");

            entity.HasOne(d => d.ReviewProduct).WithMany(p => p.ReviewProductReactions)
                .HasForeignKey(d => d.ReviewProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewProdReactions_Reviews");
        });

        modelBuilder.Entity<ReviewProductReply>(entity =>
        {
            entity.HasKey(e => e.ReplyProductId).HasName("PK__ReviewPr__2DCE233C5317F637");

            entity.Property(e => e.ReplyProductId).HasColumnName("ReplyProductID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Content).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReviewProductId).HasColumnName("ReviewProductID");

            entity.HasOne(d => d.Account).WithMany(p => p.ReviewProductReplies)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewProdReplies_Accounts");

            entity.HasOne(d => d.ReviewProduct).WithMany(p => p.ReviewProductReplies)
                .HasForeignKey(d => d.ReviewProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewProdReplies_Reviews");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A64DE7FCA");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160A622BCCC").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Shape>(entity =>
        {
            entity.HasKey(e => e.ShapesId).HasName("PK__Shapes__590E7E2AB57D7E34");

            entity.Property(e => e.ShapesId).HasColumnName("ShapesID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ShapesName).HasMaxLength(255);
        });

        modelBuilder.Entity<StatusOrder>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__StatusOr__C8EE2043C37D7245");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
        });
        modelBuilder.Entity<EmailOtp>(entity =>
        {
            entity.HasKey(e => e.EmailOtpID);

            entity.Property(e => e.EmailOtpID)
                  .HasColumnName("EmailOtpID");

            entity.Property(e => e.AccountID)
                  .HasColumnName("AccountID");

            entity.Property(e => e.Email)
                  .HasMaxLength(255);

            entity.Property(e => e.Purpose)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(e => e.OtpCode)
                  .HasMaxLength(10)
                  .IsRequired();

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("(getdate())");

            entity.HasOne(e => e.Account)
                  .WithMany(a => a.EmailOtps)
                  .HasForeignKey(e => e.AccountID)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressID);

            entity.Property(e => e.AddressID)
                  .HasColumnName("AddressID");

            entity.Property(e => e.AccountID)
                  .HasColumnName("AccountID");

            entity.Property(e => e.AddressLine)
                  .HasMaxLength(500)
                  .IsRequired();

            entity.Property(e => e.City)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(e => e.Ward)
                  .HasMaxLength(100);

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("(getdate())");

            entity.HasOne(e => e.Account)
                  .WithMany(a => a.Addresses)
                  .HasForeignKey(e => e.AccountID)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.WishlistId).HasName("PK__Wishlist__233189CBCB4337F5");

            entity.HasIndex(e => new { e.AccountId, e.ProductId }, "UQ_Wishlists").IsUnique();

            entity.Property(e => e.WishlistId).HasColumnName("WishlistID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Account).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Wishlists_Accounts");

            entity.HasOne(d => d.Product).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Wishlists_Products");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
