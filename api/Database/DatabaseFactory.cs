using Microsoft.Extensions.Options;
using SqlWeb.Audit;
using SqlWeb.Database.SqlServer;

namespace SqlWeb.Database
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly IOptions<Options> options;
        private readonly IAuditLogFactory auditLogFactory;

        public DatabaseFactory(IOptions<Options> options, IAuditLogFactory auditLogFactory)
        {
            this.options = options;
            this.auditLogFactory = auditLogFactory;
        }
        
        public IDatabase Database(ISession session)
        {
            var auditLog = auditLogFactory.Logger();
            
            switch (session)
            {
                case SqlServerSession sqlServerSession:
                    var sql = new SqlServerDatabase(sqlServerSession, options.Value);
                    return new AuditDatabase(sql, auditLog, "todo");
                
                default:
                    return null;
            }
        }
    }
}