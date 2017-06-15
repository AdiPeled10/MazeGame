using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;

namespace Project
{
    public static class WebApiConfig
    {
        public static object RouteConfig { get; private set; }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            

            config.Routes.MapHttpRoute(
                    name: "GenerateMaze",
                    routeTemplate: "api/{controller}/{name}/{rows}/{cols}",
                    defaults: new {controller = "MazeController"} 
                );

            config.Routes.MapHttpRoute(
                name: "SolveMaze",
                routeTemplate: "api/{controller}/{name}/{algo}",
                defaults: new { controller = "MazeController" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
