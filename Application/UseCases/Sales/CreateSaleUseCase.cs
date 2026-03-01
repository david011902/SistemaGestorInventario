using Application.DTOs.Sales;
using Application.Services;
using Domain.Abstractions;
using Domain.Entities;


namespace Application.UseCases.Sales
{
    public class CreateSaleUseCase
    {
        private readonly ILotRepository _lotRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IFolioService _folioService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSaleUseCase(ILotRepository lotRepository, ISaleRepository saleRepository, IFolioService folioService, IUnitOfWork unitOfWork)
        {
            _lotRepository = lotRepository;
            _saleRepository = saleRepository;
            _folioService = folioService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> ExecuteAsync(CreateSaleDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try {
                var folio = _folioService.GenerateFolio();
                var sale = new SaleEntity(folio);
                foreach (var item in dto.Items)
                {   //Obtener lote del producto, validar existencia y stock, agregar detalle a la venta
                    var lot = await _lotRepository.GetByIdAsync(item.LotId);
                    if (lot == null) throw new Exception($"Lote con ID {item.LotId} no encontrado.");
                    if (lot.CurrentAmount < item.Quantity) throw new Exception($"Stock insuficiente {item.LotId}.");
                    //agregar detalle a la venta y actualizar stock del lote
                    sale.AddDetail(lot.Id, item.Quantity, lot.Product.Price);
                    lot.SubtractStock(item.Quantity);
                    await _lotRepository.UpdateAsync(lot);
                }
                await _saleRepository.AddAsync(sale);
                await _saleRepository.SaveChangesAsync();
                return sale.Id;
            }
            catch { 
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
           
        }
    }
}
