using System;
using System.Web.Http;
using System.Web.Routing;

namespace Shopping_Admin_web {
    public class Global : System.Web.HttpApplication {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Application_Start(object sender, EventArgs e) {
            // Add Routes.
            RegisterCustomRoutes(RouteTable.Routes);
        }

        void RegisterCustomRoutes(RouteCollection routes) {
            routes.MapPageRoute(
                "index",
                "*",
                "~/Index.html"
            );
            RouteTable.Routes.MapHttpRoute(
                name: "AgentApi",
                routeTemplate: "api/{controller}/{action}"
            );
        }

    }
}