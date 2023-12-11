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

        //savechanges oeverride edicez createdupdated date için
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //save changes çağırana kadar tüm entityleri memoryde track ediyor ef core, veritabanına yansıtmadan önce burada entity update me edildi yoksa
            //yeni  mi insert edildi ona bakıp createddate yada updateddate değiştricez
            //bunun için önce track edilmiş entityleri döncez.

            foreach (var item in ChangeTracker.Entries()) //changeTracker.Entries ile entitylere ulaştık
            {
                if (item.Entity is BaseEntity entityReferance) //ulaştığımız entityler bir baseEntity ise entityREferance ataması yap
                {
                    switch (item.State)
                    {
                        case EntityState.Added:  //eğer entitystate insert ise createdDate değiş
                            {
                                entityReferance.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified: //eğer entitystate update ise updatedDate değiş
                            {
                                //eğer güncelleme yapılıyorsa EFcore a createdDate alanının bu güncellemeye dahil olmadığını belirtmemiz gerek
                                //yoksa default bir değer atıyor createdDAte kayboluyor
                                Entry(entityReferance).Property(x => x.CreatedDate).IsModified = false; //bu alanı modified değil. eğer bunu yapmazsak efcore bunu modified tanımlayıp bu alanı değişcek.
                                entityReferance.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        //aynı işlemi saveChanges için de yaptık hem asenkron hem senkron metotlar override edildi
        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries()) //changeTracker.Entries ile entitylere ulaştık
            {
                if (item.Entity is BaseEntity entityReferance) //ulaştığımız entityler bir baseEntity ise entityREferance ataması yap
                {
                    switch (item.State)
                    {
                        case EntityState.Added:  //eğer entitystate insert ise createdDate değiş
                            {
                                entityReferance.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified: //eğer entitystate update ise updatedDate değiş
                            {
                                //eğer güncelleme yapılıyorsa EFcore a createdDate alanının bu güncellemeye dahil olmadığını belirtmemiz gerek
                                //yoksa default bir değer atıyor createdDAte kayboluyor
                                Entry(entityReferance).Property(x => x.CreatedDate).IsModified = false; //bu alanı modified değil. eğer bunu yapmazsak efcore bunu modified tanımlayıp bu alanı değişcek.

                                entityReferance.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }
            return base.SaveChanges();
        }
    }
}