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
    public class EOBIModel
    {
        public int EOBIId { get; set; }
        public string EOBIName { get; set; }
        public int? ValueTypeId { get; set; }
        public double? EOBIValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public int CategoryId { get; set; }


        public async Task<bool> SaveEOBI()
        {

            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var eobi = new tbl_EOBIMaster();
                eobi.EOBIName = EOBIName;
                eobi.EOBIValue = EOBIValue;
                eobi.ValueTypeId = ValueTypeId;
                eobi.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                eobi.CreatedOn = DateTime.Now;
                eobi.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                eobi.EditedOn = DateTime.Now;
                eobi.CategoryId = CategoryId;
                _db.tbl_EOBIMaster.Add(eobi);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "EOBIModel", "EOBI_SaveEOBI");
                return false;
            }

        }
        public async Task<IList<EOBIModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<EOBIModel> list = await (from q in _db.tbl_EOBIMaster
                                                    orderby q.Id ascending
                                                    select new EOBIModel
                                                    {
                                                        EOBIId = q.Id,
                                                        EOBIName = q.EOBIName,
                                                        EOBIValue = q.EOBIValue
                                                    }).ToListAsync();
                return list;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "EOBIModel", "EOBI_GetAll");

                return null;
            }
        }
        public async Task<bool> DeleteEOBI(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var eobiModel = _db.tbl_EOBIMaster.Where(q => q.Id == id).FirstOrDefault();
                if (eobiModel != null)
                {
                    _db.tbl_EOBIMaster.Remove(eobiModel);
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "EOBIModel", "EOBI_Delete");

                return false;
            }
        }
        public async Task<EOBIModel> Get_EOBI_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                EOBIModel am = await (from q in _db.tbl_EOBIMaster
                                           where q.Id == id
                                           select new EOBIModel
                                           {
                                               EOBIId = q.Id,
                                               EOBIName = q.EOBIName,
                                               EOBIValue = q.EOBIValue,
                                               ValueTypeId = q.ValueTypeId,
                                               CategoryId = q.CategoryId.Value
                                           }).SingleOrDefaultAsync();
                return am;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "EOBIModel", "EOBI_GetEOBIById");

                return null;
            }
        }

        public async Task<bool> EditEOBI()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var eobi = await _db.tbl_EOBIMaster.Where(x => x.Id == this.EOBIId).FirstOrDefaultAsync();
                if (eobi  != null)
                {
                    eobi.EOBIName = this.EOBIName;
                    eobi.ValueTypeId = this.ValueTypeId;
                    eobi.EOBIValue = this.EOBIValue;
                    eobi.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    eobi.EditedOn = DateTime.Now;
                    eobi.CategoryId = CategoryId;
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "EOBIModel", "EOBI_Edit");

                return false;

            }
        }
        public static IList<SelectListItem> GetCategories()
        {
            var _db = new PakOman_NedoEntities();
            IList<SelectListItem> items =   _db.tbl_DecConCategory.Select(q => new SelectListItem { Text = q.DedConDesc, Selected = false, Value = q.DedConId.ToString() }).ToList();
            return items;
        }
    }
}