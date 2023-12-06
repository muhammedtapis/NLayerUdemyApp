using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;

namespace NLayer.Service.Services
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork, IMapper mapper, ICategoryRepository categoryRepository) : base(repository, unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CustomResponseDTO<CategoryWithProductsDTO>> GetSingleCategoryByIdWithProductsAsync(int categoryId)
        {
            var categoryWithProducts = await _categoryRepository.GetSingleCategoryByIdWithProductsAsync(categoryId);
            var categoryWithProductsDTO = _mapper.Map<CategoryWithProductsDTO>(categoryWithProducts);
            return CustomResponseDTO<CategoryWithProductsDTO>.Success(200, categoryWithProductsDTO);
        }
    }
}