using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using NIDO_TimeAttandance_MVC.Utilities;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class SalaryPostingModel
    {
        public async Task<bool> PublishPosting(string DateFrom, string DateTo)
        {
            try
            {
                var from = Convert.ToDateTime(DateFrom);
                var to = Convert.ToDateTime(DateTo);

                var db = new PakOman_NedoEntities();
                var listInDb = await db.tbl_SalaryPostingMaster.Where(c => c.FromDate == from.Date && c.TillDate == to).ToListAsync();
                if(listInDb == null || listInDb.Count == 0)
                {
                    return false;
                }
                var data = listInDb.Select(c => { c.IsPublish = true;
                    c.PublishDate = DateTime.Now;
                    return c; }).ToList();
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "SalaryPostingModel", "PublishPosting");
                return false;
            }
        }
    }
}