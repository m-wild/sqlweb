using System.Data;
using SqlWeb.Types;

namespace SqlWeb.Database
{
    public static class DbConnectionExtensions
    {
        public static Result Query(this IDbConnection db, string query)
        {
            return db.Query(query, null);
        }

        public static Result Query(this IDbConnection db, string query, params object[] args)
        {
            using var reader = db.ExecuteReader(query, args);

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

        public static IDataReader ExecuteReader(this IDbConnection db, string query, params object[] args)
        {
            using var cmd = db.CreateCommand();
            cmd.CommandText = query;

            if (args != null)
            {
                for (var i = 0; i < args.Length; i++)
                {
                    var param = cmd.CreateParameter();
                    param.ParameterName = (i + 1).ToString();
                    param.Value = args[i];
                    cmd.Parameters.Add(param);
                }
            }
            
            return cmd.ExecuteReader();
        }
        
    }
}