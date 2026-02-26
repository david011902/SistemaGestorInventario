using Domain.Abstractions;
using Domain.Entities;


namespace Application.UseCases.Products
{
    public class DeleteProductUseCase
    {
        private readonly IRepository<ProductEntity, Guid> _repository;
        public DeleteProductUseCase(IRepository<ProductEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new InvalidOperationException($"No se encontro un producto con el ID: {id} ");
            }
            product.Desactivate();
            await _repository.UpdateAsync(product);
            await _repository.SaveChangesAsync();
        }

    }
}
