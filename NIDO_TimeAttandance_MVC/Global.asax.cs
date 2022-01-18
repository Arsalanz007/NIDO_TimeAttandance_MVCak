using NIDO_TimeAttandance_MVC.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Linq;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string con = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //here in application start we will start sql dependancy
            //SqlDependency.Start(con);
            using (PakOman_NedoEntities db = new PakOman_NedoEntities())
            {
                try
                {

                    var setting = (from x in db.tbl_AutoEmailSetting select x).FirstOrDefault();
                    if (setting != null)
                    {
                        if (setting.AutoEmailState == true)
                        {
                            AutoEmail model = new AutoEmail();
                            model.AutoEmailServiceON();
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }


                //if (setting.AutoEmailState == true)
                //{


                //    AutoEmail model = new AutoEmail();
                //    model.AutoEmailServiceON();


                //}

            }
        }
            protected void Application_BeginRequest()
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
            }

            protected void Session_Start(object sender, EventArgs e)
            {


            }

            protected void Application_End()
            {
                SqlDependency.Stop(con);
            }
        }
    }


