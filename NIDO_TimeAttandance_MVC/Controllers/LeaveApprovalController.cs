using Microsoft.Ajax.Utilities;
using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class LeaveApprovalController : Controller
    {
        public async Task<ActionResult> Index(int? EmpId, string FromDate, string ToDate)
        {
            try
            {
                clsEmployeeProfile model = new clsEmployeeProfile();
                ViewBag.Employees = await model._GetEmployeeForLeave();
                if (EmpId != null)
                {
                    var AbsentRecord = await model._getEmpAbsentRecord(EmpId.Value, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                    var query = await Task.Run(() => AbsentRecord.DistinctBy(m => new { m.AttDate, m.DepartmentDesc, m.DesignationDesc, m.EmpCode, m.EmpID, m.EmpName, m.AttDay }).ToList());
                    ViewBag.AbsentRecord = query;
                    ViewBag.LeaveRecord = await Task.Run(() => AbsentRecord.DistinctBy(m => new { m.LeaveID, m.LeaveDsc, m.LeaveAllowed }).ToList());
                    return PartialView("_LeaveApproval", ViewBag.AbsentRecord);
                }

                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveApprovalController", "Index");
                return Content("Something Goes Wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> save(Array EmpId, string Remarks, int LeaveID)
        {
            try
            {
                ModelLeaveApproval model = new ModelLeaveApproval();
                if (await model.Save(EmpId, Remarks, LeaveID))
                {
                    return Json(new { Status = "Success", Message = "Leave(s) successfully updated." });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Leave(s) not successfully updated.Please try again" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveApprovalController", "Save");
                return Content("Something Goes Wrong");
            }
        }


        public async Task<ActionResult> ListAll()
        {
            try
            {
                clsEmployeeProfile model = new clsEmployeeProfile();
                var data = await model.GetListLeaveApproval();
                return View(data);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveApprovalController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Details(int id, string PartialViewName)
        {
            try
            {
                clsEmployeeProfile model = new clsEmployeeProfile();
                var data = await model.GetListLeaveApproval_by_ID(id);
                ViewBag.LeaveApprovalDetails = data;
                return PartialView(PartialViewName, ViewBag.LeaveApprovalDetails);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveApprovalController", "ListAll");
                return Content("Something Goes Wrong");
            }


        }
        public async Task<ActionResult> Delete(Array EmpId)
        {
            try
            {
                ModelLeaveApproval md = new ModelLeaveApproval();
                if (await md.DeleteLeave(EmpId))
                {
                  
                    return Json(new { Status = "Success", Message = "Leave(s) successfully Deleted." });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Leave(s) not successfully Deleted.Please try again" });
                }
            }
            catch (Exception ex)
            {

               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveApprovalController", "Delete");
                return Content("Something Goes Wrong");
            }

        }
    }
}
