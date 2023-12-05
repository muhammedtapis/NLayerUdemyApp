using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}