using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Threading;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelLogin
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public bool HasError { get; set; }
        public string Error { get; set; }
        public long UserID { get; set; }
        public UserMaster User { get; set; }
        public async Task<bool> CheckLogin()
        {
            try
            {
                string sPassword = EncryptDecrypt.EncryptEX(Password, true);
              

                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var User = await (from q in _db.UserMasters
                                  where q.UserName == this.Username && q.UserPwd == sPassword && q.IsActive == true
                                  select q).SingleOrDefaultAsync();

                if (User != null)
                {
                    DataHelper.Set_Session(User);
                    DataHelper._createCookie();
                    await  createLoginHistory();
                    return true;
                }
                else
                {
                    this.HasError = true;
                    this.Error = "Incorrect Password";
                    return false;
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLogin", "CheckLogin");
                this.HasError = true;
                this.Error = "Unexpected Error Occured";
                return false;
            }
        }
        public async Task<bool> createLoginHistory()
        {
            try
            {
                var a = HttpContext.Current.Request.UserHostAddress;
                string PCName = "";
                string Browser = "";
                string PC_Type = "";
                string strUserAgent = HttpContext.Current.Request.UserAgent.ToString().ToLower();
                if (strUserAgent != null)
                {
                    if (HttpContext.Current.Request.Browser.IsMobileDevice == true || strUserAgent.Contains("iphone") ||
                        strUserAgent.Contains("blackberry") || strUserAgent.Contains("mobile") ||
                        strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") ||
                        strUserAgent.Contains("palm"))
                    {
                        PC_Type = "Mobile";
                        PCName = HttpContext.Current.Request.Browser.MobileDeviceModel + "-" + HttpContext.Current.Request.Browser.MobileDeviceModel + "--" + HttpContext.Current.Request.Browser.Platform + "--";

                    }
                    else
                    {
                        PC_Type = "Desktop";
                        if (a != "103.245.193.163")
                        {
                            IPAddress myIP = IPAddress.Parse(a);
                            IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
                            List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
                            PCName = compName.First();
                        }
                    }
                }


                Browser = HttpContext.Current.Request.Browser.Browser + "--" + HttpContext.Current.Request.Browser.Version;

                using (PakOman_NedoEntities db = new PakOman_NedoEntities())
                {
                    tblUserLoginHistory tblUser = new tblUserLoginHistory();
                    tblUser.IP = a;
                    tblUser.LoginTime = DateTime.Now;
                    tblUser.UserName = HttpContext.Current.Session["UserName"].ToString();
                    tblUser.Status = "Online";
                    tblUser.System = PCName;
                    tblUser.Browser = Browser;
                    tblUser.PC_Type = PC_Type;
                    db.tblUserLoginHistories.Add(tblUser);
                    await db.SaveChangesAsync();
                    HttpContext.Current.Session["LoginId"] = tblUser.ID;
                }
            }
            catch (Exception ex)
            {

               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLogin", "createLoginHistory");
            }

            return true;
        }
        public async Task<bool> Logout()
        {
            using (PakOman_NedoEntities db = new PakOman_NedoEntities())
            {

                if (HttpContext.Current.Session["LoginId"] != null)
                {
                    long loginId = long.Parse(HttpContext.Current.Session["LoginId"].ToString());
                    var data = db.tblUserLoginHistories.Where(a => a.ID == loginId).FirstOrDefault();
                    if (data != null)
                    {
                        data.LogoutTime = DateTime.Now;
                        data.Status = "Offline";
                        await db.SaveChangesAsync();
                        HttpContext.Current.Session["LoginId"] = "";
                        DataHelper._deleteCookie();
                        DataHelper.delete_Session();
                    }
                }

            }

            return true;
        }
       
    }
}