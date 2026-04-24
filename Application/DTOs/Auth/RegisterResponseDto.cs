using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
