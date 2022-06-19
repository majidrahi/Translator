using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Translator.Data.Context;
using Translator.Domain.Models;
using Translator.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var translatorDBConnection = builder.Configuration.GetConnectionString("TranslatorDBConnection");


builder.Services.AddDbContext<TranslatorDbContext>(options =>
    options.UseSqlServer(translatorDBConnection)
);

builder.Services.AddHttpClient("Translate", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetSection("TanslationApiUrl").Value);
});

//DI
builder.Services.RegisterServices();
//mapper
builder.Services.AddAutoMapper(cfg => {
    cfg.AddExpressionMapping();
}, AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
}
)
    .AddEntityFrameworkStores<TranslatorDbContext>();
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
