using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Sockets
{
    public class GetSocketByIdUseCase
    {
        private readonly ISocketTypeRepository _repository;
        public GetSocketByIdUseCase(ISocketTypeRepository repository)
        {
            _repository = repository;
        }
        public async Task<SocketTypeEntity?> ExecuteAsync(Guid id)
        {
            var exists = await _repository.GetByIdAsync(id);
            if (exists == null)
            {
                throw new InvalidOperationException($"No se encontro el tipo de socket con el ID: {id} ");
            }
            return exists;
        }
    }
}
