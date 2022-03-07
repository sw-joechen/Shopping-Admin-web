using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Shopping_Admin_web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Add Routes.
            RegisterCustomRoutes(RouteTable.Routes);
        }

        void RegisterCustomRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(
                "login",
                "login",
                "~/login.aspx"
            );
            RouteTable.Routes.MapHttpRoute(
                name: "MemberApi",
                routeTemplate: "api/{controller}/{action}"
            );
            //routes.MapPageRoute(
            //    "ProductByNameRoute",
            //    "Product/{productName}",
            //    "~/ProductDetails.aspx"
            //);
        }

    }
}