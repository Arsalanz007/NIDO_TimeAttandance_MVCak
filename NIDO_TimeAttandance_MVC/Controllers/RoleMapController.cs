using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class RoleMapController : Controller
    {

        public RoleMapController()
        {
            try
            {
                ViewBag.Roles = DataHelper._getRoles();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoleMapController", "Constructor");
            }
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                ModelRoleMap model = new ModelRoleMap();
                var data = await model._getMenu();
                return View(data);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoleMapController", "Index");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> _getMenu(int? RoleID)
        {
            try
            {
                ModelRoleMap model = new ModelRoleMap();
                var data = await model._getMenu(RoleID);
                return PartialView("RoleMapTable", data);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoleMapController", "_getMenu");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Save(Array Menu, int Roleid)
        {
            try
            {
                ModelRoleMap model = new ModelRoleMap();
                if (await model.Role_Update(Menu, Roleid))
                {
                    return Json(new { Status = "Success", Message = "Role has been successfully Mapped", JsonRequestBehavior.AllowGet });
                }
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoleMapController", "Save");
                return Content("Something Goes Wrong");
            }
        }
    }
}