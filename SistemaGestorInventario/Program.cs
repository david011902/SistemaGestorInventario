using Application.Services;
using Application.UseCases.Auth;
using Application.UseCases.Inventory;
using Application.UseCases.Products;
using Application.UseCases.Sales;
using Application.UseCases.Sockets;
using Application.UseCases.Vehicles;
using Data.Persistence;
using Data.Repositories;
using Data.Services;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SistemaGestorInventario.Endpoints;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Base de datos 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, b => {
        b.MigrationsAssembly("Data");
        b.EnableRetryOnFailure(3); // Reintenta si Supabase tarda en responder
    }));
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
builder.Services.AddScoped<GetStockByIdUseCase>();

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

builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<RefreshTokenUseCase>();


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
builder.Services.AddScoped<IAuthRepository, PgAuthRepository>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

//Rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("peticiones-limite", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1); // Ventana de tiempo
        opt.PermitLimit = 50;                // Máximo de 50 peticiones
        opt.QueueLimit = 0;                  // No encolar peticiones extra
    });
});
//Relaciones de objetos JSON
builder.Services.Configure<JsonOptions>(options =>
{
    // Permite que el JSON incluya las propiedades de navegación (los Includes)
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

    options.SerializerOptions.WriteIndented = true;
});

//JWT
var accessSecret = builder.Configuration["Jwt:AccessSecret"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(accessSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();

//Scalar
builder.Services.AddOpenApi();
//Cors para acceder desde Angular
var misReglasCors = "ReglasAngular";
var frontendUrl = builder.Configuration["FRONTEND_URL"];
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: misReglasCors, policy =>
    {
        var origins = new List<string> { "http://localhost:4200" };
        if(!string.IsNullOrEmpty(frontendUrl))
            origins.Add(frontendUrl);
        policy.WithOrigins(origins.ToArray())
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();//funciona para las cookies del refresh token
    });
});
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: misReglasCors, builder =>
//    {
//        builder.WithOrigins("http://localhost:4200")
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//    });
//});

var app = builder.Build();

//Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Mi API";
        options.AddHttpAuthentication("Bearer", bearer =>
        {
            bearer.Token = "";
        });
    });
}
app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseCors(misReglasCors);
app.UseAuthentication();
app.UseAuthorization();
//Inyecccion de dependencias de los endpoints
app.MapProductsEndpoints();
app.MapVehicleTypeEndpoints();
app.MapSocketTypeEndpoints();
app.MapSalesEndpoints();
app.MapStockEndpoints();
app.MapAuthEndpoints();
//app.MapGet("/health", () => Results.Ok(new { status = "alive" }))
//   .AllowAnonymous(); //Solo para verificar el deploy en azure en el plan F1
app.Run();

