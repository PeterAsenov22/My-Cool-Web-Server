namespace WebServer.Server.Http
{
    using System.Collections.Concurrent;
    using Contracts;

    public static class SessionStore
    {
        public const string SessionCookieKey = "MY_SID";
        public const string CurrentUserKey = "^%Current_User_Session_Key%^";

        private static readonly ConcurrentDictionary<string, IHttpSession> sessions 
            = new ConcurrentDictionary<string, IHttpSession>();

        public static IHttpSession Get(string id) => sessions.GetOrAdd(id, _ => new HttpSession(id));
    }
}
