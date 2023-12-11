using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        //listeleme sayfası olcak
        public async Task<IActionResult> Index()
        {
            var customResponse = await _productService.GetProductsWithCategory();
            //yukardaki metod bize customresponseDTO dönüyo bize ProductWİthCAtegoryDto lazım o da onun içindeki Data propertysinin içinde
            //CRDto sınıfına bakarsan T Data alanını görürsün api kullanırken metodlarda genel bir response dönmek için yaptık.
            return View(customResponse);
        }

        public async Task<IActionResult> Save()
        {
            //product kaydetmek için kategori bilgisi lazım çünk aralarında ilişki var bu sebeple dropdown liste kategorileri listeliyoruz
            var categories = await _categoryService.GetAllAsync();
            var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            ViewBag.Categories = new SelectList(categoriesDTO, "Id", "Name"); //dropdown listte arka planda id tutulcak kullanıcı ise name görcek
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddAsync(_mapper.Map<Product>(productDTO)); //metod bizden product istiyor ama bize ekrandan dto geliyor onu mapledik
                return RedirectToAction(nameof(Index));
            }

            //eğer kayıt başarısız olursa kategorileri tekrar dropdownda göster
            var categories = await _categoryService.GetAllAsync();
            var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories.ToList()); //bizden list istiyo ama getall metodu IEnumerable dönüyo tolist yap
            ViewBag.Categories = new SelectList(categoriesDTO, "Id", "Name"); //dropdown listte arka planda id tutulcak kullanıcı ise name görcek

            return View();
        }

        //filterin kullanılcağı sayfayı belirttik üstünde yazarak.
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productService.GetByIdAsync(id); //idye sahip productı bul

            //gelen productın kategorisini de dropdown listte göstermemiz lazım
            var categories = await _categoryService.GetAllAsync();
            var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories.ToList()); //bizden list istiyo ama getall metodu IEnumerable dönüyo tolist yap
            ViewBag.Categories = new SelectList(categoriesDTO, "Id", "Name", product.CategoryId); //productın idsine göre kategorisi gelcek

            return View(_mapper.Map<ProductDTO>(product)); //productı dtoya mapleyip kullancıya göster.
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDTO productDTO)
        {
            if (ModelState.IsValid) //kullanıcı herhangi bir alanı bol bırakmadıysa kaydet
            {
                await _productService.UpdateAsync(_mapper.Map<Product>(productDTO)); //metod bizden product istiyor ama bize ekrandan dto geliyor onu mapledik ve update ettik
                return RedirectToAction(nameof(Index));
            }

            //herhangi bir alan boşsa vs kategorileri tekrar listelemen gerek o producta ait olan default gözükecek.
            var categories = await _categoryService.GetAllAsync();
            var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories.ToList()); //bizden list istiyo ama getall metodu IEnumerable dönüyo tolist yap
            ViewBag.Categories = new SelectList(categoriesDTO, "Id", "Name", productDTO.CategoryId); //productın idsine göre kategorisi gelcek

            return View(productDTO);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var product = await _productService.GetByIdAsync(id); //idye sahip entity al
            await _productService.RemoveAsync(product); //sil

            return RedirectToAction(nameof(Index));
        }
    }
}