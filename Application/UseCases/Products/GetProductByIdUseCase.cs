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

        public async Task<ProductEntity?> ExecuteAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new InvalidOperationException($"No se encontro un producto con el SKU: {id} ");
            }
            return product;
        }
    }
}
