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
    public class MonitoringController : Controller
    {
        public async Task<ActionResult> Index()
        {
            try
            {
                ModelManagement model = new ModelManagement();
                var data = await model.getTeamAttendance();
                if (data != null)
                {
                    ViewBag.Team = data;
                    return View();
                }
                else
                {
                    DataHelper.IsAnyNoticiation = true;
                    DataHelper.MessageForNotication = "Sory Only Managers are authorized to view his/her team";
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MonitoringController", "Index");
                throw;
            }

        }

    }
}