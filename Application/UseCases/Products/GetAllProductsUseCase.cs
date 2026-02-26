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

        public async Task<IEnumerable<ProductEntity>> ExecuteAsync()
        {
            return await _repository.GetAllAsync();
        }

    }

   
}
