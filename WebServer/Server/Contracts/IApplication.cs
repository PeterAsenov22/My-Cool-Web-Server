namespace WebServer.Server.Contracts
{
    using Routing.Contracts;

    public interface IApplication
    {
        void Configure(IAppRoutingConfig appRouteConfig);
    }
}
