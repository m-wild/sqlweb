namespace SqlWeb.Database
{
    public interface ISessionStore
    {
        SessionId SaveSession(ISession session);

        ISession GetSession(SessionId sessionId);
    }
}