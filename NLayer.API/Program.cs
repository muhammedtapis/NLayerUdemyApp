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

//validatorumuzu AddControllers tan sonra ekledik. Ardýndan bu validator ile kullancaðýmýz filterimizi ekledik.
builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDTOValidator>());

//yukarýdkai filteri kullanabilmek için API nin kendi FluentValidatorunu devre dýþý býrak filter yapmasýný iptal et
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    //frameworkün kendi dönmüþ olduu filtreyi baskýladýk bizim filterimizi çalýþsýn diye. MVC tarafýnda bu þekilde baskýlamana gerek yok default aktif deðil.
    options.SuppressModelStateInvalidFilter = true;
});

//Db yolu programa tanýtma.
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name); //migration dosyasý oluþacak yerin adresini verdik.
    });
});

//oluþturduðumuz interfaceleri burada ekliyoruz
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddScoped<IProductRepository, ProductRepository>(); //productýn özel sorgularýnýn metodlarýnýn olduðu repository ve servis tanýtýmý.
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); //category özel sorgularýnýn metodlarýnýn olduðu repository ve servis tanýtýmý.
builder.Services.AddScoped<ICategoryService, CategoryService>();

//Mapleme tanýtýmý
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

app.UseCustomException();  //custom exception Middleware tanýtýmý.

app.UseAuthorization();

app.MapControllers();

app.Run();