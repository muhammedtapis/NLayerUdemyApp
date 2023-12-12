using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using System.Threading.Tasks;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryWithDTOController : CustomBaseController
    {
        private readonly IServiceWithDTO<Category, CategoryDTO> _categoryServiceWithDTO;

        public CategoryWithDTOController(IServiceWithDTO<Category, CategoryDTO> categoryServiceWithDTO)
        {
            _categoryServiceWithDTO = categoryServiceWithDTO;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return CreateActionResult(await _categoryServiceWithDTO.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Save(CategoryDTO categoryDto)
        {
            return CreateActionResult(await _categoryServiceWithDTO.AddAsync(categoryDto));
        }
    }
}