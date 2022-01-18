using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class IncomeTaxModel
    {
        public long IncomeTaxId { get; set; }
        public double? ExtraTaxAmount { get; set; }
        public double? SalaryStartSlab { get; set; }
        public double? SalaryEndSlab { get; set; }
        public double? Percentage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }

        public async Task<IList<IncomeTaxModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<IncomeTaxModel> list = await (from q in _db.tbl_IncomeTaxMaster
                                              orderby q.Id ascending
                                              where q.IsActive
                                              select new IncomeTaxModel
                                              {
                                                  IncomeTaxId = q.Id,
                                                  ExtraTaxAmount = q.ExtraAmount,
                                                  Percentage = q.Tax_Percentage,
                                                  SalaryStartSlab = q.SalaryStartSlab,
                                                  SalaryEndSlab = q.SalaryEndSlab

                                              }).ToListAsync();
                return list;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelIncomeTax", "IncomeTax_GetAll");

                return null;
            }
        }

        public async Task<IncomeTaxModel> Get_IncomeTax_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IncomeTaxModel am = await (from q in _db.tbl_IncomeTaxMaster
                                              
                                     where q.Id == id && q.IsActive
                                           select new IncomeTaxModel
                                     {
                                         IncomeTaxId = q.Id,
                                         ExtraTaxAmount = q.ExtraAmount,
                                               Percentage = q.Tax_Percentage,
                                         SalaryStartSlab = q.SalaryStartSlab,
                                         SalaryEndSlab = q.SalaryEndSlab

                                     }).SingleOrDefaultAsync();
                return am;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "IncomeTaxModel", "IncomeTax_GetIncomeTaxById");

                return null;
            }
        }
        public async Task<bool> EditIncomeTax()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var am = await _db.tbl_IncomeTaxMaster.Where(x => x.Id == this.IncomeTaxId).FirstOrDefaultAsync();
                if (am != null)
                {
                   
                    am.Tax_Percentage = this.Percentage;

                    am.SalaryStartSlab = this.SalaryStartSlab;
                    am.SalaryEndSlab = this.SalaryEndSlab;
                    am.ExtraAmount = this.ExtraTaxAmount;

                    am.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    am.EditedOn = DateTime.Now;
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "IncomeTaxModel", "IncomeTax_Edit");

                return false;

            }
        }
    }
}