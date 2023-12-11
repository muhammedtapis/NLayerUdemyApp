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
//Mapleme tanýtýmý
builder.Services.AddAutoMapper(typeof(MapProfile));  //assembly de verebiliriz biz type of verdik.

//Db yolu programa tanýtma.
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name); //migration dosyasý oluþacak yerin adresini verdik.
    });
});

//notFoundFilter programa tanýtma
builder.Services.AddScoped(typeof(NotFoundFilter<>));

//autofac Dependency Injection container!!!! program.cs içinde yaptýðýmýz dependency injectionlarý artýk yeni modülümüzde yapcaz.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //indirdiðimiz autofac kütüphanesi
//ardýndan bir modül ekleyeceðiz bu modül içerisinde dinamik olarak ekleme  iþlemleri yapacaðýz bu modülü de burada ekledik.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

var app = builder.Build();

//notfoundfilteri error sayfasýna yönlendiriyorz o yüzden aþaðýdaki ifin dýþýna aldýk o developmentta yönlendirmiyor.
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