using Electronics.Application.Interface;
using Electronics.Application.ProductTag;
using Electronics.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Electronics.Application.Discount
{
    public class DiscountService : IDiscountService
    {
        private readonly IBaseRepository<Domain.Entities.Discount> _repository;

        public DiscountService(IBaseRepository<Domain.Entities.Discount> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DiscountDto>> GetAllAsync()
        {
            var discount = await _repository.GetAllAsync();
            var dto = discount.Select(d => new DiscountDto
            {
                Id = d.Id,
                Name = d.Name,
                Percentage = d.Percentage,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                IsActive = d.IsActive
            });
            return dto;
        }
        public async Task AddAsync(DiscountDto dto)
        {
            var discount = new Domain.Entities.Discount
            {
                Name = dto.Name,
                Percentage = dto.Percentage,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive
            };
            await _repository.AddAsync(discount);
        }

        public async Task<DiscountDto> GetByIdAsync(int id)
        {
            var discount = await _repository.GetByIdAsync(id);
           
            return new DiscountDto
            {
                Id = discount.Id,
                Name = discount.Name,
                Percentage = discount.Percentage,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                IsActive = discount.IsActive
            };
        }


        public async Task UpdateAsync(DiscountDto dto)
        {
            var entity = new Domain.Entities.Discount {
                Id = dto.Id,
                Name = dto.Name,
                Percentage = dto.Percentage,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive
            };
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }





    }
}
