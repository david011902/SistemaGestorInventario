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
            await _unitOfWork.BeginTransactionAsync();
            try {
                var folio = _folioService.GenerateFolio();
                var sale = new SaleEntity(folio);
                foreach (var item in dto.Items)
                {   //Obtener lote del producto, validar existencia y stock, agregar detalle a la venta
                    var lots = await _lotRepository.GetActiveLotsBySkuAsync(item.Sku);
         
                    if (!lots.Any() || lots == null)
                        throw new Exception($"No hay lotes activos para el producto con Sku {item.Sku}.");

                    var sortedLots = lots.OrderBy(l => l.ArrivateDate).ToList();
                    int remainingToSell = item.Quantity;
                    foreach (var l in sortedLots)
                    {
                        int quantityFromLot = Math.Min(l.CurrentAmount, remainingToSell);

                        sale.AddDetail(l.ProductId,l.Id, quantityFromLot, l.Product.Price);
                        l.SubtractStock(quantityFromLot);
                        await _lotRepository.UpdateAsync(l);

                        remainingToSell -= quantityFromLot;
                    }
                    if (remainingToSell > 0)
                        throw new Exception($"Stock insuficiente para el SKU {item.Sku}. Faltaron {remainingToSell} piezas");
                }
                await _saleRepository.AddAsync(sale);
                await _saleRepository.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseSaleDto
                {
                    Id = sale.Id,
                    Folio = sale.Folio,
                    Date = sale.Date,
                    Total = sale.Total,
                    Status = sale.Status.ToString(),
                    Details = sale.Details.Select(d => new ResponseDetailSaleDto
                    {
                        Id = d.Id,
                        Quantity = d.Quantity,
                        PriceAtSale = d.PriceAtSale,
                        Subtotal = d.Subtotal,
                        ProductName = d.Product?.Name ?? "N/A", 
                        ProductSku = d.Product?.Sku ?? "N/A",
                        LotId = d.LotId
                    }).ToList(),
                };
            }
            catch { 
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
           
        }
    }
}
