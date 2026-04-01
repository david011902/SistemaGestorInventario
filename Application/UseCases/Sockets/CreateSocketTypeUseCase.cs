using Application.DTOs.SocketsType;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Sockets
{
    public class CreateSocketTypeUseCase 
    {
        private readonly ISocketTypeRepository _repository;
        public CreateSocketTypeUseCase(ISocketTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<SocketTypeEntity> ExecuteAsync(CreateSocketTypeDto dto)
        {
            var exists = await _repository.GetByNameAsync(dto.Name);
            if (exists != null && exists.Any(x=>x?.NameSocket == dto.Name))
            {
                throw new InvalidOperationException($"El tipo de socket '{dto.Name}' ya existe.");
            }
            var socketType = new SocketTypeEntity(dto.Name);
            await _repository.AddAsync(socketType);
            await _repository.SaveChangesAsync();
            return socketType;
        }
    }
}
