using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    //servis katmanında implemente edilecek repositorydeki metodlarla aynı ama bunların ileride productRepository productService interface
    //yazdığımızda bu metodların dönüş tipleri vs değişecek.
    public interface IService<T> where T : class
    {
        Task<T> GetByIdAsync(int id);

        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> GetAllAsync(); //IGenericRepositoryden değişik bu mesela collection döncek sorgusuz tüm datayı çekcez.

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        //Burada update remove metodları void olamaz çünkü SaveChanges(); yapacağız veritabanına bu değişiklikleri yansıtacağız.
        Task UpdateAsync(T entity);

        Task RemoveAsync(T entity);

        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}