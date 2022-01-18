using NIDO_TimeAttandance_MVC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using NIDO_TimeAttandance_MVC.Utilities;
using System.Threading.Tasks;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class ScheduleUpdateController : Controller
    {
        public ScheduleUpdateController()
        {
            try
            {
                ViewBag.Roasters = DataHelper._getAllRoaster();

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ScheduleUpdateController", "Constructor");
            }
        }
        public async Task<ActionResult> Index(int? id)
        {
            try
            {
                if (id != null)
                {
                    ModelScheduleUpdate model = new ModelScheduleUpdate();
                    ViewBag.Employees = await model._GetEmployee(int.Parse(id.ToString()));
                }
                else
                {
                    ModelScheduleUpdate model = new ModelScheduleUpdate();
                    ViewBag.Employees = await model._GetEmployee();
                }
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ScheduleUpdateController", "Index");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Save(string[] EmpId, int id, string StartDate, string EndDate, long ScheduleID)
        {
            try
            {
                ModelScheduleUpdate MD = new ModelScheduleUpdate();
                if (ModelState.IsValid)
                {
                    //if (ScheduleID > 0)
                    //{
                    //    if (await MD.Schedule_Update(EmpId, id, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), ScheduleID))
                    //    {
                    //        return Json(new { Status = "Success", Message = "Employee(s) has been successfully updated", JsonRequestBehavior.AllowGet });
                    //    }
                    //}
                    //else
                    //{
                        if (await MD.Schedule_Save(EmpId, id, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate)))
                        {
                            return Json(new { Status = "Success", Message = "Employee(s) has been successfully mapped .", JsonRequestBehavior.AllowGet });
                        }
                        else
                        {

                            return Json(new { Status = "Success", Message = "Employee(s) has been successfully mapped", JsonRequestBehavior.AllowGet });
                        }
                    }
                
                return View();

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ScheduleUpdateController", "Save");
                //return Content("Something Goes Wrong");
                return Json(new { Status = "Error", Message = "Something Goes Wrong", JsonRequestBehavior.AllowGet });
            }
        }
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelScheduleUpdate MD = new ModelScheduleUpdate();
                var model = await MD.GetScheduleUpdate();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ScheduleUpdateController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Delete(int id)
        {

            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    var Data = await Task.Run(() => _db.EmpScheduleDetails.Where(x => x.MID == id).ToList());
                    if (Data.Count > 0)
                    {
                        _db.EmpScheduleDetails.RemoveRange(Data);
                        await _db.SaveChangesAsync();
                    }
                    var MasterData = await Task.Run(() => _db.EmpScheduleMasters.Where(x => x.ID == id).ToList());
                    if (MasterData.Count > 0)
                    {
                        _db.EmpScheduleMasters.RemoveRange(MasterData);
                        await _db.SaveChangesAsync();
                    }
                    return Json(new { Status = "Success", Message = " Data Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ScheduleUpdateController", "ListAll");
                return Json(new { Status = "Error", Message = " Something Went Wrong", JsonRequestBehavior.AllowGet });
            }
        }
    }
}