using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        //bu metodu çağırdığımızda responseun status code ne ise onu dönecek önceden aşağıdaki gibi hem Ok hem de metodun içinde statusCode dönüyorduk
        //return Ok(CustomResponseDTO<List<ProductDTO>>.Success(200, productsDTO));
        //bu durumu engellemek için aşağıdak metodu oluşturduk.

        //ENDPOINT OLMADIĞINI BELİRT yoksa efcore post get metodu arar ve hata verir.
        [NonAction]
        public IActionResult CreateActionResult<T>(CustomResponseDTO<T> response)
        {
            if (response.StatusCode == 204)//204 noContent geriye bişey dönmeyecek kod ise
            {
                return new ObjectResult(null)   //object result dön içi boş olsun içindeki StatusCode ise responsetan gelene eşit olsun
                {
                    StatusCode = response.StatusCode
                };
            }
            return new ObjectResult(response) //204 NoContent değil de 200 ise content dönebiliriz o yüzden  object result içine response customresponseDTO atıldı
            {
                StatusCode = response.StatusCode
            };
        }
    }
}