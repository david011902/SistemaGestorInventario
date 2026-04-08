using Application.DTOs.Lots;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Application.UseCases.Inventory
{
    public class GetStockByIdUseCase
    {
        private readonly ILotRepository _lotRepository;

        public GetStockByIdUseCase(ILotRepository lotRepository)
        {
            _lotRepository = lotRepository;
        }

        public async Task<ResponseLotDto> ExecuteAsync(Guid id)
        {
            var lot = await _lotRepository.GetByIdAsync(id);
            if (lot == null)
            {
                throw new InvalidOperationException($"No se encontro el lote");
            }
            return LotMapper.ToDto(lot);
        }


    }
}
