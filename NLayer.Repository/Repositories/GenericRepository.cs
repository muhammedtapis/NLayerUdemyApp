using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using System.Linq.Expressions;

namespace NLayer.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class //efcore classlarla çalışıyo bu T nin class olduğunu belirt.
    {
        //readonly keywordu ile sadece constrcutor ve burada değer atanabilir.
        protected readonly AppDbContext _context; //protected yapmamızın sebebi ilerde miras aldığımız sınıflarda erişebilmek farklı metodlar için.

        //bu _context nesnesine ProductRepositoryde eriştik.

        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); //T generic entitysini DbSet et. generic metodlarla çalışıyoruz gelen entitynin türü belirsiz.
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity); //bu metod dbset sınıfından geliyor.tablodan ekliyoruzz direkt.
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities); //bu metodlar EF.Core kendi metodları
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression); //Lambda ifadesine göre var mı yok mu true false dönen metod.
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking().AsQueryable(); //efcore çekmiş olduğu dataları memory almasın track etmesin daha performanslı çalışsın.
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id); //idye göre tablodaki satırı bul.
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity); //silme işlemi ama state değişiyor burada sadece deleted flag oluyo .
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}