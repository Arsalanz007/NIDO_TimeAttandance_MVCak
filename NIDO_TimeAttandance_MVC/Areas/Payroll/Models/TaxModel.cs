using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class TaxModel
    {
        public long TaxId    { get; set; }
        public string TaxName { get; set; }
        public int ValueTypeId { get; set; }
        public string ValueTypeDesc { get; set; }
        public double TaxValue { get; set; }
        public double SalaryStartSlab { get; set; }
        public double SalaryEndSlab { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }

        public async Task<bool> SaveTax()
        {
            
            try {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_TaxMaster am = new tbl_TaxMaster();
                am.TaxDesc = TaxName;
                am.TaxValue = TaxValue;
                am.ValueTypeId = ValueTypeId;
                am.SalaryStartSlab = SalaryStartSlab;
                am.SalaryEndSlab = SalaryEndSlab;
                am.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                am.CreatedOn = DateTime.Now;
                am.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                am.EditedOn = DateTime.Now;
                _db.tbl_TaxMaster.Add(am);
                await _db.SaveChangesAsync();
                return true;

            } catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelTax", "Tax_Save");
                return false;
            }
            
        }
        public async Task<IList<TaxModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<TaxModel> list = await(from q in _db.tbl_TaxMaster
                                                   orderby q.TaxId ascending
                                                    select new TaxModel
                                                    {
                                                        TaxId = q.TaxId,
                                                        TaxName = q.TaxDesc,
                                                        TaxValue = q.TaxValue,
                                                        ValueTypeDesc = q.ValueType.ValueTypeName,
                                                        SalaryStartSlab = q.SalaryStartSlab,
                                                        SalaryEndSlab = q.SalaryEndSlab
                                                        
                                                    }).ToListAsync();
                return list;

            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelTax", "Tax_GetAll");

                return null;
            }
        }
        public async Task<bool> DeleteTax(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var mTaxModel = _db.tbl_TaxMaster.Where(q => q.TaxId == id).FirstOrDefault();
                if(mTaxModel != null)
                {
                    _db.tbl_TaxMaster.Remove(mTaxModel);
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "TaxModel", "Tax_Delete");

                return false;
            }
        }
        public async Task<TaxModel> Get_Tax_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                TaxModel am = await (from q in _db.tbl_TaxMaster
                                           where q.TaxId == id
                                           select new TaxModel
                                           {
                                               TaxId = q.TaxId,
                                               TaxName = q.TaxDesc,
                                               TaxValue = q.TaxValue,
                                               ValueTypeId = q.ValueTypeId,
                                               SalaryStartSlab= q.SalaryStartSlab,
                                               SalaryEndSlab = q.SalaryEndSlab
                                               
                                           }).SingleOrDefaultAsync();
                return am;

            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "TaxModel", "Tax_GetTaxById");

                return null;
            }
        }

        public async Task<bool> EditTax()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var am = await _db.tbl_TaxMaster.Where(x => x.TaxId == this.TaxId).FirstOrDefaultAsync();
                if (am != null)
                {
                    am.TaxDesc = this.TaxName;
                    am.ValueTypeId = this.ValueTypeId;
                    am.TaxValue = this.TaxValue;
                    am.SalaryStartSlab = this.SalaryStartSlab;
                    am.SalaryEndSlab = this.SalaryEndSlab;
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
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "TaxModel", "Tax_Edit");

                return false;

            }
        }
    }
}