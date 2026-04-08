using Application.DTOs.SocketsType;
using Application.DTOs.VehiclesType;
using Application.UseCases.Sockets;
using Application.UseCases.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace SistemaGestorInventario.Endpoints
{
    public static class SocketTypeEndpoints
    {
        public static void MapSocketTypeEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/sockettypes").WithTags("SocketTypes");

            group.MapGet("/{id:guid}", async (Guid id, [FromServices] GetSocketByIdUseCase useCase) =>
            {
                try
                {
                    var socketType = await useCase.ExecuteAsync(id);
                    return Results.Ok(socketType);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            }).WithName("GetSocketTypeById")
            .WithSummary("Obtener un tipo de socket por su id")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);


            group.MapPost("/", async (CreateSocketTypeDto dto, CreateSocketTypeUseCase useCase) =>
            {
                try
                {
                    var socket = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/socketTypes/{socket.Id}", socket);
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
            }).WithName("CreateSocketType")
           .WithSummary("Crea un tipo de socket nuevo")
           .Produces(StatusCodes.Status201Created)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/", async (GetAllSocketTypeUseCase useCase) =>
            {
                try
                {
                    var socketTypes = await useCase.ExecuteAsync();
                    return Results.Ok(socketTypes);
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }

            })
               .WithName("GetAllSocketType")
               .WithSummary("Obtener todos los tipos de sockets")
               .Produces(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:guid}", async (Guid id, UpdateSocketTypeDto dto, UpdateSocketTypeUseCase useCase) =>
            {
                
                try
                {
                    var socketType = await useCase.ExecuteAsync(dto, id);
                    return Results.Ok(socketType);
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
            })
            .WithName("UpdateSocket")
            .WithSummary("Actualizar un tipo de socket en existencia")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id:guid}", async (Guid id, DeleteSocketTypeUseCase useCase) =>
            {
                try
                {
                    await useCase.ExecuteAsync(id);
                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });

                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }
            }).WithName("DeleteSocketType")
             .WithSummary("Eliminar un tipo de socket")
             .Produces(StatusCodes.Status204NoContent)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/name{name}", async (string name, GetSocketTypeByNombreUseCase useCase) =>
            {
                try
                {
                    var socketType = await useCase.ExecuteAsync(name);
                    return Results.Ok(socketType);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }
            }).WithName("BuscarTipoDeSocketNombre")
          .WithSummary("Buscar un tipo de socket por nombre")
          .Produces(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status404NotFound)
          .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
