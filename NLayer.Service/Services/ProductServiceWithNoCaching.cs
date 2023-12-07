using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;

namespace NLayer.Service.Services
{
    //servis katmanından gelen metodları alması için miras aldık. daha sonra özel metodları almak için IProductService miras aldık
    public class ProductServiceWithNoCaching : Service<Product>, IProductService
    {
        private readonly IProductRepository _repository; //veritabanı metodlarına erişcez.
        private readonly IMapper _mapper; //mapleme yapcaz

        public ProductServiceWithNoCaching(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _repository = productRepository;
        }

        //bu metod önemli API nin istediği türü gönderiyoruz direkt exception yakalamayı da burda yapabiliriz şuan.
        public async Task<CustomResponseDTO<List<ProductWithCategoryDTO>>> GetProductsWithCategory()
        {
            var productsWithCategory = await _repository.GetProductsWithCategory();
            var productsWithCategoryDTO = _mapper.Map<List<ProductWithCategoryDTO>>(productsWithCategory);
            return CustomResponseDTO<List<ProductWithCategoryDTO>>.Success(200, productsWithCategoryDTO);
        }
    }
}