using System.Data;
using SqlWeb.Types;

namespace SqlWeb.Dialect.SqlServer
{
    public class SqlServer
    {
        public Result GetAllObjects(IDbConnection db)
        {

            var query = @"SELECT 
                s.name AS [schema],
                o.name, 
                CASE o.type
                    WHEN 'U' THEN 'table'
                    WHEN 'V' THEN 'view'
                    WHEN 'P' THEN 'procedure'
                    WHEN 'FN' THEN 'function'
                    WHEN 'IF' THEN 'function'
                    WHEN 'TF' THEN 'function'
                    WHEN 'SO' THEN 'sequence'
                    END AS [type]
                FROM sys.objects o
                INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
                WHERE o.type IN ('U', 'V', 'P', 'FN', 'IF', 'TF', 'SO')";

            return db.Query(query);

        }
        
        
    }

    public static class DbExtensions
    {
        public static Result Query(this IDbConnection db, string query)
        {
            using var cmd = db.CreateCommand();
            cmd.CommandText = query;

            using var reader = cmd.ExecuteReader();

            var result = new Result();
            
            for (var i = 0; i < reader.FieldCount; i++)
            {
                result.Columns.Add(reader.GetName(i));
            }
            
            while (reader.Read())
            {
                var values = new object[reader.FieldCount];
                reader.GetValues(values);
                result.Rows.Add(values);
            }

            return result;
        }
    }
    
}