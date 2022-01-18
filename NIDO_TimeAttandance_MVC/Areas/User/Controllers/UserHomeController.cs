using Microsoft.Ajax.Utilities;
using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Areas.User.Controllers
{
    [DashboardSession]
    public class UserHomeController : Controller
    {
        public UserHomeController()
        {
            ViewBag.Navs = Navigation.GetNavigation();
           
        }
        public async Task <ActionResult> Index()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();  
                int Id = int.Parse(HttpContext.Session["UserId"].ToString());
                DataHelper.Empid = Id;
                // getting first and last day
                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                DateTime dateFrom = firstDayOfMonth; //Convert.ToDateTime("10/1/2019");
                DateTime dateTo = lastDayOfMonth;
                #region BasicInfo
                var data =await Task.Run(()=>  db.Nstp_GetDailyUserData(Id, dateFrom, dateTo).ToList());                
                var DistinctData = data.DistinctBy(a => new
                {
                    a.EmpId,
                    a.EmpCode,
                    a.AppointmentDate,
                    a.CNICNo,
                    a.DepartmentDesc,
                    a.DesignationDesc,
                    a.DOB,
                    a.EmailAdd,
                    a.EmpImg,
                    a.EmployeeTypeDsc,
                    a.EmpName,
                    a.EmpPermAdd,
                    a.FatherName,
                    a.Gender,
                    a.GradeDesc,
                    a.MobileNo,
                    a.Religion,
                    a.Leave,
                    a.No_Of_Absent,
                    a.No_Of_Late,
                    a.No_of_Present,
                    a.Workingdays,
                    a.GazettedHolidays
                }).ToList();
                ViewBag.BasicInfo = DistinctData;
                #endregion

                #region LeaveData
                ViewBag.LeaveData =await Task.Run(()=> db.Nstp_GetLeaveDataBy_ID(Id).ToList());
                #endregion

                #region MissingEntries
                var MissingEntriesData = await Task.Run(() => db.Nstp_GetMissingEntriesBy_ID(Id).ToList());
                ViewBag.UniqueColoumn = MissingEntriesData.Select(m => m.Remarks).Distinct();
                ViewBag.MissingEntries = MissingEntriesData;
                #endregion


                


            




                //DateTime dateTime = Convert.ToDateTime("11/28/2019");
                var CheckIn =await db.AttendancePostings.Where(x => x.EmpId == Id && x.AttDate == DateTime.Now).Select(x => x.TimeIn).FirstOrDefaultAsync();
                if (CheckIn != null)
                {
                    ViewBag.CheckInTime = Convert.ToDateTime(CheckIn.Value.ToString()).ToString("hh:mm");
                }

                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "UserHomeController", "Index");
                throw;
            }
        }
        public async Task<ActionResult> UpdateEmergencyContact(List<EmpMaster> EmergemcyContact)
        {
            try
            {

                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = await db.EmpMasters.Where(m => m.EmpId == DataHelper.Empid).FirstOrDefaultAsync();
                if (data != null)
                {
                    foreach (var item in EmergemcyContact)
                    {
                        data.EmgName = item.EmgName;
                        data.EmgEmailAdd = item.EmgEmailAdd;
                        data.EmgHomeTel = item.EmgHomeTel;
                        data.EmgMobileNo = item.EmgMobileNo;
                        data.EmgOfficeTel = item.EmgOfficeTel;
                        data.EmgAdd = item.EmgAdd;
                        data.EditedBy = HttpContext.Session["UserName"].ToString();
                        data.EditedOn = DateTime.Now.ToString();
                       await db.SaveChangesAsync();
                    }
                }
                return Json(new { Status = "Success" });
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "UserHomeController", "UpdateEmergencyContact");
                return Json(new { Status = "error" });
            }


        }

        public async Task<ActionResult> Profile()
        {
           
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            int Id = int.Parse(HttpContext.Session["UserId"].ToString());
            DataHelper.Empid = Id;
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            DateTime dateFrom = firstDayOfMonth;
            DateTime dateTo = lastDayOfMonth;
           
            var data = await  Task.Run(() =>db.Nstp_GetDailyUserData(Id, dateFrom, dateTo).ToList());
            var DistinctData = data.DistinctBy(a => new
            {
                a.EmpId,
                a.EmpCode,
                a.AppointmentDate,
                a.CNICNo,
                a.DepartmentDesc,
                a.DesignationDesc,
                a.DOB,
                a.EmailAdd,
                a.EmpImg,
                a.EmployeeTypeDsc,
                a.EmpName,
                a.EmpPermAdd,
                a.FatherName,
                a.Gender,
                a.GradeDesc,
                a.MobileNo,
                a.Religion,
                a.Leave,
                a.No_Of_Absent,
                a.No_Of_Late,
                a.No_of_Present,
                a.Workingdays,
                a.GazettedHolidays
            }).ToList();
            ViewBag.BasicInfo = DistinctData;
            var Experince = data.DistinctBy(a => new
            {
                a.Position,
                a.CompanyName,
                a.FromDate,
                a.ToTime,
            }).ToList();
            if (Experince.Select(a => a.Position).FirstOrDefault() != null)
            {
                ViewBag.EmployeeHistory = Experince;
            }
            var Education = data.DistinctBy(a => new
            {
                a.Degree,
                a.Institute,
                a.GradeOrGPA,
                a.CompletionYear,
            }).ToList();
            if (Education.Select(a => a.Degree).FirstOrDefault() != null)
            {
                ViewBag.Education = Education;
            }
            var Family = data.DistinctBy(a => new
            {
                a.Relation,
                a.Name,
            }).ToList();
            if (Family.Select(a => a.Name).FirstOrDefault() != null)
            {
                ViewBag.Family = Family;
            }
            var Emergency = data.DistinctBy(a => new
            {
                a.EmgName,
                a.EmgAdd,
                a.EmgEmailAdd,
                a.EmgHomeTel,
                a.EmgMobileNo,
                a.EmgOfficeTel,

            }).ToList();
            if (Emergency.Select(a => a.EmgName).FirstOrDefault() != null)
            {
                ViewBag.Emergency = Emergency;
            }


            // Get ManagerList
            var ManagerList =await (from EP in db.EmpMasters
                               join M in db.tbl_Manager on EP.DepartmentId equals M.DepartmentID
                               join E in db.EmpMasters on M.ManagerID equals E.EmpId
                               where EP.EmpId == Id
                               select E).ToListAsync();
            ViewBag.ManagerList = ManagerList;


            return View();
        }
        public async Task<JsonResult> GetRoastersByID()
        {
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            int id = int.Parse(HttpContext.Session["UserId"].ToString());
            db.Configuration.ProxyCreationEnabled = false;
            
            var events =await Task.Run(()=>  db.Nstp_GetRoaster_By_ID(id).ToList());
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpPost]
        public async Task <ActionResult> UpdatePic(ModelEmpProfile model)
        {
          
            if (ModelState.IsValid)
            {
                model.EmpId = int.Parse(HttpContext.Session["UserId"].ToString());
                model.UpdateOnlyPic = true;
                if (await model.UpdateEmployee())
                {
                    return Json(new { Status = "Success", Message = "Employee Profile successfully updated." });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Employee Profile not successfully updated.Please try again" });
                }
            }
            return View(model);
        }
    }
}