using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class ServiceWithDTO<Entity, DTO> : IServiceWithDTO<Entity, DTO> where Entity : BaseEntity where DTO : class
    {
        //bu serviste ihtiyacımız olanlar repository veritabanı işlemleri yapıyor lazım bize

        private readonly IGenericRepository<Entity> _repository;

        //services klasründeki özel servisler CAtegoryService gibi bu ServiceWithDTO servisine ek CategoryServiceWithDTO gibi
        //ek servisler yazarsak bu servisten kalıtım alacaklar o sebeple unitOfWork erişmek için protected yaptık.
        protected readonly IUnitOfWork _unitOfWork;

        protected readonly IMapper _mapper;

        public ServiceWithDTO(IGenericRepository<Entity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomResponseDTO<DTO>> AddAsync(DTO dto)
        {
            Entity newEntity = _mapper.Map<Entity>(dto);  //gelen dto yau entity çevir
            await _repository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();
            var newDTO = _mapper.Map<DTO>(newEntity); //kllanıcıya göstermek için tekrar dtoya çevir

            //dto gönderen bir CustomResponseDTO sınıfı
            return CustomResponseDTO<DTO>.Success(StatusCodes.Status200OK, newDTO);
        }

        public async Task<CustomResponseDTO<IEnumerable<DTO>>> AddRangeAsync(IEnumerable<DTO> dtoList)
        {
            //gelen birden fazla dtoyu i tek tek entity nesnelerine maple
            IEnumerable<Entity> newEntities = _mapper.Map<IEnumerable<Entity>>(dtoList);
            await _repository.AddRangeAsync(newEntities);
            await _unitOfWork.CommitAsync();
            var newDtoList = _mapper.Map<IEnumerable<DTO>>(newEntities);
            return CustomResponseDTO<IEnumerable<DTO>>.Success(StatusCodes.Status200OK, newDtoList);
        }

        public async Task<CustomResponseDTO<bool>> AnyAsync(Expression<Func<Entity, bool>> expression)
        {
            var isAny = await _repository.AnyAsync(expression);
            return CustomResponseDTO<bool>.Success(StatusCodes.Status200OK, isAny);
        }

        public async Task<CustomResponseDTO<IEnumerable<DTO>>> GetAllAsync()
        {
            var entities = await _repository.GetAll().ToListAsync();
            var newDtoList = _mapper.Map<IEnumerable<DTO>>(entities);
            return CustomResponseDTO<IEnumerable<DTO>>.Success(StatusCodes.Status200OK, newDtoList);
        }

        public async Task<CustomResponseDTO<DTO>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            var newDto = _mapper.Map<DTO>(entity);
            return CustomResponseDTO<DTO>.Success(StatusCodes.Status200OK, newDto);
        }

        public async Task<CustomResponseDTO<NoContentDTO>> RemoveAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            return CustomResponseDTO<NoContentDTO>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDTO<NoContentDTO>> RemoveRangeAsync(IEnumerable<int> idList)
        {
            //güzel metod Liste deki entityler ile veritabanındaki entityler arasınd id içeren varsa liste olarak dönüyor.
            var entities = await _repository.Where(x => idList.Contains(x.Id)).ToListAsync();
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            return CustomResponseDTO<NoContentDTO>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDTO<NoContentDTO>> UpdateAsync(DTO dto)
        {
            Entity entity = _mapper.Map<Entity>(dto); //önce gelen dtoyu entity çevir bu entity BaseEntity bu sayede idsine erişebiliyoz.
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return CustomResponseDTO<NoContentDTO>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDTO<IEnumerable<DTO>>> Where(Expression<Func<Entity, bool>> expression)
        {
            var entities = await _repository.Where(expression).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<DTO>>(entities);
            return CustomResponseDTO<IEnumerable<DTO>>.Success(StatusCodes.Status200OK, dtos);
        }
    }
}