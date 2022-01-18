using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC
{
    [DashboardSession]
    public static class DataHelper
    {
        public static string Data { get; set; }
        public static long Empid { get; set; }
        public static string MsgSuccess { get; set; }
        public static string EmpimgUrl { get; set; }
        public static long SessionEmpID { get; set; }
        public static string SessionUserName { get; set; }
        public static string[] QuerystringData { get; set; }
        public static IList<string> emailsList { get; set; }
        public static IList<long> EmpList { get; set; }
        public static bool IsNearExpire { get; set; }
        public static int DaysExpireRemaining { get; set; }
        public static bool IsAnyNoticiation { get; set; }
        public static string MessageForNotication { get; set; }
        public static string DateFrom { get; set; }
        public static string Dateto { get; set; }
        public static bool isWish { get; set; }
        public static string isWishDetail { get; set; }
        public static string isWishTitle { get; set; }
        public static tbl_IconControl IconSession { get; set; }
        public static bool SuperAdmin { get; set; }


        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        public static PakOman_NedoEntities _db = new PakOman_NedoEntities();


        public class FileTypesAttribute : ValidationAttribute
        {
            private readonly List<string> _types;

            public FileTypesAttribute(string types)
            {
                _types = types.Split(',').ToList();
            }

            public override bool IsValid(object value)
            {
                if (value == null) return true;

                var fileExt = System.IO
                                    .Path
                                    .GetExtension((value as
                                             HttpPostedFileBase).FileName).Substring(1);
                return _types.Contains(fileExt, StringComparer.OrdinalIgnoreCase);
            }

            public override string FormatErrorMessage(string name)
            {
                return string.Format("Invalid file type. Only the following types {0} are supported.", String.Join(", ", _types));
            }
        }
        public static void _createCookie()
        {
            HttpContext.Current.Response.Cookies["UserName"].Value = (HttpContext.Current.Session["UserName"]?.ToString());
            HttpContext.Current.Response.Cookies["UserId"].Value = (HttpContext.Current.Session["UserId"]?.ToString());
            HttpContext.Current.Response.Cookies["RoleID"].Value = (HttpContext.Current.Session["RoleID"]?.ToString());
            HttpContext.Current.Response.Cookies["SuperAdmin"].Value = (HttpContext.Current.Session["SuperAdmin"]?.ToString());
            HttpCookie aCookie = new HttpCookie("Last Visit");
            aCookie.Value = DateTime.Now.ToString();
            HttpCookie UserName = new HttpCookie("UserName");
            UserName.Value = (HttpContext.Current.Session["UserName"]?.ToString());
            HttpCookie UserId = new HttpCookie("UserId");
            UserId.Value = (HttpContext.Current.Session["UserId"]?.ToString());
            HttpCookie RoleID = new HttpCookie("RoleID");
            RoleID.Value = (HttpContext.Current.Session["RoleID"]?.ToString());
            HttpCookie SuperAdmin = new HttpCookie("SuperAdmin");
            SuperAdmin.Value = (HttpContext.Current.Session["SuperAdmin"]?.ToString());
            aCookie.Expires = DateTime.Now.AddMinutes(20);
            UserName.Expires = DateTime.Now.AddMinutes(20);
            UserId.Expires = DateTime.Now.AddMinutes(20);
            RoleID.Expires = DateTime.Now.AddMinutes(20);
            SuperAdmin.Expires = DateTime.Now.AddMinutes(20);
            HttpContext.Current.Response.Cookies.Add(aCookie);
            HttpContext.Current.Response.Cookies.Add(UserName);
            HttpContext.Current.Response.Cookies.Add(UserId);
            HttpContext.Current.Response.Cookies.Add(RoleID);
            HttpContext.Current.Response.Cookies.Add(SuperAdmin);
        }
        public static void _deleteCookie()
        {
            HttpContext.Current.Response.Cookies["UserName"].Value = "";
            HttpContext.Current.Response.Cookies["UserId"].Value = "";
            HttpContext.Current.Response.Cookies["RoleID"].Value = "";
            HttpContext.Current.Response.Cookies["SuperAdmin"].Value = "";
            HttpCookie aCookie = new HttpCookie("Last Visit");
            aCookie.Value = DateTime.Now.ToString();
            HttpCookie UserName = new HttpCookie("UserName");
            UserName.Value = (HttpContext.Current.Session["UserName"]?.ToString());
            HttpCookie UserId = new HttpCookie("UserId");
            UserId.Value = (HttpContext.Current.Session["UserId"]?.ToString());
            HttpCookie SuperAdmin = new HttpCookie("SuperAdmin");
            SuperAdmin.Value = (HttpContext.Current.Session["SuperAdmin"]?.ToString());
            HttpCookie RoleID = new HttpCookie("RoleID");
            RoleID.Value = (HttpContext.Current.Session["RoleID"]?.ToString());
            aCookie.Expires = DateTime.Now.AddMinutes(-300);
            UserName.Expires = DateTime.Now.AddMinutes(-300);
            UserId.Expires = DateTime.Now.AddMinutes(-300);
            SuperAdmin.Expires = DateTime.Now.AddMinutes(-300);
            RoleID.Expires = DateTime.Now.AddMinutes(-300);
            HttpContext.Current.Response.Cookies.Add(aCookie);
            HttpContext.Current.Response.Cookies.Add(UserName);
            HttpContext.Current.Response.Cookies.Add(UserId);
            HttpContext.Current.Response.Cookies.Add(SuperAdmin);
            HttpContext.Current.Response.Cookies.Add(RoleID);
        }
        public static bool Set_Session(UserMaster user)
        {

            HttpContext.Current.Session["UserName"] = user.UserName;
            HttpContext.Current.Session["EmailAdd"] = user.EmpMaster.EmailAdd;
            HttpContext.Current.Session["SuperAdmin"] = user.IsSuperUser;
            HttpContext.Current.Session["RoleID"] = user.RoleID;
            HttpContext.Current.Session["IsLoggedIn"] = true;
            HttpContext.Current.Session["UserId"] = user.Empid;
            SessionEmpID = long.Parse(HttpContext.Current.Session["UserId"].ToString());
            SessionUserName = HttpContext.Current.Session["UserName"].ToString();
            HttpContext.Current.Session["EmpName"] = user.EmpMaster.EmpName;
            HttpContext.Current.Session["EmpImg"] = user.EmpMaster.EmpImg;
            return true;

        }
        public static bool delete_Session()
        {
            HttpContext.Current.Session["SuperAdmin"] = "";
            HttpContext.Current.Session["EmailAdd"] = "";
            HttpContext.Current.Session["IsLoggedIn"] = "";
            HttpContext.Current.Session["UserName"] = "";
            HttpContext.Current.Session["RoleID"] = "";
            SessionEmpID = 0;
            HttpContext.Current.Session["UserId"] = "";
            HttpContext.Current.Session["EmpName"] = "";
            HttpContext.Current.Session["EmpImg"] = "";

            return true;

        }
        public static IList<SelectListItem> _getSecurityDeposits()
        {

            IList<SelectListItem> items = (from q in _db.tbl_SecurityDepositMaster
                                           select new SelectListItem
                                           {
                                               Text = q.DepositName,
                                               Selected = false,
                                               Value = q.SecurityDepositId.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getAllowances()
        {

            IList<SelectListItem> items = (from q in _db.AllownceMasters
                                           select new SelectListItem
                                           {
                                               Text = q.AllownceName,
                                               Selected = false,
                                               Value = q.Id.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getLatePolicies()
        {

            IList<SelectListItem> items = (from q in _db.tbl_LatePolicyMaster
                                           select new SelectListItem
                                           {
                                               Text = q.LatePolicName,
                                               Selected = false,
                                               Value = q.LatePolicyId.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getBonuses()
        {

            IList<SelectListItem> items = (from q in _db.tbl_BonusMaster
                                           select new SelectListItem
                                           {
                                               Text = q.BonusName,
                                               Selected = false,
                                               Value = q.Id.ToString()
                                           }).ToList();

            return items;

        }

        public static IList<SelectListItem> _getEOBI()
        {

            IList<SelectListItem> items = (from q in _db.tbl_EOBIMaster
                                           select new SelectListItem
                                           {
                                               Text = q.EOBIName,
                                               Selected = false,
                                               Value = q.Id.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getBanks()
        {

            IList<SelectListItem> items = (from q in _db.tbl_BankMaster
                                           select new SelectListItem
                                           {
                                               Text = q.Bank_Name,
                                               Selected = false,
                                               Value = q.Id.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getProvidentFund()
        {

            IList<SelectListItem> items = (from q in _db.tbl_ProvidentFundMaster
                                           select new SelectListItem
                                           {
                                               Text = q.ProvidentFundName,
                                               Selected = false,
                                               Value = q.Id.ToString()
                                           }).ToList();

            return items;

        }

        public static IList<SelectListItem> _getDeductions()
        {

            IList<SelectListItem> items = (from q in _db.DeductionMasters
                                           select new SelectListItem
                                           {
                                               Text = q.DeductionName,
                                               Selected = false,
                                               Value = q.Id.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getLoanStatus()
        {

            IList<SelectListItem> items = (from q in _db.tbl_LoanStatusMaster
                                           select new SelectListItem
                                           {
                                               Text = q.LoanStatusDesc,
                                               Selected = false,
                                               Value = q.LoanStatusId.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getPaymentStatus()
        {

            IList<SelectListItem> items = (from q in _db.tbl_PaymentStatusMaster
                                           select new SelectListItem
                                           {
                                               Text = q.PaymentStatusDesc,
                                               Selected = false,
                                               Value = q.PaymentStatusId.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> GetPolicyName()
        {



            using (var _db = new PakOman_NedoEntities())
            {


                List<SelectListItem> data = (from ep in _db.tblPolicyTypes
                                             select new SelectListItem
                                             {
                                                 Text = ep.PolicyName,
                                                 Value = ep.ID.ToString(),
                                                 Selected = false
                                             }).ToList();


                return data;
            }





        }
        public static IList<SelectListItem> GetAutoEmailName()
        {



            using (var _db = new PakOman_NedoEntities())
            {


                List<SelectListItem> data = (from ep in _db.tbl_AutoEmailTypes
                                             where ep.isActive == true
                                             select new SelectListItem
                                             {
                                                 Text = ep.Name,
                                                 Value = ep.ID.ToString(),
                                                 Selected = false
                                             }).ToList();


                return data;
            }





        }
        public static IList<SelectListItem> GetAllLeaves()
        {



            using (var _db = new PakOman_NedoEntities())
            {


                List<SelectListItem> data = (from ep in _db.LeaveSetups
                                             select new SelectListItem
                                             {
                                                 Text = ep.LeaveDsc,
                                                 Value = ep.ID.ToString(),
                                                 Selected = false
                                             }).ToList();


                return data;
            }




        }
        public static IList<SelectListItem> _getInoutType()
        {

            IList<SelectListItem> items = (from ep in _db.InOutTypes
                                           select new SelectListItem
                                           {
                                               Text = ep.Type,
                                               Value = ep.ID.ToString(),
                                               Selected = false
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getCountry()
        {

            IList<SelectListItem> items = (from q in _db.CountryMasters
                                           select new SelectListItem
                                           {
                                               Text = q.CountryName,
                                               Selected = false,
                                               Value = q.ID.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getCity()
        {

            IList<SelectListItem> items = (from q in _db.CityMasters
                                           select new SelectListItem
                                           {
                                               Text = q.CityDesc,
                                               Selected = false,
                                               Value = q.CityId.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getDesignation()
        {

            IList<SelectListItem> items = (from q in _db.DesignationMasters
                                           select new SelectListItem
                                           {
                                               Text = q.DesignationDesc,
                                               Selected = false,
                                               Value = q.DesignationId.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getDepartment()
        {

            IList<SelectListItem> items = (from q in _db.DepartmentMasters
                                           select new SelectListItem
                                           {
                                               Text = q.DepartmentDesc,
                                               Selected = false,
                                               Value = q.DepartmentId.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getValueTypes()
        {
            IList<SelectListItem> items = (from q in _db.ValueTypes
                                           select new SelectListItem { Text = q.ValueTypeName, Selected = false, Value = q.ValueTypeId.ToString() }).ToList();
            return items;
        }

        public static IList<SelectListItem> _getAccountTypes()
        {
            IList<SelectListItem> items = (from q in _db.tbl_AccountType
                                           select new SelectListItem { Text = q.Name, Selected = false, Value = q.Id.ToString() }).ToList();
            return items;
        }
        public static IList<SelectListItem> _getMartialStatus()
        {

            IList<SelectListItem> items = (from q in _db.MartialStatus
                                           select new SelectListItem
                                           {
                                               Text = q.MartialStatusDesc,
                                               Selected = false,
                                               Value = q.MartialStatusId.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getCompany()
        {

            IList<SelectListItem> items = (from q in _db.CompanyMasters
                                           select new SelectListItem
                                           {
                                               Text = q.CompanyDesc,
                                               Selected = false,
                                               Value = q.CompanyId.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getGrade()
        {

            IList<SelectListItem> items = (from q in _db.GradeMasters
                                           select new SelectListItem
                                           {
                                               Text = q.GradeDesc,
                                               Selected = false,
                                               Value = q.GradeId.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getEmployeeType()
        {

            IList<SelectListItem> items = (from q in _db.EmployeeTypes
                                           select new SelectListItem
                                           {
                                               Text = q.EmployeeTypeDsc,
                                               Selected = false,
                                               Value = q.ID.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getEmployee()
        {

            IList<SelectListItem> items = (from q in _db.EmpMasters
                                           where q.ActiveInActive == false
                                           select new SelectListItem
                                           {
                                               Text = q.EmpName,
                                               Selected = false,
                                               Value = q.EmpId.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getEmployeeStatus()
        {

            IList<SelectListItem> items = (from q in _db.tbl_EmployeeStatus
                                           where q.IsActive == true
                                           select new SelectListItem
                                           {
                                               Text = q.EmployeeStatus,
                                               Selected = false,
                                               Value = q.Id.ToString()
                                           }).ToList();

            return items;

        }

        public static IList<SelectListItem> _getEmployeePF()
        {

            IList<SelectListItem> items = (from q in _db.EmpMasters
                                           join r in _db.tbl_ProvidentFundPosting on q.EmpId equals r.EmpId
                                           where q.ActiveInActive == false
                                           select new SelectListItem
                                           {
                                               Text = q.EmpName,
                                               Selected = false,
                                               Value = q.EmpId.ToString()
                                           }).ToList();

            return items;

        }

        public static IList<SelectListItem> _getAllShift()
        {

            IList<SelectListItem> items = (from q in _db.ShiftMasters
                                           select new SelectListItem
                                           {
                                               Text = q.ShiftDesc,
                                               Selected = false,
                                               Value = q.ShiftId.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getAllRoaster()
        {

            IList<SelectListItem> items = (from q in _db.ShiftScheduleGeneralMasters
                                           select new SelectListItem
                                           {
                                               Text = q.ScheduleDesc,
                                               Selected = false,
                                               Value = q.PKId.ToString()
                                           }).ToList();

            return items;

        }

        public static IList<ShiftMaster> _GetShifts()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                var data = (from q in _db.ShiftMasters
                            orderby q.ShiftDesc ascending
                            select q).ToList<ShiftMaster>();

                if (data != null)
                {
                    return data;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "DataHelper", "_GetShifts");
                return null;
            }
        }

        public static IList<SelectListItem> _getReason()
        {

            IList<SelectListItem> items = (from q in _db.tblReasons
                                           select new SelectListItem
                                           {
                                               Text = q.Reason,
                                               Selected = false,
                                               Value = q.ID.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getReports()
        {

            IList<SelectListItem> items = (from q in _db.tbl_Reports
                                           select new SelectListItem
                                           {
                                               Text = q.ReportsName,
                                               Selected = false,
                                               Value = q.ID.ToString()
                                           }).ToList();

            return items;

        }
        public static IList<SelectListItem> _getAttendanceReports()
        {

            IList<SelectListItem> items = (from q in _db.tbl_Reports
                                           where q.CategoryId == 1
                                           select new SelectListItem
                                           {
                                               Text = q.ReportsName,
                                               Selected = false,
                                               Value = q.ID.ToString()
                                           }).ToList();

            return items;

        }

        public static IList<SelectListItem> _getPayrollReports()
        {

            IList<SelectListItem> items = (from q in _db.tbl_Reports
                                           where q.CategoryId == 2
                                           select new SelectListItem
                                           {
                                               Text = q.ReportsName,
                                               Selected = false,
                                               Value = q.ID.ToString()
                                           }).ToList();

            return items;

        }

        public static IList<SelectListItem> _getRoles()
        {

            IList<SelectListItem> items = (from q in _db.tbl_Role
                                           select new SelectListItem
                                           {
                                               Text = q.RoleName,
                                               Selected = false,
                                               Value = q.ID.ToString()
                                           }).ToList();

            return items;

        }

        public static IList<SelectListItem> _getManagerLevel()
        {
            IList<SelectListItem> items = (from q in _db.tbl_ManagerLevel
                                           select new SelectListItem
                                           {
                                               Text = q.LevelName,
                                               Selected = false,
                                               Value = q.ID.ToString()
                                           }).ToList();

            return items;

        }

        public static IList<SelectListItem> _getLateType()
        {
            int id = int.Parse(HttpContext.Current.Session["UserId"].ToString());
            IList<SelectListItem> items = (from q in _db.tbl_LateAttendnaceMaster
                                           where q.IsDeleted == false //&& q.Deduct_Percent > 0
                                           select new SelectListItem
                                           {
                                               Text = q.Deduct_Percent.ToString(),
                                               Selected = false,
                                               Value = q.Id.ToString()
                                           }).ToList();

            return items;

        }


        public static IList<SelectListItem> _getEmpLeaveType()
        {
            int id = int.Parse(HttpContext.Current.Session["UserId"].ToString());
            IList<SelectListItem> items = (from q in _db.Nstp_DdlLeaveRecord_By_ID(id)
                                           select new SelectListItem
                                           {
                                               Text = q.LeaveDsc + "-" + q.Balance,
                                               Selected = false,
                                               Value = q.ID.ToString()
                                           }).ToList();

            return items;

        }

        public static bool InsertNotification(string NotificationTitle, string NotificationDetail, string NotificationLink, long NotifyFor)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                tbl_Notifications obj = new tbl_Notifications();
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                obj.NotificationDetail = NotificationDetail;
                obj.NotificationTitle = NotificationTitle;
                obj.NotificationLink = NotificationLink;
                obj.NotifyFor = NotifyFor;
                obj.CreatedByID = long.Parse(HttpContext.Current.Session["UserId"].ToString());
                obj.IsAdminRequest = bool.Parse(HttpContext.Current.Session["SuperAdmin"].ToString()) == true ? true : false;
                db.tbl_Notifications.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "DataHelper", "InsertNotification");
                throw;
            }



        }
        public static bool InsertNotification(string NotificationTitle, string NotificationDetail, string NotificationLink, long NotifyFor, string SessionUserName, string SessionUserId, string SessionSuperAdmin)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                tbl_Notifications obj = new tbl_Notifications();
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = SessionUserName;
                obj.NotificationDetail = NotificationDetail;
                obj.NotificationTitle = NotificationTitle;
                obj.NotificationLink = NotificationLink;
                obj.NotifyFor = NotifyFor;
                obj.CreatedByID = long.Parse(SessionUserId);
                obj.IsAdminRequest = bool.Parse(SessionSuperAdmin) == true ? true : false;
                db.tbl_Notifications.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, SessionUserName, "DataHelper", "InsertNotification");
                throw;
            }



        }
        public static IList<SelectListItem> _getTemplateName()
        {

            IList<SelectListItem> items = (from q in _db.tbl_TemplateName
                                           select new SelectListItem
                                           {
                                               Text = q.TemplateName,
                                               Selected = false,
                                               Value = q.ID.ToString()
                                           }).ToList();

            return items;

        }

        public static IList<tbl_Circular> getCircular()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = db.tbl_Circular.Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now && x.StartDate <= x.EndDate && x.IsActive == true).ToList();
                if (data.Count > 0)
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }



        }

        //created  by moiez
        public static IList<string> _getEmpEmails(long[] empids)
        {
            IList<string> items = (from x in _db.EmpMasters
                                   where empids.Contains(x.EmpId) && x.EmailAdd != null
                                   select x.EmailAdd).ToList();
            return items;

        }
        //created  by moiez
        public static string _getEmpEmails(long empids)
        {
            string items = (from x in _db.EmpMasters
                            where empids == (x.EmpId) && x.EmailAdd != null
                            select x.EmailAdd).FirstOrDefault();
            return items;

        }
        public static IList<SelectListItem> _getExemptTypes()
        {
            IList<SelectListItem> items = (from x in _db.tbl_ExemptTypes
                                           where x.IsActive.Value
                                           select new SelectListItem
                                           {
                                               Text = x.ExemptType,
                                               Selected = false,
                                               Value = x.Id.ToString()
                                           }).ToList();


            return items;

        }
        public static IList<SelectListItem> _getMonthNames()
        {
            IList<SelectListItem> items = new List<SelectListItem>{
                new SelectListItem { Text = "Jan",Selected = false,Value ="1"},
                new SelectListItem { Text = "Feb",Selected = false,Value ="2"},
                new SelectListItem { Text = "Mar",Selected = false,Value ="3"},
                new SelectListItem { Text = "Apr",Selected = false,Value ="4"},
                new SelectListItem { Text = "May",Selected = false,Value ="5"},
                new SelectListItem { Text = "Jun",Selected = false,Value ="6"},
                new SelectListItem { Text = "Jul",Selected = false,Value ="7"},
                new SelectListItem { Text = "Aug",Selected = false,Value ="8"},
                new SelectListItem { Text = "Sep",Selected = false,Value ="9"},
                new SelectListItem { Text = "Oct",Selected = false,Value ="10"},
                new SelectListItem { Text = "Nov",Selected = false,Value ="11"},
                new SelectListItem { Text = "Dec",Selected = false,Value ="12"},
            };



            return items;

        }

        public static IList<string> _getEmpEmailsWithID(long[] empids)
        {
            IList<string> items = (from x in _db.EmpMasters
                                   where empids.Contains(x.EmpId) && x.EmailAdd != null
                                   select x.EmailAdd + "," + x.EmpId.ToString()).ToList();
            return items;

        }

        public static bool CheckMac(string MAC)
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            List<string> MACs = new List<string>();
            foreach (var item in nics)
            {
                string abc = item.Id.Replace('{', ' ');
                abc = abc.Replace('}', ' ');
                MACs.Add(Environment.MachineName + abc.Trim());
            }
            if (MACs.Contains(MAC.ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string GetCurrency()
        {
            //var db = new PakOman_NedoEntities();
            //var value = db.tbl_Payroll_Options.FirstOrDefault().Curreny;
            //return value;
            return "";
        }
    }
}


