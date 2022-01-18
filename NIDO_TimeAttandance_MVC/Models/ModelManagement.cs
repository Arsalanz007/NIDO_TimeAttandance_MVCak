using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelManagement
    {
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public string EmpImg { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public DateTime? LeaveEndDate { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public TimeSpan? AttendanceTime { get; set; }
        public string Code { get; set; }
        public int? InOutTypeID { get; set; }
        public string LeaveName { get; set; }
        public string Reason { get; set; }
        public long MID { get; set; }
        public long DID { get; set; }
        public long EmpID { get; set; }
        public long RequestID { get; set; }
        public string TrackingID { get; set; }
        public long tblManager_ID { get; set; }
        public int HLevel { get; set; }



        public async Task<IList<ModelManagement>> GetEmpRequest()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    long id = long.Parse(HttpContext.Current.Session["UserId"].ToString());
                    IList<ModelManagement> data = await (from ep in _db.EmpMasters
                                                         join Man in _db.tbl_Manager on ep.DepartmentId equals Man.DepartmentID
                                                         join r in _db.tbl_Request on ep.EmpId equals r.EmpID
                                                         join m in _db.tbl_PendingRequestMaster on r.ID equals m.RequestID
                                                         join d in _db.tbl_PendingRequestDetail on m.ID equals d.MID
                                                         join lev in _db.tbl_ManagerLevel on Man.LevelID equals lev.ID
                                                         where Man.ManagerID == id && lev.LevelCount == d.HLevel && d.ApprovedByThisManager == false && m.IsRejected == false && r.IsCancel == false
                                                         select new ModelManagement
                                                         {
                                                             MID = m.ID,
                                                             EmpName = ep.EmpName,
                                                             EmpCode = ep.EmpCode,
                                                             EmpImg = ep.EmpImg,
                                                             AttendanceDate = r.AttendanceDate,
                                                             AttendanceTime = r.AttendanceTime,
                                                             Code = r.Code.Trim(),
                                                             InOutTypeID = r.InOutTypeID,
                                                             LeaveEndDate = r.LeaveEndDate,
                                                             LeaveName = r.LeaveSetup.LeaveDsc,
                                                             LeaveStartDate = r.LeaveStartDate,
                                                             Reason = r.Reason,
                                                             DID = d.DID,
                                                             EmpID = ep.EmpId,
                                                             RequestID = r.ID,
                                                             TrackingID = r.ReqTrackingID,
                                                         }).Distinct().OrderBy(x => x.Code).ToListAsync();
                    return data;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelManagement", "GetEmpRequest");
                return null;
            }
        }

        public async Task<bool> ApproveRequest(long MID, long DID, long EmpID, long RequestID)
        {
            try
            {
                bool IsOtherManagerExist = false;
                long id = long.Parse(HttpContext.Current.Session["UserId"].ToString());
                PakOman_NedoEntities db = new PakOman_NedoEntities();

                //check k isi managr k pass forward hui wi he ya ni 
                var ExistEntry = await (from ep in db.EmpMasters
                                        join m in db.tbl_Manager on ep.DepartmentId equals m.DepartmentID
                                        join lev in db.tbl_ManagerLevel on m.LevelID equals lev.ID
                                        join d in db.tbl_PendingRequestDetail on m.ID equals d.tblManager_ID
                                        join ma in db.tbl_PendingRequestMaster on d.MID equals ma.ID
                                        where ep.EmpId == EmpID && m.ManagerID != id && ma.RequestID == RequestID
                                        select m.ID).ToListAsync();
                //check requester is in levelcount(managerial level par he ya ni)
                var IsManagerLevel = await db.tbl_Manager.Where(x => x.ManagerID == EmpID).FirstOrDefaultAsync();

                //agar normal Employee he to 
                if (IsManagerLevel != null)
                {
                    int RequesterID = int.Parse(EmpID.ToString());
                    var data = await Task.Run(() => db.Nstp_GetManagers_Manager(RequesterID).ToList());
                    var ManagerExist = data.Where(x => x.ManagerID != id && !ExistEntry.Contains(x.ID)).ToList();
                    if (ManagerExist.Count > 0)
                    {
                        var query = ManagerExist.GroupBy(r => r.ID)
                       .Select(grp => new
                       {
                           ID = grp.Key,
                           Min = grp.Min(t => t.LevelCount),
                       }).FirstOrDefault();
                        tblManager_ID = query.ID;
                        HLevel = query.Min.Value;
                        IsOtherManagerExist = true;
                    }
                }
                else
                {
                    // check manager k kisi aur level ka manager he ta k age forward kre
                    var ManagerDetail = await (from ep in db.EmpMasters
                                               join m in db.tbl_Manager on ep.DepartmentId equals m.DepartmentID
                                               join lev in db.tbl_ManagerLevel on m.LevelID equals lev.ID
                                               where ep.EmpId == EmpID && m.ManagerID != id && !ExistEntry.Contains(m.ID)
                                               select new
                                               {
                                                   m.ID,
                                                   lev.LevelCount,
                                               }).ToListAsync();
                    if (ManagerDetail.Count > 0)
                    {
                        var queryMin = ManagerDetail.GroupBy(r => r.ID)
                                        .Select(grp => new
                                        {
                                            ID = grp.Key,
                                            Min = grp.Min(t => t.LevelCount),
                                        }).FirstOrDefault();

                        tblManager_ID = queryMin.ID;
                        HLevel = queryMin.Min.Value;
                        IsOtherManagerExist = true;
                    }
                }
                //agar aur kisi aur level ka manager hua to usko send kre ga leave
                if (IsOtherManagerExist == true)
                {
                    tbl_PendingRequestDetail objD = new tbl_PendingRequestDetail();
                    objD.tblManager_ID = tblManager_ID;
                    objD.HLevel = HLevel;
                    objD.MID = MID;
                    objD.CreatedDate = DateTime.Now;
                    db.tbl_PendingRequestDetail.Add(objD);
                    db.SaveChanges();
                    // ApprovedThisManager bit true k lye 
                    var data = db.tbl_PendingRequestDetail.Where(x => x.DID == DID).FirstOrDefault();
                    if (data != null)
                    {
                        data.ApprovedByThisManager = true;
                        db.SaveChanges();
                    }

                    #region InsertInNotificationTable
                    var NextManager = db.tbl_Manager.Where(x => x.ID == tblManager_ID).FirstOrDefault();
                    var EmpData = db.tbl_Request.Where(x => x.ID == RequestID).FirstOrDefault();
                    DataHelper.InsertNotification("Approved & Forward", EmpData.EmpMaster.EmpName + " ,There is a approval of your request (" + EmpData.ReqTrackingID + ")", "/TrackRequest/Index/" + EmpData.ReqTrackingID, EmpData.EmpID.Value);
                    DataHelper.InsertNotification("Forward", "Sir " + NextManager.EmpMaster.EmpName + " ,There is a new request of " + EmpData.EmpMaster.EmpName, "/Management/ListAllRequest/", NextManager.ManagerID);
                    #endregion
                }
                // else req ko approved krdy ga 
                else
                {
                    // ApprovedThisManager bit true k lye 
                    var data = db.tbl_PendingRequestDetail.Where(x => x.DID == DID).FirstOrDefault();
                    if (data != null)
                    {
                        data.ApprovedByThisManager = true;
                        db.SaveChanges();
                    }
                    tbl_PendingRequestMaster objM = new tbl_PendingRequestMaster();
                    objM.IsApproved = true;
                    db.SaveChanges();

                    // Approved bit true k lye 
                    var dataR = db.tbl_Request.Where(x => x.ID == RequestID).FirstOrDefault();
                    if (dataR != null)
                    {
                        dataR.IsApproved = true;
                        dataR.ReqStatus = "Approved";
                        db.SaveChanges();
                    }

                    // Save in leaveApproval table
                    if (dataR.Code.Trim() == "L")
                    {
                        LeaveApproval leave = new LeaveApproval();
                        //check balance of specific leave
                        int Balance = db.Database.SqlQuery<int>("select [dbo].[GetLeaveBalance](" + EmpID + "," + dataR.LeaveID + ")").Single();
                        int TotalDays = (int)(dataR.LeaveEndDate.Value - dataR.LeaveStartDate.Value).TotalDays + 1;
                        int addLeaveDay = 0;
                        // agar leave balance km hua to jitna balance hua uthni leave approve hongi
                        if (TotalDays > Balance)
                        {
                            addLeaveDay = Balance - TotalDays;
                        }
                        for (var dt = dataR.LeaveStartDate.Value; dt <= dataR.LeaveEndDate.Value.AddDays(addLeaveDay); dt = dt.AddDays(1))
                        {
                            leave.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                            leave.CreatedOn = DateTime.Now;
                            leave.EmpId = EmpID;
                            leave.IP = HttpContext.Current.Request.UserHostAddress;
                            leave.LeaveDate = dt.Date;
                            leave.LeaveID = dataR.LeaveID;
                            leave.PCName = Environment.MachineName;
                            leave.Remarks = dataR.Reason;
                            leave.TranDate = DateTime.Now;
                            db.LeaveApprovals.Add(leave);
                            db.SaveChanges();
                        }
                        // posting to get the real record 
                        int res = await Task.Run(() => db.stp_PostingNew_V2("," + EmpID + ",", dataR.LeaveStartDate.ToString(), dataR.LeaveEndDate.ToString(), DataHelper.SessionUserName + "- OnLeaveRequestApproved"));
                        //

                    }
                    //agar manual attendance ki request hue to attendacnelogmaster me save kre ga 
                    if (dataR.Code.Trim() == "M")
                    {
                        var ecode = db.EmpMasters.Where(x => x.EmpId == EmpID).FirstOrDefault();
                        DateTime checktime = Convert.ToDateTime(Convert.ToDateTime(dataR.AttendanceDate).ToString("MM/dd/yyy") + " " + dataR.AttendanceTime);
                        AttendanceLogMaster master = new AttendanceLogMaster();
                        master.AcNo = ecode.EmpCode.ToString();
                        master.CheckTime = checktime;
                        master.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                        master.CreatedDate = DateTime.Now;
                        master.InOutTypeId = dataR.InOutTypeID.Value;
                        master.IsManual = true;
                        master.IsProcessed = 0;
                        master.MachineNo = 1;
                        master.Remarks = "Approved by Manager";
                        db.AttendanceLogMasters.Add(master);
                        db.SaveChanges();
                        // posting to get the real record 
                        int res = await Task.Run(() => db.stp_PostingNew_V2("," + EmpID + ",", dataR.AttendanceDate.ToString(), dataR.AttendanceDate.ToString(), DataHelper.SessionUserName + "- OnManualAttendanceRequestApproved"));
                        //
                    }
                    #region InsertInNotificationTable
                    var EmpData = db.tbl_Request.Where(x => x.ID == RequestID).FirstOrDefault();
                    DataHelper.InsertNotification("Approved", EmpData.EmpMaster.EmpName + " ,There is a final approval of your request (" + EmpData.ReqTrackingID + ")", "/TrackRequest/Index/" + EmpData.ReqTrackingID, EmpData.EmpID.Value);
                    #endregion
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelManagement", "ApproveRequest");
                return false;
            }

        }

        public async Task<IList<Nstp_GetTeamBy_ID_Result>> getTeamAttendance()
        {
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            int? id = int.Parse(HttpContext.Current.Session["UserId"].ToString());
            IList<Nstp_GetTeamBy_ID_Result> data = await Task.Run(() => db.Nstp_GetTeamBy_ID(id).ToList());

            return data;

        }

    }
}