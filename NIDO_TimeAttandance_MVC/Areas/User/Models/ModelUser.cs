using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.User.Models
{
    public class ModelUser
    {
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }       
        public string MobileNo { get; set; }
        public string EmailAdd { get; set; }
        public string DOB { get; set; }
        public string Religion { get; set; }        
        public string AppointmentDate { get; set; }        
        public string CNICNo { get; set; }        
        public string Ref1Name { get; set; }
        public string Ref1Add { get; set; }
        public string Ref1HomeTel { get; set; }
        public string Ref1OfficeTel { get; set; }
        public string Ref1MobileNo { get; set; }
        public string Ref1EmailAdd { get; set; }
        public string Ref2Name { get; set; }
        public string Ref2Add { get; set; }
        public string Ref2HomeTel { get; set; }
        public string Ref2OfficeTel { get; set; }
        public string Ref2MobileNo { get; set; }
        public string Ref2EmailAdd { get; set; }
        public string EmgName { get; set; }
        public string EmgAdd { get; set; }
        public string EmgHomeTel { get; set; }
        public string EmgOfficeTel { get; set; }
        public string EmgMobileNo { get; set; }
        public string EmgEmailAdd { get; set; }        
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public string EmpImg { get; set; }
        public string CityDesc { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentDesc { get; set; }
        public string DesignationDesc { get; set; }
        public string GradeDesc { get; set; }
        public string EmployeeType { get; set; } 
        public int LeaveID { get; set; }
        public int LeaveAllowed { get; set; }
        public string LeaveDsc { get; set; }
        public string AttDay { get; set; }        

        public IList<ModelUser> EmpInfo()
        {
            try
            {
                long id = long.Parse(HttpContext.Current.Session["UserId"].ToString());
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelUser> data = db.EmpMasters.Where(m => m.EmpId == id).Select(m => new ModelUser
                {
                    EmpId=m.EmpId,
                    DepartmentDesc=m.DepartmentMaster.DepartmentDesc,
                    DesignationDesc=m.DesignationMaster.DesignationDesc,
                    EmpName=m.EmpName,
                    CompanyName= m.CompanyMaster.CompanyDesc,
                    EmployeeType= m.EmployeeType.EmployeeTypeDsc,
                    AppointmentDate = m.AppointmentDate,
                    CNICNo=m.CNICNo,
                    DOB=m.DOB,
                    EmailAdd=m.EmailAdd,



                }).ToList();
                return data;
            }
            catch (Exception )
            {

                return null;
            }


        }

    }
}