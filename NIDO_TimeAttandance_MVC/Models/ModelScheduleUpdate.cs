using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelScheduleUpdate
    {
        public long DID { get; set; }
        public long MID { get; set; }
        public DateTime AttDate { get; set; }
        public string AttDay { get; set; }
        public long ID { get; set; }
        public int GeneralSchedulePKId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long EmpId { get; set; }
        public string CompanyName { get; set; }
        public string CityDesc { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DepartmentDesc { get; set; }
        public string DesignationDesc { get; set; }
        public string RoasterName { get; set; }
        public string GradeDesc { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public long? empidExist { get; set; }

        public async Task<IList<ModelScheduleUpdate>> _GetEmployee()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                IList<ModelScheduleUpdate> data = await (from q in _db.EmpMasters
                                                         join s in _db.EmpScheduleMasters on q.EmpId equals s.EmpId
                                into gj
                                                         from y in gj.DefaultIfEmpty()
                                                         where q.ActiveInActive == false
                                                         orderby q.EmpName ascending
                                                         select new ModelScheduleUpdate
                                                         {
                                                             EmpId = q.EmpId,
                                                             EmpCode = q.EmpCode,
                                                             EmpName = q.EmpName,
                                                             CityDesc = q.CityMaster.CityDesc,
                                                             CompanyName = q.CompanyMaster.CompanyDesc,
                                                             DepartmentDesc = q.DepartmentMaster.DepartmentDesc,
                                                             DesignationDesc = q.DesignationMaster.DesignationDesc,
                                                             GradeDesc = q.GradeMaster.GradeDesc,
                                                             empidExist = y.EmpId
                                                         }).Distinct().ToListAsync();

                if (data != null)
                {
                    return data;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelScheduleUpdate", "_GetEmployee");
                return null;
            }
        }
        public async Task<IList<ModelScheduleUpdate>> _GetEmployee(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                IList<ModelScheduleUpdate> data = await (from q in _db.EmpMasters
                                                         join s in _db.EmpScheduleMasters
                                                         on q.EmpId equals s.EmpId
                                                         orderby q.EmpName ascending
                                                         select new ModelScheduleUpdate
                                                         {
                                                             ID = s.ID,
                                                             EmpId = q.EmpId,
                                                             EmpCode = q.EmpCode,
                                                             EmpName = q.EmpName,
                                                             CityDesc = q.CityMaster.CityDesc,
                                                             CompanyName = q.CompanyMaster.CompanyDesc,
                                                             DepartmentDesc = q.DepartmentMaster.DepartmentDesc,
                                                             DesignationDesc = q.DesignationMaster.DesignationDesc,
                                                             GradeDesc = q.GradeMaster.GradeDesc,
                                                             Startdate = s.StartDate,
                                                             Enddate = s.EndDate,
                                                             RoasterName = s.ShiftScheduleGeneralMaster.ScheduleDesc,
                                                         }).Where(x => x.ID == id).ToListAsync();

                if (data != null)
                {
                    return data;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelScheduleUpdate", "_GetEmployee");
                return null;
            }
        }
        public async Task<bool> Schedule_Save(string[] EmpID, int RoasterID, DateTime Startdate, DateTime EndDate)
        {
            try
            {

                // ArrayList ExistCode = new ArrayList();
                               
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                string Username = CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                var result = String.Join(",", EmpID);
                //Functions.SendProgress(":-  schedule update in progress...", 0, 0, DataHelper.SessionUserName);
                _Db.Database.CommandTimeout = 360000;
                _Db.SaveEmpSchedule(Startdate, EndDate, "," + result + ",", RoasterID, Username);
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelScheduleUpdate", "Schedule_Save");
                return false;
            }

        }
        public async Task<IList<ModelScheduleUpdate>> GetScheduleUpdate()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelScheduleUpdate> List = await (from m in _db.EmpScheduleMasters
                                                             orderby m.EmpMaster.EmpCode
                                                             select new ModelScheduleUpdate
                                                             {
                                                                 ID = m.ID,
                                                                 EmpId = m.EmpId,
                                                                 EmpCode = m.EmpMaster.EmpCode,
                                                                 EmpName = m.EmpMaster.EmpName,
                                                                 RoasterName = m.ShiftScheduleGeneralMaster.ScheduleDesc,
                                                                 Startdate = m.StartDate,
                                                                 Enddate = m.EndDate,

                                                             }).Distinct().ToListAsync();
                    return List;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelScheduleUpdate", "GetScheduleUpdate");
                return null; ;
            }


        }
        public async Task<bool> Schedule_Update(Array EmpID, int RoasterID, DateTime Startdate, DateTime EndDate, long ScheduleID)
        {
            try
            {
                #region Master                              
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                int itemsCount = EmpID.Length;
                int i = 0;
                foreach (var Emp in EmpID)
                {
                    string[] EmpName = Emp.ToString().Split('|');
                    i++;
                    //Thread.Sleep(10);
                    long MID = 0;
                    if (ScheduleID > 0)
                    {
                        var data = await _Db.EmpScheduleMasters.Where(x => x.ID == ScheduleID).FirstOrDefaultAsync();
                        if (data != null)
                        {
                            data.EndDate = EndDate.ToString();
                            data.StartDate = Startdate.ToString();
                            data.GeneralSchedulePKId = RoasterID;
                            _Db.SaveChanges();
                            MID = ScheduleID;
                        }
                    }

                    var DataDelete = await _Db.EmpScheduleDetails.Where(c => c.MID == ScheduleID).ToListAsync();
                    if (DataDelete.Count > 0)
                    {
                        _Db.EmpScheduleDetails.RemoveRange(DataDelete);
                        await _Db.SaveChangesAsync();
                    }
                    //getting shif id
                    var shiftID = await (from q in _Db.ShiftScheduleGeneralMasters join d in _Db.ShiftScheduleGeneralDetails on q.PKId equals d.MID where q.PKId == RoasterID select d.ShiftID).FirstOrDefaultAsync();
                    for (var dt = Startdate; dt <= EndDate; dt = dt.AddDays(1))
                    {
                        EmpScheduleDetail objempD = new EmpScheduleDetail();
                        objempD.AttDate = dt.Date;
                        objempD.AttDay = dt.DayOfWeek.ToString();
                        objempD.MID = MID;
                        objempD.ShiftID = long.Parse(shiftID.ToString());
                        _Db.EmpScheduleDetails.Add(objempD);
                        await _Db.SaveChangesAsync();
                    }
                    Functions.SendProgress(EmpName[1].ToString() + ":-  schedule update in progress...", i, itemsCount, HttpContext.Current.Session["UserName"].ToString());
                }
                #endregion

                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelScheduleUpdate", "Schedule_Update");
                return false;
            }

        }


    }
}