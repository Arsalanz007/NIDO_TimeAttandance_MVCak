using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC
{
    public class DashboardSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (HttpContext.Current.Session["UserId"] == null || HttpContext.Current.Session["RoleID"] == null || HttpContext.Current.Session["RoleID"] == null || HttpContext.Current.Session["SuperAdmin"] == null || HttpContext.Current.Session["SuperAdmin"] == null || HttpContext.Current.Session["Icon"] == null)
                {

                    filterContext.Result = new RedirectResult("/Login/Index");

                }
                else
                {
                    //NotificationComponents NC = new NotificationComponents(); 
                    //NC.RegisterNotification(DataHelper.SessionEmpID);

                    base.OnActionExecuting(filterContext);
                }
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("/Login/Index");

            }

        }
    }
    public class MACRestricted : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        
        {
            try

            {
                string sSecurityData = "";
                if (File.Exists(filterContext.HttpContext.Server.MapPath(@"\bin\Nedo.ini")) == true)
                {
                    StreamReader sr = new StreamReader(filterContext.HttpContext.Server.MapPath(@"\bin\Nedo.ini"));
                    sSecurityData = sr.ReadToEnd();
                    if (sSecurityData != "")
                    {
                        sSecurityData = EncryptDecrypt.DecryptEX(sSecurityData, true);
                        if (sSecurityData.Substring(0, 1) != "@")
                        {
                            sr.Close();
                            sr = null;
                            return;
                        }
                        else
                        {
                            sSecurityData = sSecurityData.Substring(1);
                            string[] sData = sSecurityData.Split(new Char[] { '@' });
                            if (DataHelper.CheckMac(sData[0]))
                            {
                                if (DateTime.Now.Date <= Convert.ToDateTime(sData[2]))
                                {
                                    DataHelper.DaysExpireRemaining = (int)(Convert.ToDateTime(sData[2]) - DateTime.Now).TotalDays;
                                    if (DataHelper.DaysExpireRemaining <= 30)
                                    {
                                        DataHelper.IsNearExpire = true;
                                    }
                                }
                                else
                                {
                                     filterContext.Result = new RedirectResult("/Error/Error_404");
                                    return ;
                                }
                            }
                            else
                            {
                                filterContext.Result = new RedirectResult("/Error/Error_404");
                                return;
                            }

                        }
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Error/Error_404");
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}

