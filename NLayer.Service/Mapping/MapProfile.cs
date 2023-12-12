using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;

namespace NLayer.Service.Mapping
{
    //profile sınıfını miras alması gerekiyor.
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            //iki taraflı mapleme yapılmasını sağladık
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDTO>().ReverseMap();
            CreateMap<ProductUpdateDTO, Product>();//programda productupdateDTO gelirse onu producta maple bu DTOyu özel oluşturmuştuk.
            CreateMap<Product, ProductWithCategoryDTO>();
            CreateMap<Category, CategoryWithProductsDTO>();  //her mapleme için buraya eklemen lazım
            CreateMap<ProductCreateDTO, Product>(); //sonradan oluşturduğmuz serviste var bu dto maplemesi  DTOservisi
        }
    }
}