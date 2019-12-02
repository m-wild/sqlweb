using Microsoft.Extensions.Options;
using SqlWeb.Database.SqlServer;

namespace SqlWeb.Database
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly IOptions<Options> options;

        public DatabaseFactory(IOptions<Options> options)
        {
            this.options = options;
        }
        
        public IDatabase Database(ISession session)
        {
            switch (session)
            {
                case SqlServerSession sqlServerSession:
                    return new SqlServerDatabase(sqlServerSession, options.Value);
                
                default:
                    return null;
            }
        }
    }
}