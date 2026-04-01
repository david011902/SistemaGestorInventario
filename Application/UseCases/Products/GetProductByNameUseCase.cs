using Application.DTOs.Products;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Products
{
    public class GetProductByNameUseCase
    {
        private readonly IProductRepository _repository;
        public GetProductByNameUseCase(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ResponseProductDto?>> ExecuteAsync(string name)
        {
            var products = await _repository.GetByNameAsync(name);
            if (products == null)
            {
                throw new InvalidOperationException($"No se encontro el tipo de socket con el nombre: {name} ");
            }
            return products.Select(p => new ResponseProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Sku = p.Sku,
                Price = p.Price,
                VehicleTypeName = p.VehicleType?.NameVehicle ?? "No asignado",
                SocketTypeName = p.SocketType?.NameSocket ?? "No asignado",
                Stock = p.Stock,
                IsActive = p.IsActive
            });
        }
    }
}
