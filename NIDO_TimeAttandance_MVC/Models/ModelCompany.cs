using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelCompany
    {
        public int CompanyId { get; set; }
        public string CompanyDesc { get; set; }
        public string CompanyAdd { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyFax { get; set; }
        public string CompanyWeb { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public async Task<IList<ModelCompany>> GetCompany()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelCompany> List = await (from q in _db.CompanyMasters
                                                      select new ModelCompany
                                                      {
                                                          CompanyAdd = q.CompanyAdd,
                                                          CompanyDesc = q.CompanyDesc,
                                                          CompanyFax = q.CompanyFax,
                                                          CompanyId = q.CompanyId,
                                                          CompanyPhone = q.CompanyPhone,
                                                          CompanyWeb = q.CompanyWeb,
                                                      }
                                            ).ToListAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelCompany", "GetCompany");
                throw;
            }


        }

        public async Task<bool> DeleteCompany(int ID)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.CompanyMasters.Where(x => x.CompanyId == ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    _db.CompanyMasters.Remove(data);
                    await _db.SaveChangesAsync();

                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelCompany", "DeleteCompany");
                return false;
            }
        }

        public async Task<bool> EditComapny()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data =await _db.CompanyMasters.Where(x => x.CompanyId == this.CompanyId).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    data.CompanyAdd = this.CompanyAdd;
                    data.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.EditedOn = DateTime.Now;
                    data.CompanyDesc = this.CompanyDesc;
                    data.CompanyFax = this.CompanyFax;
                    data.CompanyPhone = this.CompanyPhone;
                    data.CompanyWeb = this.CompanyWeb;
                   await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelCompany", "EditComapny");
                return false;
            }
        }
        public async Task<bool> Company_Save()
        {
            try
            {
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                CompanyMaster ObjComp = new CompanyMaster();
                ObjComp.CompanyAdd = CompanyAdd;
                ObjComp.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                ObjComp.CreatedOn = DateTime.Now;
                ObjComp.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                ObjComp.EditedOn = DateTime.Now;
                ObjComp.CompanyDesc = CompanyDesc;
                ObjComp.CompanyFax = CompanyFax;
                ObjComp.CompanyPhone = CompanyPhone;
                ObjComp.CompanyWeb = CompanyWeb;
                _Db.CompanyMasters.Add(ObjComp);
                await _Db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelCompany", "Company_Save");
                return false;
            }

        }
        public async Task<ModelCompany> Get_Company_By_Id(long Id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelCompany List = await (from q in _db.CompanyMasters
                                               where q.CompanyId == Id
                                               select new ModelCompany
                                               {
                                                   CompanyAdd = q.CompanyAdd,
                                                   CompanyDesc = q.CompanyDesc,
                                                   CompanyFax = q.CompanyFax,
                                                   CompanyId = q.CompanyId,
                                                   CompanyPhone = q.CompanyPhone,
                                                   CompanyWeb = q.CompanyWeb,
                                               }
                                            ).SingleOrDefaultAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelCompany", "Get_Company_By_Id");
                return null;
            }

        }

    }
}