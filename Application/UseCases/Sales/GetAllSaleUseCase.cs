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
        public async Task<IEnumerable<SaleEntity>> ExecuteAsync()
        {
            return await _repository.GetAllAsync();
        }

    }
}
