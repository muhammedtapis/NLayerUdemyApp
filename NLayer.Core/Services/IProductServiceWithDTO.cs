using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{   //Generic ServiceWithDTO ek olarak product özel metotların olduğu servis
    //nocachign service te yaptığımız gibi interface oluşturduk bu interface in kalıtım aldığı interface ise DTO dönen yeni interface ve buna ek bi metodumuz var.
    public interface IProductServiceWithDTO : IServiceWithDTO<Product, ProductDTO>
    {
        Task<CustomResponseDTO<List<ProductWithCategoryDTO>>> GetProductsWithCategory();

        //ServiceControllerdaki update metodu ProductUpdateDTO alıyor ama bizim Iservicewithdto dan generic DTO gidiyor bunu düzeltmek için overload metod yazca
        Task<CustomResponseDTO<NoContentDTO>> UpdateAsync(ProductUpdateDTO updateDTO);

        //create-add için de overload ediyoruz. ikisinin deDTO ları farklı!
        Task<CustomResponseDTO<ProductDTO>> AddAsync(ProductCreateDTO createDTO);

        Task<CustomResponseDTO<IEnumerable<ProductDTO>>> AddRangeAsync(IEnumerable<ProductCreateDTO> productCreateDtoList);
    }
}