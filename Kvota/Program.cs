using BlazorBootstrap;
using Kvota.Areas.Identity.Data;
using Kvota.Data;
using Kvota.Interfaces;
using Kvota.Migrations;
using Kvota.Models;
using Kvota.Models.Content;
using Kvota.Models.Products;
using Kvota.Repositories;
using Kvota.Repositories.Products;
using Kvota.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("KvotaContextConnection") ?? throw new InvalidOperationException("Connection string 'KvotaContextConnection' not found.");
var connectionStringProduct = builder.Configuration.GetConnectionString("ProductContextConnection") ?? throw new InvalidOperationException("Connection string 'ProductContextConnection' not found.");
builder.Services.AddOptions();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IRepo<Home>, HomeRepo>();
builder.Services.AddScoped<IRepo<ContactsModel>, ContactRepo>();
builder.Services.AddScoped<IRepo<Product>, ProductRepo>();
builder.Services.AddScoped<IRepo<Category>, CategoryRepo>();
builder.Services.AddScoped<IRepo<GrandCategory>, GrandCategoryRepo>();
builder.Services.AddScoped<IRepo<Brand>, BrandRepo>();
builder.Services.AddScoped<IRepo<CategoryOption>, CategoryOptionsRepo>();
builder.Services.AddScoped<IRepo<ProductOption>, ProductOptionRepo>();

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ISerializeService<Home>, SerializeService<Home>>();
builder.Services.AddScoped<ISerializeService<ContactsModel>, SerializeService<ContactsModel>>();

builder.Services.AddDbContext<KvotaContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<KvotaProductContext>(options =>
    options.UseNpgsql(connectionStringProduct));

builder.Services.AddDefaultIdentity<KvotaUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<KvotaContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorBootstrap();



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
