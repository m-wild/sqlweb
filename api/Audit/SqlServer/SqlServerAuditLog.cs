using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using SqlWeb.Types;

namespace SqlWeb.Audit.SqlServer
{
    public class SqlServerAuditLog : IAuditLog
    {
        private const string TableExists = "SELECT 1 FROM sys.tables WHERE name = 'auditlog'";

        private const string CreateTable = @"CREATE TABLE auditlog (
            id BIGINT IDENTITY PRIMARY KEY,
            user_id VARCHAR(250) NOT NULL,
            action VARCHAR(50) NOT NULL,
            args VARCHAR(MAX) NULL)";

        private const string Insert = "INSERT INTO auditlog (user_id, action, args) VALUES (@user_id, @action, @args)";
        
        private readonly string connectionString;

        public SqlServerAuditLog(string connectionString)
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
                var exists = cmd.ExecuteScalar() != null;
                if (exists)
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
        
        
        public void Log(string userId, string action, IDictionary<string, object> args)
        {
            var argsString = SerializeArgs(args);

            using var conn = Connection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = Insert;
            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@action", action);
            cmd.Parameters.AddWithValue("@args", argsString);

            cmd.ExecuteNonQuery();
        }

        private string SerializeArgs(IDictionary<string, object> args)
        {
            var log = new Dictionary<string, object>();

            foreach (var a in args)
            {
                switch (a.Value)
                {
                    case Result r:
                        if (r.Columns.FirstOrDefault() == "Rows Affected")
                        {
                            log.Add("rows_affected", r.Rows[0][0]);
                        }
                        else
                        {
                            var selected = r.Rows.Count;
                            log.Add("count", selected);
                        }
                        break;
                    
                    case Exception ex:
                        log.Add(a.Key, ex.Message);
                        break;
                    
                    default:
                        log.Add(a.Key, a.Value);
                        break;
                }
            }
            
            return JsonSerializer.Serialize(log, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }
    }
}