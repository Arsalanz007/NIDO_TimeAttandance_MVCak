using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using Microsoft.Ajax.Utilities;
using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using System.Collections.Generic;
using System.Data;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            try
            {

                // PakOman_NedoEntities db = new PakOman_NedoEntities();             

                int ManagerID = int.Parse(HttpContext.Session["UserId"].ToString());
                var servics = new ModelServices();
                var home = new Home();
                clsEmployeeProfile cls = new clsEmployeeProfile();
                //var Data = await servics.GetDailyAttendanceStats(ManagerID);
                //var DepartmentWise = await servics.GetDailyAttendanceDepartmentWise(ManagerID);
                //var GazettedHolidays = await home.GetGazetted();
                //var UniqueGazetted = await Task.Run(() => GazettedHolidays.Select(m => m.Month_Name).Distinct().ToList());
                //var OnLeave = await cls.GetEmployeeListOnLeave();
                //var NewEmployees = await cls._GetNewJoiningEmployee();
                //var RequestStat = await servics.GetRequestStatus(ManagerID);
                //var birthdayAniversaryData = await servics.GetCompleteBirthdayAndAniversaryData();


                var GazettedHolidays = await home.GetGazetted();

                var DataTask = servics.GetDailyAttendanceStats(ManagerID);
                var DepartmentWiseTask = servics.GetDailyAttendanceDepartmentWise(ManagerID);
                var UniqueGazettedTask = Task.Run(() => GazettedHolidays.Select(m => m.Month_Name).Distinct().ToList());
                var OnLeaveTask = cls.GetEmployeeListOnLeave();
                var NewEmployeesTask = cls._GetNewJoiningEmployee();
                var RequestStatTask = servics.GetRequestStatus(ManagerID);
                var birthdayAniversaryDataTask = servics.GetCompleteBirthdayAndAniversaryData();

                await Task.WhenAll(DataTask, DepartmentWiseTask, UniqueGazettedTask, OnLeaveTask, NewEmployeesTask, RequestStatTask, birthdayAniversaryDataTask);

                var Data = await DataTask;
                var DepartmentWise = await DepartmentWiseTask;
                var UniqueGazetted = await UniqueGazettedTask;
                var OnLeave = await OnLeaveTask;
                var NewEmployees = await NewEmployeesTask;
                var RequestStat = await RequestStatTask;
                var birthdayAniversaryData = await birthdayAniversaryDataTask;

                ViewBag.NewEmployees = NewEmployees;

                if (Data.Count > 0)
                {
                    ViewBag.Stats = Data;
                    ViewBag.Gazetted = GazettedHolidays;
                    ViewBag.UniqueG = UniqueGazetted;
                    ViewBag.DeparmentWise = DepartmentWise;
                    ViewBag.OnLeave = OnLeave;
                }



                if (RequestStat != null)
                {
                    ViewBag.TotalRequest = RequestStat.Total;
                    ViewBag.Pending = RequestStat.Pending;
                }
                #region Birthday and Aniversary





                if (birthdayAniversaryData.WishMessage != null)
                {
                    ViewBag.WishMsg = birthdayAniversaryData.WishMessage.WishTemplate;
                }
                else
                {
                    ViewBag.WishMsg = birthdayAniversaryData.wishMsg;
                }
                ViewBag.EventsWeekMsg = birthdayAniversaryData.EventsWeekMsg;
                ViewBag.TodayBirthdays = birthdayAniversaryData.TodayBirthdays;
                ViewBag.TodayJobAnniversaries = birthdayAniversaryData.TodayJobAnniversaries;
                ViewBag.UpcomingEvents = birthdayAniversaryData.UpcomingEvents;
                ViewBag.TodayEvents = birthdayAniversaryData.WishCheckers; //await db.WishCheckers.Where(x => x.ActionDate == DateTime.Today).ToListAsync();



                #endregion


                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "HomeController", "Index");
                return Content("something goes Wrong");
            }

        }

        public async Task<ActionResult> Report()
        {
            return View();
        }
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = await Task.Run(() => db.Nstp_GetDailyAttaendanceDetail(DateTime.Now, id).ToList());
                ViewBag.Details = data;

                return PartialView("_DepartmentWiseDetail", ViewBag.Details);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "HomeController", "Details");
                return Content("something goes Wrong");
            }
        }

        public async Task<ActionResult> CheckRightforTrack(string ID)
        {
            try
            {
                ID = ID.Trim();
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var IsTrackIDValid = await db.tbl_Request.Where(x => x.ReqTrackingID == ID).FirstOrDefaultAsync();
                if (IsTrackIDValid != null)
                {
                    if (bool.Parse(HttpContext.Session["SuperAdmin"].ToString()))
                    {
                        //return success and go to track 

                        return Json(new { Status = "Success", Message = "Super Admin", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        var ManagerList = await (from e in db.EmpMasters
                                                 join man in db.tbl_Manager on e.DepartmentId equals man.DepartmentID
                                                 where e.EmpId == IsTrackIDValid.EmpID.Value
                                                 select man.ManagerID).ToListAsync();
                        long UserID = long.Parse(HttpContext.Session["UserId"].ToString());
                        if (ManagerList.Contains(UserID) == true)
                        {
                            // return success and go to track 
                            return Json(new { Status = "Success", Message = "Manager", JsonRequestBehavior.AllowGet });
                        }
                        else
                        {
                            if (UserID == IsTrackIDValid.EmpID)
                            {
                                // return success and go to track
                                return Json(new { Status = "Success", Message = "User", JsonRequestBehavior.AllowGet });
                            }
                            else
                            {
                                // return error and go to track
                                return Json(new { Status = "Error", Message = "You are not allowed to see detail on this Tracking ID.", JsonRequestBehavior.AllowGet });
                            }
                        }
                    }
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Sory Tracking ID is not Exist, Please enter valid Tracking ID", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "HomeController", "CheckRightforTrack");
                return Content("something goes Wrong");
            }
        }

        public async Task<JsonResult> GetNotificationContacts()
        {
            try
            {

                long empid = long.Parse(Session["UserId"].ToString());
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var list = await (from n in db.tbl_Notifications
                                  join ep in db.EmpMasters
                                  on n.CreatedByID equals ep.EmpId
                                  where n.NotifyFor == empid && n.IsSeen == false
                                  orderby n.CreatedDate descending
                                  select new
                                  {
                                      ep.EmpImg,
                                      n.NotificationTitle,
                                      n.NotificationLink,
                                      n.NotificationDetail,
                                      n.CreatedDate,
                                      n.IsAdminRequest,
                                      n.ID,
                                  }).ToListAsync();

                return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "HomeController", "GetNotificationContacts");
                return Json(new { Status = "Error", Message = "Sory Something Goes Wrong", JsonRequestBehavior.AllowGet });
            }
        }

        public async Task<ActionResult> IsSeenTrue(int id)
        {
            try
            {


                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = await db.tbl_Notifications.Where(x => x.ID == id).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.IsSeen = true;
                    data.SeenDateTime = DateTime.Now.ToString();
                    await db.SaveChangesAsync();
                }
                return new JsonResult { Data = "Success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "HomeController", "IsSeenTrue");
                return Content("Something goes Wrong");
            }
        }

        public async Task<ActionResult> GetAllNotifications()
        {
            try
            {


                Home model = new Home();
                ViewBag.Notifications = await model.GetNotifications();
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "HomeController", "GetAllNotifications");
                return Content("Something goes Wrong");
            }
        }


        public async Task<ActionResult> Testing(string empId)
        {
            var abc = ",21474,21437,1646,21438,";
            var dateFrom = "8/1/2020";

            var dateTo = "8/30/2020";

            var paySlip = new PaySlipMasterSheetModel();

            var list = new List<long>();
            list.AddRange(new List<long> { 21474, 21437, 1646, 21438 });
            //list.Add(Convert.ToInt64(empId));

            var result = await paySlip.GetPaySlipsMasterSheet(list.ToArray(), dateFrom, dateTo);
            ViewBag.AllLates = await paySlip.GetLates();
            ViewBag.DataMaster = result;
            return View();
        }
        public async Task<ActionResult> Testing1(string empId)
        {

            PakOman_NedoEntities db = new PakOman_NedoEntities();
            db.Database.CommandTimeout = 36000;

           var  empIds = "21446,21471,21469,21459,1646,21453,21470,21457,21448,21464,21461,21460,21472,21447,21449,21454,21444,21458,21466,21437,21474,21455,21450,21475,21478,21481,21480,21479,21473,21468,21462,21463,21465,21451,21467,21477,21476,21445,21440,21441,21442,21443,21456,21452,21438,21439";
            var data = await Task.Run(() => db.Nstp_MonthlyAttendance(',' + empIds + ',', Convert.ToDateTime("8/1/2020"), Convert.ToDateTime("8/31/2020")).OrderBy(c => c.AttDate).ToList());
            ViewBag.Data = data;
            //var query = data.DistinctBy(q => new
            //{
            //    q.EmpId,
            //    q.EmpCode,
            //    q.EmpName,
            //    q.DepartmentDesc,
            //    q.Total_Days,
            //    q.Weekends,
            //    q.Presents,
            //    q.late,
            //    q.Overtime,
            //    q.HalfDay,
            //    q.WorkingDays,
            //    q.Gazetted,
            //    q.Absent,
            //    q.TotalLates,
            //    q.late_Deduction,
            //    q.WorkingHours,
            //    q.DesignationDesc,
            //}).ToList();





            var a = OldDB.GeneratePaySlip(","+ empIds + ",", "8/1/2020", "8/31/2020");

            var list = new List<DataRow>();

            list = a.Tables[0].AsEnumerable().ToList();
            //foreach (DataRow item in a.Tables[0].Rows)
            //{

            //}
            var lateDeducts = new List<LateDeductViewModel>();
            foreach(DataRow item in a.Tables[1].Rows)
            {
                var newList = new LateDeductViewModel();
                newList.deduct = (double)item["Deduct_Amount"];
                newList.count = (int)item["Count"];
                newList.empId = (long)item["EmpId"];

                lateDeducts.Add(newList);

            }

            ViewBag.lates = lateDeducts;
            return View(a);
        }

        public ActionResult Test()
        {
            var datetime = Convert.ToDateTime("11/13/2021");
            return View();
        }
      
    }
}