using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Auth
{
    public class RefreshRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
