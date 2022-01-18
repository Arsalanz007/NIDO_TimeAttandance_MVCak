using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class LateAttendanceModel
    {
        public int Id { get; set; }
        public int LatePolicyId { get; set; }

        public TimeSpan? Start_Time { get; set; }
        public TimeSpan? End_Time { get; set; }
        public int? Deduct_Percent { get; set; }
        public int? Days { get; set; }


        public  IList<LateAttendanceModel> GetAll(int LatePolicyId)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<LateAttendanceModel> aam =   (from q in _db.tbl_LateAttendnaceMaster
                                                  where q.IsDeleted == false && q.LatePolicyId == LatePolicyId
                                                  select new LateAttendanceModel
                                                  {
                                                      Id = q.Id,
                                                      Days = q.Days,
                                                      Deduct_Percent = q.Deduct_Percent,
                                                      End_Time = q.End_Time,
                                                      Start_Time = q.Start_Time

                                                  }).OrderBy(c => c.Deduct_Percent).ToList();
                return aam;

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "LateAttendanceModel", "LateAttendance_GetAll");
                return null;
            }
        }
        public async Task<bool> Save()
        {
            try
            {

                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var lam = new tbl_LateAttendnaceMaster();
                lam.Days = Days;
                lam.Deduct_Percent = Deduct_Percent;
                lam.Start_Time = Start_Time;
                lam.End_Time = End_Time;
                lam.LatePolicyId = LatePolicyId;
                lam.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                lam.CreatedOn = DateTime.Now;
                lam.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                lam.EditedOn = DateTime.Now;
                lam.IsDeleted = false;
                _db.tbl_LateAttendnaceMaster.Add(lam);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "LateAttendanceModel", "LateAttendance_Save");
                return false; ;

            }
        }

        public async Task<bool> Edit(int id)
        {
            try
            {

                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                var lam = _db.tbl_LateAttendnaceMaster.Where(c => c.Id == id).FirstOrDefault();

                lam.Days = Days;
                lam.Deduct_Percent = Deduct_Percent;
                lam.Start_Time = Start_Time;
                lam.End_Time = End_Time;
                lam.LatePolicyId = LatePolicyId;
             
                lam.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                lam.EditedOn = DateTime.Now;
               
                
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "LateAttendanceModel", "LateAttendance_Edit");
                return false; ;

            }
        }
        public async Task<bool> Delete_LateAttendnace(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var late = _db.tbl_LateAttendnaceMaster.Where(x => x.Id == id).FirstOrDefault();
                if (late != null)
                {
                    late.IsDeleted = true;

                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "LateAttendanceModel", "DeleteLateAttendance");
                return false;
            }
        }
        public async Task<LateAttendanceModel> Get_LateAttendance_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_LateAttendnaceMaster tlm = await (from q in _db.tbl_LateAttendnaceMaster
                                            where q.Id == id
                                            select q).SingleOrDefaultAsync();

                var lam = new LateAttendanceModel();

                lam.Id = tlm.Id;
                lam.Start_Time = tlm.Start_Time;
                lam.End_Time = tlm.End_Time;
                lam.Days = tlm.Days;
                lam.Deduct_Percent = tlm.Deduct_Percent;
                lam.LatePolicyId = tlm.LatePolicyId.GetValueOrDefault();
               

                return lam;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "LateAttendanceModel", "Allowance_GetLateAttendanceById");

                return null;
            }
        }

        public async Task<List<LateExemptModel>> Get_LateRecord(long[] empId , DateTime fromDate,DateTime toDate )
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var tlm = await (from q in _db.tbl_EmpLateDeduct
                                  join x in _db.EmpMasters on q.EmpId.Value equals x.EmpId
                                  join d in _db.tbl_LateAttendnaceMaster on q.Late_DeductId.Value equals d.Id
                                  where empId.Contains(q.EmpId.Value) && q.MonthNo == fromDate.Month && q.YearNo == q.YearNo
                                  select new LateExemptModel {
                                  empCode = x.EmpCode,
                                  empName = x.EmpName,
                                  deductAmount = d.Deduct_Percent + "%",
                                  deductId = q.Late_DeductId.Value,
                                  empId = q.EmpId.Value,
                                  monthNo = q.MonthNo.Value,
                                  year = q.YearNo.Value,
                                  count = q.Count.Value
                                
                                  }).ToListAsync();


                return tlm;

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "LateAttendanceModel", "Allowance_GetLateAttendanceById");

                return null;
            }
        }


    }
}