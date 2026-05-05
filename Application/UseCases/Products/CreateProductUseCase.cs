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
            string? finalSku = dto.Sku;
            if (string.IsNullOrWhiteSpace(finalSku))
            {
                finalSku = await GenerateUniqueNumericSkuAsync();
            }
            else if (await _skuRepository.ExistsWithSkuAsync(finalSku))
            {
                throw new Exception("SKU ya se encuentra registrado.");
            }

            var product = new ProductEntity(
                dto.Name, 
                finalSku, 
                dto.Price,
                dto.VehicleTypeId,
                dto.SocketTypeId
                //dto.CategoryId
                );

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();
            return product;
        }

        private async Task<string> GenerateUniqueNumericSkuAsync()
        {
            Random random = new Random();
            string newSku = string.Empty;
            bool exists = true;

            while (exists)
            {
                // Genera un número de 8 a 10 dígitos 
                newSku = random.Next(10000000, 99999999).ToString();

                exists = await _skuRepository.ExistsWithSkuAsync(newSku);
            }

            return newSku;
        }
    }
}
