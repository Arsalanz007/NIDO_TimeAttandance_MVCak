using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class Logo
    {
        public HttpPostedFileBase URL { get; set; }
        public string Iconpath { get; set; }
        public bool Dashboard { get; set; }
        public bool Login { get; set; }
        public bool Report { get; set; }
        public bool TitleBar { get; set; }
        public string path { get; set; }



        public bool Save( )
        {
            string filename = Path.GetFileName(URL.FileName);
            path = System.Web.HttpContext.Current.Server.MapPath("~");
            path += "\\Uploads\\Logo\\" + filename;
            URL.SaveAs(path);
            Iconpath = "/Uploads/Logo/" + filename;
            UploadFile();

            return true;

        }

        public bool UploadFile( )
        {

            PakOman_NedoEntities db = new PakOman_NedoEntities();

            var data = (from x in db.tbl_IconControl select x).SingleOrDefault();
            if (Dashboard)
            {
                data.Dashboard = Iconpath;
            }
            if (Report)
            {
                data.Report = "data:image/png;base64,"+Functions.GetBase64StringForImage(path);
            }
            if (TitleBar)
            {
                data.Title = Iconpath;
            }
            if (Login)
            {
                data.Login = Iconpath;
            }
            db.SaveChanges();

            return true;


        }
    }
}