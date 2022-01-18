using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Controllers
{
    [DashboardSession]
    public class SalaryPostingController : Controller
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SalaryPostingController", "Index");
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
                int res = await Task.Run(() => db.stp_SalaryPosting(abc, DateFrom, DateTo, DataHelper.SessionUserName));
                return Json(new { Status = "Success", Message = "Employee(s) has been successfully posted", JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SalaryPostingController", "SavePosting");
                return Json(new { Status = "Error", Message = "Employee(s) has not been successfully posted.Please try again", JsonRequestBehavior.AllowGet });
            }
        }

        public async Task<ActionResult> SalaryPostingPublish(string DateFrom, string DateTo)
        {
            try
            {
                var model = new SalaryPostingModel();
                if(await model.PublishPosting(DateFrom, DateTo))
                {
                return Json(new { Status = "Success", Message = "Salary Publish has been successfully posted", JsonRequestBehavior.AllowGet });

                }
                else
                {
                    return Json(new { Status = "Error", Message = "Salary Publish has not been successfully posted.Please try again", JsonRequestBehavior.AllowGet });

                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SalaryPostingController", "SavePosting");
                return Json(new { Status = "Error", Message = "Salary Publish has not been successfully posted.Please try again", JsonRequestBehavior.AllowGet });
            }
        }
    }
}