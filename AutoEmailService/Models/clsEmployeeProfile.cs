using AutoEmailService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace AutoEmailService.Models
{
    public class clsEmployeeProfile
    {
        #region Properties
        public long EmpID { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string CityDesc { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentDesc { get; set; }
        public string DesignationDesc { get; set; }
        public string GradeDesc { get; set; }
        public string EmployeeType { get; set; }
        public long? empidExist { get; set; }
        public string Missing_date { get; set; }
        public DateTime? Time_Enter { get; set; }
        public DateTime? AttDate { get; set; }
        public int LeaveID { get; set; }
        public int LeaveAllowed { get; set; }
        public string LeaveDsc { get; set; }
        public string AttDay { get; set; }
        public string EmpImg { get; set; }
        public TimeSpan? TimeIn { get; set; }
        public int NoOflates { get; set; }
        public int NoOfAbsents { get; set; }
        public int NoOfHalfDay { get; set; }

        #endregion


        #region _getEmployees for LeaveMap
        public async Task<IList<clsEmployeeProfile>> _GetEmployee()
        {
            try
            {

                NedoDb _db = new NedoDb();
                IList<clsEmployeeProfile> data = await (from q in _db.EmpMasters
                                                        join et in _db.EmployeeTypes
                                                        on q.EmployeeTypeID equals et.ID
                                                        join m in _db.LeaveMapMasters on q.EmpId equals m.EmpID
                                                        into gj
                                                        from y in gj.DefaultIfEmpty()
                                                        where q.ActiveInActive == false
                                                        orderby q.EmpName ascending
                                                        select new clsEmployeeProfile
                                                        {
                                                            EmpID = q.EmpId,
                                                            EmpCode = q.EmpCode,
                                                            EmpName = q.EmpName,
                                                            CityDesc = q.CityMaster.CityDesc,
                                                            CompanyName = q.CompanyMaster.CompanyDesc,
                                                            DepartmentDesc = q.DepartmentMaster.DepartmentDesc,
                                                            DesignationDesc = q.DesignationMaster.DesignationDesc,
                                                            GradeDesc = q.GradeMaster.GradeDesc,
                                                            EmployeeType = et.EmployeeTypeDsc,
                                                            empidExist = y.EmpID,
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "clsEmployeeProfile", "_GetEmployee");
                return null;
            }
        }
        public async Task<IList<clsEmployeeProfile>> _GetEmployee(int id)
        {
            try
            {

                NedoDb _db = new NedoDb();
                IList<clsEmployeeProfile> data = await (from q in _db.EmpMasters
                                                        join et in _db.EmployeeTypes
                                                        on q.EmployeeTypeID equals et.ID
                                                        join m in _db.LeaveMapMasters on q.EmpId equals m.EmpID
                                                        join d in _db.LeaveMapDetails on m.ID equals d.MID
                                                        where q.ActiveInActive == false && m.ID == id
                                                        orderby q.EmpName ascending
                                                        select new clsEmployeeProfile
                                                        {
                                                            EmpID = q.EmpId,
                                                            EmpCode = q.EmpCode,
                                                            EmpName = q.EmpName,
                                                            CityDesc = q.CityMaster.CityDesc,
                                                            CompanyName = q.CompanyMaster.CompanyDesc,
                                                            DepartmentDesc = q.DepartmentMaster.DepartmentDesc,
                                                            DesignationDesc = q.DesignationMaster.DesignationDesc,
                                                            GradeDesc = q.GradeMaster.GradeDesc,
                                                            EmployeeType = et.EmployeeTypeDsc
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "clsEmployeeProfile", "_GetEmployee");
                return null;
            }
        }
        #endregion

        #region _getEmployees for Posting
        public async Task<IList<clsEmployeeProfile>> _GetEmployeePosting()
        {
            try
            {
                NedoDb _db = new NedoDb();
                IList<clsEmployeeProfile> data = await (from q in _db.EmpMasters
                                                        join et in _db.EmployeeTypes
                                                        on q.EmployeeTypeID equals et.ID
                                                        
                                                        where q.ActiveInActive == false
                                                        orderby q.EmpName ascending
                                                        select new clsEmployeeProfile
                                                        {
                                                            EmpID = q.EmpId,
                                                            EmpCode = q.EmpCode,
                                                            EmpName = q.EmpName,
                                                            CompanyName = q.CompanyMaster.CompanyDesc,
                                                            DepartmentDesc = q.DepartmentMaster.DepartmentDesc,
                                                            DesignationDesc = q.DesignationMaster.DesignationDesc,
                                                            EmployeeType = et.EmployeeTypeDsc,
                                                          
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "clsEmployeeProfile", "_GetEmployeePosting");
                return null;
            }
        }
        public async Task<List<clsEmployeeProfile>> _GetEmployeePosting(long ID)
        {
            try
            {
                NedoDb db = new NedoDb();
                var IsManager = await db.tbl_Manager.Where(x => x.ManagerID == ID).FirstOrDefaultAsync();
                List<clsEmployeeProfile> data = new List<clsEmployeeProfile>();
                if (IsManager != null)
                {
                    data = await (from M in db.tbl_Manager
                                  join EP in db.EmpMasters on M.DepartmentID equals EP.DepartmentId
                                  join et in db.EmployeeTypes on EP.EmployeeTypeID equals et.ID
                                  where EP.ActiveInActive == false && M.ManagerID == ID
                                  orderby EP.EmpName ascending
                                  select new clsEmployeeProfile
                                  {
                                      EmpID = EP.EmpId,
                                      EmpCode = EP.EmpCode,
                                      EmpName = EP.EmpName,
                                      CompanyName = EP.CompanyMaster.CompanyDesc,
                                      DepartmentDesc = EP.DepartmentMaster.DepartmentDesc,
                                      DesignationDesc = EP.DesignationMaster.DesignationDesc,
                                      EmployeeType = et.EmployeeTypeDsc,
                                  }).Distinct().ToListAsync();
                }
                else
                {
                    data = await (from q in db.EmpMasters
                                  join et in db.EmployeeTypes
                                  on q.EmployeeTypeID equals et.ID
                                  where q.ActiveInActive == false && q.EmpId == ID
                                  orderby q.EmpName ascending
                                  select new clsEmployeeProfile
                                  {
                                      EmpID = q.EmpId,
                                      EmpCode = q.EmpCode,
                                      EmpName = q.EmpName,
                                      CompanyName = q.CompanyMaster.CompanyDesc,
                                      DepartmentDesc = q.DepartmentMaster.DepartmentDesc,
                                      DesignationDesc = q.DesignationMaster.DesignationDesc,
                                      EmployeeType = et.EmployeeTypeDsc,
                                  }).Distinct().ToListAsync();
                }


                if (data != null)
                {
                    return data;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "clsEmployeeProfile", "_GetEmployeePosting");
                return null;
            }
        }
        #endregion

     
        public async Task<IList<clsEmployeeProfile>> _GetEmployeeForManualEntries(string Date)
        {
            try
            {
                string Missing_date1 = Convert.ToDateTime(Date).ToString("dd MMM yyy");
                NedoDb _db = new NedoDb();
                IList<clsEmployeeProfile> data = await (from q in _db.EmpMasters
                                                        where q.ActiveInActive == false
                                                        select new clsEmployeeProfile
                                                        {
                                                            EmpID = q.EmpId,
                                                            EmpName = q.EmpName,
                                                            EmpCode = q.EmpCode,
                                                            Missing_date = Missing_date1,
                                                            DepartmentDesc = q.DepartmentMaster.DepartmentDesc,
                                                            DesignationDesc = q.DesignationMaster.DesignationDesc,
                                                            Time_Enter = null,
                                                        }).ToListAsync();

                if (data != null)
                {
                    return data;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "clsEmployeeProfile", "_GetEmployeeForManualEntries");
                return null;
            }
        }

    
        public async Task<IList<clsEmployeeProfile>> _getEmpAbsentRecord(int empid, DateTime FromDate, DateTime ToDate)
        {

            try
            {
                NedoDb db = new NedoDb();
                IList<clsEmployeeProfile> AbsentRecord = await (from ep in db.EmpMasters
                                                                join att in db.AttendancePostings on ep.EmpId equals att.EmpId
                                                                join lm in db.LeaveMapMasters on att.EmpId equals lm.EmpID
                                                                join ld in db.LeaveMapDetails on lm.ID equals ld.MID
                                                                join ls in db.LeaveSetups on ld.LeaveID equals ls.ID
                                                                join desg in db.DesignationMasters on ep.DesignationID equals desg.DesignationId
                                                                join dept in db.DepartmentMasters on ep.DepartmentId equals dept.DepartmentId
                                                                where ep.EmpId == empid && att.NoOfAbsent == true && att.HolidayType == "A" && att.AttDate >= FromDate && att.AttDate <= ToDate
                                                                select new clsEmployeeProfile
                                                                {
                                                                    EmpID = ep.EmpId,
                                                                    EmpCode = ep.EmpCode,
                                                                    EmpName = ep.EmpName,
                                                                    DesignationDesc = desg.DesignationDesc,
                                                                    DepartmentDesc = dept.DepartmentDesc,
                                                                    AttDate = att.AttDate,
                                                                    AttDay = att.AttDay,
                                                                    LeaveID = ls.ID,
                                                                    LeaveDsc = ls.LeaveDsc,
                                                                    LeaveAllowed = (int)ld.LeaveAllowed,
                                                                }
                             ).ToListAsync();
                return AbsentRecord;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "clsEmployeeProfile", "_getEmpAbsentRecord");
                throw;
            }


        }

        public async Task<IList<clsEmployeeProfile>> GetListLeaveApproval()
        {
            try
            {
                using (var _db = new NedoDb())
                {
                    IList<clsEmployeeProfile> List = await (from q in _db.LeaveApprovals
                                                            select new clsEmployeeProfile
                                                            {
                                                                DepartmentDesc = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                                                DesignationDesc = q.EmpMaster.DesignationMaster.DesignationDesc,
                                                                EmpName = q.EmpMaster.EmpName,
                                                                EmpCode = q.EmpMaster.EmpCode,
                                                                EmpID = q.EmpMaster.EmpId,
                                                            }
                                            ).Distinct().ToListAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "clsEmployeeProfile", "GetCity");
                return null;
            }


        }
        public async Task<IList<clsEmployeeProfile>> GetListLeaveApproval_by_ID(int empid)
        {
            try
            {
                using (var _db = new NedoDb())
                {
                    IList<clsEmployeeProfile> List = await (from q in _db.LeaveApprovals
                                                            where q.EmpId == empid
                                                            select new clsEmployeeProfile
                                                            {
                                                                DepartmentDesc = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                                                DesignationDesc = q.EmpMaster.DesignationMaster.DesignationDesc,
                                                                EmpName = q.EmpMaster.EmpName,
                                                                EmpCode = q.EmpMaster.EmpCode,
                                                                EmpID = q.EmpMaster.EmpId,
                                                                AttDate = q.LeaveDate,
                                                                LeaveDsc = q.LeaveSetup.LeaveDsc,
                                                            }
                                            ).ToListAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "clsEmployeeProfile", "GetCity");
                return null;
            }


        }
        public async Task<IList<clsEmployeeProfile>> GetEmployeeListOnLeave()
        {
            try
            {
                using (var _db = new NedoDb())
                {
                    IList<clsEmployeeProfile> List = await (from q in _db.LeaveApprovals
                                                            select new clsEmployeeProfile
                                                            {
                                                                DepartmentDesc = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                                                DesignationDesc = q.EmpMaster.DesignationMaster.DesignationDesc,
                                                                EmpName = q.EmpMaster.EmpName,
                                                                EmpCode = q.EmpMaster.EmpCode,
                                                                EmpID = q.EmpMaster.EmpId,
                                                                EmpImg = q.EmpMaster.EmpImg,
                                                            }
                                            ).Distinct().ToListAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "clsEmployeeProfile", "GetEmployeeListOnLeave");
                return null;
            }


        }
        public async Task<IList<clsEmployeeProfile>> _GetEmployeeAttendanceLogMaster()
        {
            try
            {
                NedoDb _db = new NedoDb();
                IList<clsEmployeeProfile> data = await (from q in _db.AttendanceLogMasters
                                                        join et in _db.EmpMasters
                                                        on q.AcNo equals et.EmpCode
                                                        join x in _db.EmployeeTypes on et.EmployeeTypeID equals x.ID
                                                        where et.ActiveInActive == false
                                                        orderby et.EmpName ascending
                                                        select new clsEmployeeProfile
                                                        {
                                                            EmpID = et.EmpId,
                                                            EmpCode = et.EmpCode,
                                                            EmpName = et.EmpName,
                                                            CompanyName = et.CompanyMaster.CompanyDesc,
                                                            DepartmentDesc = et.DepartmentMaster.DepartmentDesc,
                                                            DesignationDesc = et.DesignationMaster.DesignationDesc,
                                                            EmployeeType = x.EmployeeTypeDsc,
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "clsEmployeeProfile", "_GetEmployeePosting");
                return null;
            }
        }
       
        public  IList<clsEmployeeProfile> _GetEmployeeLate(string id, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                NedoDb _db = new NedoDb();
                if (id == "1")
                {
                    IList<clsEmployeeProfile> data =  (from q in _db.EmpMasters
                                                            join et in _db.EmployeeTypes
                                                            on q.EmployeeTypeID equals et.ID
                                                            join AP in _db.AttendancePostings on q.EmpId equals AP.EmpId
                                                            where q.ActiveInActive == false && AP.NoOfLates != 0 && AP.AttDate >= FromDate && AP.AttDate <= ToDate
                                                            orderby q.EmpName ascending
                                                            group AP by AP.EmpId into grp
                                                            select new clsEmployeeProfile
                                                            {
                                                                NoOflates = grp.Select(x => x.NoOfLates).Count(),
                                                                EmpID = grp.Key,
                                                                EmpName = grp.Select(x => x.EmpMaster.EmpName).FirstOrDefault(),
                                                                EmpCode = grp.Select(x => x.EmpMaster.EmpCode).FirstOrDefault(),
                                                                DepartmentDesc = grp.Select(x => x.EmpMaster.DepartmentMaster.DepartmentDesc).FirstOrDefault(),
                                                                DesignationDesc = grp.Select(x => x.EmpMaster.DesignationMaster.DesignationDesc).FirstOrDefault()
                                                            }).ToList();
                    if (data != null)
                    {
                        return data;
                    }
                    else
                        return null;
                }
                if (id == "2")
                {
                    IList<clsEmployeeProfile> data =  (from q in _db.EmpMasters
                                                            join et in _db.EmployeeTypes
                                                            on q.EmployeeTypeID equals et.ID
                                                            join AP in _db.AttendancePostings on q.EmpId equals AP.EmpId
                                                            where q.ActiveInActive == false && AP.NoOfAbsent == true && AP.AttDate >= FromDate && AP.AttDate <= ToDate && AP.IsHoliday == false
                                                            orderby q.EmpName ascending
                                                            group AP by AP.EmpId into grp
                                                            select new clsEmployeeProfile
                                                            {
                                                                NoOfAbsents = grp.Select(x => x.NoOfAbsent).Count(),
                                                                EmpID = grp.Key,
                                                                EmpName = grp.Select(x => x.EmpMaster.EmpName).FirstOrDefault(),
                                                                EmpCode = grp.Select(x => x.EmpMaster.EmpCode).FirstOrDefault(),
                                                                DepartmentDesc = grp.Select(x => x.EmpMaster.DepartmentMaster.DepartmentDesc).FirstOrDefault(),
                                                                DesignationDesc = grp.Select(x => x.EmpMaster.DesignationMaster.DesignationDesc).FirstOrDefault()
                                                            }).ToList();
                    if (data != null)
                    {
                        return data;
                    }
                    else
                        return null;
                }
                if (id == "3")
                {
                    IList<clsEmployeeProfile> data =  (from q in _db.EmpMasters
                                                            join et in _db.EmployeeTypes
                                                            on q.EmployeeTypeID equals et.ID
                                                            join AP in _db.AttendancePostings on q.EmpId equals AP.EmpId
                                                            where q.ActiveInActive == false && AP.NoOfHalfDay != 0 && AP.AttDate >= FromDate && AP.AttDate <= ToDate
                                                            orderby q.EmpName ascending
                                                            group AP by AP.EmpId into grp
                                                            select new clsEmployeeProfile
                                                            {
                                                                NoOfHalfDay = grp.Select(x => x.NoOfHalfDay).Count(),
                                                                EmpID = grp.Key,
                                                                EmpName = grp.Select(x => x.EmpMaster.EmpName).FirstOrDefault(),
                                                                EmpCode = grp.Select(x => x.EmpMaster.EmpCode).FirstOrDefault(),
                                                                DepartmentDesc = grp.Select(x => x.EmpMaster.DepartmentMaster.DepartmentDesc).FirstOrDefault(),
                                                                DesignationDesc = grp.Select(x => x.EmpMaster.DesignationMaster.DesignationDesc).FirstOrDefault()

                                                            }).ToList();
                    if (data != null)
                    {
                        return data;
                    }
                    else
                        return null;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "ConsoleApp", "clsEmployeeProfile", "_GetEmployeePosting");
                return null;
            }
        }
    }
}