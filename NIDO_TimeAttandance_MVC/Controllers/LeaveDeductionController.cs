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
    public class LeaveDeductionController : Controller
    {

        public async Task<ActionResult> Index()
        {
            try
            {
                clsEmployeeProfile model = new clsEmployeeProfile();
                if (bool.Parse(HttpContext.Session["SuperAdmin"].ToString()))
                {
                    ViewBag.Employees = await model._GetEmployeePosting();
                }
                else
                {
                    int id = int.Parse(HttpContext.Session["UserId"].ToString());
                    ViewBag.Employees = await model._GetEmployeePosting(id);
                }
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "Index");
                return Content("Something Goes Wrong ..");
            }
        }

        [HttpPost]
        public async Task<ActionResult> startDeduction(Array EmpID, DateTime fromDate)
        {
            string Month = fromDate.Month.ToString();
            string SelectedEmp = "";
            string EmployeeID = "";
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            foreach (var item in EmpID)
            {
                int id = int.Parse(item.ToString());
                SelectedEmp = id + "," + SelectedEmp;
            }
            SelectedEmp = SelectedEmp.TrimEnd(',');
            EmployeeID = "," + SelectedEmp + ",";

            await Task.Run(() => db.Nstp_Deduction(EmployeeID, Month));

            return Json(new { status = "success", Message = "Deduction(s) has been successfully executed" });

        }
    }
}