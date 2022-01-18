using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class AllocatedBonusModel
    {
        public int AllocatedBonusId { get; set; }
        public int? BonusId { get; set; }
        public string BonusName { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeType { get; set; }
        public DateTime? AllocatedMonth { get; set; }

        public async Task<IList<AllocatedBonusModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<AllocatedBonusModel> aam = await (from q in _db.tbl_AllocatedBonuses
                                                            select new AllocatedBonusModel
                                                            {
                                                                AllocatedBonusId = q.AllocatedBonusId,
                                                                BonusId = q.BonusId,
                                                                BonusName = q.tbl_BonusMaster.BonusName,
                                                                EmpId = q.EmpMaster.EmpId,
                                                                EmpCode = q.EmpMaster.EmpCode,
                                                                EmpName = q.EmpMaster.EmpName,
                                                                Department = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                                                Designation = q.EmpMaster.DesignationMaster.DesignationDesc,
                                                                EmployeeType = q.EmpMaster.EmployeeType.EmployeeTypeDsc,
                                                                AllocatedMonth = q.BonusMonth

                                                            }).ToListAsync();
                return aam;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedBonus", "AllocatedBonus_GetAll");
                return null;
            }
        }
        public async Task<bool> DeleteAllocation(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var allocation = _db.tbl_AllocatedBonuses.Where(x => x.AllocatedBonusId == id).FirstOrDefault();
                if (allocation != null)
                {
                    _db.tbl_AllocatedBonuses.Remove(allocation);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocateBonus", "DeleteBonusAllocation");
                return false;
            }
        }
        public async Task<bool> AllocateBonus(string empIds, DateTime dtFrom, DateTime dtTill, int iBonusId)
        {
            try
            {
                string[] sEmployeeIds = empIds.Split(new char[] { ',' });
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                foreach (string empid in sEmployeeIds)
                {
                    if (empid.Trim() == string.Empty)
                        continue;
                    DateTime dtCurrent = dtFrom;
                    long lEmpid = Convert.ToInt64(empid.Trim());
                    while (dtCurrent <= dtTill)
                    {
                        var pAllocation = _db.tbl_AllocatedBonuses.Where(x => x.EmpId == lEmpid && x.BonusId.Value == iBonusId && x.BonusMonth.Value.Month == dtCurrent.Month && x.BonusMonth.Value.Year == dtCurrent.Year).FirstOrDefault();
                        if (pAllocation != null)
                        {
                            dtCurrent = dtCurrent.AddMonths(1);
                            continue;
                        }
                        tbl_AllocatedBonuses taa = new tbl_AllocatedBonuses();
                        taa.BonusId = iBonusId;
                        taa.EmpId = lEmpid;// Convert.ToInt64(empid.Trim());
                        taa.BonusMonth = dtCurrent;
                        _db.tbl_AllocatedBonuses.Add(taa);
                        dtCurrent = dtCurrent.AddMonths(1);

                    }


                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedBonus", "AllocatedBonus_Allocate");
                return false; ;

            }
        }
    }
}