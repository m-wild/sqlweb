using Microsoft.Extensions.Options;
using SqlWeb.Audit.SqlServer;

namespace SqlWeb.Audit
{
    public interface IAuditLogFactory
    {
        IAuditLog Logger();
    }

    public class AuditLogFactory : IAuditLogFactory
    {
        private readonly IOptions<Options> options;

        public AuditLogFactory(IOptions<Options> options)
        {
            this.options = options;
        }
        
        public IAuditLog Logger()
        {
            switch (options.Value.AuditLogType.ToLower())
            {
                case "sqlserver":
                    return new SqlServerAuditLog(options.Value.SqlServerConnectionString);
                
                default:
                    return null;
            }
        }
    }
}