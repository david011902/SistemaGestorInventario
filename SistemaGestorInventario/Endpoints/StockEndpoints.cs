using Application.DTOs.Lots;
using Application.UseCases.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGestorInventario.Endpoints
{
    public static class StockEndpoints
    {
        public static void MapStockEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/stock").WithTags("Stock");

            group.MapPut("/{id:guid}", async (Guid id, UpdateLotDto dto, AdjustStockUseCase useCase) =>
            {
                try
                {
                    dto.Id = id;
                    await useCase.ExecuteAsync(dto);
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
            }).WithName("UpdateStock")
            .WithSummary("Actualizar stock en existencia")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);


            group.MapPost("/", async (ReceiveStockUseCase useCase, CreateLotDto dto) =>
            {
                try
                {
                    var lot = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/stock/{lot.Id}", lot);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).WithName("CreateLot")
            .WithSummary("Crea un lote nuevo")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/", async (GetAllStockUseCase useCase) =>
            {
                try
                {
                    var lots = await useCase.ExecuteAsync();
                    return Results.Ok(lots);
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }
            }).WithName("GetAllLots")
               .WithSummary("Obtener todos los lotes")
               .Produces(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/search/{id:guid}", async (Guid id, [FromServices] GetStockByIdUseCase useCase) =>
            {
                try
                {
                    var lot = await useCase.ExecuteAsync(id);
                    return Results.Ok(lot);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }
            })
                .WithName("GetLotById")
                .WithSummary("Obtener un lote por su id")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
                        }

       
    }
}
