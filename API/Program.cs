var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TMSContext") ?? throw new InvalidOperationException("Connection string 'TMSContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Application Services

builder.Services.AddScoped(typeof(IBaseRepo<>), typeof(BaseService<>));
builder.Services.AddScoped<IPartyRepo, PartyService>();
builder.Services.AddScoped<IPurchaseOrderRepo, PurchaseOrderService>();
builder.Services.AddScoped<IProductDetailsRepo, ProductDetailsService>();
builder.Services.AddScoped<IOrderRepo, OrderService>();
builder.Services.AddScoped<IOrderDetailRepo, OrderDetailService>();
builder.Services.AddScoped<IOrderTransactionsRepo, OrderTransectionService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
