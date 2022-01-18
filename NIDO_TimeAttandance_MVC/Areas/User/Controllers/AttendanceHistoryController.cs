using Microsoft.Ajax.Utilities;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Areas.User.Controllers
{
    [DashboardSession]
    public class AttendanceHistoryController : Controller
    {
        public AttendanceHistoryController()
        {
            ViewBag.Navs = Navigation.GetNavigation();

           
        }
        public async Task<ActionResult> Index()
        {
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            int id = int.Parse(HttpContext.Session["UserId"].ToString());
            var dataEmpAttendanceTrend =await Task.Run(()=> db.Nstp_GetYearlyAttendanceDataBy_ID(id).ToList());
            ViewBag.tabular = dataEmpAttendanceTrend;
            TempData["data"] = dataEmpAttendanceTrend;
            return View();
        }

        public ActionResult getEmpdata ()
        {
            try
            {               
                var dataEmpAttendanceTrend = TempData["data"]; 
                ViewBag.tabular = dataEmpAttendanceTrend;
                return Json(new { Status = "Success", Trend = dataEmpAttendanceTrend, JsonRequestBehavior.AllowGet });                
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AttendanceHistoryController", "getEmpdata");
                throw;
            }
        }

    }
}