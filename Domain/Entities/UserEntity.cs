using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Entities
{
   public class UserEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public UserRole Role { get; private set; }
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private UserEntity() { }
        public static UserEntity Create(
           string name,
           string email,
           string passwordHash,
           UserRole role = UserRole.Employee)
            {
                ValidateName(name);
                ValidateEmail(email);
                ValidatePassword(passwordHash);

                return new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    PasswordHash = passwordHash,
                    Role = role,
                };
        }
        public static UserEntity Reconstitute(
           Guid id, string name, string email,
           string passwordHash, UserRole role)
            {
                return new UserEntity
                {
                    Id = id,
                    Name = name,
                    Email = email,
                    PasswordHash = passwordHash,
                    Role = role,
                };
        }
        public bool IsAdministrator() => Role == UserRole.Administrator;
        public bool IsEmployee() => Role == UserRole.Employee;
        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo electrónico no puede estar vacío.", nameof(email));
            if (!EmailRegex.IsMatch(email))
                throw new ArgumentException("El correo electrónico no es válido.", nameof(email));
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña no puede estar vacía.", nameof(password));
          
        }

        public static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));
        }
    }
}
