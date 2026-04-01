using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Sockets
{
    public class GetAllSocketTypeUseCase
    {
        private readonly ISocketTypeRepository _repository;
        public GetAllSocketTypeUseCase(ISocketTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SocketTypeEntity>> ExecuteAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
