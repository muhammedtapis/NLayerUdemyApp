using NLayer.Core.DTOs;
using NLayer.Core.Models;

namespace NLayer.Core.Services
{
    //serviste genel oluşturulan metodlara eriştik.
    public interface IProductService : IService<Product>
    {
        //metodun dönceği türü değiştirdk controllerda srekli return ile customResponseDto oluşturuyorduk biz serviste oluşturup onu yollayacağız artık.
        Task<CustomResponseDTO<List<ProductWithCategoryDTO>>> GetProductsWithCategory();
    }
}