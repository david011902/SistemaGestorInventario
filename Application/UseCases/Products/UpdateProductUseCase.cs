using Application.DTOs.Products;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.UseCases.Products
{
    public class UpdateProductUseCase
    {
        private readonly IRepository<ProductEntity, Guid> _repository;

        public UpdateProductUseCase(IRepository<ProductEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ProductEntity?> ExecuteAsync(UpdateProductDto dto)
        {
            var person = await _repository.GetByIdAsync(dto.Id);
            if (person == null)
            {
                throw new InvalidOperationException($"No se encontro el producto con el id: {dto.Id}");
            }
            person.UpdateProduct(dto.Name, dto.Price, dto.CategoryId);
            await _repository.UpdateAsync(person);
            await _repository.SaveChangesAsync();
            return person;

        }
    }
}
