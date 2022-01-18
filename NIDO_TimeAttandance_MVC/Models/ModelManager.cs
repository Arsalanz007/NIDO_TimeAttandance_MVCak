using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelManager
    {
        public long ID { get; set; }
        [Required(ErrorMessage = "Manager ID required")]
        public long ManagerID { get; set; }
        public string Title { get; set; }
        public string DepartmentName { get; set; }
        public string EmpName { get; set; }
        public int LevelID { get; set; }
        public string LevelName { get; set; }
        public int DepartmentID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        public async Task < IList<ModelManager>> GetDeptManager()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelManager> List =await (from q in _db.tbl_Manager
                                             select new ModelManager
                                             {
                                                DepartmentName=q.DepartmentMaster.DepartmentDesc,
                                                EmpName=q.EmpMaster.EmpName,
                                                Title=q.Title,
                                                LevelName=q.tbl_ManagerLevel.LevelName,
                                                ID=q.ID,

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
        public async Task< bool> Manager_Save(Array Department, int LevelID, int EmployeeID, string Title)
        {
            try
            {
                         
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                tbl_Manager obj = new tbl_Manager();
                foreach (var Dept in Department)
                {
                    obj.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    obj.CreatedDate = DateTime.Now;
                    obj.DepartmentID =int.Parse(Dept.ToString());
                    obj.IsActive = true;
                    obj.LevelID = LevelID;
                    obj.ManagerID = EmployeeID;
                    obj.Title = Title;
                    _Db.tbl_Manager.Add(obj);
                   await _Db.SaveChangesAsync();
                }
                return true;
               
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelManager", "Manager_Save");
                return false;
            }

        }
        public async Task<bool> ManagerDelete(int id)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.tbl_Manager.Where(x => x.ID == id).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    _db.tbl_Manager.Remove(data);
                    await _db.SaveChangesAsync();

                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelManager", "ManagerDelete");
                return false;
            }
        } 
    }
}