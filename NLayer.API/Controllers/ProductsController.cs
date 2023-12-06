using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NLayer.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //endpointe gerek yok  çünkü miras aldığı controllerda zaten var.
    public class ProductsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _service;

        public ProductsController(IMapper mapper, IProductService service)
        {
            _mapper = mapper;
            _service = service;
        }

        //GET api/products/GetProductsWithCategory  şeklinde çağırcaz burada başka get metoduda var bu sebeple bu şekilde belirtmemiz gerke.
        //[HttpGet(nameof(GetProductsWithCategory))]
        [HttpGet("[action]")]   // üst satırla aynı iş
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _service.GetProductsWithCategory());
            //aşağıdaki metodlarda CreateActionREsult metodu içinde CustomREsponseDto oluşturuyorduk burda onu servis katmanında yapıp buraya yolladık.
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync(); //bütün product datalarını al.
            var productsDTO = _mapper.Map<List<ProductDTO>>(products.ToList()); //products IEnumerable tipinde liste castet listesini ver productDTO list şeklinde maple.
            //geriye döneceğimiz şey CustomResponseDTO içinde veriyi de hataları da gönderebiliyoruz.
            //return Ok(CustomResponseDTO<List<ProductDTO>>.Success(200, productsDTO));
            //üst satırdaki metodu custom hale çevirmek için CustomBaseController oluşturduk bunu kullanmak  için miras alman lazım bu controllerda.
            return CreateActionResult(CustomResponseDTO<List<ProductDTO>>.Success(200, productsDTO));
            //bu metodun bize sağladığı fayda geriye return OK return BadREquest gibi farklı metodlar yerine tek metod dönmek
        }

        //endpointten id gelcek.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //yukarıdaki metodun açıklamayı okuyup bununla karşılaştırırsan daha iyi anlarsın birinde product liste birinde tek product dönülüyor.
            var product = await _service.GetByIdAsync(id); //bütün product datalarını al.
            var productDTO = _mapper.Map<ProductDTO>(product); //products IEnumerable tipinde liste castet listesini ver productDTO list şeklinde maple.
            return CreateActionResult(CustomResponseDTO<ProductDTO>.Success(200, productDTO));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO productDto)
        {
            //clientten productDTO gelcek eklemek için metod Product istiyor mapliyoruz gelen Dto yu
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var productDTO = _mapper.Map<ProductDTO>(product); //kullanıcıya göstermek için tekrar productDTO nesnesine mapliyoruz
            return CreateActionResult(CustomResponseDTO<ProductDTO>.Success(201, productDTO)); //kullanıcıya gösteriyoruz.201 created durum kodu.
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDTO productDto)
        {
            //clientten productDTO gelcek güncellemek için metod Product istiyor mapliyoruz gelen Dto yu ama update metodu geriye bişey dmnmüyor.!!!
            await _service.UpdateAsync(_mapper.Map<Product>(productDto));
            return CreateActionResult(CustomResponseDTO<NoContentDTO>.Success(204)); //kullanıcıya gösteriyoruz.204 NOCONTENT durum kodu. OVerload edilmiş datasız metod.
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(product);
            return CreateActionResult(CustomResponseDTO<NoContentDTO>.Success(204));
        }
    }
}