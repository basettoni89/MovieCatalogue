using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Api.Settings
{
    public class TokenSettings
    {
        public const string SettingsKey = "TokenSettings";

        public string PrivateKeyPath { get; set; }

        public string PublicKeyPath { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan ValidationPeriod { get; set; }
    }
}
