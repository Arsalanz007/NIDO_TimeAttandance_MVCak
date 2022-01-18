using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class PayrollOptionModel
    {

        public int Id { get; set; }
        public int PayScheduleId { get; set; }
        public int CalculationMethodId { get; set; }
        public string Currency { get; set; }
        public bool SignatureInPaySlip { get; set; }
        public int Month_Start { get; set; }
        public int Month_End { get; set; }

        public static IList<SelectListItem> GetPaySchedule()
        {
            var db = new PakOman_NedoEntities();
            IList<SelectListItem> items = (from q in db.tbl_Pay_Schedule
                                           select new SelectListItem
                                           {
                                               Text = q.Pay_ScheduleDesc,
                                               Selected = false,
                                               Value = q.Id.ToString()
                                           }).ToList();

            return items;

        }

        public static IList<SelectListItem> GetPayCalculationMethod()
        {
            var db = new PakOman_NedoEntities();
            IList<SelectListItem> items = (from q in db.tbl_EmpSalary_CalculationMethod
                                           select new SelectListItem
                                           {
                                               Text = q.CalculationDesc,
                                               Selected = false,
                                               Value = q.Id.ToString()
                                           }).ToList();

            return items;

        }

        public static async Task<PayrollOptionModel> GetPayrollOption()
        {
            var db = new PakOman_NedoEntities();

            var model = await db.tbl_Payroll_Options.Where(c => c.IsActive.Value).Select(c=> new PayrollOptionModel { 
             Id = c.Id,
             CalculationMethodId = c.Salary_CaluclationId,
             Currency = c.Curreny,
             PayScheduleId = c.Pay_ScheduleId.Value,
             SignatureInPaySlip = c.PaySlip_Signature.Value,
             Month_End = c.Month_End.Value,
             Month_Start = c.Month_Start.Value
            
            }).FirstOrDefaultAsync();
        
            if(model == null)
            {
                model = new PayrollOptionModel();
            }
            return model;
        }

        public  async Task<bool> Save_PayrollOption()
        {
            try
            {
                var db = new PakOman_NedoEntities();

                var model = new tbl_Payroll_Options();

                if (Id > 0)
                {
                    model = await db.tbl_Payroll_Options.Where(c => c.Id == Id).FirstOrDefaultAsync();

                }
                else
                {
                    model.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    model.CreatedOn = DateTime.Now;
                }

                model.IsActive = true;
                model.IsDeleted = false;
                model.Pay_ScheduleId = PayScheduleId;
                model.Salary_CaluclationId = CalculationMethodId;
                model.Curreny = Currency;
                model.PaySlip_Signature = SignatureInPaySlip;
                model.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                model.EditedOn = DateTime.Now;
                model.Month_Start = Month_Start;
                model.Month_End = Month_End;

                if (Id == 0)
                    db.tbl_Payroll_Options.Add(model);

                await db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "PayrollOptionModel", "PayrollOption_Save_PayrollOption");
                return false;
            }
           
        }
    }
}