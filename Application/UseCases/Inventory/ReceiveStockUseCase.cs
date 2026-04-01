using Application.DTOs.Lots;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Inventory
{
    public class ReceiveStockUseCase
    {
        private readonly ILotRepository _lotRepository;
        private readonly IRepository<ProductEntity, Guid> _Repository;
        private readonly IUnitOfWork _unitOfWork;

        public ReceiveStockUseCase(ILotRepository lotRepository, IRepository<ProductEntity, Guid> Repository, IUnitOfWork unitOfWork)
        {
            _lotRepository = lotRepository;
            _Repository = Repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<LotsEntity> ExecuteAsync(CreateLotDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var product = await _Repository.GetByIdAsync(dto.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException($"No se encontro un producto con el id: {dto.ProductId} ");
                }
                var lot = new LotsEntity(dto.ProductId, dto.InitialAmount, dto.PurchaseCost, dto.ArrivatelDate, dto.Supplier);
                await _lotRepository.AddAsync(lot);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return lot;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }
    }
}
