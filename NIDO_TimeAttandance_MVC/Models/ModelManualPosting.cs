using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelManualPosting
    {


        public decimal ID { get; set; }
        public long? ReasonID { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
        public string EmpName { get; set; }
        public string Empcode { get; set; }
        public int CheckType { get; set; }
        public DateTime CheckTime { get; set; }


        public async Task< bool> Save(Array Empid, string time, string Remarks, long ReasonID, int TypeID)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                string startDate = ((string[])Empid)[0].Split('|')[1];
                string EndDate = ((string[])Empid)[Empid.Length - 1].Split('|')[1];
                string SelectedEmp = "";
                foreach (var emp in Empid)
                {                   
                    string[] data = emp.ToString().Split('|');
                    DateTime checktime = Convert.ToDateTime((data[1].ToString() + " " + time).ToString());
                     string id = data[2].ToString();
                    SelectedEmp = id + "," + SelectedEmp;
                    AttendanceLogMaster master = new AttendanceLogMaster();
                    master.AcNo = data[0].ToString();
                    master.CheckTime = checktime;
                    master.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    master.CreatedDate = DateTime.Now;
                    master.InOutTypeId = TypeID;
                    master.IsManual = true;
                    master.IsProcessed = 0;
                    master.MachineNo = 1;
                    master.ReasonID = ReasonID;
                    master.Remarks = Remarks;
                    _db.AttendanceLogMasters.Add(master);
                   await _db.SaveChangesAsync();
                }
                // posting to get the real record 
                SelectedEmp = SelectedEmp.TrimEnd(',');
                string abc = "," + SelectedEmp + ",";
                var a = HttpContext.Current.Session["UserName"].ToString();
                int res = await Task.Run(()=> _db.stp_PostingNew_V2(abc, startDate, EndDate, a + "- OnManualAttendanceSave"));
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelManualPosting", "Save");
                return false;
            }
        }
        public string GetReason(long? id)
        {
            using (var _db = new PakOman_NedoEntities())
            {
                if(id.HasValue && id > 0)
                {
                var reasonObj = _db.tblReasons.Where(c => c.ID == id).FirstOrDefault();
                    return reasonObj != null ? reasonObj.Reason : string.Empty;
                }
                return string.Empty; 
            }
        }

        public async Task < IList<ModelManualPosting>> GetManualEntries()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelManualPosting> List =await (from q in _db.AttendanceLogMasters
                                                      join emp in _db.EmpMasters
                                                      on q.AcNo equals emp.EmpCode
                                                      where q.IsManual.Value == true
                                                      orderby emp.EmgName ascending
                                                      select new ModelManualPosting
                                                      {
                                                          EmpName = emp.EmpName,
                                                          Empcode = emp.EmpCode,
                                                          CheckTime = q.CheckTime,
                                                          ReasonID = q.ReasonID,
                                                          Remarks = q.Remarks,
                                                          ID = q.ID,
                                                          CheckType = q.InOutTypeId,
                                                      }
                                            ).ToListAsync();
                    foreach (var item in List)
                    {
                        item.Reason = GetReason(item.ReasonID);
                    }
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelManualPosting", "GetManualEntries");
                return null; ;
            }


        }
    }
}