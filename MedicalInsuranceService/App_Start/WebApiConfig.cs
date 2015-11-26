using MedicalInsuranceService.Formatter;
using MedicalInsuranceService.LogAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MedicalInsuranceService.Filter;
namespace MedicalInsuranceService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            config.Filters.Add(new ElmahErrorAttribute());
            config.Filters.Add(new DeflateCompressionAttribute());
            config.Formatters.RemoveAt(0);
            config.Formatters.Insert(0,new JilFormatter());
            
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
