using AutoMapper;
using Microsoft.AspNetCore.Http;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class ProductServiceWithDTO : ServiceWithDTO<Product, ProductDTO>, IProductServiceWithDTO
    {
        //product özel ve diğer repository databae kodlarına erişmek için bunu aldık generic repository işimize yaramıyo ulaşamıyoruz.
        private readonly IProductRepository _productRepository;

        public ProductServiceWithDTO(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository) : base(repository, unitOfWork, mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<CustomResponseDTO<List<ProductWithCategoryDTO>>> GetProductsWithCategory()
        {
            var productsWithCategory = await _productRepository.GetProductsWithCategory();
            var productsWithCategoryDTO = _mapper.Map<List<ProductWithCategoryDTO>>(productsWithCategory);
            return CustomResponseDTO<List<ProductWithCategoryDTO>>.Success(200, productsWithCategoryDTO);
        }

        //overload edilmiş metod
        public async Task<CustomResponseDTO<ProductDTO>> AddAsync(ProductCreateDTO createDTO)
        {
            //gelen dto yau product çevir Mapper üst classtan yanş ServiceWithDTO dan geliyo ama protected olduğu için erişebiliyoruz. üst sınıfta nası tanımladıysak.
            //repository erişemiyoruz çünkü private tanımlanmış. b sebeple IProductRepository dependency inj. yapıyoz
            var product = _mapper.Map<Product>(createDTO); //ProductCreateDTO yu Product maplediğimiz için mapProfile sınıfında map oluşturduk.
            await _productRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();
            var newDTO = _mapper.Map<ProductDTO>(product); //kllanıcıya göstermek için tekrar dtoya çevir

            //dto gönderen bir CustomResponseDTO sınıfı
            return CustomResponseDTO<ProductDTO>.Success(StatusCodes.Status200OK, newDTO); //fark ettiysen genericDTO değil product dto
        }

        //overload edilmiş AddRangeAsync() kullanıcıdan ProductCreateDTO list alıyoruz productDTO list değil.
        public async Task<CustomResponseDTO<IEnumerable<ProductDTO>>> AddRangeAsync(IEnumerable<ProductCreateDTO> productCreateDtoList)
        {
            IEnumerable<Product> newEntities = _mapper.Map<IEnumerable<Product>>(productCreateDtoList);
            await _productRepository.AddRangeAsync(newEntities);
            await _unitOfWork.CommitAsync();
            var newDtoList = _mapper.Map<IEnumerable<ProductDTO>>(newEntities);
            return CustomResponseDTO<IEnumerable<ProductDTO>>.Success(StatusCodes.Status200OK, newDtoList);
        }

        //overload edilmiş metod
        public async Task<CustomResponseDTO<NoContentDTO>> UpdateAsync(ProductUpdateDTO updateDTO)
        {
            var product = _mapper.Map<Product>(updateDTO); //önce gelen dtoyu entity çevir bu entity BaseEntity bu sayede idsine erişebiliyoz.
            _productRepository.Update(product);
            await _unitOfWork.CommitAsync();
            return CustomResponseDTO<NoContentDTO>.Success(StatusCodes.Status204NoContent);
        }
    }
}