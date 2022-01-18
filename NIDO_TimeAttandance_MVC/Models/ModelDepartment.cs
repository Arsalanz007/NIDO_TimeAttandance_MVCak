using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelDepartment
    {
        public int DepartmentId { get; set; }
        public string DepartmentDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public string[] DepartmentDscArray { get; set; }

        public async Task<IList<ModelDepartment>> GetDepartment()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelDepartment> List = await (from q in _db.DepartmentMasters
                                                         select new ModelDepartment
                                                         {
                                                             DepartmentId = q.DepartmentId,
                                                             DepartmentDesc = q.DepartmentDesc,

                                                         }).ToListAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDepartment", "GetDepartment");
                return null;
            }
        }

        public async Task<bool> DeleteDepartment(int ID)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.DepartmentMasters.Where(x => x.DepartmentId == ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    _db.DepartmentMasters.Remove(data);
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDepartment", "DeleteDepartment");
                return false;

            }
        }

        public async Task<bool> EditDepartment()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.DepartmentMasters.Where(x => x.DepartmentId == this.DepartmentId).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    data.DepartmentDesc = this.DepartmentDesc;
                    data.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.EditedOn = DateTime.Now;
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDepartment", "EditDepartment");
                return false;
            }
        }
        public async Task<bool> Department_Save()
        {
            try
            {
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                DepartmentMaster ObjDept = new DepartmentMaster();
                for (int i = 0; i < DepartmentDscArray.Length; i++)
                {

                    ObjDept.DepartmentDesc = DepartmentDscArray[i];
                    ObjDept.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjDept.CreatedOn = DateTime.Now;
                    ObjDept.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjDept.EditedOn = DateTime.Now;
                    _Db.DepartmentMasters.Add(ObjDept);
                    await _Db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDepartment", "Department_Save");
                return false;
            }

        }
        public async Task<ModelDepartment> Get_Department_By_Id(long Id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelDepartment List = await (from q in _db.DepartmentMasters
                                                  where q.DepartmentId == Id
                                                  select new ModelDepartment
                                                  {
                                                      DepartmentId = q.DepartmentId,
                                                      DepartmentDesc = q.DepartmentDesc,
                                                  }).SingleOrDefaultAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDepartment", "Get_Department_By_Id");
                return null;
            }
        }

    }
}