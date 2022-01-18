using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;
namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class AllocatedDeductionModel
    {
        public int AllocatedDeductionId { get; set; }
        public int? DeductionId { get; set; }
        public string DeductionName { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeType { get; set; }
        public DateTime? AllocatedMonth { get; set; }

        public async Task<IList<AllocatedDeductionModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<AllocatedDeductionModel> aam = await (from q in _db.tbl_AllocatedDeductions
                                                            select new AllocatedDeductionModel
                                                            {
                                                                AllocatedDeductionId = q.AllocatedDeductionId,
                                                                DeductionId = q.DeductionId,
                                                                DeductionName = q.DeductionMaster.DeductionName,
                                                                EmpId = q.EmpMaster.EmpId,
                                                                EmpCode = q.EmpMaster.EmpCode,
                                                                EmpName = q.EmpMaster.EmpName,
                                                                Department = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                                                Designation = q.EmpMaster.DesignationMaster.DesignationDesc,
                                                                EmployeeType = q.EmpMaster.EmployeeType.EmployeeTypeDsc,
                                                                AllocatedMonth = q.DeductionMonth

                                                            }).ToListAsync();
                return aam;

            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedDeduction", "AllocatedDeduction_GetAll");
                return null;
            }
        }
        public async Task<bool> DeleteAllocation(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var allocation = _db.tbl_AllocatedDeductions.Where(x => x.AllocatedDeductionId == id).FirstOrDefault();
                if (allocation != null)
                {
                    _db.tbl_AllocatedDeductions.Remove(allocation);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocateDeduction", "DeleteDeductionAllocation");
                return false;
            }
        }
        public async Task<bool> AllocateDeduction(string empIds,DateTime dtFrom,DateTime dtTill,int iDeductionId)
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
                        var pAllocation = _db.tbl_AllocatedDeductions.Where(x =>x.EmpId ==lEmpid && x.DeductionId.Value == iDeductionId && x.DeductionMonth.Value.Month == dtCurrent.Month && x.DeductionMonth.Value.Year == dtCurrent.Year).FirstOrDefault();
                        if (pAllocation != null)
                        {
                            dtCurrent = dtCurrent.AddMonths(1);
                            continue;
                        }
                        tbl_AllocatedDeductions taa = new tbl_AllocatedDeductions();
                        taa.DeductionId = iDeductionId;
                        taa.EmpId = lEmpid;// Convert.ToInt64(empid.Trim());
                        taa.DeductionMonth = dtCurrent;
                        _db.tbl_AllocatedDeductions.Add(taa);
                        dtCurrent = dtCurrent.AddMonths(1);

                    }
                    

                }
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedDeduction", "AllocatedDeduction_Allocate");
                return false; ;

            }
        }
        
    }
}