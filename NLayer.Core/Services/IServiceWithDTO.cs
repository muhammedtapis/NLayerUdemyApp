using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    //geriye dto dönen servis controllerin istediği gibi
    //bu servis veritabanı bağlantısı için generic repository kullanıyo orda entity istediiği için biz de burada entity verdik.
    public interface IServiceWithDTO<Entity, DTO> where Entity : BaseEntity where DTO : class
    {
        Task<CustomResponseDTO<DTO>> GetByIdAsync(int id);

        //eski where servis metodunda IQueryable dönüyıduk onu döndüğümüzzaman controllerda koşul ekleyebiliyorduk LINQ sorguları gibi artık yazmak istemediğimiz için
        //IQueryable yerine IEnumerable döncez
        Task<CustomResponseDTO<IEnumerable<DTO>>> Where(Expression<Func<Entity, bool>> expression); //functionlar artık entity alıcak T almayacak

        Task<CustomResponseDTO<IEnumerable<DTO>>> GetAllAsync(); //IGenericRepositoryden değişik bu mesela collection döncek sorgusuz tüm datayı çekcez.

        Task<CustomResponseDTO<bool>> AnyAsync(Expression<Func<Entity, bool>> expression); //

        Task<CustomResponseDTO<DTO>> AddAsync(DTO dto); //DTO alıp ekleme yapcak

        //IEnumerable List gibi düüşün collection ama bu halde kullanmak List halinde kullanmaktan daha iyi.
        Task<CustomResponseDTO<IEnumerable<DTO>>> AddRangeAsync(IEnumerable<DTO> dtoList);

        Task<CustomResponseDTO<NoContentDTO>> UpdateAsync(DTO dto); //geriye değer dönmücez boş DTO döncek remove için de id ile silcez artık.

        Task<CustomResponseDTO<NoContentDTO>> RemoveAsync(int id);

        Task<CustomResponseDTO<NoContentDTO>> RemoveRangeAsync(IEnumerable<int> idList);
    }
}