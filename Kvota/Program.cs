using BlazorBootstrap;
using Kvota.Areas.Identity.Data;
using Kvota.Data;
using Kvota.Interfaces;
using Kvota.Migrations;
using Kvota.Models.Content;
using Kvota.Models.Products;
using Kvota.Repositories;
using Kvota.Repositories.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("KvotaContextConnection") ?? throw new InvalidOperationException("Connection string 'KvotaContextConnection' not found.");
var connectionStringProduct = builder.Configuration.GetConnectionString("ProductContextConnection") ?? throw new InvalidOperationException("Connection string 'ProductContextConnection' not found.");

builder.Services.AddScoped<IRepo<Home>, HomeRepo>();
builder.Services.AddScoped<IRepo<ContactsModel>, ContactRepo>();
builder.Services.AddScoped<IRepo<Product>, ProductRepo>();
builder.Services.AddScoped<IRepo<Category>, CategoryRepo>();
builder.Services.AddScoped<IRepo<Brand>, BrandRepo>();
builder.Services.AddDbContext<KvotaContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<KvotaProductContext>(options =>
    options.UseNpgsql(connectionStringProduct));

builder.Services.AddDefaultIdentity<KvotaUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<KvotaContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorBootstrap();



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
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


app.Run();
