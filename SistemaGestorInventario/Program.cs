using Application.Services;
using Application.UseCases.Inventory;
using Application.UseCases.Products;
using Application.UseCases.Sales;
using Data.Persistence;
using Data.Repositories;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using SistemaGestorInventario.Endpoints;
using Microsoft.AspNetCore.Builder;
using Application.UseCases.Vehicles;
using Application.UseCases.Sockets;
using Microsoft.AspNetCore.Http.Json;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Data")));
//Use case inyeccion de dependencia
builder.Services.AddScoped<CreateProductUseCase>();
builder.Services.AddScoped<DeleteProductUseCase>();
builder.Services.AddScoped<GetAllProductsUseCase>();
builder.Services.AddScoped<GetProductByIdUseCase>();
builder.Services.AddScoped<GetProductBySkuUseCase>();
builder.Services.AddScoped<GetProductByNameUseCase>();
builder.Services.AddScoped<UpdateProductUseCase>();

builder.Services.AddScoped<AdjustStockUseCase>();
builder.Services.AddScoped<ReceiveStockUseCase>();
builder.Services.AddScoped<GetAllStockUseCase>();

builder.Services.AddScoped<CreateSaleUseCase>();
builder.Services.AddScoped<GetAllSaleUseCase>();
builder.Services.AddScoped<GetSaleByFolioUseCase>();
builder.Services.AddScoped<GetSaleByIdUseCase>();
builder.Services.AddScoped<ReturnSaleUseCase>();

builder.Services.AddScoped<CreateVehicleTypeUseCase>();
builder.Services.AddScoped<GetAllVehicleTypeUseCase>();
builder.Services.AddScoped<GetVehicleTypeByIdUseCase>();
builder.Services.AddScoped<GetVehicleTypeByNameUseCase>();
builder.Services.AddScoped<DeleteVehicleTypeUseCase>();
builder.Services.AddScoped<UpdateVehicleTypeUseCase>();

builder.Services.AddScoped<CreateSocketTypeUseCase>();
builder.Services.AddScoped<GetAllSocketTypeUseCase>();
builder.Services.AddScoped<GetSocketByIdUseCase>();
builder.Services.AddScoped<GetSocketTypeByNombreUseCase>();
builder.Services.AddScoped<DeleteSocketTypeUseCase>();
builder.Services.AddScoped<UpdateSocketTypeUseCase>();


//Inyeccion de dependencias de los repositorios
builder.Services.AddScoped<IRepository<ProductEntity, Guid>, ProductRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IFolioRepository<SaleEntity>, FolioRepository>();
builder.Services.AddScoped<ILotRepository, LotRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISkuRepository<ProductEntity>, SkuRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFolioService, FolioService>();
builder.Services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
builder.Services.AddScoped<ISocketTypeRepository, SocketTypeRepository>();

//Relaciones de objetos
builder.Services.Configure<JsonOptions>(options =>
{
    // Permite que el JSON incluya las propiedades de navegación (los Includes)
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

    options.SerializerOptions.WriteIndented = true;
});

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Inyecccion de dependencias de los endpoints
app.MapProductsEndpoints();
app.MapVehicleTypeEndpoints();
app.MapSocketTypeEndpoints();
app.MapSalesEndpoints();
app.MapStockEndpoints();
app.Run();

