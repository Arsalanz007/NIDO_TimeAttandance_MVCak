using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelLeaveMap
    {
        public long ID { get; set; }
        public long EmpID { get; set; }
        public int Year { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string EmployeeName { get; set; }
        public string Empcode { get; set; }
        public long DID { get; set; }
        public long MID { get; set; }
        public long LeaveID { get; set; }
        public int LeaveAllowed { get; set; }
        public string LeaveDsc { get; set; }
        public string LeaveCode { get; set; }
        public string[] LeaveId { get; set; }
        public string checked_Status { get; set; }

        public async Task<IList<ModelLeaveMap>> GetLeave()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelLeaveMap> List = await (from m in _db.LeaveMapMasters
                                                       join d in _db.LeaveMapDetails on m.ID equals d.MID
                                                       select new ModelLeaveMap
                                                       {
                                                           ID = m.ID,
                                                           Year = m.Year,
                                                           EmployeeName = m.EmpMaster.EmpName,
                                                           Empcode = m.EmpMaster.EmpCode,
                                                           LeaveCode = m.LeaveCodes,
                                                       }
                                            ).Distinct().ToListAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLeaveMap", "GetLeave");
                return null; ;
            }


        }

        public async Task<bool> Leave_Save(Array EmpID, Array LeaveDetail, long ExistMid,DateTime StartDate , DateTime EndDate)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                //get all leave with code
                IList<LeaveSetup> LeaveCodeWithID = await _db.LeaveSetups.ToListAsync();
                //
                int itemsCount = EmpID.Length;
                int i = 0;
                foreach (var emp in EmpID)
                {
                    string[] EmpName = emp.ToString().Split('|');
                    i++;
                    string LeaveCodes = "";
                    bool isExist = false;
                    // jb MID exist krti hue to update krne aye ga 
                    if (ExistMid > 0)
                    {
                        MID = ExistMid;
                        var dataDelete = await Task.Run(() => _db.LeaveMapDetails.Where(m => m.MID == ExistMid).ToList());
                        if (dataDelete.Count > 0)
                        {
                            _db.LeaveMapDetails.RemoveRange(dataDelete);
                            await _db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        // agar current year ki leaves maped hue wi he to again save ni kre ga.
                        long emp_Id = long.Parse(EmpName[0].ToString());
                        var IsExist = await (from m in _db.LeaveMapMasters
                                             join d in _db.LeaveMapDetails on m.ID equals d.MID
                                             where m.EmpID == emp_Id && m.Year == DateTime.Now.Year
                                             select m).ToListAsync();
                        if (IsExist.Count > 0)
                        {
                            isExist = true;
                        }
                        else
                        {
                            LeaveMapMaster leaveMap = new LeaveMapMaster();
                            leaveMap.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                            leaveMap.CreatedDate = DateTime.Now.ToString();
                            leaveMap.EmpID = long.Parse(EmpName[0].ToString());
                            leaveMap.Year = DateTime.Now.Year;
                            leaveMap.StartDate = StartDate;
                            leaveMap.EndDate = EndDate;

                            _db.LeaveMapMasters.Add(leaveMap);
                            await _db.SaveChangesAsync();
                            MID = leaveMap.ID;
                        }
                    }

                    if (isExist == false)
                    {
                        foreach (var leave in LeaveDetail)
                        {
                            LeaveMapDetail leaveMapDetail = new LeaveMapDetail();
                            string[] Leavearry = leave.ToString().Split('_');
                            long LeaveID = long.Parse(Leavearry[1].ToString());
                            var code = LeaveCodeWithID.Where(m => m.ID == LeaveID).Select(m => m.LeaveCode).FirstOrDefault();
                            LeaveCodes += " " + code;
                            leaveMapDetail.LeaveAllowed = double.Parse(Leavearry[0].ToString());
                            leaveMapDetail.LeaveID = long.Parse(Leavearry[1].ToString());
                            leaveMapDetail.MID = MID;
                            _db.LeaveMapDetails.Add(leaveMapDetail);
                            await _db.SaveChangesAsync();
                        }
                        var data = await _db.LeaveMapMasters.Where(m => m.ID == MID).FirstOrDefaultAsync();
                        if (data != null)
                        {
                            data.LeaveCodes = LeaveCodes;
                            await _db.SaveChangesAsync();
                        }
                    }
                    //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
                    Functions.SendProgress(EmpName[1].ToString() + ":-  Leave's is mapping in progress...", i, itemsCount, DataHelper.SessionUserName);
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLeaveMap", "Leave_Save");
                return false;
            }
        }

        public async Task<IList<stp_GetLeaveMapStatus_by_MID_Result>> _getLeaveType(int id)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var items = await Task.Run(() => _db.stp_GetLeaveMapStatus_by_MID(id, 0, "", "", "").ToList());
            return items;
        }

        public async Task<IList<ModelLeaveMap>> _getLeaveType()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            IList<ModelLeaveMap> items = await (from q in _db.LeaveSetups
                                                select new ModelLeaveMap
                                                {
                                                    LeaveDsc = q.LeaveDsc,
                                                    MID = 0,
                                                    ID = q.ID,
                                                    checked_Status = "",
                                                    LeaveAllowed = q.NoOfDays,
                                                }).ToListAsync();

            return items;

        }



    }
}