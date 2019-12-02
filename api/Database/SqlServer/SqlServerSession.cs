using System.Data;
using System.Data.SqlClient;

namespace SqlWeb.Database.SqlServer
{
    public class SqlServerSession : ISession
    {
        private readonly SqlConnectionStringBuilder builder;

        public SqlServerSession(SqlConnectionStringBuilder builder)
        {
            this.builder = builder;

            builder.ApplicationName = "sqlweb"; // todo: +version
            
            var readOnly = true;
            if (readOnly)
            {
                builder.ApplicationIntent = ApplicationIntent.ReadOnly;
            }
        }

        public string CurrentDatabase()
        {
            return builder.InitialCatalog;
        }

        public void SwitchDatabase(string database)
        {
            builder.InitialCatalog = database;
        }

        public IDbConnection Connection()
        {
            var conn = new SqlConnection(builder.ToString());
            conn.Open();
            return conn;
        }
    }
}