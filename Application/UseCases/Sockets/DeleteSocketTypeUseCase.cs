using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Sockets
{
    public class DeleteSocketTypeUseCase
    {
        private readonly ISocketTypeRepository _repository;
        public DeleteSocketTypeUseCase(ISocketTypeRepository repository)
        {
            _repository = repository;
        }
        public async Task ExecuteAsync(Guid id)
        {
            var socketType = await _repository.GetByIdAsync(id);
            if (socketType == null)
            {
                throw new InvalidOperationException($"No se encontro el tipo de socket con el ID: {id} ");
            }
            socketType.Desactivate();
            await _repository.UpdateAsync(socketType);
            await _repository.SaveChangesAsync();
        }
    }
}
