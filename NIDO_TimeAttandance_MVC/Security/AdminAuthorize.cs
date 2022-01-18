using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC
{
    public class AdminAuthorize
    {
        public class AdminAuthorizeAttribute : ActionFilterAttribute
        {
            public static IList<tblMenu> NavLists { get; set; }

            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (HttpContext.Current.Session["IsLoggedIn"] != null && HttpContext.Current.Session["RoleID"] != null)
                {
                    if ((bool)HttpContext.Current.Session["IsLoggedIn"])
                    {                       
                        Navigation.NavList = Navigation.GetNavigation();
                        base.OnActionExecuting(filterContext);
                    }

                    else
                        filterContext.Result = new RedirectResult("~/Login/Index");
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Login/Index");
                }


            }

        }

        //Check specific page rights
        public class MethodsRightsChecks : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (bool.Parse(HttpContext.Current.Session["SuperAdmin"].ToString()) == true && Convert.ToInt32(HttpContext.Current.Session["UserId"]) > 0)
                {
                    base.OnActionExecuting(filterContext);

                }
                else if (Convert.ToInt32(HttpContext.Current.Session["UserId"]) > 0)
                {
                    //check if certain user has rights to access the page 

                    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    string methodName = filterContext.ActionDescriptor.ActionName;
                    if (Navigation.CheckSpecificRights(Convert.ToInt32(HttpContext.Current.Session["RoleID"]), controllerName, methodName))
                    {
                        base.OnActionExecuting(filterContext);
                    }
                    else
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            var result = new JsonResult();
                            result.Data = (new { message = "You Dont have rights", type = 1 });
                            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                            filterContext.Result = result;
                        }
                        else
                        {
                            filterContext.Result = new RedirectResult("/Error/Error_404/");
                        }

                    }
                }




            }

             

        }


    }
}