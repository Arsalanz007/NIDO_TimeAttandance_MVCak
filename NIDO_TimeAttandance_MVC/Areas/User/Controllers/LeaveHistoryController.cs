using NIDO_TimeAttandance_MVC.Areas.User.Models;
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
    public class LeaveHistoryController : Controller
    {
        public LeaveHistoryController()
        {
            ViewBag.Navs = Navigation.GetNavigation();
        }
        public async Task<ActionResult> Index()
        {
            int id = int.Parse(HttpContext.Session["UserId"].ToString());
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            var data = await Task.Run(() => db.Nstp_GetLeaveRecord_By_ID(id).ToList());
            if (data.Count > 0)
            {
                ViewBag.LeaveRecord = data;
            }
            return View();
        }

        public async Task<ActionResult> getLeaveData()
        {
            try
            {
                ModelLeaveHistory model = new ModelLeaveHistory();
                var data = await model.getData();
                return Json(new { dt = data });
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveHistoryController", "getLeaveData");
                throw;
            }
        }
        public async Task<ActionResult> getDeductionDetail()
        {
            ModelLeaveHistory model = new ModelLeaveHistory();
            var DeductionData = await model.getDeduction();
            return PartialView("_DeductionDetail", DeductionData);            
        }

    }
}