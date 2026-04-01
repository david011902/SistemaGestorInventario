using Application.DTOs.Products;
using Domain.Abstractions;
using Domain.Entities;


namespace Application.UseCases.Products
{
    public class GetProductBySkuUseCase
    {
        private readonly ISkuRepository<ProductEntity> _repository;

        public GetProductBySkuUseCase(ISkuRepository<ProductEntity> repository)
        {
            _repository = repository;
        }

        public async Task<ResponseProductDto?> ExecuteAsync(string sku)
        {
            var products = await _repository.GetBySkuAsync(sku);
            if(products == null)
            {
                return null;
            }
            return  new ResponseProductDto
            {
                Id = products.Id,
                Name = products.Name,
                Sku = products.Sku,
                Price = products.Price,
                VehicleTypeName = products.VehicleType?.NameVehicle ?? "No asignado",
                SocketTypeName = products.SocketType?.NameSocket ?? "No asignado",
                Stock = products.Stock,
                IsActive = products.IsActive
            };
        }
    }
}
