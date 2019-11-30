using System.Linq;
using System.Text;
using SqlWeb.Types;

namespace SqlWeb.Database.SqlServer
{
    public class SqlServerDatabase : IDatabase
    {
        private readonly SqlServerSession session;

        public SqlServerDatabase(SqlServerSession session)
        {
            this.session = session;
        }
        
        public SchemaObjects Objects()
        {
            var objects = new SchemaObjects();
            
            var query = @"SELECT 
                schema_name(o.schema_id) as schema_name,
                o.name, 
                o.type
                FROM sys.objects o
                WHERE o.type IN ('U', 'V', 'P', 'FN', 'IF', 'TF', 'SO')";

            var results = session.Query(query);

            foreach (var row in results.Rows)
            {
                var schema = (string) row[0];
                var name = (string) row[1];
                var type = (string) row[2];
                
                if (!objects.ContainsKey(schema))
                {
                    objects[schema] = new Objects();
                }

                switch (type) // CHAR(2)
                {
                    case "U ":
                        objects[schema].Tables.Add(name);
                        break;
                    case "V ":
                        objects[schema].Views.Add(name);
                        break;
                    case "P ":
                        objects[schema].Procedures.Add(name);
                        break;
                    case "FN":
                    case "IF":
                    case "TF":
                        objects[schema].Functions.Add(name);
                        break;
                    case "SO":
                        objects[schema].Sequences.Add(name);
                        break;
                }
            }
            
            return objects;
        }

        public TableInfo TableInfo(string table)
        {
            var query = "EXEC sys.sp_spaceused @1";

            var results = session.Query(query, table);

            var row = results.Rows.FirstOrDefault();
            if (row == null)
                return null;
            
            return new TableInfo
            {
                Rows = long.Parse((string) row[1]), 
                TotalSize = (string) row[2],
                DataSize = (string) row[3],
                IndexSize = (string) row[4],
            };

        }

        public Result TableDefinition(string table)
        {
            var query = @"SELECT
                    c.name,
                    type_name(c.user_type_id) as data_type,
                    c.max_length,
                    c.is_nullable,
                    c.precision,
                    c.scale,
                    c.is_identity,
                    dc.definition as column_default, 
                    p.value as comment
                FROM sys.columns c
                LEFT JOIN sys.default_constraints dc
                    ON dc.parent_column_id = c.column_id
                    AND dc.parent_object_id = c.object_id
                LEFT JOIN sys.extended_properties p
                    ON p.major_id = c.object_id
                    AND p.minor_id = c.column_id
                    AND p.name = 'MS_Description'
                WHERE c.object_id = object_id(@1)";

            return session.Query(query, table);
        }

        public Result TableRows(string table, RowsOptions opts)
        {
            var query = new StringBuilder();
            query.Append($"SELECT * FROM {table}");

            if (!string.IsNullOrWhiteSpace(opts.Where))
            {
                query.Append($" WHERE {opts.Where}");
            }

            if (!string.IsNullOrWhiteSpace(opts.SortColumn))
            {
                query.Append($" ORDER BY {opts.SortColumn} {opts.SortOrder}");
            }
            else
            {
                query.Append(" ORDER BY 1 ASC"); // hopefully will pick the primary key and hopefully the primary key direction is ASC 
            }

            query.Append($" OFFSET {opts.Offset} ROWS");
            query.Append($" FETCH NEXT {opts.Limit} ROWS ONLY");

            return session.Query(query.ToString());
        }

        public long TableRowsCount(string table, RowsOptions opts)
        {
            // Can return the estimated rows from the catalog if we're not filtering
            if (string.IsNullOrWhiteSpace(opts.Where))
            {
                return TableInfo(table).Rows;
            }

            var query = $"SELECT COUNT(1) FROM {table} WHERE {opts.Where}";
            return (long) session.ExecuteScalar(query);
        }

        public Databases Databases()
        {
            var query = "select name from sys.databases where database_id > 4;";
            var results = session.Query(query);
            
            var databases = new Databases();
            foreach (var row in results.Rows)
            {
                databases.Add((string) row[0]);
            }
            return databases;
        }
    }
}