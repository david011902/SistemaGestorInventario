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

        public async Task<ResponseProductDto?> ExecuteAsync(UpdateProductDto dto, Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new InvalidOperationException($"No se encontro el producto con el id: {id}");
            }
            product.UpdateProduct(dto.Name, dto.Price, dto.VehicleTypeId, dto.SocketTypeId);
            
            await _repository.UpdateAsync(product);
            await _repository.SaveChangesAsync();
            return new ResponseProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Price = product.Price,
                VehicleTypeName = product.VehicleType?.NameVehicle ?? "No asignado",
                SocketTypeName = product.SocketType?.NameSocket ?? "No asignado",
                Stock = product.Stock,
                IsActive = product.IsActive
            };

        }
    }
}
