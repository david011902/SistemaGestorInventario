using Application.DTOs.Auth;
using Application.UseCases.Auth;
using Domain.Abstractions;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SistemaGestorInventario.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app) 
        {
            var group = app.MapGroup("/api/auth")
            .WithTags("Auth");

            group.MapPost("/register", async (
                [FromBody] RegisterRequest request,
                RegisterUseCase useCase) =>
            {
                try
                {
                    var dto = new RegisterRequestDto
                    {
                        Name = request.Name,
                        Email = request.Email,
                        Password = request.Password
                    };

                    var response = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/auth/{response.Id}", response);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).RequireAuthorization(policy => policy.RequireRole("Administrator"))
            .RequireRateLimiting("peticiones-limite")
            .WithName("Register")
            .WithSummary("Registra un usuario nuevo")
            .Produces<RegisterResponseDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status429TooManyRequests);


            group.MapPost("/login", async (
                [FromBody] LoginRequest request,
                LoginUseCase useCase) =>
            {
                try
                {
                    var dto = new LoginRequestDto
                    {
                        Email = request.Email,
                        Password = request.Password
                    };

                    var response = await useCase.ExecuteAsync(dto);
                    return Results.Ok(response);
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.BadRequest("Email o contraseña incorrecto");
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).RequireRateLimiting("peticiones-limite")
            .WithName("Login")
            .WithSummary("Inicia sesión y devuelve tokens JWT")
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);


            group.MapPost("/refresh", (
                [FromBody] RefreshRequest request,
                RefreshTokenUseCase useCase) =>
            {
                try
                {
                    var dto = new RefreshRequestDto
                    {
                        RefreshToken = request.RefreshToken
                    };

                    var response = useCase.ExecuteAsync(dto);
                    return Results.Ok(response);
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.Unauthorized();
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).RequireRateLimiting("peticiones-limite")
            .WithName("RefreshToken")
            .WithSummary("Renueva el access token con el refresh token")
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/me", (ClaimsPrincipal currentUser) =>
            {
                try
                {
                    var userId = currentUser.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                    var email = currentUser.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
                    var role = currentUser.FindFirst(ClaimTypes.Role)?.Value;

                    return Results.Ok(new { userId, email, role });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            }).RequireRateLimiting("peticiones-limite")
            .WithName("Me")
            .WithSummary("Devuelve los datos del usuario autenticado")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);
        }

    }
    public record RegisterRequest(string Name, string Email, string Password);
    public record LoginRequest(string Email, string Password);
    public record RefreshRequest(string RefreshToken);
}
