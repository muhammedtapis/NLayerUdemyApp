using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Seeds
{
    internal class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product() { Id = 1, Name = "Kalem1", Price = 11, Stock = 100, CategoryId = 1 },
                new Product() { Id = 2, Name = "Kalem2", Price = 22, Stock = 200, CategoryId = 1 },
                new Product() { Id = 3, Name = "Kalem3", Price = 33, Stock = 300, CategoryId = 1 },

                new Product() { Id = 4, Name = "Kitap1", Price = 10, Stock = 100, CategoryId = 2 },
                new Product() { Id = 5, Name = "Kitap2", Price = 20, Stock = 200, CategoryId = 2 }
                );
        }
    }
}