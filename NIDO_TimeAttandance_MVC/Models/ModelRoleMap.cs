using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelRoleMap
    {
        public long ID { get; set; }
        public int RoleID { get; set; }
        public string ControllerName { get; set; }
        public string MethodName { get; set; }
        public string ParentMenu { get; set; }
        public string Caption { get; set; }
        public int Order_by { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Icon { get; set; }
        public long ParentID { get; set; }

        public async  Task< IList<ModelRoleMap>> _getMenu()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelRoleMap> data =await (from q in db.tblMenus
                                                //where q.IsActive== true
                                            select new ModelRoleMap
                                            {
                                                ID = q.ID,
                                                Caption = q.Caption,
                                                ParentMenu = q.tblMenu2.Caption,
                                            }).ToListAsync();
                return data;


               
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRoleMap", "_getMenu");
                return null;
            }



        }

        // created by moiez
        public async Task< IList<Nstp_Get_RoleMap_Result>> _getMenu(int? RoleID)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data =await Task.Run(()=> db.Nstp_Get_RoleMap(RoleID).ToList());
                return data;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRoleMap", "_getMenu");
                return null;
            }
        }


        public async Task < bool> Role_Update(Array Menu, int RoleId)
        {
            try
            {
                #region Master                              
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                tbl_UserRights obj = new tbl_UserRights();
                var AlreadyExist = await Task.Run(()=> _Db.tbl_UserRights.Where(m => m.RoleId == RoleId).ToList());
                if (AlreadyExist.Count > 0)
                {
                    _Db.tbl_UserRights.RemoveRange(AlreadyExist);
                }
                foreach (var MenuID in Menu)
                {
                    obj.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    obj.CreatedDate = DateTime.Now;
                    obj.MenuID = long.Parse(MenuID.ToString());
                    obj.RoleId = RoleId;
                    _Db.tbl_UserRights.Add(obj);
                 await   _Db.SaveChangesAsync();
                }


                #endregion

                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRoleMap", "Role_Update");
                return false;
            }

        }

    }
}