namespace SqlWeb.Database
{
    public interface IDatabaseFactory
    {
        IDatabase Database(ISession session);
    }
}