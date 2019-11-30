using System.Data;

namespace SqlWeb.Database
{
    public interface ISession
    {
        string CurrentDatabase();

        void SwitchDatabase(string database);
        
        IDbConnection Connection();
    }
}