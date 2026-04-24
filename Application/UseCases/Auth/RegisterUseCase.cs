using Application.DTOs.Auth;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Auth
{
    public class RegisterUseCase(IAuthRepository authRepository)
    {
        public async Task<RegisterResponseDto> ExecuteAsync(RegisterRequestDto dto)
        {
            ValidatePassword(dto.Password);

            var exists = await authRepository.FindByEmailAsync(dto.Email);
            if (exists is not null)
                throw new InvalidOperationException("El correo ya está registrado.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, workFactor: 12);
            var user = UserEntity.Create(dto.Name, dto.Email, passwordHash);

            await authRepository.SaveAsync(user);

            return new RegisterResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString() 
            };
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña no puede estar vacía.");
            if (password.Length < 8)
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");
            if (!password.Any(char.IsUpper))
                throw new ArgumentException("La contraseña debe tener al menos una mayúscula.");
            if (!password.Any(char.IsDigit))
                throw new ArgumentException("La contraseña debe tener al menos un número.");
        }
    }
}
