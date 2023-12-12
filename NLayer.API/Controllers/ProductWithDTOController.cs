using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWithDTOController : CustomBaseController
    {
        private readonly IProductServiceWithDTO _productServiceWithDTO;

        public ProductWithDTOController(IProductServiceWithDTO productServiceWithDTO)
        {
            _productServiceWithDTO = productServiceWithDTO;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _productServiceWithDTO.GetProductsWithCategory());
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {   //servisimiz direkt bize CreateActionREsult metodunun istediği DTO yu döndüğü için tek satırda işimizi hallettik
            return CreateActionResult(await _productServiceWithDTO.GetAllAsync());
        }

        //endpointten id gelcek.
        [HttpGet("{id}")]
        //eğer bir filter constructorda parametre alıyorsa mutlaka [ServiceFilter] üzerinden kullanmanız gerek.
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResult(await _productServiceWithDTO.GetByIdAsync(id));
        }

        //bu metodu productServiceDto da overload ettik ServiceDTO da add metodunda ProductDTO alıyor ama productServiceDto da ProductCreateDto alıyor
        //ikisini de kullanabiliriz bu sefer createDTO kullanmak istedik
        [HttpPost]
        public async Task<IActionResult> Save(ProductCreateDTO productCreateDto)
        {
            return CreateActionResult(await _productServiceWithDTO.AddAsync(productCreateDto)); //kullanıcıya gösteriyoruz.201 created durum kodu.
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDTO productUpdateDto)
        {
            return CreateActionResult(await _productServiceWithDTO.UpdateAsync(productUpdateDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            return CreateActionResult(await _productServiceWithDTO.RemoveAsync(id));
        }

        [HttpPost("SaveAll")]
        public async Task<IActionResult> Save(List<ProductCreateDTO> productDTOList)
        {
            return CreateActionResult(await _productServiceWithDTO.AddRangeAsync(productDTOList));
        }

        [HttpDelete("RemoveAll")]
        public async Task<IActionResult> RemoveAll(List<int> idList)
        {
            return CreateActionResult(await _productServiceWithDTO.RemoveRangeAsync(idList));
        }

        [HttpGet("Any/{id}")]
        public async Task<IActionResult> Any(int id)
        {
            return CreateActionResult(await _productServiceWithDTO.AnyAsync(x => x.Id == id));
        }
    }
}