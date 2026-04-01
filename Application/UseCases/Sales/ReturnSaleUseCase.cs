using Application.DTOs.Sales;
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

        public async Task ExecuteAsync(string folio, List<ReturnItemDto> itemsToReturn)
        {
            var sale = await _saleRepository.GetByFolioAsync(folio);

            if (sale == null)
                throw new InvalidOperationException($"No se encontró la venta con el folio: {folio}");

            if (sale.Status == SaleStatus.Cancelled || sale.Status == SaleStatus.TotalReturn)
                throw new InvalidOperationException("Esta venta ya no permite devoluciones.");

            foreach (var itemDto in itemsToReturn)
            {
                // Buscar el detalle por SKU (asegúrate de incluir .Product en la consulta)
                var detail = sale.Details.FirstOrDefault(d =>
                    d.Product.Sku.Trim().ToUpper() == itemDto.ProductSku.Trim().ToUpper());

                if (detail != null)
                {
                    //Aumentamos la cantidad devuelta en el detalle
                    detail.ApplyReturn(itemDto.QuantityToReturn);

                    //Devolvemos el stock físicamente al lote
                    var lot = await _lotRepository.GetByIdAsync(detail.LotId);
                    if (lot != null)
                    {
                        lot.AddStock(itemDto.QuantityToReturn);
                        await _lotRepository.UpdateAsync(lot);
                    }
                }
            }

            // Determinar nuevo estado de la venta
            // Si la suma de cantidades devueltas es igual al total de la venta original -> TotalReturn
            sale.UpdateTotal();

            if (sale.Total == 0)
                sale.MarkAsTotalReturn();
            else
                sale.MarkAsPartialReturn();

            //Persistir cambios
            await _saleRepository.UpdateAsync(sale);
            await _saleRepository.SaveChangesAsync();
        }
    }
}
