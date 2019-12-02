using System.Collections.Generic;

namespace SqlWeb.Audit
{
    public interface IAuditLog
    {
        void Log(string userId, string action, IDictionary<string, object> args);
    }

    public static class AuditLogExtensions
    {
        public static void Log(this IAuditLog audit, string userId, string action, string paramName, object paramValue)
            => audit.Log(userId, action, new Dictionary<string, object>
            {
                [paramName] = paramValue
            });
    }
}