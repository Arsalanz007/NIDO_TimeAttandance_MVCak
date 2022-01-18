using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class Unit
    {
        public string type { get; set; }
        public string value { get; set; }
    }
    public class OrganizationHeirarchy
    {
        public string name { get; set; }
        public string imageUrl { get; set; }

        public string area { get; set; }
        public string profileUrl { get; set; }

        public string office { get; set; }
        public string tags { get; set; }

        public string isLoggedUser { get; set; }
        public Unit unit { get; set; }

        public string positionName { get; set; }
        public List<OrganizationHeirarchy> children { get; set; }


        public string Get ()
        {
            PakOman_NedoEntities context = new PakOman_NedoEntities();
            context.Nstp_Organizational_Heirarchy();
            var allEmployee = context.tbl_Organization_Heirarchy.ToList();

            var roots = allEmployee.Where(x => x.ParentID == null).ToList();
            List<EmployeeOrgano> output = new List<EmployeeOrgano>();
            string path = System.Web.HttpContext.Current.Server.MapPath("~");
            foreach (var root in roots)
            {
                var baby = new EmployeeOrgano() {

                    EmployeeID = root.empid.Value,
                    name = root.EmpName,
                    imageUrl = root.EmpImg == null || root.EmpImg == "" ?  "/Assests/Home/img/message/User_Circle.png" : root.EmpImg,
                    profileUrl = "#",
                    positionName = root.DesignationDesc,
                    area = root.DepartmentDesc,
                    office = "",
                    tags = "",
                    isLoggedUser = false,
                    unit = new Unit {type="buisness",value="buisness first" },
                    children = new List<EmployeeOrgano>()
                };
                output.Add(baby);
                AddChildren(baby, allEmployee);
            }

            var myJson = JsonConvert.SerializeObject(output);
            return myJson;
        }
        public static void AddChildren(EmployeeOrgano emp, List<tbl_Organization_Heirarchy> allEmployee)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~");
            var childrens = allEmployee.Where(x => x.ParentID == emp.EmployeeID).ToList();
            foreach (tbl_Organization_Heirarchy children in childrens)
            {
                var baby = new EmployeeOrgano()
                {
                    EmployeeID = children.empid.Value,
                    name = children.EmpName,
                    imageUrl = children.EmpImg == null || children.EmpImg == "" ?"/Assests/Home/img/message/User_Circle.png" : children.EmpImg ,
                    positionName = children.DesignationDesc,
                    profileUrl = "#",
                    area = children.DepartmentDesc,
                    office = "",
                    tags = "",
                    isLoggedUser = false,
                    unit = new Unit { type = "buisness", value = "buisness first" },
                    children = new List<EmployeeOrgano>() };
                emp.children.Add(baby);
                AddChildren(baby, allEmployee);
            }
        }

        public IList<tbl_Organization_Heirarchy> GetHeirarchy()
        {
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            int count = db.Nstp_Organizational_Heirarchy();
            var Heirarchy = (from x in db.tbl_Organization_Heirarchy orderby x.ParentID select x).ToList(); 
            return Heirarchy;
        }

        public OrganizationHeirarchy GetOrganizationHeirarchy(List<tbl_Organization_Heirarchy> data)
        {
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            return null;
        }

        

    }


    public class EmployeeOrgano
    {
        public string name { get; set; }
        public string imageUrl { get; set; }
        public string area { get; set; }
        public string profileUrl { get; set; }
        public string office { get; set; }
        public string tags { get; set; }
        public bool isLoggedUser { get; set; }
        public Unit unit { get; set; }
        public string positionName { get; set; }
        public int EmployeeID { get; set; }
        public List<EmployeeOrgano> children { get; set; }
    }





}