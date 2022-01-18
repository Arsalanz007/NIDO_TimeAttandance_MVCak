using Microsoft.Ajax.Utilities;
using NIDO_TimeAttandance_MVC.Areas.User.Models;

using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using static System.Net.Mime.MediaTypeNames;
using System.Data.OleDb;
using DocumentFormat.OpenXml.Spreadsheet;


namespace NIDO_TimeAttandance_MVC.Areas.User.Controllers
{
    [DashboardSession]
    public class AttendanceController : Controller
    {
        public AttendanceController()
        {
            ViewBag.Navs = Navigation.GetNavigation();

        }
        public async Task<ActionResult> Index()
        {
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            string id = HttpContext.Session["UserId"].ToString();
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            string abc = "," + id + ",";
            var data = await Task.Run(() => db.Nstp_MonthlyAttendance(abc, firstDayOfMonth, lastDayOfMonth).OrderBy(a => a.AttDate).ToList());
            if (data.Count > 0)
            {
                var query = data.DistinctBy(q => new
                {
                    q.EmpId,
                    q.EmpCode,
                    q.EmpName,
                    q.DepartmentDesc,
                    q.Total_Days,
                    q.Weekends,
                    q.Presents,
                    q.Late,
                    q.Overtime,
                    q.HalfDay,
                    q.WorkingDays,
                    q.Gazetted,
                    q.Absent,
                    q.TotalLates,
                    q.late_Deduction,
                    q.WorkingHours,
                    q.DesignationDesc
                }).ToList();
                ViewBag.DataMaster = query;
            }

            return View();
        }
        public async Task<JsonResult> GetUserAttendance()
        {
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            //db.Configuration.ProxyCreationEnabled = false;
            int id = int.Parse(HttpContext.Session["UserId"].ToString());
            var events = await Task.Run(() => db.Nstp_GetAttendanceDataBy_ID(id).ToList());
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult Manual()
        {
            ViewBag.EmpLeave = DataHelper._getEmpLeaveType();
            return View();
        }

        public ActionResult LateDeduct()
        {
            ViewBag.EmpLeave = DataHelper._getEmpLeaveType();

            ViewBag.EmpLates = DataHelper._getLateType();

            return View();
        }

        public async Task<ActionResult> SendLateDeductRequest(ModelAttendance model)
        {
            try
            {

                model.InOutTypeID = int.Parse(Request["txtIn_Out"]);
                model.AttendanceTime = Convert.ToDateTime(Request["txttime"]).TimeOfDay;
                model.Code = "D";


                if (await model.SaveManaualRequest())
                {

                    return Json(new { Status = "Success", Message = "Request has been successfully Initiated, you can easily check your status by this tracking ID:" + model.ReqTrackingID, JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Request has not been successfully saved", JsonRequestBehavior.AllowGet });
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "403")
                {
                    return Json(new { Status = "Error", Message = "Request Already Exist", JsonRequestBehavior.AllowGet });
                }
                else if (ex.Message == "500")
                {
                    return Json(new { Status = "Error", Message = "Cannot perform this task, Becuase Leave are not assigned", JsonRequestBehavior.AllowGet });
                }
                return Json(new { Status = "Error", Message = "Something went Wrong", JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AttendanceController", "SendRequestManual");
                throw;
            }
        }

        public async Task<ActionResult> SendRequestManual(ModelAttendance model)
        {
            try
            {
                if (Request["txtIn_Out"] != null)
                {
                    model.InOutTypeID = int.Parse(Request["txtIn_Out"]);
                    model.AttendanceTime = Convert.ToDateTime(Request["txttime"]).TimeOfDay;
                    model.Code = "M";
                }

                if (await model.SaveManaualRequest())
                {

                    return Json(new { Status = "Success", Message = "Request has been successfully Initiated, you can easily check your status by this tracking ID:" + model.ReqTrackingID, JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Request has not been successfully saved", JsonRequestBehavior.AllowGet });
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "403")
                {
                    return Json(new { Status = "Error", Message = "Request Already Exist", JsonRequestBehavior.AllowGet });
                }
                else if (ex.Message == "500")
                {
                    return Json(new { Status = "Error", Message = "Cannot perform this task, Becuase Leave are not assigned", JsonRequestBehavior.AllowGet });
                }
                return Json(new { Status = "Error", Message = "Something went Wrong", JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AttendanceController", "SendRequestManual");
                throw;
            }
        }



        public async Task<ActionResult> ListAll()
        {
            try
            {

                ModelAttendance MD = new ModelAttendance();
                var model = await MD.GetManualRequest();
                return View(model);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AttendanceController", "ListALL");
                return View();
            }

        }

        public async Task<ActionResult> Import()
        {
            return View();
        }

        public async Task<ActionResult> ImportAttendance()
        {
            return View();
        }

        public async Task<ActionResult> ImportLeaves()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaveExcelAttendanceData(HttpPostedFileBase FileUpload)
        {
            try
            {
                var model = new ModelAttendance();
                var list = await model.SaveAttendanceDataExcel(FileUpload);
                ViewBag.Error = null;
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "SomeThing Goes Wrong Please check your file template";
                return View(new ImportExcelResult());
            }

        }

        [HttpPost]
        public async Task<ActionResult> SaveExcelLeaveData(HttpPostedFileBase FileUpload)
        {
            try
            {
                var model = new ModelAttendance();
                var list = await model.SaveExcelLeaveData(FileUpload);
                ViewBag.Error = null;
                return View(list);
            }
            catch (Exception ex)    
            {
                ViewBag.Error = "SomeThing Goes Wrong Please check your file template";
                return View(new ImportExcelResult());
            }

        }

        public async Task<ActionResult> ImportApprovedLeaves()
        {
            return View();
        }
        public async Task<ActionResult> ApprovedLeavesResult()
        {
            return View();
        }
        public async Task<ActionResult> SaveApprovedLeaves(HttpPostedFileBase FileUpload)
        {
            try
            {
                var model = new ModelAttendance();
                var list = await model.SaveExcelLeaveApproveData(FileUpload);
                ViewBag.Error = null;
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "SomeThing Goes Wrong Please check your file template";
                return View(new ImportExcelResult());
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveExcelData(HttpPostedFileBase FileUpload)
        {
            try
            {
                var model = new ModelAttendance();
                var list = await model.ImportExcel(FileUpload);
                ViewBag.Error = null;
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "SomeThing Goes Wrong Please check your file template";
                return View(new ImportExcelResult());
            }

        }
        public async Task<ActionResult> CancelRequest(long id)
        {
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            var data = await db.tbl_Request.Where(m => m.ID == id && m.IsApproved == false && m.IsRejected == false).FirstOrDefaultAsync();
            var ManagerDetail = await (from m in db.tbl_PendingRequestMaster
                                       join d in db.tbl_PendingRequestDetail on m.ID equals d.MID
                                       join r in db.tbl_Request on m.RequestID equals r.ID
                                       where d.ApprovedByThisManager == false && r.ID == id
                                       select d).FirstOrDefaultAsync();
            if (ManagerDetail != null)
            {
                ManagerDetail.IsCancelByUser = true;
                ManagerDetail.CreatedDate = DateTime.Now;
                await db.SaveChangesAsync();
            }
            if (data != null)
            {
                data.IsCancel = true;
                data.ReqStatus = "Cancelled";
                await db.SaveChangesAsync();
                #region InsertInNotificationTable
                if (ManagerDetail != null)
                {
                    var ManagerID = await db.tbl_Manager.Where(x => x.ID == ManagerDetail.tblManager_ID).FirstOrDefaultAsync();
                    if (ManagerID != null)
                    {
                        DataHelper.InsertNotification("Request Cancelled", "(" + data.ReqTrackingID + ") This request has been cancelled by User itself.", "/TrackRequest/Index/" + data.ReqTrackingID, ManagerID.ManagerID);
                        if (ManagerID.EmpMaster.EmailAdd != null || ManagerID.EmpMaster.EmailAdd != "")
                        {
                            EmailSending.SendEmail("Request Has been cancelled", "(" + data.ReqTrackingID + ") This request has been cancelled by User itself.", 4, ManagerID.EmpMaster.EmailAdd);
                        }
                    }
                }

                #endregion
                return Json(new { Status = "Success", Message = "Request hase been Successfully cancelled", JsonRequestBehavior.AllowGet });

            }
            else
            {
                return Json(new { Status = "Error", Message = " Request hase not been Successfully cancelled", JsonRequestBehavior.AllowGet });



            }

        }

        public async Task<ActionResult> AllRequest()
        {
            try
            {
                ModelAttendance MD = new ModelAttendance();
                var model = await MD.GetAllRequest();
                return View(model);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AttendanceController", "AllRequest");
                return View();
            }
        }
    }
}