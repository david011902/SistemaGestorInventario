using Application.DTOs.Lots;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Inventory
{
    public class AdjustStockUseCase
    {
        private readonly ILotRepository _lotRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdjustStockUseCase(ILotRepository lotRepository, IUnitOfWork unitOfWork)
        {
            _lotRepository = lotRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task ExecuteAsync(UpdateLotDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var lot = await _lotRepository.GetByIdAsync(dto.Id);
                if (lot == null)
                {
                    throw new InvalidOperationException($"No se encontro un lote con el ID: {dto.Id} ");
                }
                lot.AdjustStock(dto.NewActualQuantity);

                await _lotRepository.UpdateAsync(lot);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch(Exception) 
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
