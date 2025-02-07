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
    }

    public DbSet<Party> Party { get; set; } = default!;
    public DbSet<ProductDetails> ProductDetails { get; set; } = default!;
    public DbSet<PurchaseOrder> PurchaseOrder { get; set; } = default!;
}
