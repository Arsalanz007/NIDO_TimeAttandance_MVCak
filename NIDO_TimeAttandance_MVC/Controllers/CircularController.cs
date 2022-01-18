using NIDO_TimeAttandance_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class CircularController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> save(ModelCircular model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SaveCircular())
                    {
                        return Json(new { Status = "Success", Message = "Circular has been successfully saved.", URL = "/Circular/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Circular has not been successfully saved.", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Circular", "save");
                return Json(new { Status = "Error", JsonRequestBehavior.AllowGet });
            }
        }

        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelCircular model = new ModelCircular();
                return View(await model.getCircular());
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Circular", "ListAll");
                return View();
            }
        }

        public async Task<ActionResult> Edit(int ID)
        {
            try
            {
                ModelCircular model = new ModelCircular();
                return View(await model.getCircularBy_Id(ID));
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Circular", "Edit");
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> Update(ModelCircular model)
        {
            try
            {
                if (await model.UpdateCircular())
                {
                    return Json(new { Status = "Success", Message = "Circular has been successfully Updated.", URL = "/Circular/ListAll", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Circular has not been successfully Updated.", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Circular", "Update");
                return Json(new { Status = "Error", Message = "Something Goes Wrong.", JsonRequestBehavior.AllowGet });

            }
        }
        public async Task<ActionResult> Delete(int ID)
        {
            try
            {
                ModelCircular MD = new ModelCircular();
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = await db.tbl_Circular.Where(x => x.ID == ID).FirstOrDefaultAsync();
                if (data != null)
                {
                    db.tbl_Circular.Remove(data);
                    await db.SaveChangesAsync();
                    return Json(new { Status = "Success", Message = "Circular Successfully Deleted" });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Circular Cannot be Deleted Deleted" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Circular", "Delete");
                return Json(new { status = "Error", Message = "Something Goes Wrong.", JsonRequestBehavior.AllowGet });

            }
        }

    }
}