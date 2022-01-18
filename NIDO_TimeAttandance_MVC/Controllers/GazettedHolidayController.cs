using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class GazettedHolidayController : Controller
    {
        public ActionResult CreateNew()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public async Task<ActionResult> DeleteGazettedHoliday(int GazettedHolidayId)
        {
            try
            {
                ModelGazettedHoliday MD = new ModelGazettedHoliday();
                bool Result = await MD.DeleteGazettedHoliday(GazettedHolidayId);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "GazettedHoliday has been successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Gazetted Holiday has not been successfully Deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "GazettedHolidayController", "DeleteGazettedHoliday");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveGazettedHoliday(ModelGazettedHoliday model)
        {
            try
            {
                string Holidays = Request["txtHolidays"];
                if (ModelState.IsValid)
                {
                    if (await model.GazettedHoliday_Save(Holidays))
                    {
                        return Json(new { Status = "Success", Message = "GazettedHoliday has been successfully saved", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Gazetted Holiday has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "GazettedHolidayController", "SaveGazettedHoliday");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<JsonResult> GetHolidays()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Configuration.ProxyCreationEnabled = false;
                var events = await db.GazettedHolidays.ToListAsync();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "GazettedHolidayController", "GetHolidays");
                return Json(new { Status = "Error", Message = "Something Goes Wrong", JsonRequestBehavior.AllowGet });
            }
        }


    }
}