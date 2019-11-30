using System.Collections.Generic;

namespace SqlWeb.Database
{
    public class InMemorySessionStore : ISessionStore
    {
        private static readonly Dictionary<string, ISession> Sessions = new Dictionary<string, ISession>();
        
        public SessionId SaveSession(ISession session)
        {
            var id = SessionId.NewSessionId();       
            Sessions.Add(id.Value, session);
            return id;
        }

        public ISession GetSession(SessionId sessionId)
        {
            return Sessions.TryGetValue(sessionId.Value, out var session)
                ? session
                : null;
        }
    }
}