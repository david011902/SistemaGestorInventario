using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Sockets
{
    public class GetSocketTypeByNombreUseCase
    {
        private readonly ISocketTypeRepository _repository;
        public GetSocketTypeByNombreUseCase(ISocketTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SocketTypeEntity?>> ExecuteAsync (string name)
        {
            var socketType = await _repository.GetByNameAsync(name);
            if (socketType == null)
            {
                throw new InvalidOperationException($"No se encontro el tipo de socket con el nombre: {name} ");
            }
            return socketType;
        }
    }
}
