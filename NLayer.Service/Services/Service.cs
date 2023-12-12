using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System.Linq.Expressions;

namespace NLayer.Service.Services
{
    //bu servis repo katmanıyla haberleşicek veritabanıyla işlem yapcak.
    //update remove add metodlarında _unitOfWork.CommitAsync() metodu çağırıyoruz çünkü bu işlemlerde SaveChanges() yapılıyor.
    public class Service<T> : IService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public Service(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<T> AddAsync(T entity)
        {
            //UNIT OF WORK kullanımı
            await _repository.AddAsync(entity); //tabloya ekleme işlemi  FLAG DEĞİŞTİRDİ
            await _unitOfWork.CommitAsync(); //veritabanına bu ekleme işlemini kaydetme İŞLEMİ ,KAYIT YAPTI
            return entity;//ekledğimiz satırla işlem yapmak isteyebiliriz o yüzden döndük.
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            return entities; //geriye listesini dön
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.AnyAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            //null kontrolunu burada yapıyoruz ACtion metodu dolmasın.oluşturduğumuz custom hata fırlatıyoruz.
            var hasIt = await _repository.GetByIdAsync(id);
            if (hasIt == null)
            {
                throw new NotFoundException($"{typeof(T).Name}-({id}) not found!"); //generic T classının tipini al ismini göster
            }
            return hasIt;
        }

        public async Task RemoveAsync(T entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _repository.Where(expression);
        }
    }
}