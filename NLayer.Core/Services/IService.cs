using System.Linq.Expressions;

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

        Task<T> AddAsync(T entity);

        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        //Burada update remove metodları void olamaz çünkü SaveChanges(); yapacağız veritabanına bu değişiklikleri yansıtacağız.
        Task UpdateAsync(T entity);

        Task RemoveAsync(T entity);

        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}