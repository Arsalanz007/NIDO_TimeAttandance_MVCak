using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class CompanyController : Controller
    {  
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelCompany MD = new ModelCompany();
                var model = await MD.GetCompany();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "CompanyController", "ListAll");
                ViewBag.Error = "Something is wrong.";
                return View(new List<ModelCompany>());
            }
        }

        public ActionResult Create()
        {
            return View();
        }
       
        public async Task<ActionResult> DeleteCompany(int id)
        {
            try
            {
                ModelCompany MD = new ModelCompany();
                bool Result = await MD.DeleteCompany(id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "Comapny Successfully Deleted", JsonRequestBehavior.AllowGet });
                
                }
                else
                {
                    return Json(new { Status = "Error", Message = " Sory this Company can't be deleted because there are Employees maped on this Company", JsonRequestBehavior.AllowGet });
                   
                }
             
              
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "CompanyController", "DeleteCompany");
                return Content("Something Goes Wrong");
            }


        }
        [HttpPost]
        public async Task<ActionResult> SaveCompany(ModelCompany model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.Company_Save())
                    {   
                        return Json(new { Status = "Success", Message = "Company has been successfully saved",URL= "/Company/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {   
                        return Json(new { Status = "Error", Message = "Company has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {  
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {

               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "CompanyController", "SaveCompany");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ModelCompany MD = new ModelCompany();
                var Result = await MD.Get_Company_By_Id(id);
                return View(Result);
            }
            catch (Exception ex)
            {

               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "CompanyController", "Edit");
                return Content("Something Goes Wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Update(ModelCompany model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.EditComapny())
                    {
                       
                        return Json(new { Status = "Success", Message = "Company successfully updated.", URL = "/Company/ListAll" });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Company not successfully updated.Please try again" });
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "CompanyController", "Edit");
                return Content("Something Goes Worng");
            }
        }
    }
}