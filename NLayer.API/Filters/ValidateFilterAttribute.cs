using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using System.Linq;

namespace NLayer.API.Filters
{
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //ModelState hataları entegre buraya direkt ulaşabiliyorsun.
            if (!context.ModelState.IsValid)  //eğer hata varsa
            {
                //önce hataları al SelectMany() den ModelError geliyor onu select ile içine girip ErrorMessage ları alıyoruz ve listeye atıyoruz.
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

                //bu validation hatası client hatası o sebeple 400 le başlayan hataları döncez
                //BadREquestObjectResult metoduna CustomREsponseDTO verdik ve DTO nun içinde  data olmayacak NoContentDTO verdik
                //fail metodunu çağırdık errors listesini buraya ekledik status code gönderdik.
                context.Result = new BadRequestObjectResult(CustomResponseDTO<NoContentDTO>.Fail(400, errors));
            }
        }
    }
}