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

    public class TrackRequestController : Controller
    {
        public async Task<ActionResult> Index(string id)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = await Task.Run(() => db.Nstp_TrackRequestBy_TrackingID(id).ToList());
                if (data.Count > 0)
                {
                    ViewBag.Track = data;
                    return View();
                }
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "TrackRequestController", "Index");
                return Content("Something Goes Wrong");
            }
        }
    }
}