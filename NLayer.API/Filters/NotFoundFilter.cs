using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using System.Linq;
using System.Threading.Tasks;

namespace NLayer.API.Filters
{
    //dinamik yaptık farklı modellerde kullanmak için id üzerinden null kontrol.
    //productsControllerde GetById(int id) metoduna daha gelmeden bu filter çalışacak onun için yapıyoruz.
    //bu yazacağımız filter da GetById(int id) parametredeki id ye göre işlem yapacak bu  id yi yakalamamız lazım
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity   //idye erişebilmek için BaseEntity yaptk
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

            context.Result = new NotFoundObjectResult(CustomResponseDTO<NoContentDTO>.Fail(404, $"{typeof(T).Name}({id}) not found."));
        }
    }
}