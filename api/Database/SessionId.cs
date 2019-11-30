using System;
using System.Security.Cryptography;

namespace SqlWeb.Database
{
    public class SessionId
    {
        public static SessionId NewSessionId()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[32];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var key = Convert.ToBase64String(randomBytes);
            
            return new SessionId(key);
        }

        public static SessionId Parse(string str)
        {
            return new SessionId(str);
        }
        
        private SessionId(string value)
        {
            Value = value;
        }
        
        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }
    }
}