using WebServer.Server.Routing.Contracts;

namespace WebServer.Server.Contracts
{ 
    public interface IApplication
    {
        void Configure(IAppRoutingConfig appRouteConfig);
    }
}
