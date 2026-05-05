using Application.DTOs.Auth;
using Application.UseCases.Auth;
using Domain.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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


            //Es necesario "httpcontext" para las cookies
            group.MapPost("/login", async (
                [FromBody] LoginRequest request,
                LoginUseCase useCase,
                HttpContext httpContext) =>
            {
                try
                {
                    var dto = new LoginRequestDto
                    {
                        Email = request.Email,
                        Password = request.Password
                    };

                    var result = await useCase.ExecuteAsync(dto); 
                    SetTokenCookies(httpContext, new TokenPair(result.AccessToken, result.RefreshToken));

                    return Results.Ok(new
                    {
                        message = "Login exitoso",
                        user = new
                        {
                            id = result.UserId,
                            email = result.Email,
                            role = result.Role
                        }
                    });
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
            .WithSummary("Inicia sesión y setea cookies HttpOnly")
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPost("/logout", (HttpContext httpContext) =>
            {
                httpContext.Response.Cookies.Delete("access_token");
                httpContext.Response.Cookies.Delete("refresh_token");
                return Results.Ok(new { message = "Sesión cerrada" });
            }).WithName("Logout")
            .AllowAnonymous();

            group.MapPost("/refresh", async (
                [FromBody] RefreshRequest request,
                HttpContext httpContext,
                RefreshTokenUseCase useCase) =>
            {
                try
                {
                    var refreshToken = httpContext.Request.Cookies["refresh_token"];
                    if (string.IsNullOrEmpty(refreshToken))
                        return Results.Unauthorized();

                    var tokens = await useCase.ExecuteAsync(refreshToken);
                    SetTokenCookies(httpContext, tokens);

                    return Results.Ok(new { message = "Token renovado" });
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
            .WithSummary("Renueva el access token con el refresh token en la cookie")
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/me", (
            HttpContext httpContext,
            ITokenService tokenService) =>
            {
                var accessToken = httpContext.Request.Cookies["access_token"];

                if (string.IsNullOrEmpty(accessToken))
                    return Results.Unauthorized();

                try
                {
                    var payload = tokenService.Verify(accessToken);

                    return Results.Ok(new
                    {
                        id = payload.Sub,
                        email = payload.Email,
                        role = payload.Role.ToString()
                    });
                }
                catch (Exception ex)
                {
                    return Results.Json(new { message = "Sesión inválida o expirada", error = ex.Message },
                                        statusCode: StatusCodes.Status401Unauthorized);
                }
            })
                .AllowAnonymous() 
                .WithName("GetUserSession")
                .WithSummary("Obtiene los datos del usuario a partir de la cookie access_token")
                .WithDescription("Este endpoint extrae el JWT de las cookies y lo decodifica para retornar el perfil.")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError);
        }
        static void SetTokenCookies(HttpContext httpContext, TokenPair tokens)
        {
            var accessOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,//Es de esta forma porque el front no se encuentra alojado en el mismo sitio que el back
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            };

            var refreshOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };

            httpContext.Response.Cookies.Append("access_token", tokens.AccessToken, accessOptions);
            httpContext.Response.Cookies.Append("refresh_token", tokens.RefreshToken, refreshOptions);
        }
        public record RegisterRequest(string Name, string Email, string Password);
        public record LoginRequest(string Email, string Password);
        public record RefreshRequest(string RefreshToken);
    }
}