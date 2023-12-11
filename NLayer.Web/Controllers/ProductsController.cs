using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Web.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        //buradaki servisleri kaldırcaz çünkü artık bize apiden gelcek bu servisler orada o işler yapılmış oluyor zaten
        //APIYE ISTEK YOLLAYAN SERVISTEKİ KODLARI ENTEGRE ETTIK
        //private readonly IProductService _productService;
        //private readonly ICategoryService _categoryService;
        //private readonly IMapper _mapper;

        //public ProductsController(IProductService productService, ICategoryService categoryService, IMapper mapper)
        //{
        //    _productService = productService;
        //    _categoryService = categoryService;
        //    _mapper = mapper;
        //}

        private readonly ProductAPIService _productApiService;
        private readonly CategoryAPIService _categoryApiService;

        public ProductsController(ProductAPIService productApiService, CategoryAPIService categoryApiService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
        }

        //listeleme sayfası olcak
        public async Task<IActionResult> Index()
        {
            //isteği apiden gelmeden önceki kod
            //var customResponse = (await _productService.GetProductsWithCategory()).Data;
            //yukardaki metod bize customresponseDTO dönüyo bize ProductWİthCAtegoryDto lazım o da onun içindeki Data propertysinin içinde
            //CRDto sınıfına bakarsan T Data alanını görürsün api kullanırken metodlarda genel bir response dönmek için yaptık.

            //apiye istek gönderdiiğimiz servisi çağırdık.
            var customResponse = await _productApiService.GetProductWithCategoryAsync();
            return View(customResponse);
        }

        public async Task<IActionResult> Save()
        {
            //apiden istek atılmayan kod
            //product kaydetmek için kategori bilgisi lazım çünk aralarında ilişki var bu sebeple dropdown liste kategorileri listeliyoruz
            //var categories = await _categoryService.GetAllAsync();
            //var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories.ToList());

            //apiden istek attığımız servisin kodu
            var categoriesDTO = await _categoryApiService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categoriesDTO, "Id", "Name"); //dropdown listte arka planda id tutulcak kullanıcı ise name görcek
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                //apiden istek atılmayan servis kodu
                //await _productService.AddAsync(_mapper.Map<Product>(productDTO)); //metod bizden product istiyor ama bize ekrandan dto geliyor onu mapledik

                //apdien istek atılan servis kodu
                await _productApiService.SaveAsync(productDTO);
                return RedirectToAction(nameof(Index));
            }
            //apiden istek atılmayan servis kodu
            //eğer kayıt başarısız olursa kategorileri tekrar dropdownda göster
            //var categories = await _categoryService.GetAllAsync();
            //var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories.ToList()); //bizden list istiyo ama getall metodu IEnumerable dönüyo tolist yap

            //apdien istek atılan servis kodu
            var categoriesDTO = await _categoryApiService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categoriesDTO, "Id", "Name"); //dropdown listte arka planda id tutulcak kullanıcı ise name görcek

            return View();
        }

        //filterin kullanılcağı sayfayı belirttik üstünde yazarak.
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            //apiden istek atılmayan servis kodu
            //var product = await _productService.GetByIdAsync(id); //idye sahip productı bul

            var productDto = await _productApiService.GetByIdAsync(id);

            //apiden istek atılmayan servis kodu
            //gelen productın kategorisini de dropdown listte göstermemiz lazım
            //var categories = await _categoryService.GetAllAsync();
            //var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories.ToList()); //bizden list istiyo ama getall metodu IEnumerable dönüyo tolist yap

            //apiden istek atılan servis kodu
            var categoriesDTO = await _categoryApiService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categoriesDTO, "Id", "Name", productDto.CategoryId); //productın idsine göre kategorisi gelcek

            //apiden istek atılmayan servis kodu
            //return View(_mapper.Map<ProductDTO>(product)); //productı dtoya mapleyip kullancıya göster.

            //apiden istek atılan servis kodu
            return View(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDTO productDTO)
        {
            if (ModelState.IsValid) //kullanıcı herhangi bir alanı bol bırakmadıysa kaydet
            {
                //await _productService.UpdateAsync(_mapper.Map<Product>(productDTO)); //metod bizden product istiyor ama bize ekrandan dto geliyor onu mapledik ve update ettik

                await _productApiService.UpdateAsync(productDTO);
                return RedirectToAction(nameof(Index));
            }

            //herhangi bir alan boşsa vs kategorileri tekrar listelemen gerek o producta ait olan default gözükecek.
            //var categories = await _categoryService.GetAllAsync();
            //var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories.ToList()); //bizden list istiyo ama getall metodu IEnumerable dönüyo tolist yap

            var categoriesDTO = await _categoryApiService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categoriesDTO, "Id", "Name", productDTO.CategoryId); //productın idsine göre kategorisi gelcek

            return View(productDTO);
        }

        public async Task<IActionResult> Remove(int id)
        {
            //var product = await _productService.GetByIdAsync(id); //idye sahip entity al
            //await _productService.RemoveAsync(product); //sil

            await _productApiService.RemoveAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}