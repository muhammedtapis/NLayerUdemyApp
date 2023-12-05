using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        //options almamızın sebebi bu optionsla beraber veritabanı yolunu startup dosyasında vercez. oradaki optionsa gönderiyoruz
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //veritabanı tablolarımız.
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        //bu alanı kapadık çünkü productFeature eklemek istiyorsak mutlaka Product üzerinden eklenmesini istiyoruz,ikisi zaten bire bir ilişkili.
        //public DbSet<ProductFeature> ProductFeatures { get; set; }

        //entity configuration override ettik model oluşurken çalışcak.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //configuration sınıflarında implemente ettiğimiz interface sayesinde hepsini buluyor.
            //modelBuilder.ApplyConfiguration(new ProductConfiguration()); //böyle tek tek vermek yerine yukarıdaki metodla hepsini veriyoruz

            base.OnModelCreating(modelBuilder);
        }
    }
}