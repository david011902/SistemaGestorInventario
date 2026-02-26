using Application.DTOs.Products;
using Domain.Abstractions;
using Domain.Entities;


namespace Application.UseCases.Products
{
    public class CreateProductUseCase
    {
        private readonly IRepository<ProductEntity, Guid> _repository;
        private readonly ISkuRepository<ProductEntity> _skuRepository;

        public CreateProductUseCase(IRepository<ProductEntity, Guid> repository, ISkuRepository<ProductEntity> skuRepository)
        {
            _repository = repository;
            _skuRepository = skuRepository;
        }

        public async Task<ProductEntity> ExecuteAsync(CreateProductDto dto)
        { 
            if(await _skuRepository.ExistsWithCodeAsync(dto.Sku))
            {
                throw new Exception("SKU ya se encuentra registrado.");
            }

            var product = new ProductEntity(
                dto.Name, 
                dto.Sku, 
                dto.Price, 
                dto.Stock, 
                dto.CategoryId
                );

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();
            return product;
        }
    }
}
