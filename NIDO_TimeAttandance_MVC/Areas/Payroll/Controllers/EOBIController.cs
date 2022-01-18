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
    public class EOBIController : Controller
    {
        // GET: Payroll/EOBI
        public EOBIController()
        {
            ViewBag.ValueType = DataHelper._getValueTypes();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Categories = EOBIModel.GetCategories();
            return View();
        }
        public async Task<ActionResult> ListAll()
        {
            try
            {
                var eobi = new EOBIModel();
                var lst = await eobi.GetAll();
                return View(lst);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EOBIController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveEOBI(EOBIModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SaveEOBI())
                    {
                        return Json(new { Status = "Success", Message = "EOBI has been successfully saved", URL = "/Payroll/EOBI/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "EOBI has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EOBIController", "SaveEOBI");
                return Content("Something Goes Wrong");
            }

        }
        public async Task<ActionResult> DeleteEOBI(int id)
        {
            try
            {
                EOBIModel am = new EOBIModel();
                bool result = await am.DeleteEOBI(id);
                if (result)
                {
                    return Json(new { Status = "Success", Message = "EOBI Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "EOBI has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EOBIController", "DeleteEOBI");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ViewBag.Categories = EOBIModel.GetCategories();
                EOBIModel am = new EOBIModel();
                var alModel = await am.Get_EOBI_By_Id(id);
                return View(alModel);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EOBIController", "EditEOBI");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Update(EOBIModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.EditEOBI();
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "EOBI has been successfully saved", URL = "/Payroll/EOBI/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "EOBI has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EOBIController", "UpdateEOBI");
                return Content("Something Goes Wrong");
            }
        }
    }
}