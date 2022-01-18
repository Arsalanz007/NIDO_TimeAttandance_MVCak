using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelDesignation
    {
        public string[] DesignationDescArray { get; set; }
        public int DesignationId { get; set; }
        public string DesignationDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }

        public async Task<IList<ModelDesignation>> GetDesignation()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelDesignation> List = await (from q in _db.DesignationMasters
                                                          select new ModelDesignation
                                                          {
                                                              DesignationId = q.DesignationId,
                                                              DesignationDesc = q.DesignationDesc,
                                                          }
                                            ).ToListAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDesignation", "GetDesignation");
                return null; ;
            }


        }

        public async Task<bool> DeleteDesignation(int ID)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.DesignationMasters.Where(x => x.DesignationId == ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    _db.DesignationMasters.Remove(data);
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDesignation", "DeleteDesignation");
                return false;

            }
        }

        public async Task<bool> EditDesignation()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.DesignationMasters.Where(x => x.DesignationId == this.DesignationId).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    data.DesignationDesc = this.DesignationDesc;
                    data.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.EditedOn = DateTime.Now;
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDesignation", "EditDesignation");
                return false;

            }
        }
        public async Task<bool> Designation_Save()
        {
            try
            {
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                DesignationMaster ObjDesig = new DesignationMaster();
                for (int i = 0; i < DesignationDescArray.Length; i++)
                {
                    ObjDesig.DesignationDesc = DesignationDescArray[i];
                    ObjDesig.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjDesig.CreatedOn = DateTime.Now;
                    ObjDesig.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjDesig.EditedOn = DateTime.Now;
                    _Db.DesignationMasters.Add(ObjDesig);
                    await _Db.SaveChangesAsync();
                }
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDesignation", "Designation_Save");
                return false;
            }

        }
        public async Task<ModelDesignation> Get_Designation_By_Id(long Id)
        {

            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelDesignation List = await (from q in _db.DesignationMasters
                                                   where q.DesignationId == Id
                                                   select new ModelDesignation
                                                   {
                                                       DesignationId = q.DesignationId,
                                                       DesignationDesc = q.DesignationDesc,
                                                   }).SingleOrDefaultAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDesignation", "Get_Designation_By_Id");
                return null;
            }

        }

    }
}