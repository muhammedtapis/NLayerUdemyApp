using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;

namespace NLayer.Repository.Seeds
{
    internal class ProductFeatureSeed : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            builder.HasData(
                new ProductFeature() { Id = 1, Color = "black", Height = 4, Width = 2, ProductId = 1 },
                new ProductFeature() { Id = 2, Color = "red", Height = 12, Width = 10, ProductId = 4 }
                );
        }
    }
}