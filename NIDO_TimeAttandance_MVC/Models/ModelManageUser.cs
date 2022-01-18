using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelManageUser
    {
        public int UserId { get; set; }
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string RoleName { get; set; }
        public string EmpCode { get; set; }
        public string UserPwd { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public bool IsActive { get; set; }
        public long Empid { get; set; }
        public bool IsSuperUser { get; set; }


        public async Task< IList<ModelManageUser>> GetUsers()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelManageUser> List =await (from q in _db.UserMasters
                                                   orderby q.IsSuperUser ascending
                                                   select new ModelManageUser
                                                   {
                                                       UserId = q.UserId,
                                                       IsActive = q.IsActive,
                                                       IsSuperUser = q.IsSuperUser,
                                                       UserName = q.UserName,
                                                       UserFullName = q.EmpMaster.EmpName,
                                                       EmpCode = q.EmpMaster.EmpCode,
                                                       RoleName = q.tbl_Role.RoleName,
                                                   }
                                            ).ToListAsync();
                    return List;

                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelManageUser", "GetUsers");
                return null;
            }


        }
        public async Task < bool> DeleteUser(int ID)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.UserMasters.Where(x => x.UserId == ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    _db.UserMasters.Remove(data);
                  await  _db.SaveChangesAsync();

                }
                return true;


            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelManageUser", "DeleteUser");
                return false;

            }
        }
        public async Task < bool >EditUser()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data =await _db.UserMasters.Where(x => x.UserId == this.UserId).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    data.RoleID = RoleID;
                    data.UserName = UserName;
                    data.UserPwd = EncryptDecrypt.EncryptEX("123", true);
                    data.IsActive = IsActive;
                    data.IsSuperUser = IsSuperUser;
                    data.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.EditedOn = DateTime.Now;
                   await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelManageUser", "EditUser");
                return false;

            }
        }
        public async Task < bool> User_Save()
        {
            try
            {
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                UserMaster ObjUser = new UserMaster();
                ObjUser.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                ObjUser.CreatedOn = DateTime.Now;
                ObjUser.Empid = Empid;
                ObjUser.IsActive = true;
                ObjUser.IsSuperUser = IsSuperUser;
                ObjUser.RoleID = RoleID;
                ObjUser.UserName = UserName.Trim();
                ObjUser.UserPwd = EncryptDecrypt.EncryptEX("123", true);
                _Db.UserMasters.Add(ObjUser);
             await   _Db.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelManageUser", "User_Save");
                return false;
            }
        }
        public async Task < ModelManageUser >Get_User_By_Id(long Id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelManageUser List =await (from q in _db.UserMasters
                                      where q.UserId == Id
                                      select new ModelManageUser
                                      {
                                        IsActive=q.IsActive,
                                        IsSuperUser=q.IsSuperUser,
                                        RoleID=q.RoleID,
                                        UserId=q.UserId,
                                        UserName=q.UserName, 
                                        Empid=q.Empid,
                                      }
                                            ).SingleOrDefaultAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelManageUser", "Get_User_By_Id");
                return null;
            }

        }
    }
    }