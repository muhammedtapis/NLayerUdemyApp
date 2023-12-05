using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Seeds
{
    //default kayıt oluiturmak için var bu sınıf bu sınıfta id alanını kendiniz vermeniz gerekir.
    internal class CategorySeed : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category() { Id = 1, Name = "Kalemler" },
                new Category() { Id = 2, Name = "Kitaplar" },
                new Category() { Id = 3, Name = "Defterler" });
        }
    }
}