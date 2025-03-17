var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TMSContext") ?? throw new InvalidOperationException("Connection string 'TMSContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
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
builder.Services.AddScoped<IStockTransactionsRepo, StockTransactionsService>();

#endregion

// Configure HttpClient to bypass SSL validation
builder.Services.AddHttpClient("").ConfigurePrimaryHttpMessageHandler(() =>
    new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
    });

// In-Memory Caching
builder.Services.AddMemoryCache();

#region Jwt Authentication Config

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer= jwtSettings["Issuer"],
        ValidAudience= jwtSettings["Audience"]
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

if (app.Environment.IsDevelopment())
{
}
    app.UseSwagger();
    app.UseSwaggerUI();

// Client-side Caching
app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
    {
        Public = true,
        MaxAge = TimeSpan.FromSeconds(60) // Cache duration
    };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new[] { "Accept-Encoding" };

    await next();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();