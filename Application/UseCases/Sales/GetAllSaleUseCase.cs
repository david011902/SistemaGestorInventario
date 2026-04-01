using Application.DTOs.Sales;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Sales
{
    public class GetAllSaleUseCase
    {
        private readonly ISaleRepository _repository;
        public GetAllSaleUseCase(ISaleRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ResponseSaleDto>> ExecuteAsync()
        {
            var sales = await _repository.GetAllAsync();
            return sales.Select(sale => new ResponseSaleDto
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
                }).ToList()
            }).ToList();
        }

    }
}
