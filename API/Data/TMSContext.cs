namespace API.Data;

public class TMSContext : DbContext
{
    public TMSContext (DbContextOptions<TMSContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductDetails>().Navigation(x => x.Party).AutoInclude();

        modelBuilder.Entity<PurchaseOrder>().Navigation(x => x.Vendor).AutoInclude();
        modelBuilder.Entity<PurchaseOrder>().Navigation(x => x.PODetail).AutoInclude();
        
        modelBuilder.Entity<Order>().Navigation(x => x.Party).AutoInclude();
        modelBuilder.Entity<Order>().Navigation(x => x.OrderDetail).AutoInclude();
        modelBuilder.Entity<Order>().Navigation(x => x.OT).AutoInclude();

        modelBuilder.Entity<OrderDetail>().Navigation(x => x.Item).AutoInclude();
        modelBuilder.Entity<OrderDetail>().Navigation(x => x.OT).AutoInclude();
        
        modelBuilder.Entity<PurchaseOrderDetail>().Navigation(x => x.Item).AutoInclude();
        
        modelBuilder.Entity<SaleOrder>().Navigation(x => x.Supplier).AutoInclude();
        modelBuilder.Entity<SaleOrder>().Navigation(x => x.SODetails).AutoInclude();

        modelBuilder.Entity<SaleOrderDetail>().Navigation(x => x.Item).AutoInclude();

        modelBuilder.Entity<StockTransactions>().Navigation(x => x.Item).AutoInclude();
        
        modelBuilder.Entity<Invoice>().Navigation(x => x.Customer).AutoInclude();
        modelBuilder.Entity<Invoice>().Navigation(x => x.InvoiceDetails).AutoInclude();

        modelBuilder.Entity<InvoiceDetails>().Navigation(x => x.Product).AutoInclude();

      
        #region Seed Users

        modelBuilder.Entity<AppUsers>().HasData(
            new AppUsers { Id = 1, UserName = "admin", Password = "admin", FullName = "Administrator" }
            );

        #endregion
    }

    public DbSet<AppUsers> AppUsers { get; set; } = default!;
    public DbSet<MasterMenu> MasterMenu { get; set; } = default!;
    public DbSet<Party> Party { get; set; } = default!;
    public DbSet<ProductDetails> ProductDetails { get; set; } = default!;
    public DbSet<ProductionOrder> ProductionOrders { get; set; } = default!;
    public DbSet<ProductionOrderDetail> ProductionOrderDetails { get; set; } = default!;
    public DbSet<PurchaseOrder> PurchaseOrder { get; set; } = default!;
    public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = default!;
    public DbSet<SaleOrder> SaleOrders { get; set; } = default!;
    public DbSet<SaleOrderDetail> SaleOrderDetails { get; set; } = default!;
    public DbSet<Order> Order { get; set; } = default!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = default!;
    public DbSet<OrderTransactions> OrderTransactions { get; set; } = default!;
    public DbSet<StockTransactions> StockTransactions { get; set; } = default!;
    public DbSet<Invoice> Invoice { get; set; } = default!;
    public DbSet<InvoiceDetails> InvoiceDetails { get; set; } = default!;
}
