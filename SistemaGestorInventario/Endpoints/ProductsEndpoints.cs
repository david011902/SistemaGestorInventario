using Application.DTOs.Products;
using Application.UseCases.Products;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace SistemaGestorInventario.Endpoints
{
    public static class ProductsEndpoints
    {
        public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/products")
                .WithTags("Products");//Para agrupar los endpoints en la documentacion de swagger

            group.MapGet("/{id:guid}", async (Guid id, GetProductByIdUseCase useCase) =>
            {
                try
                {
                    var product = await useCase.ExecuteAsync(id);
                    return Results.Ok(product);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            }).WithName("GetProductById")
            .WithSummary("Obtener un producto por su id")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);


            group.MapPost("/", async (CreateProductDto dto, CreateProductUseCase useCase) =>
            {
                try
                {
                    var product = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/persons/{product.Id}", product);
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
            }).WithName("CreateProduct")
            .WithSummary("Crea un producto nuevo")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/", async (GetAllProductsUseCase useCase) =>
            {
                try
                {
                    var products = await useCase.ExecuteAsync();
                    return Results.Ok(products);
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }

            })
                .WithName("GetAllProducts")
                .WithSummary("Obtener todos los productos")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError);


            group.MapPut("/{id:guid}", async (Guid id, UpdateProductDto dto, UpdateProductUseCase useCase) =>
            {
                
                try
                {
                    var product = await useCase.ExecuteAsync(dto, id);
                    return Results.Ok(product);
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
            .WithName("UpdateProduct")
            .WithSummary("Actualizar un producto en existencia")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);


            group.MapDelete("/{id:guid}", async (Guid id, DeleteProductUseCase useCase) =>
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
            }).WithName("DeleteProduct")
              .WithSummary("Eliminar un producto")
              .Produces(StatusCodes.Status204NoContent)
              .Produces(StatusCodes.Status404NotFound)
              .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/code{code}", async (string code, GetProductBySkuUseCase useCase) =>
            {
                try
                {
                    var product = await useCase.ExecuteAsync(code);
                    return Results.Ok(product);
                }
                catch(InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch(Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }
            }).WithName("BuscarProductoSKU")
            .WithSummary("Buscar un producto por SKU")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/search", async (string? name, GetProductByNameUseCase useCase) =>
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Results.BadRequest(new { error = "El parámetro 'name' es obligatorio." });
                }
                try
                {
                  
                    var product = await useCase.ExecuteAsync(name);
                    return Results.Ok(product);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }
            }).WithName("BuscarProductoNombre")
           .WithSummary("Buscar un producto por Nombre")
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status404NotFound)
           .Produces(StatusCodes.Status500InternalServerError);
        }
    }

}
