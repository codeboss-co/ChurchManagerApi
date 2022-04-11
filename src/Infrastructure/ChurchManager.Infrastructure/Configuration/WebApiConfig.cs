﻿namespace ChurchManager.Infrastructure.Configuration
{
    public class WebApiConfig
    {
        public string SecretKey { get; set; }
        public bool ValidateIssuer { get; set; }
        public string ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public string ValidAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public int ExpiryInMinutes { get; set; }
        public int RefreshTokenExpiryInMinutes { get; set; }
    }
}
