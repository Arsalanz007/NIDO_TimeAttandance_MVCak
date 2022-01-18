using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;
namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class AllocatedAllowanceModel
    {
        public int AllocatedAllowanceId { get; set; }
        public int? AllowanceId { get; set; }
        public string AllowanceName { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeType { get; set; }
        public string ArrearDesc { get; set; }
        public DateTime? AllocatedMonth { get; set; }

        public async Task<IList<AllocatedAllowanceModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<AllocatedAllowanceModel> aam = await (from q in _db.tbl_AllocatedAllowances
                                                            select new AllocatedAllowanceModel
                                                            {
                                                                AllocatedAllowanceId = q.AllocatedAllowanceId,
                                                                AllowanceId = q.AllowanceId,
                                                                AllowanceName = q.AllownceMaster.AllownceName,
                                                                EmpId = q.EmpMaster.EmpId,
                                                                EmpCode = q.EmpMaster.EmpCode,
                                                                EmpName = q.EmpMaster.EmpName,
                                                                Department = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                                                Designation = q.EmpMaster.DesignationMaster.DesignationDesc,
                                                                EmployeeType = q.EmpMaster.EmployeeType.EmployeeTypeDsc,
                                                                AllocatedMonth = q.AllowanceMonth,
                                                                ArrearDesc=q.Desc
                                                                

                                                            }).ToListAsync();
                return aam;

            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedAllowance", "AllocatedAllowance_GetAll");
                return null;
            }
        }
        public async Task<bool> DeleteAllocation(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var allocation = _db.tbl_AllocatedAllowances.Where(x => x.AllocatedAllowanceId == id).FirstOrDefault();
                if (allocation != null)
                {
                    _db.tbl_AllocatedAllowances.Remove(allocation);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocateAllowance", "DeleteAllowanceAllocation");
                return false;
            }
        }
        public async Task<bool> AllocateAllowance(string empIds,DateTime dtFrom,DateTime dtTill,int iAllowanceId,string ArrearDesc)
        {
            try
            {
                string[] sEmployeeIds = empIds.Split(new char[] { ',' });
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                foreach(string empid in sEmployeeIds)
                {
                    if (empid.Trim() == string.Empty)
                        continue;
                    DateTime dtCurrent = dtFrom;
                    long lEmpid = Convert.ToInt64(empid.Trim());
                   while (dtCurrent <= dtTill)
                    {
                        var pAllocation = _db.tbl_AllocatedAllowances.Where(x =>x.EmpId ==lEmpid && x.AllowanceId.Value == iAllowanceId && x.AllowanceMonth.Value.Month == dtCurrent.Month && x.AllowanceMonth.Value.Year == dtCurrent.Year).FirstOrDefault();
                        if (pAllocation != null)
                        {
                            dtCurrent = dtCurrent.AddMonths(1);
                            continue;
                        }
                        tbl_AllocatedAllowances taa = new tbl_AllocatedAllowances();
                        taa.AllowanceId = iAllowanceId;
                        taa.EmpId = lEmpid;// Convert.ToInt64(empid.Trim());
                        taa.AllowanceMonth = dtCurrent;
                        taa.Desc = ArrearDesc;
                        _db.tbl_AllocatedAllowances.Add(taa);
                        dtCurrent = dtCurrent.AddMonths(1);

                    }
                    

                }
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedAllowance", "AllocatedAllowance_Allocate");
                return false; ;

            }
        }
        
    }
}