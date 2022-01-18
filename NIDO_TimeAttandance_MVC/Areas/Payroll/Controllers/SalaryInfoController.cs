using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;
using NIDO_TimeAttandance_MVC.Models;


namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Controllers
{
    [DashboardSession]
    public class SalaryInfoController : Controller
    {
        public SalaryInfoController()
        {
            try
            {
                ViewBag.Country = DataHelper._getCountry();
                ViewBag.City = DataHelper._getCity();
                ViewBag.Designation = DataHelper._getDesignation();
                ViewBag.Department = DataHelper._getDepartment();
                ViewBag.MartialStatus = DataHelper._getMartialStatus();
                ViewBag.Company = DataHelper._getCompany();
                ViewBag.Grade = DataHelper._getGrade();
                ViewBag.AllEmployeeType = DataHelper._getEmployeeType();
                ViewBag.Employee = DataHelper._getEmployee();

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SalaryInfoController", "Constructor");
            }
        }
        // GET: Payroll/SalaryInfo
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> SaveSalaryInfo([Bind(Exclude = "MartialStatusId")] ModelEmpProfile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.UpdateEmployeeSalaryInfo())
                    {
                        return Json(new { Status = "Success", Message = "Employee Salary Info has been successfully saved", URL = "/Payroll/SalaryInfo/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Employee Salary Info has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SalaryInfoController", "SaveSalaryInfo");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> EditSalaryInfo(int id)
        {
            try
            {


                ModelEmpProfile MD = new ModelEmpProfile();
                var Result = await MD.Get_Employee_By_Id(id);
                ViewBag.BankAccounts = ModelEmpProfile._getBankAccount();
                DataHelper.Empid = id;
                return View(Result);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "Edit");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> ListAll()
        {
            try
            {


                ModelEmpProfile MD = new ModelEmpProfile();
                var model = await MD.GetEmployeeList();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SalaryInfoController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }

    }
}