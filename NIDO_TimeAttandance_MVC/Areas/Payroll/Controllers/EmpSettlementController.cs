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
    public class EmpSettlementController : Controller
    {
        // GET: Payroll/EmpSettlement
        public async Task<ActionResult> Index()
        {
            clsEmployeeProfile model = new clsEmployeeProfile();
            if (bool.Parse(HttpContext.Session["SuperAdmin"].ToString()))
            {
                 
                var empList = await model._GetEmployeePosting();
                var leftEmp = await model._GetLeftEmployee();


                ViewBag.Employees = empList.Where(c => !leftEmp.Contains(c.EmpID)).ToList();
             
                    
                    
            }
            else
            {
                int id = int.Parse(HttpContext.Session["UserId"].ToString());
                var empList = await model._GetEmployeePosting(id);

               
                var leftEmp = await model._GetLeftEmployee();


                ViewBag.Employees = empList.Where(c => !leftEmp.Contains(c.EmpID)).ToList();
            }
            ViewBag.BankList = DataHelper._getBanks();
            ViewBag.ReportsName = DataHelper._getReports();
            return View();

        }

        
        public async Task<ActionResult> Posting(string data)
        {
            try
            {
                

                try
                {
                    var empIds = data.Split('?')[0];
                  //  var currentMonth = data.Split('?')[1];

                    var model = new EmpSettlementModel();


                    if (await model.SaveEmpFinalSettlement(empIds))
                    {
                       return Json(new { Status = "Success", Message = "Employee(s) Settlement has been successfully posted", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Employee(s) Settlement has not been successfully posted.Please try again", JsonRequestBehavior.AllowGet });

                    }
                    // DataTable dt = OldDB.GetAttendaceSummaryReport(empIDs, QueryStringData[1], QueryStringData[2]);
                    //  ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                    return View();
                }
                catch (Exception ex)
                {
                    ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetLeaveReport");
                    return Json(new { Status = "Error", Message = "Employee(s) Settlement has not been successfully posted.Please try again", JsonRequestBehavior.AllowGet });


                    
                }


            }
            catch (Exception ex)
            {

               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmpSettlementController", "Generate");
                return null;
            }

        }


      

    }
}