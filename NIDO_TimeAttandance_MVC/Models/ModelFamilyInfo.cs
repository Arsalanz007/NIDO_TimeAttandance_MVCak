using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelFamilyInfo
    {
        public int PKId { get; set; }
        public int EmpId { get; set; }
        public string Relation { get; set; }        
        public string Name { get; set; }  
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }

        public async Task<IList<EmpFamilyDetail>> Get_FamilyDetails_By_Id(long id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                var data = await (from q in _db.EmpFamilyDetails
                            where q.EmpId == id
                            select q).ToListAsync<EmpFamilyDetail>();

                if (data != null)
                {
                    return data;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelFamilyInfo", "Get_FamilyDetails_By_Id");
                return null;
            }
        }
        
    }
}