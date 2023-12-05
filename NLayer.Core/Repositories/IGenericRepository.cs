using System.Linq.Expressions;

namespace NLayer.Core.Repositories
{
    //veritabanına yapacağımız tüm temel sorguları burada ekleyeceğiz.Repository katmanında implemente edilecek
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id); //idye göre data dönme metodu

        //şu şekilde sorgu yazabilmek için expression belirtmek gerek productRepository.Where(x => x.id > 5).OrderBy.ToListAsync();
        //Where içinde bir expression ifadesi olacak bu expressionın tipi func delegesi olcak parametre olarak T alacak geriye ise bool dönecek
        //T burada entity oluyor expressionın solundaki x lambda ifadesi geriye döndüğü değer ise (x => x.id > 5) x.id büyük mü değil mi onu dönecek her bir satır için.
        IQueryable<T> Where(Expression<Func<T, bool>> expression);  //IQueryable döndüğümüzde sorgular direkt veritabanına gitmez toList toListASync yaptığımızda veritabanına gider.

        //IQueryable yapmamızın diğer nedeni biz bu metodu çağırdığımızda metodun devamına başka sorgular da yazabiliriz en son ToList eklediğimizde veritabanına gider.

        IQueryable<T> GetAll();

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression); //var mı yok mu true false dön.

        Task AddAsync(T entity); // ekleme

        Task AddRangeAsync(IEnumerable<T> entities);  //birden fazla entity ekleyebiliriz IEnumerable collection liste gibi Interface olarak belirtmemiz bu şekilde daha iyi.

        //Ef.Core DbContextin update ve remove  için asenkron metodları yok.IServicete değişecek bu
        void Update(T entity);  //güncellemede async olmasına gerek yok ef.core zaten takip ediyor bu entityi sadece state değiştiriyor.

        void Remove(T entity);  //silmede async olmasına gerek yok ef.core zaten takip ediyor bu entityi sadece state değiştiriyor.

        void RemoveRange(IEnumerable<T> entities); //birden fazla entity silme
    }
}