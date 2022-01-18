using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class LateDeductModel
    {
        public int Count { get; set; }
        public double? Amount { get; set; }
    }
    public class LateDeductDbModel
    {
        public int Id { get; set; }
        public double? Amount { get; set; }
    }
    public class LateDeductViewModel
    {
        public long empId { get; set; }
        public int count { get; set; }

        public double deduct { get; set; }
    }
    public class EmpLateDetails
    {
        public long Id { get; set; }
        public long EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public int DeductId { get; set; }

        public double? Deduct_Amount { get; set; }
        public DateTime? AttDate { get; set; }
        public TimeSpan? TimeIn { get; set; }
        public int LatePolicyId { get; set; }

        public string Remarks { get; set; }
        public static async Task<IList<tbl_LateAttendnaceMaster>> GetLatePolicies()
        {
            using (var db = new PakOman_NedoEntities())
            {
                var result =await db.tbl_LateAttendnaceMaster.Where(c => !c.IsDeleted.Value).ToListAsync();
                return result;
            }
        }

    }

    public class LateDeductAdjustViewModel
    {
        public long Id { get; set; }
        public long EmpId { get; set; }
        public int Late_DeductId { get; set; }
        public double Late_DeductAmount { get; set; }
        public string Remarks { get; set; }




        public async Task<bool> AdjustEmpLateDetails(List<LateDeductAdjustViewModel> model)
        {
            try
            {
                using (var db = new PakOman_NedoEntities())
                {
                    var idList = model.Select(c => c.Id).ToList();
                    var lateDetails = await db.tbl_EmpLateDetails.Where(c => idList.Contains(c.Id)).ToListAsync();
                   
                    foreach (var item in model)
                    {
                       
                        lateDetails.Where(c => c.Id == item.Id).Select(c =>
                         {
                            c.IsExempt= c.Deduct_Amount == item.Late_DeductAmount && !c.IsExempt.Value ?  false :  true;
                             c.DeductId = item.Late_DeductId;
                             c.Deduct_Amount = item.Late_DeductAmount;
                             c.Remarks = item.Remarks;
                             
                             return c;
                         }).ToList();
                    }
                    await db.SaveChangesAsync();
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

           
        } 

    }

}