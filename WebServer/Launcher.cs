﻿namespace WebServer
{
    using Server.Contracts;
    using Server;
    using Server.Routing;
    using GameStoreApplication;

    public class Launcher : IRunnable
    {
        private WebServer webServer;

        public static void Main()
        {
            new Launcher().Run();
        }

        public void Run()
        {
            var mainApplication = new GameStoreApp();
            mainApplication.InitializeDatabase();

            var appRouteConfig = new AppRouteConfig();
            mainApplication.Configure(appRouteConfig);
            this.webServer = new WebServer(1337,appRouteConfig);
            this.webServer.Run();
        }
    }
}
