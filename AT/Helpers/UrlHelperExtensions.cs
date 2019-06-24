using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;


namespace AT.Helpers
{
    public static class UrlHelperExtensions
    {
        

        public static string QualifiedAction(
            this Microsoft.AspNetCore.Mvc.IUrlHelper url,
            string actionName,
            string controllerName,
            object routeValues = null)
        {
           
            return url.Action(actionName, controllerName,
                routeValues, 
                url.ActionContext.HttpContext.Request.Scheme);
        }

    }
}