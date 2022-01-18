using NIDO_TimeAttandance_MVC.Controllers;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelCity
    {
        public int CityId { get; set; }
        public string CityDesc { get; set; }
        public long CountryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public string[] CityDscArray { get; set; }
        public string CountryName { get; set; }

        public async Task<IList<ModelCity>> GetCity()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelCity> List = await (from q in _db.CityMasters
                                                   orderby q.CityDesc ascending
                                                   select new ModelCity
                                                   {
                                                       CityId = q.CityId,
                                                       CityDesc = q.CityDesc,
                                                       CountryName = q.CountryMaster.CountryName,
                                                   }
                                            ).ToListAsync();
                    return List;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelCity", "GetCity");
                return null;
            }


        }
        public async Task<bool> DeleteCity(int ID)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.CityMasters.Where(x => x.CityId == ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    _db.CityMasters.Remove(data);
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelCity", "DeleteCity");
                return false;

            }
        }
        public async Task<bool> EditCity()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.CityMasters.Where(x => x.CityId == this.CityId).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    data.CityDesc = this.CityDesc;
                    data.CountryID = this.CountryId;
                    data.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.EditedOn = DateTime.Now;
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelCity", "EditCity");
                return false;

            }
        }
        public async Task<bool> City_Save()
        {
            try
            {
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                CityMaster ObjCity = new CityMaster();
                for (int i = 0; i < CityDscArray.Length; i++)
                {

                    ObjCity.CityDesc = CityDscArray[i];
                    ObjCity.CountryID = CountryId;
                    ObjCity.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjCity.CreatedOn = DateTime.Now;
                    ObjCity.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjCity.EditedOn = DateTime.Now;
                    _Db.CityMasters.Add(ObjCity);
                    await _Db.SaveChangesAsync();
                }
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelCity", "City_Save");
                return false;
            }

        }
        public async Task<ModelCity> Get_City_By_Id(long Id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {

                    ModelCity List = await (from q in _db.CityMasters
                                            where q.CityId == Id
                                            select new ModelCity
                                            {
                                                CityId = q.CityId,
                                                CityDesc = q.CityDesc,
                                                CountryId = q.CountryID,
                                            }
                                            ).SingleOrDefaultAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelCity", "Get_City_By_Id");
                return null;
            }

        }
    
    }
}