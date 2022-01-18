using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class SystemPolicyController : Controller
    {
        ModelPolicy obj;

        public SystemPolicyController()
        {
            try
            {
                ViewBag.PolicyName = DataHelper.GetPolicyName();
                ViewBag.Leaves = DataHelper.GetAllLeaves();
                obj = new ModelPolicy();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SystemPolicyController", "Constructor");
            }
        }

        public async Task<ActionResult> ListAll()
        {
            try
            {
                var model = await obj.GetPolicy();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SystemPolicyController", "ListALL");
                return View();
            }
        }
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                ModelPolicy MP = new ModelPolicy();
                bool Result = await MP.DeletePolicy(id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "Policy Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Sory this Policy can't be deleted because there are Employees maped on this City", JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SystemPolicyController", "Delete");
                return Json(new { Status = "Error", Message = "Sory this Policy can't be deleted because there are Employees maped on this City", JsonRequestBehavior.AllowGet });
            }
        }
        public ActionResult Create()
        {
            try
            {
                ViewBag.PolicyName = DataHelper.GetPolicyName();
                ViewBag.Leaves = DataHelper.GetAllLeaves();
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SystemPolicyController", "Index");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> CreatePolicy(ModelPolicy model)
        {
            try
            {
                ModelPolicy policy = new ModelPolicy();
                if (await policy.SavePolicy(model))
                {
                    return Json(new { Status = "Success", Message = "Policy(s) has been successfully saved", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Policy Already Exist", JsonRequestBehavior.AllowGet });
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SystemPolicyController", "SavePolicy");
                return Json(new { Status = "Error", Message = "Policy(s) has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
            }
        }
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ModelPolicy data = await obj.GetPolicyByID(id);
                return View(data);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SystemPolicyController", "Edit");
                return View();
            }
        }

        public async Task<ActionResult> Update(ModelPolicy model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.UpdatePolicy())
                    {
                        return Json(new { Status = "Success", Message = "Policy Successfully Updated", JsonRequestBehavior.AllowGet });
                    }
                }
                return Json(new { Status = "Error", Message = "Something Went Wrong", JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "CityController", "Update");
                return Json(new { Status = "Error", Message = "Something Went Wrong", JsonRequestBehavior.AllowGet });
            }
        }

        public async Task<ActionResult> UpdatePolicy(int id)
        {
            bool IsSandwichStatus = false;
            bool IsFifo = false;
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            var data = await db.SysPolicies.FirstOrDefaultAsync();
            if (data != null)
            {
                if (id == 1)
                {
                    if (data.IsSandwichAllowed == true)
                    {
                        IsSandwichStatus = false;
                    }
                    else
                    {
                        IsSandwichStatus = true;
                    }
                    data.IsSandwichAllowed = IsSandwichStatus;
                    await db.SaveChangesAsync();
                    Session["Policies"] = data;
                    return Json(new { status = "success", msg = "Sandwich policy updated", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    if (data.InOutTypeOrFifoBasedAttend == true)
                    {
                        IsFifo = false;
                    }
                    else
                    {
                        IsFifo = true;
                    }
                    data.InOutTypeOrFifoBasedAttend = IsFifo;
                    await db.SaveChangesAsync();
                    Session["Policies"] = data;
                    return Json(new { status = "success", msg = "Fifo policy updated", JsonRequestBehavior.AllowGet });
                }
            }
            else
            {

                return Json(new { status = "error", msg = "failed", JsonRequestBehavior.AllowGet });
            }
        }

    }
}