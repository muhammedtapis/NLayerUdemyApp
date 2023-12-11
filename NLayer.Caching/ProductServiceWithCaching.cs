using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        //nelere ihtiyacımız var onu al
        private const string CacheProductKey = "productsCache"; //products tutacak olan cache

        private readonly IMapper mapper;  //mapleme yapcaz metodlarda
        private readonly IMemoryCache _memoryCache; //inmemory caching için
        private readonly IProductRepository _repository;  //repository lazım database metodlarına erişim için
        private readonly IUnitOfWork _unitOfWork; //unitOfWork metodları lazım saveChanges yapcaz Db ye yansıtmak için.

        //program.cs tarafında cache aktif et!!
        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository repository, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            _memoryCache = memoryCache;
            _repository = repository;
            _unitOfWork = unitOfWork;

            //ilk nesne örneği oluştuğu an burası burada cacheleme yapmamız gerekiyor.
            if (!memoryCache.TryGetValue(CacheProductKey, out _))//cachedeki datayı almıcaz sadece true fals durumunu öğrenmek istiyoruz bu sebeple
            {                                                  //boş karakter bıraktık _ memoryde yer tutmasın,out _ geri dönülen kısım.
                memoryCache.Set(CacheProductKey, _repository.GetProductsWithCategory().Result);//eğer cache yoksa oluştur repositoryden bütün datayı al listele set et
            }
        }

        //cacheleyeceğiniz data çok sık eriştiğiniz ama çok sık güncellemediğiniz bir data olmalı en iyi kullanım böyle.
        public async Task<Product> AddAsync(Product entity)
        {
            await _repository.AddAsync(entity); //veritabanıne ekleme yaptık flagı değiştirdik
            await _unitOfWork.CommitAsync(); //daha sonra bu flagı veritabanına yansıttık.kaydettik
            await CacheAllProductsAsync();   //veri değiştiği için tekrar cacheleme yaptık.
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _repository.AddRangeAsync(entities); //veritabanıne ekleme yaptık flagı değiştirdik
            await _unitOfWork.CommitAsync(); //daha sonra bu flagı veritabanına yansıttık.kaydettik
            await CacheAllProductsAsync();   //veri değiştiği için tekrar cacheleme yaptık.
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            return await _repository.AnyAsync(expression);
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            //get metodundan sonra gelen   <IEnumerable<Product>> bize bu cachein getireceği verinin tipidir.
            return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(CacheProductKey));
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                throw new NotFoundException($"{typeof(Product).Name}({id}) not found in cache!!");
            }
            return Task.FromResult(product);
        }

        public Task<CustomResponseDTO<List<ProductWithCategoryDTO>>> GetProductsWithCategory()
        {
            //cacheleme yaparken sadece Products dönmüştük constructorda eğer öyle yaparsak cache üzerinden kategorilerine erişemeyiz ya direkt veritabanından
            //bilgiyi çekicez ya da kategorilerle birlikte cacheleme yapıcaz
            var productsWithCategory = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey); //cacheten productları getirdik
            var productsWithCategoryDTO = mapper.Map<List<ProductWithCategoryDTO>>(productsWithCategory); //bunları mapledik biliyoruz çünkü kategorileri de var
            return Task.FromResult(CustomResponseDTO<List<ProductWithCategoryDTO>>.Success(200, productsWithCategoryDTO));
        }

        public async Task RemoveAsync(Product entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            //sorgulama işini artık cache üzerinden yapcaz bu cache ne gelecek List<Product> gelcek,sonra hangi cache onu ver,Where sorgusu yazılcak
            //ama  içinbe direkt expression veremiyoruz çünkü func istiyor Compile() metodu ile çevirip en son IQueryable dönceğimiz için AsQueryable() yazıyoruz.
            return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
            //aşağıdaki CacheallProductsAsync metodunda cachelemeyi list olarak yaptık bu sebeple get yaptığımızda o listeyi ve tipini vermemiz gerekiyor.
        }

        //bu metodu her çağırdığımızda sıfırdan datayı çekiğ cachleme yapıyor.
        public async Task CacheAllProductsAsync()
        {
            //hangı cache set edilcek onun key , ve ne set edilcek o
            _memoryCache.Set(CacheProductKey, await _repository.GetAll().ToListAsync());
        }
    }
}