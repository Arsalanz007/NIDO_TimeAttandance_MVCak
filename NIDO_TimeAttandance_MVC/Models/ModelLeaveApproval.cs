using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelLeaveApproval
    {
        public long TranNo { get; set; }
        public DateTime TranDate { get; set; }
        public long EmpId { get; set; }
        public int LeaveID { get; set; }
        public DateTime LeaveDate { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public string PCName { get; set; }
        public string IP { get; set; }
        public async Task< bool> Save(Array EmpId, string Remarks, int LeaveID)
        {
            try
            {

                PakOman_NedoEntities db = new PakOman_NedoEntities();
                LeaveApproval ObjleaveApproval = new LeaveApproval();
                var host = Dns.GetHostEntry(Dns.GetHostName());
                string startDate = ((string[])EmpId)[0].Split('|')[1];
                string EndDate = ((string[])EmpId)[EmpId.Length - 1].Split('|')[1];
                string id = "," + ((string[])EmpId)[0].Split('|')[0] + ",";
                var IPv4 = await Task.Run(() => (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList());
                foreach (var emp in EmpId)
                {
                    string[] data = emp.ToString().Split('|');
                    //id = ","+data[0].ToString()+",";
                    ObjleaveApproval.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjleaveApproval.CreatedOn = DateTime.Now;
                    ObjleaveApproval.EmpId = long.Parse(data[0].ToString());
                    ObjleaveApproval.LeaveDate = Convert.ToDateTime(data[1].ToString());
                    ObjleaveApproval.LeaveID = LeaveID;
                    ObjleaveApproval.Remarks = Remarks;
                    ObjleaveApproval.PCName = Environment.MachineName;
                    ObjleaveApproval.IP = IPv4[0].ToString();
                    ObjleaveApproval.TranDate = DateTime.Now;
                    db.LeaveApprovals.Add(ObjleaveApproval);
                   await db.SaveChangesAsync();
                }

                // posting to get the real record 
                var  res =await  Task.Run(()=> db.stp_PostingNew_V2(id, startDate, EndDate, DataHelper.SessionUserName + "- OnLeaveApprovalSave"));
                //
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLeaveApproval", "Save");
                throw;
            }


        }
        public async Task< bool> DeleteLeave(Array Empid)
        {

            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                string startDate = ((string[])Empid)[0].Split('|')[1];
                string EndDate = ((string[])Empid)[Empid.Length - 1].Split('|')[1];
                string ID = "," + ((string[])Empid)[0].Split('|')[0] + ",";
                foreach (var item in Empid)
                {
                    string[] EmpData = item.ToString().Split('|');
                    int id = int.Parse(EmpData[0].ToString());
                    DateTime leaveDate = Convert.ToDateTime(EmpData[1].ToString());
                    var data = await db.LeaveApprovals.Where(m => m.EmpId == id && m.LeaveDate == leaveDate).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        db.LeaveApprovals.Remove(data);
                      await  db.SaveChangesAsync();
                    }
                }
                // posting to get the real record 
                var res =await Task.Run(() => db.stp_PostingNew_V2(ID, startDate, EndDate, DataHelper.SessionUserName + "- OnLeaveApprovalDelete"));
                //
                return true;
            }
            catch (Exception ex)
            {

               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLeaveApproval", "DeleteLeave");
                throw;
            }

        }

    }
}