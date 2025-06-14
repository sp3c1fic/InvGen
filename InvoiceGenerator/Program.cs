using InvoiceGenerator.InvoiceGenerator.Core.Contracts;
using InvoiceGenerator.InvoiceGenerator.Core.Mapping;
using InvoiceGenerator.InvoiceGenerator.Core.Services;
using InvoiceGenerator.InvoiceGenerator.Infrastructure;
using InvoiceGenerator.InvoiceGenerator.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Set QuestPDF license
QuestPDF.Settings.License = LicenseType.Community;

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("WindowsDefaultConnection") ??
       throw new InvalidOperationException("Connection string 'DefaultConnection' not found!");

builder.Services.AddDbContext<InvoiceGeneratorDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 16;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<InvoiceGeneratorDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();


builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IPdfTemplateService, PdfTemplateService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();


builder.Services.AddHttpClient();
builder.Services.AddRazorPages();

builder.Services.AddAutoMapper(typeof(MappingProfile));
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
