using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System.Threading.Tasks;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Controllers
{
    [DashboardSession]
    public class BonusController : Controller
    {
        // GET: Payroll/Bonus
        public BonusController()
        {
            ViewBag.ValueType = DataHelper._getValueTypes();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {


            return View();
        }
        public async Task<ActionResult> ListAll()
        {
            try
            {
                BonusModel am = new BonusModel();
                var lst = await am.GetAll();
                return View(lst);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "BonusController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveBonus(BonusModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SaveBonus())
                    {
                        return Json(new { Status = "Success", Message = "Bonus has been successfully saved", URL = "/Payroll/Bonus/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Bonus has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "BonusController", "SaveBonus");
                return Content("Something Goes Wrong");
            }

        }
        public async Task<ActionResult> DeleteBonus(int id)
        {
            try
            {
                BonusModel am = new BonusModel();
                bool result = await am.DeleteBonus(id);
                if (result)
                {
                    return Json(new { Status = "Success", Message = "Bonus Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Bonus has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "BonusController", "DeleteBonus");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                BonusModel am = new BonusModel();
                var alModel = await am.Get_Bonus_By_Id(id);
                return View(alModel);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "BonusController", "EditBonus");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Update(BonusModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.EditBonus();
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "Bonus has been successfully saved", URL = "/Payroll/Bonus/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Bonus has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "BonusController", "UpdateBonus");
                return Content("Something Goes Wrong");
            }
        }
    }
}