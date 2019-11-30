using System.Data;

namespace SqlWeb.Database
{
    public interface ISession
    {
        IDbConnection Connection();
    }
}