using Application.DTOs.Products;
using Domain.Abstractions;
using Domain.Entities;


namespace Application.UseCases.Products
{
    public class GetProductByIdUseCase
    {
        private readonly IRepository<ProductEntity, Guid> _repository;

        public GetProductByIdUseCase(IRepository<ProductEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ResponseProductDto?> ExecuteAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new InvalidOperationException($"No se encontro un producto con el id: {id} ");
            }
            return new ResponseProductDto{
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Price = product.Price,
                VehicleTypeName = product.VehicleType?.NameVehicle ?? "No asignado",
                SocketTypeName = product.SocketType?.NameSocket ?? "No asignado",
                VehicleTypeId = product.VehicleTypeId,
                SocketTypeId = product.SocketTypeId,
                Stock = product.Stock,
                IsActive = product.IsActive
            };
        }
    }
}
