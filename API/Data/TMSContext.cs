namespace API.Data;

public class TMSContext : DbContext
{
    public TMSContext (DbContextOptions<TMSContext> options)
        : base(options)
    {
    }

    public DbSet<CustomerInfo> CustomerInfo { get; set; } = default!;
}
