using Application.DTOs.Sales;
using Application.UseCases.Sales;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace SistemaGestorInventario.Endpoints
{
    public static class SalesEndpoints
    {
        public static void MapSalesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/sales").WithTags("Sales").RequireAuthorization();

            group.MapGet("/{id:guid}", async (Guid id, GetSaleByIdUseCase useCase) =>
            {
                try
                {
                    var sale = await useCase.ExecuteAsync(id);
                    return Results.Ok(sale);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            }).RequireAuthorization(policy => policy.RequireRole("Employee", "Administrator"))
            .RequireRateLimiting("peticiones-limite")
            .WithName("GetSaleById")
            .WithSummary("Obtener una venta por su id")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status429TooManyRequests);


            group.MapPost("/", async (CreateSaleDto dto, CreateSaleUseCase useCase) =>
            {
                try
                {
                    var sale = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/sales/{sale.Id}", sale);
                }
                catch(InvalidOperationException ex)
                {
                  return Results.BadRequest(new { error = ex.Message });
                }
                catch(ArgumentException ex)
                {
                  return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).RequireAuthorization(policy => policy.RequireRole("Employee", "Administrator"))
            .RequireRateLimiting("peticiones-limite")
            .WithName("CreateSale")
            .WithSummary("Crear un venta nueva")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/", async (GetAllSaleUseCase useCase) =>
            {
                try
                {
                    var sales = await useCase.ExecuteAsync();
                    return Results.Ok(sales);
                }
                catch(Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }
            }).RequireAuthorization(policy => policy.RequireRole("Employee", "Administrator"))
            .RequireRateLimiting("peticiones-limite")
            .WithName("GetAllSales")
            .WithSummary("Obtener todas las ventas")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPut("/folio/{folio}", async (string folio, [FromBody] List<ReturnItemDto> returnItemDtos, ReturnSaleUseCase useCase) =>
            {
                try
                {
                    await useCase.ExecuteAsync(folio, returnItemDtos);
                    return Results.NoContent();
                    
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });

                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }
            }).RequireAuthorization(policy => policy.RequireRole("Employee", "Administrator"))
            .RequireRateLimiting("peticiones-limite")
            .WithName("UpdateSale")
            .WithSummary("Actualizar una venta en existencia para devoluciones")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/folios/{folio}", async (GetSaleByFolioUseCase useCase, string folio) =>
            {
                try
                {
                    var sale = await useCase.ExecuteAsync(folio);
                    return Results.Ok(sale);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }
            }).RequireAuthorization(policy => policy.RequireRole("Employee", "Administrator"))
            .RequireRateLimiting("peticiones-limite")
            .WithName("BuscarVentaFolio")
            .WithSummary("Buscar una venta por folio")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError); ;
        }
    }
}
