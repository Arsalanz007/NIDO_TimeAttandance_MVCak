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
    public class AllowanceAllocationController : Controller
    {
        // GET: Payroll/AllowanceAllocation
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                AllocatedAllowanceModel am = new AllocatedAllowanceModel();//                DeductionModel dm = new DeductionModel();
                bool result = await am.DeleteAllocation(id);// dm.DeleteDeduction(id);
                if (result == true)
                {
                    return Json(new { Status = "Success", Message = "Allowance Allocation Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Allowance Allocation has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }


            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AllowanceAllocatioinController", "DeleteAllocation");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Select()
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
            
            ViewBag.Allowances = DataHelper._getAllowances();
            return View();
        }
      
        public async Task<ActionResult> ListAll()
        {
            AllocatedAllowanceModel aam = new AllocatedAllowanceModel();
            IList<AllocatedAllowanceModel> lst = await aam.GetAll();

            return View(lst);
        }
       
        public async Task<ActionResult> Allocate(string data)
        {
            try
            {
                string []QueryStringData = data.Split('?');
                AllocatedAllowanceModel aam = new AllocatedAllowanceModel();
              bool  check ;
                if (QueryStringData[QueryStringData.Length - 1] == "undefined")
                {
                    var result = await aam.AllocateAllowance(QueryStringData[0], Convert.ToDateTime(QueryStringData[1]), Convert.ToDateTime(QueryStringData[2]), Convert.ToInt32(QueryStringData[3]),null);
                    check = result;
                }
                else {

                    var result = await aam.AllocateAllowance(QueryStringData[0], Convert.ToDateTime(QueryStringData[1]), Convert.ToDateTime(QueryStringData[2]), Convert.ToInt32(QueryStringData[3]),QueryStringData[QueryStringData.Length-1]);
                    check = result;
                }
                if (check == true)
                {
                    return RedirectToAction("ListAll", "AllowanceAllocation", new { area = "Payroll" });
                }
               else
                {
                    return RedirectToAction("Select", "AllowanceAllocation", new { area = "Payroll" });
                    //     return this.Json(new { Status = "Error", Message = "Allowance Allocation unsuccessfull. try again", JsonRequestBehavior.AllowGet });
                }
                
            }
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AllowanceAllocationController", "Allocate");
                return Content("Something Goes Wrong");
            }
        }
    }
}