using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelCircular
    {
        public long ID { get; set; }
        public string CircularText { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public async Task<bool> SaveCircular()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                tbl_Circular obj = new tbl_Circular();
                obj.CircularText = CircularText;
                obj.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                obj.CreatedDate = DateTime.Now;
                obj.EndDate = EndDate;
                obj.StartDate = StartDate;
                obj.IsActive = true;
                db.tbl_Circular.Add(obj);
                await db.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                return false;
            }



        }

        public async Task<IList<ModelCircular>> getCircular()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelCircular> data = await (from x in db.tbl_Circular
                                                   where x.IsActive == true
                                                   select new ModelCircular
                                                   {
                                                       CircularText = x.CircularText,
                                                       StartDate = x.StartDate,
                                                       EndDate = x.EndDate,
                                                       ID=x.ID,
                                                   }).ToListAsync();
                return data;
            }
            catch (Exception)
            {

                return null;
            }



        }

        public async Task<bool> UpdateCircular()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = await db.tbl_Circular.Where(x => x.ID == ID).FirstOrDefaultAsync();
                data.CircularText = CircularText;
                data.EndDate = EndDate;
                data.IsActive = true;
                data.StartDate = StartDate;
                await db.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                throw;
            }



        }
        public async Task<ModelCircular> getCircularBy_Id(int ID)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
               var data = await (from x in db.tbl_Circular
                                                   where x.IsActive == true && x.ID == ID
                                                   select new ModelCircular
                                                   {
                                                       ID = x.ID,
                                                       CircularText = x.CircularText,
                                                       StartDate = x.StartDate,
                                                       EndDate = x.EndDate,
                                                   }).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception )
            {

                throw;
            }



        }

    }
}