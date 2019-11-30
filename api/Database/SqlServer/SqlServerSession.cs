using System.Data;
using System.Data.SqlClient;

namespace SqlWeb.Database.SqlServer
{
    public class SqlServerSession : ISession
    {
        private readonly string connectionString;

        public SqlServerSession(SqlConnectionStringBuilder builder)
        {
            connectionString = builder.ToString();
        }
        
        
        public IDbConnection Connection()
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}