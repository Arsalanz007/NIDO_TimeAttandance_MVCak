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
    public class CityController : Controller
    {
        
        public CityController()
        {
            try
            {              
                ViewBag.Country = DataHelper._getCountry();                
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "CityController", "Constructor");
            }
        }
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelCity MD = new ModelCity();
                var model =await MD.GetCity();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "CityController", "ListALL");
                return View();
            }

        }

        public ActionResult Create()
        {
            return View();
        }
        public async Task<ActionResult> DeleteCity(int id)
        {
            try
            {
                ModelCity MD = new ModelCity();
                bool Result =await MD.DeleteCity(id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "City Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Sory this City can't be deleted because there are Employees maped on this City", JsonRequestBehavior.AllowGet });
                }
                
            }
            catch (Exception ex)
            {

               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "CityController", "DeleteCity");
                return Json(new { Status = "Error", Message = "Something Went Wrong", JsonRequestBehavior.AllowGet });

               
            }


        }
        [HttpPost]
        public async Task<ActionResult> SaveCity(ModelCity model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.City_Save())
                    {   
                        return Json(new { Status = "Success", Message = "City(s) has been successfully saved",URL= "/City/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {   
                        return Json(new { Status = "Error", Message = "City(s) has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {   
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {

               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "CityController", "SaveCity");
                return View();
            }

        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ModelCity MD = new ModelCity();
                var Result =await MD.Get_City_By_Id(id);
                return View(Result);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "CityController", "Edit");
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Update(ModelCity model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.EditCity())
                    {
                       
                        return Json(new { Status = "Success", Message = "City successfully updated.", URL = "/City/ListAll" });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "City not successfully updated.Please try again" });
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {

               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "CityController", "Update");
                return View();
            }

        }
    }
}