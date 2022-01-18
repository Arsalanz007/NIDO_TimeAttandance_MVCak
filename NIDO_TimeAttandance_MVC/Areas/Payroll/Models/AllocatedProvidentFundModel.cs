using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class AllocatedProvidentFundModel
    {
        public int AllocatedProvidentFundId { get; set; }
        public int? ProvidentFundId { get; set; }
        public string ProvidentFundName { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeType { get; set; }

        public async Task<IList<AllocatedProvidentFundModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<AllocatedProvidentFundModel> aam = await (from q in _db.tbl_AllocatedProvidentFund
                                                       select new AllocatedProvidentFundModel
                                                       {
                                                           AllocatedProvidentFundId = q.AllocatedProvidentFundId,
                                                           ProvidentFundId = q.ProvidentFundId,
                                                           ProvidentFundName= q.tbl_ProvidentFundMaster.ProvidentFundName,
                                                           EmpId = q.EmpMaster.EmpId,
                                                           EmpCode = q.EmpMaster.EmpCode,
                                                           EmpName = q.EmpMaster.EmpName,
                                                           Department = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                                           Designation = q.EmpMaster.DesignationMaster.DesignationDesc,
                                                           EmployeeType = q.EmpMaster.EmployeeType.EmployeeTypeDsc,
                                                           Company = q.EmpMaster.CompanyMaster.CompanyDesc
                                                           //AllocatedMonth = q.AllowanceMonth

                                                       }).ToListAsync();
                return aam;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedProvidentFund", "AllocatedProvidentFund_GetAll");
                return null;
            }
        }
        public async Task<bool> DeleteAllocation(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var allocation = _db.tbl_AllocatedProvidentFund.Where(x => x.AllocatedProvidentFundId == id).FirstOrDefault();
                if (allocation != null)
                {
                    _db.tbl_AllocatedProvidentFund.Remove(allocation);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedProvidentFund", "DeleteProvidentFundAllocation");
                return false;
            }
        }
        public async Task<bool> AllocateProvidentFund(string empIds, int iProvidentFundId)
        {
            try
            {
                string[] sEmployeeIds = empIds.Split(new char[] { ',' });
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                foreach (string empid in sEmployeeIds)
                {
                    if (empid.Trim() == string.Empty)
                        continue;
                  

                    long lEmpid = Convert.ToInt64(empid.Trim());
            
                    var pfModel = _db.tbl_AllocatedProvidentFund.Where(x => x.EmpId == lEmpid).FirstOrDefault();
                    if (pfModel == null)
                        pfModel = new tbl_AllocatedProvidentFund();

                    pfModel.ProvidentFundId = iProvidentFundId;
                    pfModel.EmpId = lEmpid;

                    if (pfModel.AllocatedProvidentFundId == 0)
                    {

                        _db.tbl_AllocatedProvidentFund.Add(pfModel);
                       
                    }

                    // dtCurrent = dtCurrent.AddMonths(1);

                    //  }


                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedProvidentFund", "AllocatedProvidentFund_Allocate");
                return false; ;

            }
        }

    }
}