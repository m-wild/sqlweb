using System;
using System.Collections.Generic;
using SqlWeb.Database;
using SqlWeb.Types;

namespace SqlWeb.Audit
{
    public class AuditDatabase : IDatabase
    {
        private readonly IDatabase databaseImplementation;
        private readonly IAuditLog auditLog;
        private readonly string user;

        public AuditDatabase(IDatabase databaseImplementation, IAuditLog auditLog, string user)
        {
            this.databaseImplementation = databaseImplementation;
            this.auditLog = auditLog;
            this.user = user;
        }

        public string Test()
        {
            return databaseImplementation.Test();
        }

        public (Result, string) RunQuery(string query)
        {
            try
            {
                auditLog.Log(user, nameof(RunQuery), "query", query);
                var (result, err) = databaseImplementation.RunQuery(query);

                auditLog.Log(user, nameof(RunQuery), new Dictionary<string, object>
                {
                    ["query"] = query,
                    ["result"] = result,
                    ["error"] = err,
                });
                
                return (result, err);
            }
            catch (Exception ex)
            {
                auditLog.Log(user, nameof(RunQuery), new Dictionary<string, object>
                {
                    ["query"] = query,
                    ["error"] = ex,
                });
                throw;
            }
        }

        public SchemaObjects Objects()
        {
            return databaseImplementation.Objects();
        }

        public TableInfo TableInfo(string table)
        {
            return databaseImplementation.TableInfo(table);
        }

        public Result TableDefinition(string table)
        {
            return databaseImplementation.TableDefinition(table);
        }

        public Result TableRows(string table, RowsOptions opts)
        {
            return databaseImplementation.TableRows(table, opts);
        }

        public long TableRowsCount(string table, RowsOptions opts)
        {
            return databaseImplementation.TableRowsCount(table, opts);
        }

        public Databases Databases()
        {
            return databaseImplementation.Databases();
        }
    }
}