namespace API.Data;

public class TMSContext : DbContext
{
    public TMSContext (DbContextOptions<TMSContext> options)
        : base(options)
    {
    }

    public DbSet<Party> Party { get; set; } = default!;
    public DbSet<PurchaseOrder> PurchaseOrder { get; set; } = default!;
}
