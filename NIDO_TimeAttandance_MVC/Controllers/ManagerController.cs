using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class ManagerController : Controller
    {
        public ManagerController()
        {
            try
            {
                ViewBag.ManagerLevel = DataHelper._getManagerLevel();
                ViewBag.Employee = DataHelper._getEmployee();

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManagerController", "Constructor");
            }
        }

        public async Task<ActionResult> Create()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                ViewBag.Department = await Task.Run(() => db.DepartmentMasters.OrderBy(x => x.DepartmentDesc).ToList());
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManagerController", "Create");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> SaveManager(Array Department, int LevelID, int EmployeeID, string Title)
        {
            try
            {
                ModelManager m = new ModelManager();
                if (await m.Manager_Save(Department, LevelID, EmployeeID, Title))
                {
                    return Json(new { Status = "Success", Message = Title + " has been successfully posted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = Title + " has been successfully posted", JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManagerController", "SaveManager");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelManager m = new ModelManager();
                var model = await m.GetDeptManager();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManagerController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                ModelManager MM = new ModelManager();
                bool Result = await MM.ManagerDelete(Id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "Manager Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = " Sory this Manager can't be deleted because there are Employees maped on this Company", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManagerController", "Delete");
                return Json(new { Status = "Error", Message = "Something Goes Wrong", JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        public async Task<ActionResult> IfExist(int DepartmentID, int LevelID)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = await db.tbl_Manager.Where(x => x.DepartmentID == DepartmentID && x.LevelID == LevelID).Select(x => x.EmpMaster.EmpName).FirstOrDefaultAsync();
                if (data != null)
                {
                    return Json(new { Status = "Exist", Name = data, JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "NotExist", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManagerController", "IfExist");
                return Content("Something Goes Wrong");
            }
        }
    }
}