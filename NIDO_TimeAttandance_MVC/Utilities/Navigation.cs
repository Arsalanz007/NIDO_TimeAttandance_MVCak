using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC
{
    [DashboardSession]
    public static class Navigation
    {
        public static IEnumerable<tblMenu> NavList { get; set; }
        public static IList<tblMenu> GetNavigation()
        {
            try
            {

                if (HttpContext.Current.Session["SuperAdmin"] != null)
                {
                    bool IsSuperAdmin = bool.Parse(HttpContext.Current.Session["SuperAdmin"].ToString());
                    PakOman_NedoEntities _db = new PakOman_NedoEntities();
                    if (IsSuperAdmin == true)
                    {
                        var dataAdmin = (from q in _db.tblMenus
                                         orderby q.Order_by ascending where q.IsActive==true
                                         select q).ToList();
                        if (dataAdmin != null)
                        {
                            return dataAdmin;
                        }
                        return null;
                    }
                    else
                    {
                        int RoleID = int.Parse(HttpContext.Current.Session["RoleID"].ToString());
                        var data = (from M in _db.tblMenus
                                    join r in _db.tbl_UserRights on M.ID equals r.MenuID
                                    join tr in _db.tbl_Role on r.RoleId equals tr.ID
                                    where tr.ID == RoleID
                                    orderby M.Order_by ascending
                                    select M).ToList();

                        if (data != null)
                        {
                            return data;
                        }
                        return null;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "Navigation", "GetNavigation");
                return null;
            }
        }



        public static bool CheckSpecificRights(int RoleID, string controllerName, string methodName)
        {
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            tblMenu Menu = db.tblMenus.Where(q => q.ControllerName == controllerName && q.MethodName == methodName).FirstOrDefault();

            var result = (from M in db.tblMenus
                          join r in db.tbl_UserRights on M.ID equals r.MenuID
                          join tr in db.tbl_Role on r.RoleId equals tr.ID
                          where tr.ID == RoleID && M.ID == Menu.ID
                          orderby M.Order_by ascending
                          select M).ToList();
            if (result.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }


        }

    }
}