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
    public class AdjustmentController : Controller
    {
        public AdjustmentController()
        {//asdsadsa
            ViewBag.InOutType = DataHelper._getInoutType();
        }
        // GET: Adjustment
        public async Task<ActionResult> Index()
        {
            clsEmployeeProfile model = new clsEmployeeProfile();
            if (bool.Parse(HttpContext.Session["SuperAdmin"].ToString()))
            {
                ViewBag.Employees = await model._GetEmployeeAttendanceLogMaster();
            }
            else
            {
                int id = int.Parse(HttpContext.Session["UserId"].ToString());
                ViewBag.Employees = await model._GetEmployeePosting(id);
            }



            return View();

        }
        [HttpPost]
        public ActionResult Generate(string data)
        {
            try
            {
                DataHelper.QuerystringData = data.Split('?');
                //return RedirectToAction("GetAttendanceLogMaster", "Adjustment");
                return Json(new { Status = "Success", URL = "/Adjustment/GetAttendanceLogMaster" });
            }
            catch (Exception ex)
            {

                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AdjustmentController", "Generate");
                return Json(new { Status = "Error" });
            }

        }

        public async Task<ActionResult> GetAttendanceLogMaster()
        {
            try
            {
                ModelAttendanceLogMaster model = new ModelAttendanceLogMaster();
                string[] QueryStringData = DataHelper.QuerystringData;
                string[] empIDs = QueryStringData[0].Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));
                string from = QueryStringData[1];
                string to = QueryStringData[2];
                var data = await model.GetAttendanceLogMaster(Convert.ToDateTime(from), Convert.ToDateTime(to), empIDS);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                return View(data);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "Generate");
                return Json(new { Status = "Error" });
            }


        }
        [HttpPost]
        public async Task<JsonResult> Update(string[] data)
        {
            try
            {

                data = data.Where(w => w != "-undefined?&undefined").ToArray();

                ModelAttendanceLogMaster model = new ModelAttendanceLogMaster();
                string[] QueryStringData = DataHelper.QuerystringData;

                if (await model.UpdateLogMaster(data))
                {
                    return Json(new { status = "Success", Message = "Records Successfully Updated", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { status = "Error", Message = "Something Goes Wrong", JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AdjustmentController", "GetAttendanceLogMaster");
                return Json(new { status = "Error", Message = "Something Goes Wrong", JsonRequestBehavior.AllowGet });
            }

        }
    }
}