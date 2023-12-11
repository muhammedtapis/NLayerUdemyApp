using Autofac;
using NLayer.Caching;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;

namespace NLayer.API.Modules
{
    //autofac ten gelen Module  sınıfını miras aldık
    public class RepoServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //program.cs te eklediğimiz scopeları artık burda yapıyoruz ordan sileceğiz.

            //generic olan ekleme
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

            //generic olmayan ekleme
            //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            var apiAssembly = Assembly.GetExecutingAssembly(); //apinin assemblysi
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext)); //repository katmanında herhangi bir classı verdik ordan assmbly bilgisi alcaz.
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile)); //aynı şekilde servis katmanında da assembly bilgisine eriştik

            //bu aşağıdaki kod program.cs te addcsope yaptığımız kodları kısaltıyor asemmblylere gidip sonu Repository ile biten classları alııyor.
            //daha sonra onların interfacelerini de implemente ediyor.
            //builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); bu satırdaki işi yapıyor ama tek seferde bütün Repository olanlar için
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces().InstancePerLifetimeScope();
            //InstancePerLifetimeScope() => AddScoped(); request bitene kadar aynı instance kullanılır
            //InstancePerDependency() => AddTransient(); herhangi bir classın constructorında o interface nerde geçildiyse her seferinde yeni instance oluşur.
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service"))
                   .AsImplementedInterfaces().InstancePerLifetimeScope();

            //Cacheten işlem yapan servisi baz al IProductService görürsen.
            //builder.RegisterType<ProductServiceWithCaching>().As<IProductService>().InstancePerLifetimeScope(); //cacheden okumayı kapadık api-mvc
        }
    }
}