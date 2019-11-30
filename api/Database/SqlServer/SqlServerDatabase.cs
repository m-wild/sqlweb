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

            using var reader = session.Connection().ExecuteReader(query);
            
            while (reader.Read())
            {
                var schema = reader.GetString(0);
                var name = reader.GetString(1);
                var type = reader.GetString(2);
                
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

            using var reader = session.Connection().ExecuteReader(query, table);

            if (reader.Read())
            {
                return new TableInfo
                {
                    Rows = long.Parse(reader.GetString(1)), 
                    TotalSize = reader.GetString(2),
                    DataSize = reader.GetString(3),
                    IndexSize = reader.GetString(4)
                };
            }

            return null;
        }

        private (string, string) GetSchemaAndTable(string value)
        {
            var chunks = value.Split('.');
            if (chunks.Length == 1)
            {
                return ("dbo", chunks[0]);
            }

            return (chunks[0], chunks[1]);
        }
    }




}