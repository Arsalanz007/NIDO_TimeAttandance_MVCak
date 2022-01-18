using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class ManagementController : Controller
    {
        public async Task<ActionResult> ListAllRequest()
        {
            try
            {
                ModelManagement model = new ModelManagement();
                var data = await model.GetEmpRequest();
                return View(data);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManagementController", "ListAllRequest");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Approve(string id)
        {
            try
            {
                string[] data = id.Split('|');
                ModelManagement model = new ModelManagement();
                if (await model.ApproveRequest(long.Parse(data[0].ToString()), long.Parse(data[1].ToString()), long.Parse(data[2].ToString()), long.Parse(data[3].ToString())))
                {
                    return Json(new { Status = "Success", Message = "Request has been Successfully Approved", JsonRequestBehavior.AllowGet });
                    //DataHelper.MsgSuccess = "Request has been Successfully Approved";
                    //DataHelper.DivID = "divSuccess";
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Request has not been Successfully Approved", JsonRequestBehavior.AllowGet });
                    //DataHelper.MsgWarning = "Request has not been Successfully Approved";
                    //DataHelper.DivID = "divWarning";
                }
                //DataHelper.Ismsg = true;
                //return Redirect("/Management/ListAllRequest");
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManagementController", "ListAllRequest");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Rejected(string id, string Reason)
        {
            try
            {
                long DID = long.Parse(id);
                long MID = 0;
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var detailData = await db.tbl_PendingRequestDetail.Where(x => x.DID == DID).FirstOrDefaultAsync();
                if (detailData != null)
                {
                    MID = detailData.MID;
                    detailData.IsRejected = true;
                    await db.SaveChangesAsync();
                }
                var detail = await db.tbl_PendingRequestMaster.Where(x => x.ID == MID).Select(x => new { x.RequestID, }).FirstOrDefaultAsync();
                long RequestID = detail.RequestID;
                var masterData = await db.tbl_PendingRequestMaster.Where(x => x.ID == MID).FirstOrDefaultAsync();
                if (masterData != null)
                {
                    masterData.IsRejected = true;
                    await db.SaveChangesAsync();
                }

                var RequestData = await db.tbl_Request.Where(x => x.ID == RequestID).FirstOrDefaultAsync();
                if (RequestData != null)
                {
                    RequestData.IsRejected = true;
                    RequestData.RejectedReason = Reason;
                    RequestData.ReqStatus = "Rejected";
                    await db.SaveChangesAsync();
                }

                #region InsertInNotificationTable               
                DataHelper.InsertNotification("Rejected", RequestData.EmpMaster.EmpName + " ,Your request (" + RequestData.ReqTrackingID + ") has been rejected.", "/TrackRequest/Index/" + RequestData.ReqTrackingID, RequestData.EmpID.Value);
                #endregion
                return Json(new { Status = "Success", Message = "Request has been successfully Rejected", JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManagementController", "Rejected");
                return Json(new { Status = "Error", Message = "Request has not been successfully Rejected", JsonRequestBehavior.AllowGet });

            }
        }

        public async Task<ActionResult> MyTeam()
        {
            try
            {
                ModelManagement model = new ModelManagement();
                var data = await model.getTeamAttendance();

               
                ViewBag.Team = data;
                
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManagementController", "MyTeam");
                return Content("Something Goes Wrong");

            }


        }

        public ActionResult TeamAttendance()
        {

            return View();
        }
        // Created By Moiz
        public async Task<ActionResult> LateAttendace()
        {
            try
            {


                PakOman_NedoEntities db = new PakOman_NedoEntities();
                int id = int.Parse(HttpContext.Session["UserId"].ToString());
                var data = await Task.Run(() => db.Nstp_GetTop5_Team_LateAttendance(id).ToList());
                ViewBag.LateUsers = data;
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManagementController", "LateAttendace");
                return Content("Something Goes Wrong");

            }
        }
        public async Task<ActionResult> GetAnnualStats()
        {
            try
            {


                PakOman_NedoEntities db = new PakOman_NedoEntities();
                int id = int.Parse(HttpContext.Session["UserId"].ToString());
                var LateCount = await Task.Run(() => db.Nstp_Get_Team_Annual_LateAttendance(id).ToList());
                ViewBag.AnnualLate = LateCount;
                return Json(new { Status = "Success", Trend = LateCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManagementController", "GetAnnualStats");
                return Content("Something Goes Wrong");

            }
        }
    }
}