using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Kvota.Interfaces;
using Kvota.Migrations;
using Kvota.Models;
using Kvota.Models.Content;
using Kvota.Models.Products;
using Kvota.Models.UserAuth;
using Kvota.Repositories;
using Kvota.Repositories.Products;
using Kvota.RepositoriesUi.Products;
using Kvota.Services;
using Kvota.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("KvotaContextConnection") ?? throw new InvalidOperationException("Connection string 'KvotaContextConnection' not found.");
var connectionStringProduct = builder.Configuration.GetConnectionString("ProductContextConnection") ?? throw new InvalidOperationException("Connection string 'ProductContextConnection' not found.");
builder.Services.AddServerSideBlazor();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddHttpClient("api", c =>
{
    c.BaseAddress = new Uri("https://localhost:7039/");
    c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
});

builder.Services.AddScoped<IRepo<Home>, HomeRepo>();
builder.Services.AddScoped<IRepo<ContactsModel>, ContactRepo>();
builder.Services.AddScoped<IRepo<Product>, ProductRepo>();
//builder.Services.AddScoped<IRepo<Product>, ProductRepoUi>();
builder.Services.AddScoped<IRepo<Category>, CategoryRepo>();
builder.Services.AddScoped<IRepo<Brand>, BrandRepo>();
builder.Services.AddScoped<IRepo<CategoryOption>, CategoryOptionsRepo>();
builder.Services.AddScoped<IRepo<ProductOption>, ProductOptionRepo>();
builder.Services.AddScoped<IRepo<Storage>, StorageRepo>();
builder.Services.AddScoped<IRepo<ApplicationOrderingProducts>, OrderRepo>();
//builder.Services.AddScoped<IRepo<ProductsInStorage>, ProductInStorageRepo>();

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ISerializeService<Home>, SerializeService<Home>>();
builder.Services.AddScoped<ISerializeService<ContactsModel>, SerializeService<ContactsModel>>();
builder.Services.AddScoped<IMissingProductConverter, MissingProductConverter>();

builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomStateProvider>();

builder.Services.AddDbContext<KvotaContext>(options =>
    options.UseNpgsql(connectionString).UseLazyLoadingProxies().AddInterceptors(new SoftDeleteInterceptor()));

builder.Services.AddDbContext<KvotaProductContext>(options =>
    options.UseLazyLoadingProxies().UseNpgsql(connectionStringProduct).AddInterceptors(new SoftDeleteInterceptor()));

builder.Services.AddIdentity<KvotaUser, IdentityRole>().AddEntityFrameworkStores<KvotaContext>().AddDefaultTokenProviders();

builder.Services.AddRazorPages();

builder.Services.AddBlazorBootstrap();
builder.Services.AddMudServices();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


app.Run();
