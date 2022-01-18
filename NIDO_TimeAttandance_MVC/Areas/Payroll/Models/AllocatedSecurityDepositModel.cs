using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;
using NIDO_TimeAttandance_MVC.Models;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class AllocatedSecurityDepositModel
    {
        public int AllocatedSecurityDepositId { get; set; }
        public int? SecurityDepositId { get; set; }
        public string SecurityDepositName { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeType { get; set; }
        //  public DateTime? AllocatedMonth { get; set; }

        public async Task<IList<AllocatedSecurityDepositModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<AllocatedSecurityDepositModel> aam = await (from q in _db.tbl_AllocatedSecurityDeposit
                                                                  select new AllocatedSecurityDepositModel
                                                                  {
                                                                      AllocatedSecurityDepositId = q.AllocatedSecurityDepositId,
                                                                      SecurityDepositId = q.SecurityDepositId,
                                                                      SecurityDepositName = q.tbl_SecurityDepositMaster.DepositName,
                                                                      EmpId = q.EmpMaster.EmpId,
                                                                      EmpCode = q.EmpMaster.EmpCode,
                                                                      EmpName = q.EmpMaster.EmpName,
                                                                      Department = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                                                      Designation = q.EmpMaster.DesignationMaster.DesignationDesc,
                                                                      EmployeeType = q.EmpMaster.EmployeeType.EmployeeTypeDsc

                                                                  }).ToListAsync();
                return aam;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedSecurityDeposit", "AllocatedSecurityDeposit_GetAll");
                return null;
            }
        }
        public async Task<bool> DeleteAllocation(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var allocation = _db.tbl_AllocatedSecurityDeposit.Where(x => x.AllocatedSecurityDepositId == id).FirstOrDefault();
                if (allocation != null)
                {
                    _db.tbl_AllocatedSecurityDeposit.Remove(allocation);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocateSecurityDeposit", "DeleteSecurityDepositAllocation");
                return false;
            }
        }
        public async Task<bool> AllocateSecurityDeposit(string empIds, DateTime dtFrom, DateTime dtTill, int iSecurityDepositId)
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
                    var pAllocation = _db.tbl_AllocatedSecurityDeposit.Where(x => x.EmpId == lEmpid && x.SecurityDepositId.Value == iSecurityDepositId).FirstOrDefault();
                    if (pAllocation != null)
                    {
                        
                        continue;
                    }
                    tbl_AllocatedSecurityDeposit taa = new tbl_AllocatedSecurityDeposit();
                    taa.SecurityDepositId = iSecurityDepositId;
                    taa.EmpId = lEmpid;// Convert.ToInt64(empid.Trim());

                    _db.tbl_AllocatedSecurityDeposit.Add(taa);





                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedSecurityDeposit", "AllocatedSecurityDeposit_Allocate");
                return false; ;

            }
        }
        public async Task<List<clsEmployeeProfile>> GetOpeningBalance()
        {
            try
            {
                var _db = new PakOman_NedoEntities();

                var empList = _db.EmpMasters.Where(c => c.ActiveInActive == false)
                    .Select(c => new clsEmployeeProfile {
                            EmpID = c.EmpId,
                            EmpCode = c.EmpCode,
                            EmpName =c.EmpName,
                            DepartmentDesc = c.DepartmentMaster.DepartmentDesc,
                            DesignationDesc = c.DesignationMaster.DesignationDesc,
                            OpeningBalance = c.SecurityDepositOP.Value,

                    }).ToList();

                return empList;
            }
            catch(Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedSecurityDeposit", "AllocatedSecurityDeposit_OpeningBalance");
                throw ex;
            }
        }

        public async Task<bool> SaveOpeningBalance(List<SecurityDepositOPModel> model)
        {
            try
            {
                var _db = new PakOman_NedoEntities();
                var empIds = model.Select(c => c.EmpId).ToList();

                var empList = await _db.EmpMasters.Where(c => empIds.Contains(c.EmpId)).ToListAsync();

                empList.Select(c => c.SecurityDepositOP = model.Where(a => a.EmpId == c.EmpId).FirstOrDefault().OpAmount).ToList();


                await _db.SaveChangesAsync();

                //var empList = await (from q in _db.EmpMasters
                //                     join op in model on q.EmpId equals op.EmpId

                //               select new { q,q.SecurityDepositOP = op.OpAmount }
                //               ).ToListAsync();

                //    from ep in db.EmpMasters
                //    join m in db.tbl_Manager on ep.DepartmentId equals m.DepartmentID
                //    join lev in db.tbl_ManagerLevel on m.LevelID equals lev.ID

                return true;
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedSecurityDeposit", "AllocatedSecurityDeposit_OpeningBalance");
                return false;
            }
        }
    }
}