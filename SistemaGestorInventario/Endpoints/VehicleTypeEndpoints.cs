using Application.DTOs.VehiclesType;
using Application.UseCases.Vehicles;

namespace SistemaGestorInventario.Endpoints
{
    public static class VehicleTypeEndpoints
    {
        public static void MapVehicleTypeEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/vehicletypes").WithTags("VehicleTypes").RequireAuthorization();

            group.MapGet("/{id:guid}", async (Guid id, GetVehicleTypeByIdUseCase useCase) => 
            {
                try
                {
                    var vehicleType = await useCase.ExecuteAsync(id);
                    return Results.Ok(vehicleType);
                }
                catch(InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            }).RequireAuthorization(policy => policy.RequireRole("Employee", "Administrator"))
            .RequireRateLimiting("peticiones-limite")
            .WithName("GetVehicleTypeById")
            .WithSummary("Obtener un tipo de vehiculo por su id")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status404NotFound);


            group.MapPost("/", async (CreateVehicleTypeDto dto, CreateVehicleTypeUseCase useCase) =>
            {
                try
                {
                    var product = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/vehicleTypes/{product.Id}", product);
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
            }).RequireAuthorization(policy => policy.RequireRole("Employee", "Administrator"))
           .RequireRateLimiting("peticiones-limite")
           .WithName("CreateVehicleType")
           .WithSummary("Crea un tipo de vehiculo nuevo")
           .Produces(StatusCodes.Status201Created)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status429TooManyRequests)
           .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/", async (GetAllVehicleTypeUseCase useCase) =>
            {
                try
                {
                    var vehicleTypes = await useCase.ExecuteAsync();
                    return Results.Ok(vehicleTypes);
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }

            }).RequireAuthorization(policy => policy.RequireRole("Employee", "Administrator"))
               .RequireRateLimiting("peticiones-limite")
               .WithName("GetAllVehiclesType")
               .WithSummary("Obtener todos los tipos de vehiculo")
               .Produces(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status429TooManyRequests)
               .Produces(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:guid}", async (Guid id, UpdateVehicleTypeDto dto, UpdateVehicleTypeUseCase useCase) =>
            {
               
                try
                {
                    var vehicleType = await useCase.ExecuteAsync(dto, id);
                    return Results.Ok(vehicleType);
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
            .WithName("UpdateVehicle")
            .WithSummary("Actualizar un tipo de vehiculo en existencia")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id:guid}", async (Guid id, DeleteVehicleTypeUseCase useCase) =>
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
            }).RequireAuthorization(policy => policy.RequireRole("Employee", "Administrator"))
             .RequireRateLimiting("peticiones-limite")
             .WithName("DeletevehicleType")
             .WithSummary("Eliminar un tipo de vehiculo")
             .Produces(StatusCodes.Status204NoContent)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status429TooManyRequests)
             .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/name{name}", async (string name, GetVehicleTypeByNameUseCase useCase) =>
            {
                try
                {
                    var vehicleType = await useCase.ExecuteAsync(name);
                    return Results.Ok(vehicleType);
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
          .WithName("BuscarTipoDeVehiculoNombre")
          .WithSummary("Buscar un tipo de vehiculo por nombre")
          .Produces(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status404NotFound)
          .Produces(StatusCodes.Status429TooManyRequests)
          .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}

