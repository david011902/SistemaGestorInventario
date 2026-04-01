using Application.DTOs.SocketsType;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Sockets
{
    public class UpdateSocketTypeUseCase
    {
        private readonly ISocketTypeRepository _repository;
        public UpdateSocketTypeUseCase(ISocketTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<SocketTypeEntity?> ExecuteAsync(UpdateSocketTypeDto dto,  Guid id)
        {
            var socketType = await _repository.GetByIdAsync(id);
            if (socketType == null)
            {
                throw new InvalidOperationException($"No se encontro el tipo de socket con el id: {id}");
            }
            socketType.UpdateSocket(dto.Name, dto.IsActive);
            await _repository.UpdateAsync(socketType);
            await _repository.SaveChangesAsync();
            return socketType;
        }
    }
}
