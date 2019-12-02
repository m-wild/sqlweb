using System.Collections.Generic;
using System.Data.SqlClient;
using SqlWeb.Types;

namespace SqlWeb.Persistence.SqlServer
{
    public class SqlServerResourceStore : IResourceStore
    {
        private const string CreateTable = @"CREATE TABLE resources (
            resource_id VARCHAR(50) PRIMARY KEY, 
            engine VARCHAR(20) NOT NULL,
            connection_string VARCHAR(1000) NOT NULL)";

        private const string TableExists = "SELECT 1 FROM sys.tables WHERE name = 'resources'";
        
        private readonly string connectionString;

        public SqlServerResourceStore(string connectionString)
        {
            this.connectionString = connectionString;

            Initialize();
        }

        private SqlConnection Connection()
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        private void Initialize()
        {
            using var conn = Connection();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = TableExists;
                var tableExists = cmd.ExecuteScalar() != null;
                if (tableExists)
                {
                    return;
                }
            }
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = CreateTable;
                cmd.ExecuteNonQuery();
            }
        }
        
        public Resource GetResource(string resourceId)
        {
            using var conn = Connection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT resource_id, engine, connection_string FROM resources WHERE resource_id = @id";
            cmd.Parameters.AddWithValue("@id", resourceId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return Resource(reader);
            }

            return null;
        }

        public List<Resource> GetAllResources()
        {
            var resources = new List<Resource>();
            
            using var conn = Connection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT resource_id, engine, connection_string FROM resources";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                resources.Add(Resource(reader));
            }

            return resources;
        }

        private static Resource Resource(SqlDataReader reader)
        {
            return new Resource
            {
                ResourceId = reader.GetString(0),
                Engine = reader.GetString(1),
                ConnectionString = reader.GetString(2),
            };
        }
    }
}