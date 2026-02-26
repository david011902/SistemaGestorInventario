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

        public async Task<ProductEntity?> ExecuteAsync(string sku)
        {
            var product = await _repository.GetBySkuAsync(sku);
            if (product == null)
            {
                throw new InvalidOperationException($"No se encontró un producto con el sku: {sku}");
            }
            return product;
        }
    }
}
