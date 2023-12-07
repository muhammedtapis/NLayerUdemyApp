using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using NLayer.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//validatorumuzu AddControllers tan sonra ekledik. Ard�ndan bu validator ile kullanca��m�z filterimizi ekledik.
builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDTOValidator>());

//yukar�dkai filteri kullanabilmek i�in API nin kendi FluentValidatorunu devre d��� b�rak filter yapmas�n� iptal et
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    //framework�n kendi d�nm�� olduu filtreyi bask�lad�k bizim filterimizi �al��s�n diye. MVC taraf�nda bu �ekilde bask�lamana gerek yok default aktif de�il.
    options.SuppressModelStateInvalidFilter = true;
});

//cache aktifleme!!!
builder.Services.AddMemoryCache();

//Db yolu programa tan�tma.
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name); //migration dosyas� olu�acak yerin adresini verdik.
    });
});

//olu�turdu�umuz interfaceleri burada ekliyorduk art�k bu ekleme i�lemlerini autofac k�t�phanesi sayesinde olu�turdu�umuz RepoServiceModule i�inde yapt�k.
//builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));  //generic interface ekliyorsan typeof yapman laz�m.

//filter tan�mlamas� yapt�k ��nk� bu filter ctor da interface al�yo
builder.Services.AddScoped(typeof(NotFoundFilter<>));
//Mapleme tan�t�m�
builder.Services.AddAutoMapper(typeof(MapProfile));  //assembly de verebiliriz biz type of verdik.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//autofac Dependency Injection container!!!! program.cs i�inde yapt���m�z dependency injectionlar� art�k yeni mod�l�m�zde yapcaz.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //indirdi�imiz autofac k�t�phanesi
//ard�ndan bir mod�l ekleyece�iz bu mod�l i�erisinde dinamik olarak ekleme  i�lemleri yapaca��z bu mod�l� de burada ekledik.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomException();  //custom exception Middleware tan�t�m�.

app.UseAuthorization();

app.MapControllers();

app.Run();