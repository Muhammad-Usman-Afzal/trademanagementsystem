var builder = WebApplication.CreateBuilder(args);
string APIBaseUrl = builder.Configuration.GetSection("APIBaseUrl").Value;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

#region Application Services

builder.Services.AddScoped(typeof(IBaseRepoUI<>), typeof(BaseServiceUI<>));

builder.Services.AddHttpClient<IPartyRepoUI, PartyServiceUI>(client =>
{
    client.BaseAddress = new Uri(APIBaseUrl);
});
builder.Services.AddHttpClient<IPurchaseOrderRepoUI, PurchaseOrderServiceUI>(client =>
{
    client.BaseAddress = new Uri(APIBaseUrl);
});
builder.Services.AddHttpClient<IProductDetailsRepoUI, ProductDetailsServiceUI>(client =>
{
    client.BaseAddress = new Uri(APIBaseUrl);
});
builder.Services.AddHttpClient<IOrderRepoUI, OrderServiceUI>(client =>
{
    client.BaseAddress = new Uri(APIBaseUrl);
});
builder.Services.AddHttpClient<IOrderDetailRepoUI, OrderDetailServiceUI>(client =>
{
    client.BaseAddress = new Uri(APIBaseUrl);
});
builder.Services.AddHttpClient<IOrderTransactionsRepoUI, OrderTransactionsServiceUI>(client =>
{
    client.BaseAddress = new Uri(APIBaseUrl);
});
builder.Services.AddHttpClient<IAppUsersRepoUI, AppUsersServiceUI>(client =>
{
    client.BaseAddress = new Uri(APIBaseUrl);
});
builder.Services.AddHttpClient<IMasterMenuRepoUI, MasterMenuServiceUI>(client =>
{
    client.BaseAddress = new Uri(APIBaseUrl);
});
builder.Services.AddHttpClient<IStockTransactionsRepoUI, StockTransactionsServiceUI>(client =>
{
    client.BaseAddress = new Uri(APIBaseUrl);
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
