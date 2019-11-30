using SqlWeb.Database.SqlServer;

namespace SqlWeb.Database
{
    public class DatabaseFactory : IDatabaseFactory
    {
        public IDatabase Database(ISession session)
        {
            switch (session)
            {
                case SqlServerSession sqlServerSession:
                    return new SqlServerDatabase(sqlServerSession);
                
                default:
                    return null;
            }
        }
    }
}