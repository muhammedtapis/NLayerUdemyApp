using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;

namespace NLayer.Repository.Repositories
{
    //productrepository üzerinden genericRepositorydeki metodlara erişmek için miras aldık ve bunu generic Product verdik.
    //ondan miras almasaydık IProductRepositoryden miras aldığımızda o interfacein üst interfaceinden gelen tüm metodları doldurmak zorunda kalcaktık.
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        //_contexte erişmemizi sağlayan şey GenericRepository sınıfında protected olarak tanımlamamız, miras alınca erişebildik.
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        //product ile beraber bağlı olduğu kategorileri getirdik JOin ettik.
        public async Task<List<Product>> GetProductsWithCategory()
        {
            //eager loading.
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }
    }
}