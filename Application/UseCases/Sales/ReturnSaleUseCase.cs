using Domain.Abstractions;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Sales
{
    public class ReturnSaleUseCase
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ILotRepository _lotRepository;

        public ReturnSaleUseCase(ISaleRepository saleRepository, ILotRepository lotRepository)
        {
            _saleRepository = saleRepository;
            _lotRepository = lotRepository;
        }

        public async Task ExecuteAsync(string folio)
        {
            var sale = await _saleRepository.GetByFolioAsync(folio);
            if (sale == null)
                throw new Exception($"No se encontró la venta con el folio: {folio}");
            if (sale.Status == SaleStatus.Cancelled)
                throw new InvalidOperationException("Esta venta ya ha sido cancelado");

            if (sale.Details != null)
            {
                foreach (var detail in sale.Details)
                {
                    var lot = await _lotRepository.GetByIdAsync(detail.LotId);
                    if (lot != null)
                    {
                        lot.AddStock(detail.Quantity);
                        await _lotRepository.UpdateAsync(lot);
                    }
                }
            }

            sale.Cancel();
            await _saleRepository.UpdateAsync(sale);
            await _saleRepository.SaveChangesAsync();
        }
    }
}
