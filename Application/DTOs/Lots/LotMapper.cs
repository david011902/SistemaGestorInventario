using Application.DTOs.Products;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Lots
{
    public static class LotMapper
    {
        public static ResponseLotDto ToDto(LotsEntity lot)
        {
            return new ResponseLotDto
            {
                Id = lot.Id,
                ProductId = lot.ProductId,
                InitialAmount = lot.InitialAmount,
                CurrentAmount = lot.CurrentAmount,
                PurchaseCost = lot.PurchaseCost,
                ArrivateDate = lot.ArrivateDate,
                Supplier = lot.Supplier,
                IsActive = lot.IsActive,

                Product = new ResponseProductDto
                {
                    Id = lot.Product.Id,
                    Name = lot.Product.Name,
                    Sku = lot.Product.Sku,
                    Price = lot.Product.Price,
                    Stock = lot.Product.Stock,
                    IsActive = lot.Product.IsActive,

                    VehicleTypeName = lot.Product.VehicleType.NameVehicle,
                    SocketTypeName = lot.Product.SocketType.NameSocket,
                }
            };
        }
    }
}
