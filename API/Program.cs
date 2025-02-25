using Microsoft.OpenApi.Models;

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
builder.Services.AddScoped<IAppUsersRepo, AppUsersService>();
builder.Services.AddScoped<IMasterMenuRepo, MasterMenuService>();

#endregion

#region Jwt Authentication Config

// Add authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TechnologyiansSolutionsKiSecretKeyBuhatHiSecret"))
            };
        });

#endregion

#region Swagger Config

var bearerTokenScheme = new OpenApiSecurityScheme
{
    Reference = new OpenApiReference
    {
        Type = ReferenceType.SecurityScheme,
        Id = "Bearer"
    },
    In = ParameterLocation.Header
};
var bearerTokenSecurity = new OpenApiSecurityRequirement
                         {

                            { bearerTokenScheme,new List<string>()}
                         };
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Trade Management System", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(bearerTokenSecurity);
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(config =>
    config.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
    {
        ["activated"] = false
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
