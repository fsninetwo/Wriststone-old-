using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Wriststone
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start()
        {
            using (Models.WriststoneContext db = new Models.WriststoneContext())
                try
                {
                    //Session["login"] = Request.Cookies["user"].Value;
                    //var login = Session["login"].ToString();
                    //Session["id"] = db.Accounts.Where(e => e.Login.Equals(login)).Single().Id;
                }
                catch (NullReferenceException) { }
        }
    }
}
