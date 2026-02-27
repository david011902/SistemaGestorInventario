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

        public async Task<SaleEntity?> ExecuteAsync(Guid id)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null)
            {
                throw new InvalidOperationException($"No se encontro una venta con el id: {id} ");
            }
            return sale;
        }

    }
}
