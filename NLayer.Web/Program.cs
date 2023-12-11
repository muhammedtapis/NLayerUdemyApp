using Autofac.Extensions.DependencyInjection;
using Autofac;
using NLayer.Web.Modules;
using Microsoft.EntityFrameworkCore;
using NLayer.Repository;
using NLayer.Service.Mapping;
using System.Reflection;
using FluentValidation.AspNetCore;
using NLayer.Service.Validations;
using NLayer.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDTOValidator>());
//Mapleme tan�t�m�
builder.Services.AddAutoMapper(typeof(MapProfile));  //assembly de verebiliriz biz type of verdik.

//Db yolu programa tan�tma.
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name); //migration dosyas� olu�acak yerin adresini verdik.
    });
});

//notFoundFilter programa tan�tma
builder.Services.AddScoped(typeof(NotFoundFilter<>));

//autofac Dependency Injection container!!!! program.cs i�inde yapt���m�z dependency injectionlar� art�k yeni mod�l�m�zde yapcaz.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //indirdi�imiz autofac k�t�phanesi
//ard�ndan bir mod�l ekleyece�iz bu mod�l i�erisinde dinamik olarak ekleme  i�lemleri yapaca��z bu mod�l� de burada ekledik.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

var app = builder.Build();

//notfoundfilteri error sayfas�na y�nlendiriyorz o y�zden a�a��daki ifin d���na ald�k o developmentta y�nlendirmiyor.
app.UseExceptionHandler("/Home/Error");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
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

app.Run();