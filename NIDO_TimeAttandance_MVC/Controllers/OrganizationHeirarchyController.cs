using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Models;
using System.IO;


namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class OrganizationHeirarchyController : Controller
    {
        // GET: OrganizationHeirarchy
        public ActionResult Index()
        {

            try
            {
                OrganizationHeirarchy model = new OrganizationHeirarchy();
               // var data = model.Get();
                getHir();
                return View();

            }
            catch (Exception)
            {

                return null;
            }
        }

        [HttpGet]
        public ActionResult getHir()
        {
            OrganizationHeirarchy model = new OrganizationHeirarchy();
            var data = model.Get();
            data = data.Substring(1, data.Length - 2);
            string path = System.Web.HttpContext.Current.Server.MapPath("~");
            
            if (System.IO.File.Exists(Path.Combine(path, "output.json")))
            {  
                System.IO.File.Delete(Path.Combine(path, "output.json"));
            }
            System.IO.File.WriteAllText(path + "output.json", data);
            return Json(new { source = data, JsonRequestBehavior.AllowGet });
        }
    }
}