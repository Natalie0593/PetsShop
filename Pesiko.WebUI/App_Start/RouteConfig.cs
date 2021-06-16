using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Pesiko.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Выводит первую страницу списка товаров всех категорий
            routes.MapRoute(null,
                "",
                new
                {
                    controller = "Pesiko",
                    action = "List",
                    category = (string)null,
                    page = 1
                }
            );

            //Выводит указанную страницу (в этом случае страницу 2),
            //отображая товары всех категорий
            // / Page2
            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Pesiko", action = "List", category = (string)null },
                constraints: new { page = @"\d+" }
            );


            /////Отображает первую страницу элементов указанной категории
            //(в этом случае игры в разделе "/Симулятор")
            routes.MapRoute(null,
               "{category}",
               new { controller = "Pesiko", action = "List", page = 1 }
           );

            ///Отображает заданную страницу (в этом случае страницу 2)
            ///элементов указанной категории (Симулятор)
            /// /Симулятор/Page2
            routes.MapRoute(null,
                "{category}/Page{page}",
                new { controller = "Pesiko", action = "List" },
                new { page = @"\d+" }
            );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
