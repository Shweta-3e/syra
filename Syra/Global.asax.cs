using Syra.Admin;
using Syra.Admin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Syra
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //private const String ReturnUrlRegexPattern = @"\?ReturnUrl=.*$";
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.Map();
        }
        //private void MvcApplicationOnPreSendRequestHeaders(object sender, EventArgs e)
        //{

        //    String redirectUrl = Response.RedirectLocation;

        //    if (String.IsNullOrEmpty(redirectUrl)
        //         || !Regex.IsMatch(redirectUrl, ReturnUrlRegexPattern))
        //    {

        //        return;

        //    }
        //    Response.RedirectLocation = Regex.Replace(redirectUrl,
        //                                       ReturnUrlRegexPattern,
        //                                       String.Empty);
        //}

            protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET,PUT, POST, OPTIONS,DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }

    }
}
