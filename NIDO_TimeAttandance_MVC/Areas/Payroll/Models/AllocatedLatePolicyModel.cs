using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;
namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class AllocatedLatePolicyModel
    {
        public int AllocatedLatePolicyId { get; set; }
        public int LatePolicyId { get; set; }
        public string LatePolicyName { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeType { get; set; }
        public DateTime? LatePolicyMonth { get; set; }
        public DateTime? LatePolicyEndDate { get; set; }

        public async Task<IList<AllocatedLatePolicyModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<AllocatedLatePolicyModel> aam = await (from q in _db.tbl_AllocatedLatePolicies
                                                            select new AllocatedLatePolicyModel
                                                            {
                                                                AllocatedLatePolicyId = q.AllocatedLatePolicyId,
                                                                LatePolicyId = q.LatePolicyId,
                                                                LatePolicyName = q.tbl_LatePolicyMaster.LatePolicName,
                                                                EmpId = q.EmpMaster.EmpId,
                                                                EmpCode = q.EmpMaster.EmpCode,
                                                                EmpName = q.EmpMaster.EmpName,
                                                                Department = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                                                Designation = q.EmpMaster.DesignationMaster.DesignationDesc,
                                                                EmployeeType = q.EmpMaster.EmployeeType.EmployeeTypeDsc,
                                                                LatePolicyMonth = q.LatePolicyStartDate,
                                                                LatePolicyEndDate = q.LatePolicyEndDate

                                                            }).ToListAsync();
                return aam;

            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedLatePolicy", "AllocatedLatePolicy_GetAll");
                return null;
            }
        }
        public async Task<bool> DeleteAllocation(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var allocation = _db.tbl_AllocatedLatePolicies.Where(x => x.AllocatedLatePolicyId == id).FirstOrDefault();
                if (allocation != null)
                {
                    _db.tbl_AllocatedLatePolicies.Remove(allocation);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocateLatePolicy", "DeleteLatePolicyAllocation");
                return false;
            }
        }
        public async Task<bool> AllocateLatePolicy(string empIds,DateTime dtFrom,DateTime dtTill,int iLatePolicyId)
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
                   //while (dtCurrent <= dtTill)
                    {
                        //var pAllocation = _db.tbl_AllocatedLatePolicies.Where(x =>x.EmpId ==lEmpid && x.LatePolicyId == iLatePolicyId && x.LatePolicyMonth.Month == dtCurrent.Month && x.LatePolicyMonth.Year == dtCurrent.Year).FirstOrDefault();
                        var pAllocation = _db.tbl_AllocatedLatePolicies.Where(x => x.EmpId == lEmpid && x.LatePolicyId == iLatePolicyId && dtCurrent >= x.LatePolicyStartDate && dtCurrent<=x.LatePolicyEndDate).FirstOrDefault();

                        if (pAllocation != null)
                        {
                            dtCurrent = dtCurrent.AddMonths(1);
                            continue;
                        }
                        tbl_AllocatedLatePolicies taa = new tbl_AllocatedLatePolicies();
                        taa.LatePolicyId = iLatePolicyId;
                        taa.EmpId = lEmpid;// Convert.ToInt64(empid.Trim());
                        taa.LatePolicyStartDate = dtFrom;
                        taa.LatePolicyEndDate = dtTill;
                        _db.tbl_AllocatedLatePolicies.Add(taa);
                        dtCurrent = dtCurrent.AddMonths(1);


                    }
                    

                }
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedLatePolicy", "AllocatedLatePolicy_Allocate");
                return false; ;

            }
        }
        
    }
}