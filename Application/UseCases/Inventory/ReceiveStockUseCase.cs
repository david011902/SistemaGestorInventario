using Application.DTOs.Lots;
using Domain.Abstractions;
using Domain.Entities;

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
            try
            {
                var product = await _Repository.GetByIdAsync(dto.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException($"No se encontró un producto con el id: {dto.ProductId}");
                }

                var lot = new LotsEntity(dto.ProductId, dto.InitialAmount, dto.PurchaseCost, dto.Supplier);
                await _lotRepository.AddAsync(lot);

                
                await _unitOfWork.SaveChangesAsync();

                return lot;
            }
            catch (Exception)
            { 
                throw;
            }

        }
    }
}
