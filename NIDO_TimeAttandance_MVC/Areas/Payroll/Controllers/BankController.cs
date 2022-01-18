using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
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
    public class BankController : Controller
    {
        // GET: Payroll/Bank

        public BankController()
        {

            ViewBag.AccountType = DataHelper._getAccountTypes();
            ViewBag.City = DataHelper._getCity();
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
                var bank = new BankModel();
                var lst = await bank.GetAll();
                return View(lst);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EOBIController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveBank(BankModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SaveBank())
                    {
                        return Json(new { Status = "Success", Message = "Bank has been successfully saved", URL = "/Payroll/Bank/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Bank has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "BankController", "SaveBank");
                return Content("Something Goes Wrong");
            }

        }

        public async Task<ActionResult> DeleteBank(int id)
        {
            try
            {
                var bank = new BankModel();
                bool result = await bank.DeleteBank(id);
                if (result)
                {
                    return Json(new { Status = "Success", Message = "Bank Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Bank has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "BankController", "DeleteBank");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
              //  ViewBag.Categories = BankModel.GetCategories();
                var bank = new BankModel();
                var alModel = await bank.Get_Bank_By_Id(id);
                return View(alModel);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "BankController", "EditBank");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Update(BankModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.EditBank();
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "Bank has been successfully saved", URL = "/Payroll/Bank/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Bank has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "BankController", "UpdateBank");
                return Content("Something Goes Wrong");
            }
        }
    }
}