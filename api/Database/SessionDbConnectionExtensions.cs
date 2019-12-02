using System.Data;
using SqlWeb.Types;

namespace SqlWeb.Database
{
    public static class SessionDbConnectionExtensions
    {
        public static Result Query(this ISession session, string query) => session.Query(query, null);

        public static Result Query(this ISession session, string query, params object[] args)
        {
            using var conn = session.Connection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.AddParameters(args);
            
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


        public static object ExecuteScalar(this ISession session, string query) => session.ExecuteScalar(query, null);

        public static object ExecuteScalar(this ISession session, string query, params object[] args)
        {
            using var conn = session.Connection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.AddParameters(args);

            return cmd.ExecuteScalar();
        }

        private static void AddParameters(this IDbCommand cmd, params object[] args)
        {
            if (args == null) return;
            
            for (var i = 0; i < args.Length; i++)
            {
                var param = cmd.CreateParameter();
                param.ParameterName = (i + 1).ToString();
                param.Value = args[i];
                cmd.Parameters.Add(param);
            }
        }

        public static int ExecuteNonQuery(this ISession session, string query)
        {
            using var conn = session.Connection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;

            return cmd.ExecuteNonQuery();
        }
    }
}