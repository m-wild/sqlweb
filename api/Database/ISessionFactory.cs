using SqlWeb.Types;

namespace SqlWeb.Database
{
    public interface ISessionFactory
    {
        ISession ConnectResource(Resource resource);

        ISession Connect(string engine, string connectionString);
    }
}