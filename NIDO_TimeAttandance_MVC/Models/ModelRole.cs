using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelRole
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string[] RoleNameArr { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public async Task< IList<ModelRole>> GetRole()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelRole> List = await(from q in _db.tbl_Role
                                             orderby q.RoleName ascending
                                             select new ModelRole
                                             {
                                                 ID = q.ID,
                                                 RoleName = q.RoleName,
                                                 IsActive=q.IsActive,
                                             }
                                            ).ToListAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRole", "GetRole");
                return null;
            }


        }
        public async Task< bool> DeleteRole(int ID)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.tbl_Role.Where(x => x.ID == ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    _db.tbl_Role.Remove(data);
                  await  _db.SaveChangesAsync();

                }
                return true;


            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRole", "DeleteRole");
                return false;

            }
        }
        public async Task <bool> EditRole()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.tbl_Role.Where(x => x.ID == this.ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    data.RoleName = this.RoleName;
                    data.IsActive = IsActive;
                  await  _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRole", "EditRole");
                return false;

            }
        }
        public async Task < bool> Role_Save()
        {
            try
            {
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                tbl_Role ObjRole = new tbl_Role();
                for (int i = 0; i < RoleNameArr.Length; i++)
                {
                    ObjRole.RoleName = RoleNameArr[i];                   
                    ObjRole.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjRole.CreatedDate = DateTime.Now;
                    ObjRole.IsActive = true;
                    _Db.tbl_Role.Add(ObjRole);
                   await _Db.SaveChangesAsync();
                }
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRole", "Role_Save");
                return false;
            }

        }
        public async Task < ModelRole> Get_Role_By_Id(long Id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelRole List =await (from q in _db.tbl_Role
                                      where q.ID == Id
                                      select new ModelRole
                                      {
                                          ID = q.ID,
                                          RoleName = q.RoleName,
                                          IsActive = q.IsActive,
                                      }
                                            ).SingleOrDefaultAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRole", "Get_Role_By_Id");
                return null;
            }

        }

    }
}