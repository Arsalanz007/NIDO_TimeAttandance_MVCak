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
    public class ProvidentFundAllocationController : Controller
    {
        // GET: Payroll/ProvidentFundAllocation
        public ActionResult Index()
        {
            return View();
        }
       

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                AllocatedProvidentFundModel am = new AllocatedProvidentFundModel();//                DeductionModel dm = new DeductionModel();
                bool result = await am.DeleteAllocation(id);// dm.DeleteDeduction(id);
                if (result == true)
                {
                    return Json(new { Status = "Success", Message = "Provident Fund Allocation Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Provident Fund Allocation has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }


            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ProvidentFundAllocatioinController", "DeleteAllocation");
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

            ViewBag.ProvidentFund = DataHelper._getProvidentFund();
            return View();
        }

        public async Task<ActionResult> ListAll()
        {
            var aam = new AllocatedProvidentFundModel();
            IList<AllocatedProvidentFundModel> lst = await aam.GetAll();

            return View(lst);
        }

        public async Task<ActionResult> Allocate(string data)
        {
            try
            {
                string[] QueryStringData = data.Split('?');
                var aam = new AllocatedProvidentFundModel();

                //  var result = await aam.AllocateEOBI(QueryStringData[0], Convert.ToDateTime(QueryStringData[1]), Convert.ToDateTime(QueryStringData[2]), Convert.ToInt32(QueryStringData[3]));
                var result = await aam.AllocateProvidentFund(QueryStringData[0], Convert.ToInt32(QueryStringData[1]));



                if (result == true)
                {
                    return RedirectToAction("ListAll", "ProvidentFundAllocation", new { area = "Payroll" });
                }
                else
                {
                    return RedirectToAction("Select", "ProvidentFundAllocation", new { area = "Payroll" });
                    //     return this.Json(new { Status = "Error", Message = "Allowance Allocation unsuccessfull. try again", JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ProvidentFundAllocationController", "Allocate");
                return Content("Something Goes Wrong");
            }
        }
    }
}