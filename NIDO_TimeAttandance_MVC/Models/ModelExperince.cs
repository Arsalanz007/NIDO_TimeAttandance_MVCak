using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelExperince
    {
        public long EmpId { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int PKId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public string EditedOn { get; set; }



        public async Task<  IList<EmpExperienceDetail>> Get_Experince_By_Id(long id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                var data =await (from q in _db.EmpExperienceDetails where q.EmpId==id      
                            select q).ToListAsync<EmpExperienceDetail>();

                if (data != null)
                {
                    return data;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelExperince", "Get_Experince_By_Id");
                return null;
            }
        }

        

    }
}