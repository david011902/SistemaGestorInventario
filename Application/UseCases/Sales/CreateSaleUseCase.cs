using Application.DTOs.Sales;
using Application.Services;
using Domain.Abstractions;
using Domain.Entities;


namespace Application.UseCases.Sales
{
    public class CreateSaleUseCase
    {
        private readonly ILotRepository _lotRepository;
        private readonly ISkuRepository<ProductEntity> _skuRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IFolioService _folioService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSaleUseCase(ILotRepository lotRepository, ISaleRepository saleRepository, IFolioService folioService, IUnitOfWork unitOfWork, ISkuRepository<ProductEntity> skuRepository )
        {
            _lotRepository = lotRepository;
            _saleRepository = saleRepository;
            _folioService = folioService;
            _unitOfWork = unitOfWork;
            _skuRepository = skuRepository;
        }

        public async Task<ResponseSaleDto> ExecuteAsync(CreateSaleDto dto)
        {
            try
            {
                var folio = _folioService.GenerateFolio();
                var sale = new SaleEntity(folio);

                foreach (var item in dto.Items)
                {
                    var lots = await _lotRepository.GetActiveLotsBySkuAsync(item.Sku);
                    if (lots == null || !lots.Any())
                        throw new Exception($"No hay lotes para el SKU {item.Sku}.");

                    var sortedLots = lots.OrderBy(l => l.ArrivateDate).ToList();
                    int remainingToSell = item.Quantity;

                    foreach (var l in sortedLots)
                    {
                        if (remainingToSell <= 0) break;
                        int quantityFromLot = Math.Min(l.CurrentAmount, remainingToSell);

                        decimal price = l.Product?.Price ?? 0;

                        sale.AddDetail(l.ProductId, l.Id, quantityFromLot, price);
                        l.SubtractStock(quantityFromLot);
                        remainingToSell -= quantityFromLot;
                    }

                    if (remainingToSell > 0)
                        throw new Exception($"Stock insuficiente para {item.Sku}.");
                }

                await _saleRepository.AddAsync(sale);

                await _unitOfWork.SaveChangesAsync();

                return new ResponseSaleDto {
                    Id = sale.Id,
                    Folio = sale.Folio,
                    Date = sale.Date,
                    Total = sale.Total,
                    Status = sale.Status.ToString(),
                    Details = dto.Items.Select(item => new ResponseDetailSaleDto
                    {
                        ProductSku = item.Sku,
                        Quantity = item.Quantity,
                    }).ToList(),
                };
            }
            catch
            {
                throw; 
            }
        }
    }
}
