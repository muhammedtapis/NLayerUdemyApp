using NLayer.Core.Models;

namespace NLayer.Core.Repositories
{
    public interface IProductRepository : IGenericRepository<Product> // genericRepositoryden gelen genel sorgulara ulaşmak için miras aldı.
    {
        Task<List<Product>> GetProductsWithCategory();
    }
}