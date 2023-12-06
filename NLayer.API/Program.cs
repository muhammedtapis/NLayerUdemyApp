using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
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

//Db yolu programa tan�tma.
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name); //migration dosyas� olu�acak yerin adresini verdik.
    });
});

//olu�turdu�umuz interfaceleri burada ekliyoruz
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddScoped<IProductRepository, ProductRepository>(); //product�n �zel sorgular�n�n metodlar�n�n oldu�u repository ve servis tan�t�m�.
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); //category �zel sorgular�n�n metodlar�n�n oldu�u repository ve servis tan�t�m�.
builder.Services.AddScoped<ICategoryService, CategoryService>();

//Mapleme tan�t�m�
builder.Services.AddAutoMapper(typeof(MapProfile));  //assembly de verebiliriz biz type of verdik.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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