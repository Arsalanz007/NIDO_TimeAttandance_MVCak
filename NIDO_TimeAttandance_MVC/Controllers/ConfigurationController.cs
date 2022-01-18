using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class ConfigurationController : Controller
    {
        public ConfigurationController()
        {
            try
            {
                ViewBag.TemplateName = DataHelper._getTemplateName();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "Constructor");
            }
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SaveEmail(ModelConfiguration model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SaveEmailSetting())
                    {
                        return Json(new { Status = "Success", Message = "Settings has been successfully saved",URL= "/Configuration/AllEmailSetting", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Success", Message = "Settings has been successfully saved", JsonRequestBehavior.AllowGet });
                    }
                }
                return View();

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "SaveEmail");
                return Content("Something Goes Wrong");
            }

        }

        public async Task<ActionResult> Template()
        {
            try
            {
                ModelConfiguration model = new ModelConfiguration();
                ViewBag.SampleEmail = await model.GetSampleEmail();
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "Template");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> WishTemplate()
        {
            try
            {
                ModelConfiguration model = new ModelConfiguration();
                ViewBag.SampleEmail = await model.GetWishTemplate();
                model.Discription = ViewBag.SampleEmail;
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "Template");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveTemplate(ModelConfiguration model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SaveTemplate())
                    {
                        return Json(new { Status = "Success", Message = "Template has been successfully saved", URL = "/Configuration/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Template has not been successfully saved", JsonRequestBehavior.AllowGet });
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "SaveTemplate");
                return Content("Something Goes Wrong");
            }


        }

        [HttpPost]
        public async Task<ActionResult> SaveWishTemplate(ModelConfiguration model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SaveWishTemplate())
                    {
                        return Json(new { Status = "Success", Message = "Template has been successfully saved", URL = "/Home/Index", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Template has not been successfully saved", JsonRequestBehavior.AllowGet });
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "SaveTemplate");
                return Content("Something Goes Wrong");
            }


        }

        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelConfiguration model = new ModelConfiguration();
                return View(await model.GetTemplate());
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "ListAll");
                return Content("Something Goes Wrong");
            }

        }
        public async Task<ActionResult> Edit(int id)
        {
            try
            {

                ModelConfiguration model = new ModelConfiguration();
                return View(await model.GetTemplateBy_Id(id));

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "Edit");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Update(ModelConfiguration model)
        {
            try
            {
                if (await model.Update())
                {
                    return Json(new { Status = "Success", Message = "Template has been successfully updated", URL = "/Configuration/ListAll", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Template has not been successfully updated", JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "Update");
                return Content("Something Goes Wrong");
            }

        }

        public async Task<ActionResult> AllEmailSetting()
        {
            try
            {
                ModelConfiguration model = new ModelConfiguration();
                return View(await model.GetEmailSettings());
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "AllEmailSetting");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> DeleteEmail(int ID)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = await db.tbl_Setting.Where(x => x.ID == ID).FirstOrDefaultAsync();
                if (data != null)
                {
                    db.tbl_Setting.Remove(data);
                    await db.SaveChangesAsync();
                    return Json(new { Status = "Success", Message = "Email setting Successfully Deleted", JsonRequestBehavior.AllowGet });

                }
                else
                {
                    return Json(new { Status = "Error", Message = "Email setting has not been Deleted", JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "DeleteEmail");
                return Json(new { Status = "Error", Message = "Something Went Wrong", JsonRequestBehavior.AllowGet });

            }
        }
        public async Task<ActionResult> EditEmail(int id)
        {
            try
            {
                ModelConfiguration model = new ModelConfiguration();
                return View(await model.GetEmailSetting_Id(id));
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "EditEmail");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> UpdateEmail(ModelConfiguration model)
        {
            try
            {
                if (await model.UpdateEmailSetting())
                {
                    return Json(new { Status = "Success", Message = "Email setting has been successfully updated", URL = "/Configuration/AllEmailSetting", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Email setting not been successfully updated", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "UpdateEmailSetting");
                return Json(new { Status = "Error", Message = "Email setting not been successfully updated", JsonRequestBehavior.AllowGet });

            }

        }
        public ActionResult LogoControl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Save(Logo model)
        {
            try
            {
                bool result = model.Save();
                return Json(new { Status = "Success", Message = "Logo has been successfully updated",URL="", JsonRequestBehavior.AllowGet });
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ConfigurationController", "LogoControl");
                return Json(new { Status = "Success", Message = "failed", JsonRequestBehavior.AllowGet });
            }
        }
    }
}