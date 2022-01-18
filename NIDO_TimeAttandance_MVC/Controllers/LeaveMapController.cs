using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class LeaveMapController : Controller
    {
        public async Task<ActionResult> Index(int? id)
        {
            try
            {
                clsEmployeeProfile model = new clsEmployeeProfile();
                ModelLeaveMap md = new ModelLeaveMap();
                if (id != null)
                {
                    ViewBag.LeaveType = await md._getLeaveType(id.Value);
                    ViewBag.Employees = await model._GetEmployee(id.Value);
                }
                else
                {
                    ViewBag.LeaveType = await md._getLeaveType();
                    ViewBag.Employees = await model._GetEmployee();
                }
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveMapController", "Index");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelLeaveMap MD = new ModelLeaveMap();
                var model = await MD.GetLeave();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveMapController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveLeave(Array EmpId, Array LeaveDetail, long MID, string connectionId,DateTime StartDate,DateTime EndDate)
        {
            try
            {
                ModelLeaveMap MD = new ModelLeaveMap();
                if (ModelState.IsValid)
                {
                    if (await MD.Leave_Save(EmpId, LeaveDetail, MID,StartDate,EndDate))
                    {
                        if (MID > 0)
                        {
                            return Json(new { Status = "Success", Message = "Leave(s) has been successfully updated", JsonRequestBehavior.AllowGet });
                        }
                        else
                        {
                            return Json(new { Status = "Success", Message = "Leave(s) has been successfully mapped", JsonRequestBehavior.AllowGet });
                        }
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Leave(s) has not been successfully mapped", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveMapController", "SaveLeave");
                return Json(new { Status = "Error", Message = "Something Goes Wrong Exception Happen", JsonRequestBehavior.AllowGet });
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    var Data = await Task.Run(() => _db.LeaveMapDetails.Where(x => x.MID == id).ToList());
                    if (Data.Count > 0)
                    {
                        _db.LeaveMapDetails.RemoveRange(Data);
                        await _db.SaveChangesAsync();
                    }
                    var MasterData = _db.LeaveMapMasters.Where(x => x.ID == id).ToList();
                    if (MasterData.Count > 0)
                    {
                        _db.LeaveMapMasters.RemoveRange(MasterData);
                        await _db.SaveChangesAsync();
                    }
                    return Json(new { Status = "Success", Message = " Data Successfully Deleted", JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveMapController", "Delete");
                return Json(new { Status = "Error", Message = " Something Went Wrong", JsonRequestBehavior.AllowGet });
            }
        }

        public async Task<ActionResult> EnterLeaveDayStatus(int LeaveID, int NoEnter)
        {
            PakOman_NedoEntities db = new PakOman_NedoEntities();
             int res=    await db.LeaveSetups.Where(x => x.ID == LeaveID).Select(x => x.NoOfDays).FirstOrDefaultAsync();
            if (res >= NoEnter )
            {
                return Json(new {status="valid",JsonRequestBehavior.AllowGet });
            }
            else
            {
                return Json(new { status = "Invalid", JsonRequestBehavior.AllowGet });
            }
        }
    }
}