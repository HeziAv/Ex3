using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Map", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute("display", "display/{ip}/{port}",
                defaults: new { Controller = "Map", action = "display" });

            routes.MapRoute("save", "display/{ip}/{port}/{times}/{second}/{file}",
                defaults: new { Controller = "Map", action = "save" });

           routes.MapRoute("updatedDisplay", "Display/{ip}/{port}/{second}",
              defaults: new { Controller = "Map", action = "updatedDisplay" });
        }

    }
}
