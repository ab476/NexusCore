using System;
using System.Collections.Generic;

namespace NC.AuthService.Infrastructure.Identity;
    public class JwtOptions
    {
        public required string Secret { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
