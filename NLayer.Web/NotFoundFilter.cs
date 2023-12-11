using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.Web
{
    //generic T base entity olcak çünkü id baseentity de tanımlı.
    //filter için built-in metoddan implemente aldık.
    //daha sonra bu filteri program cs eklicez
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
    {
        private readonly IService<T> _service;

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idValue = context.ActionArguments.Values.FirstOrDefault(); //bu metod GetById metodunun parametresindeki ilk değeri alıyoryani id yi.
            if (idValue == null)
            {
                await next.Invoke(); //eğer nullsa sen yoluna devam et
                return;
            }

            var id = (int)idValue;
            var anyEntity = await _service.AnyAsync(x => x.Id == id);

            if (anyEntity) //eğer bu idye sahip entity varsa.
            {
                await next.Invoke();
                return;
            }

            //API de notfoundObjectREsult(customresponseDTO) yönlendiriyordu mvcde error sayfasına yönlendircez
            var errorViewModel = new ErrorViewModel(); //error view model oluşturduk
            errorViewModel.Errors.Add($"{typeof(T).Name}({id}) not found");  //bu view modelin içine hataları ekledik.

            context.Result = new RedirectToActionResult("Error", "Home", errorViewModel); //home controllerdaki error sayfasına yönlendir içine de errorviewmodel gönder
        }
    }
}