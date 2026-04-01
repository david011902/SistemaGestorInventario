using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Inventory
{
    public class GetAllStockUseCase
    {
        private readonly ILotRepository _lotRepository;

        public GetAllStockUseCase(ILotRepository lotRepository)
        {
            _lotRepository = lotRepository;
        }

        public async Task<IEnumerable<LotsEntity>> ExecuteAsync()
        {
            var lots = await _lotRepository.GetAllAsync();
            return lots;
        }
    }
}
