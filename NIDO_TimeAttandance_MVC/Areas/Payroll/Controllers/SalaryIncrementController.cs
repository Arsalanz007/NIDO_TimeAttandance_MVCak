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
    public class SalaryIncrementController : Controller
    {
        // GET: Payroll/SalaryIncrement
        public ActionResult Index()
        {
            return View();
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }


        public async Task<ActionResult> Increment(long empId)
        {
            var db = new PakOman_NedoEntities();

            var model = new SalaryIncrementModel();
            ViewBag.Salarylogs = await model.GetSalaryHistory(empId);
            ViewBag.EmployeeInfo = await model.GetEmployeeInfo(empId);
            var basicSalary = await model.GetBasicSalary(empId);

            ViewBag.Designation = DataHelper._getDesignation();
            ViewBag.Department = DataHelper._getDepartment();
            return View(new SalaryIncrementModel { Id = 0,EmpId = empId , BasicSalary = basicSalary });
        }

        public async Task<ActionResult> Save_Increment(SalaryIncrementModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SaveIncrement())
                    {
                        return Json(new { Status = "Success", Message = "Increment has been successfully saved", URL = "/Payroll/SalaryIncrement/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Increment has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                       
                        }
                    }

                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "BankController", "SaveBank");
                return Content("Something Goes Wrong");
            }


        }



        public ActionResult Delete_Increment(int id)  {

            try
            {
                if (new SalaryIncrementModel().Increment_Delete(id))
                {
                    return Json(new { Status = "Success", Message = "Increment has been Deleted  ", URL = "/Payroll/SalaryIncrement/ListAll", JsonRequestBehavior.AllowGet });
                }
                else {

                    return Json(new { Status = "Error", Message = "Increment has not been successfully Deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {

                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SalaryIncrementController", "Delete_increment");
                return RedirectToAction("ListAll");
            }
        
        
        
        }

    }
}