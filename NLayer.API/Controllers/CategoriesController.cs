using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NLayer.API.Controllers
{
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int categoryId)
        {
            return CreateActionResult(await _categoryService.GetSingleCategoryByIdWithProductsAsync(categoryId));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllAsync(); //generic metoddan gelen getAllAsync

            //bu endpointi webe göndercez orada dto olarak almamız gerek dropdown da göstercez
            var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories.ToList());

            return CreateActionResult(CustomResponseDTO<List<CategoryDTO>>.Success(200,categoriesDTO));
        }
    }
}