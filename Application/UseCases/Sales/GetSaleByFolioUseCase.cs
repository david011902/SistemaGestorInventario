using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Sales
{
    public class GetSaleByFolioUseCase
    {
        private readonly IFolioRepository<LotsEntity> _folioRepository;

        public GetSaleByFolioUseCase(IFolioRepository<LotsEntity> folioRepository)
        {
            _folioRepository = folioRepository;
        }

        public async Task<LotsEntity?> ExecuteAsync(string folio)
        {
            var sale = await _folioRepository.GetByFolioAsync(folio);
            if (sale == null)
            {
                throw new InvalidOperationException($"No se encontro una venta con el folio: {folio} ");
            }
            return sale;
        }

    }
}
