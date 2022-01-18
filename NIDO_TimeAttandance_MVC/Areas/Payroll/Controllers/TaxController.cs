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
    public class TaxController : Controller
    {
        // GET: Payroll/Allowances
        public TaxController()
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
                TaxModel am = new TaxModel();
                var lst = await am.GetAll();
                return View(lst);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "TaxController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveTax(TaxModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(await model.SaveTax())
                    {
                        return Json(new { Status = "Success", Message = "Tax has been successfully saved", URL = "/Payroll/Tax/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Tax has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "TaxController", "SaveTax");
                return Content("Something Goes Wrong");
            }

        }
        public async Task<ActionResult> DeleteTax(int id)
        {
            try
            {
                TaxModel am = new TaxModel();
                bool result = await am.DeleteTax(id);
                if (result)
                {
                    return Json(new { Status = "Success", Message = "Tax Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Tax has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "TaxController", "DeleteTax");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                TaxModel am = new TaxModel();
                var alModel = await am.Get_Tax_By_Id(id);
                return View(alModel);
            }
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "TaxController", "EditTax");
                return Content("Something Goes Wrong");
            }
        }
       
        public async Task<ActionResult> Update(TaxModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.EditTax();
                    if(result == true)
                    {
                        return Json(new { Status = "Success", Message = "Tax has been successfully saved", URL = "/Payroll/Tax/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Tax has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "TaxController", "UpdateTax");
                return Content("Something Goes Wrong");
            }
        }
    }
}