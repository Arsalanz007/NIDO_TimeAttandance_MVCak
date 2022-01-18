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
    public class BonusAllocationController : Controller
    {
        // GET: Payroll/BonusAllocation
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                AllocatedBonusModel am = new AllocatedBonusModel();//                DeductionModel dm = new DeductionModel();
                bool result = await am.DeleteAllocation(id);// dm.DeleteDeduction(id);
                if (result == true)
                {
                    return Json(new { Status = "Success", Message = "Bonus Allocation Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Bonus Allocation has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }


            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "BonusAllocatioinController", "DeleteAllocation");
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

            ViewBag.Bonuses = DataHelper._getBonuses();
            return View();
        }

        public async Task<ActionResult> ListAll()
        {
            AllocatedBonusModel aam = new AllocatedBonusModel();
            IList<AllocatedBonusModel> lst = await aam.GetAll();

            return View(lst);
        }

        public async Task<ActionResult> Allocate(string data)
        {
            try
            {
                string[] QueryStringData = data.Split('?');
                AllocatedBonusModel aam = new AllocatedBonusModel();

                var result = await aam.AllocateBonus(QueryStringData[0], Convert.ToDateTime(QueryStringData[1]), Convert.ToDateTime(QueryStringData[2]), Convert.ToInt32(QueryStringData[3]));

                if (result == true)
                {
                    return RedirectToAction("ListAll", "BonusAllocation", new { area = "Payroll" });
                }
                else
                {
                    return RedirectToAction("Select", "BonusAllocation", new { area = "Payroll" });
                    //     return this.Json(new { Status = "Error", Message = "Bonus Allocation unsuccessfull. try again", JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "BonusAllocationController", "Allocate");
                return Content("Something Goes Wrong");
            }
        }
    }
}