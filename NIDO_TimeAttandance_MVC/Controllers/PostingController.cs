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
    public class PostingController : Controller
    {
       
        public async Task<ActionResult> Index()
        {
            try
            {
                clsEmployeeProfile model = new clsEmployeeProfile();
                ViewBag.Employees = await model._GetEmployeePosting();
                return View();
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "PostingController", "Index");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> SavePosting(Array EmpId, string DateFrom, string DateTo)
        {
            try
            {
                string SelectedEmp = "";
                foreach (var item in EmpId)
                {
                    int id = int.Parse(item.ToString());
                    SelectedEmp = id + "," + SelectedEmp;
                }
                SelectedEmp = SelectedEmp.TrimEnd(',');
                string abc = "," + SelectedEmp + ",";
                
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 10000;
                int res = await Task.Run(() => db.stp_PostingNew_V2(abc, DateFrom, DateTo,DataHelper.SessionUserName));             
                return Json(new { Status = "Success", Message = "Employee(s) has been successfully posted", JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "PostingController", "SavePosting");               
                return Json(new { Status = "Error", Message = "Employee(s) has not been successfully posted.Please try again", JsonRequestBehavior.AllowGet });
            }
        }
    }
}