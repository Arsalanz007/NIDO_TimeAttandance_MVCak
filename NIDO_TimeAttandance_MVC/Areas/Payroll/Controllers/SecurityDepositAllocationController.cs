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
    public class SecurityDepositAllocationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                AllocatedSecurityDepositModel am = new AllocatedSecurityDepositModel();//                DeductionModel dm = new DeductionModel();
                bool result = await am.DeleteAllocation(id);// dm.DeleteDeduction(id);
                if (result == true)
                {
                    return Json(new { Status = "Success", Message = "SecurityDeposit Allocation Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "SecurityDeposit Allocation has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }


            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SecurityDepositAllocatioinController", "DeleteAllocation");
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

            ViewBag.SecurityDeposits = DataHelper._getSecurityDeposits();
            return View();
        }

        public async Task<ActionResult> ListAll()
        {
            AllocatedSecurityDepositModel aam = new AllocatedSecurityDepositModel();
            IList<AllocatedSecurityDepositModel> lst = await aam.GetAll();

            return View(lst);
        }

        public async Task<ActionResult> Allocate(string data)
        {
            try
            {
                string[] QueryStringData = data.Split('?');
                AllocatedSecurityDepositModel aam = new AllocatedSecurityDepositModel();

                var result = await aam.AllocateSecurityDeposit(QueryStringData[0], DateTime.Now, DateTime.Now, Convert.ToInt32(QueryStringData[3]));

                if (result == true)
                {
                    return RedirectToAction("ListAll", "SecurityDepositAllocation", new { area = "Payroll" });
                }
                else
                {
                    return RedirectToAction("Select", "SecurityDepositAllocation", new { area = "Payroll" });
                    //     return this.Json(new { Status = "Error", Message = "SecurityDeposit Allocation unsuccessfull. try again", JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SecurityDepositAllocationController", "Allocate");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> OpeningBalanceList()
        {
            try
            {

                var aam = new AllocatedSecurityDepositModel();

                var result = await aam.GetOpeningBalance();

                return View(result);

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SecurityDepositAllocationController", "OpeningBalanceList");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> OpeningBalanceSave(List<SecurityDepositOPModel> model)
        {
            try
            {

                var aam = new AllocatedSecurityDepositModel();

                var result = await aam.SaveOpeningBalance(model);

                if (result)
                {
                    return Json(new { status = "Success", Message = "Opening Balance Successfully Saved", URL = "/Payroll/SecurityDepositAllocation/OpeningBalanceList", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { status = "Error", Message = "Something Goes Wrong", JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SecurityDepositAllocationController", "OpeningBalanceList");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> CustomSecurityDeposit() {

            clsEmployeeProfile model = new clsEmployeeProfile();
            ViewBag.Employees = await model._GetEmployeePosting();
            return View();
        }
        [HttpPost]
        public ActionResult CustomSecurityDeposit(CustomSecurityModel model)
        {


            return View();
        }
    }
}