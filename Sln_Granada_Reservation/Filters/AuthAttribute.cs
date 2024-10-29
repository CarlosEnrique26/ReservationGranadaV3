using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sln_Granada_Reservation.Filters
{
    public class AuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Verifica si hay un usuario en la sesión
            var user = filterContext.HttpContext.Session["user"];
            var admin = filterContext.HttpContext.Session["admin"];

            // Si no hay usuario o administrador en la sesión, redirige a la página de login
            if (user == null && admin == null)
            {
                filterContext.Result = new RedirectResult("~/Home/Login"); // Cambia la ruta si es necesario
            }

            base.OnActionExecuting(filterContext);
        }
    }
}