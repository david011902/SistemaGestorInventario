using Application.DTOs.Sales;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Sales
{
    public class GetSaleByIdUseCase
    {
        private readonly ISaleRepository _repository;

        public GetSaleByIdUseCase(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseSaleDto?> ExecuteAsync(Guid id)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null)
            {
                throw new InvalidOperationException($"No se encontro una venta con el id: {id} ");
            }
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

    }
}
