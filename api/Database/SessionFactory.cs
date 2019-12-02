using System.Data.SqlClient;
using SqlWeb.Database.SqlServer;
using SqlWeb.Types;

namespace SqlWeb.Database
{
    public class SessionFactory : ISessionFactory
    {
        public ISession ConnectResource(Resource resource)
        {
            switch (resource.Engine.ToLower())
            {
                case "sqlserver":
                    return new SqlServerSession(new SqlConnectionStringBuilder(resource.ConnectionString));
                
                default:
                    return null;
            }
        }

        public ISession Connect(string engine, string connectionString)
        {
            switch (engine.ToLower())
            {
                case "sqlserver":
                    return new SqlServerSession(new SqlConnectionStringBuilder(connectionString));
                
                default:
                    return null;
            }
        }
    }
}