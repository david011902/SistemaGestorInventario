using Application.DTOs.Products;
using Domain.Abstractions;
using Domain.Entities;


namespace Application.UseCases.Products
{
    public class GetAllProductsUseCase
    {
        private readonly IRepository<ProductEntity, Guid> _repository;

        public GetAllProductsUseCase(IRepository<ProductEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ResponseProductDto>> ExecuteAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => new ResponseProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Sku = p.Sku,
                Price = p.Price,
                VehicleTypeName = p.VehicleType?.NameVehicle?? "No asignado",
                SocketTypeName = p.SocketType?.NameSocket?? "No asignado",
                Stock = p.Stock,
                IsActive = p.IsActive
            });
        }

    }

   
}
