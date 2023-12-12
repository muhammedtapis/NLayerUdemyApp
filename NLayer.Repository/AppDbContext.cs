using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using System.Reflection;

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

        //veri kaydedildiğinde yad agüncellendiğinde tarih ataması yapıyoruz override ederek bu metodu önce bizim yazdığımız
        //date kodu çalışacak ardından base den gelen SAveChanges çalışacak ve kayıt edecek.
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateChangeTrackerCreateUpdateDate();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateChangeTrackerCreateUpdateDate();
            return base.SaveChanges();
        }

        //bu metod save changes metodundan önce çalıştırmak için var burada created ve updated dateleri ekleme güncelleme yapıyoruz.
        public void UpdateChangeTrackerCreateUpdateDate()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity baseEntity)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                Entry(baseEntity).Property(x => x.UpdatedDate).IsModified = false; //nolur nolmaz ekleme yaparken updated date atamasın diye
                                baseEntity.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                //update ederken createdDate alanını güncellemesin diye modified false yapman gerek!!!
                                //yoksa her update edildiğinde buraya bi güncelleme yapar değeri değişir.
                                Entry(baseEntity).Property(x => x.CreatedDate).IsModified = false;
                                baseEntity.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }
        }
    }
}