using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using NLayer.Core.Models;

namespace NLayer.Repository.Configuration
{
    //modelbuilder içinde tüm entityleri configure etmek yerine ayrı sınıflar oluşturup bu şekilde her entity ayrı yerde konfigure edebiliriz.
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id); //istersek böyle tanımlayabiliriz
            builder.Property(x => x.Id).UseIdentityColumn(); //identity sütunu yaptık.
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50); //isim alanı zorunlu ve max 50 karakter.

            builder.ToTable("Categories"); //tablo ismi de değiştirilebilir.
        }
    }
}