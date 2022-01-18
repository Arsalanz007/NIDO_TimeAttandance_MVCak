using Microsoft.Ajax.Utilities;
using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using NIDO_TimeAttandance_MVC.Areas.Reports.Models;
using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Areas.Reports.Controllers
{
    [DashboardSession]
    public class ReportController : Controller
    {
        string Parameters = String.Empty;

        public ReportController()
        {
            ViewBag.Currency = DataHelper.GetCurrency();
        }
        public async Task<ActionResult> Index()
        {//moie\z
            clsEmployeeProfile model = new clsEmployeeProfile();
            if (bool.Parse(HttpContext.Session["SuperAdmin"].ToString()))
            {
                ViewBag.Employees = await model._GetEmployeePosting();
            }
            else
            {
                int id = int.Parse(HttpContext.Session["UserId"].ToString());
                ViewBag.Employees = await model._GetEmployeePosting(id);
            }
            ViewBag.BankList = DataHelper._getBanks();
            ViewBag.ReportsName = DataHelper._getReports();
            return View();
        }

        public async Task<ActionResult> AttendanceReports()
        {//moie\z


            Session["ReportQuery"] = null;
            clsEmployeeProfile model = new clsEmployeeProfile();
            if (bool.Parse(HttpContext.Session["SuperAdmin"].ToString()))
            {
                ViewBag.Employees = await model._GetEmployeePosting();
            }
            else
            {
                int id = int.Parse(HttpContext.Session["UserId"].ToString());
                ViewBag.Employees = await model._GetEmployeePosting(id);
            }
            ViewBag.BankList = DataHelper._getBanks();
            ViewBag.ReportsName = DataHelper._getAttendanceReports();
            return View("Index");
        }

        public async Task<ActionResult> PayrollReports()
        {//moie\z
            Session["ReportQuery"] = null;

            clsEmployeeProfile model = new clsEmployeeProfile();
            if (bool.Parse(HttpContext.Session["SuperAdmin"].ToString()))
            {
                ViewBag.Employees = await model._GetEmployeePosting();
            }
            else
            {
                int id = int.Parse(HttpContext.Session["UserId"].ToString());
                ViewBag.Employees = await model._GetEmployeePosting(id);
            }
            ViewBag.BankList = DataHelper._getBanks();
            ViewBag.ReportsName = DataHelper._getPayrollReports();
            return View("Index");
        }

        public async Task<ActionResult> Reporting(int? Id)
        {
            string abc = "";
            string[] QueryStringData = null;
            string dateFrom = "";
            string dateTo = "";

            //agar user apne profile se hit krta he to 
            if (Id != null)
            {
                string EmpID = HttpContext.Session["UserId"].ToString();
                abc = "," + EmpID + ",";
                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                dateFrom = firstDayOfMonth.ToString();
                dateTo = lastDayOfMonth.ToString();
            }
            //agar admin employess ki report nikale to 
            else
            {
                QueryStringData = (string[])Session["ReportQuery"];
                //Session["ReportParameters"] = reportQuery;
                //QueryStringData = Session["ReportQuery"];
                int ReportId = int.Parse(QueryStringData[3]);


                //dynamic ab123 = Session["ReportParameters"];

                
                // Created by moiez
                // Updated By Anas
                //start
                //Session["ReportQuery"] = QueryStringData;
                if (ReportId == 2)
                {
                    // ViewBag.QueryStringData = QueryStringData;
                    return RedirectToAction("GetLeaveReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 3)
                {
                    //DataHelper.QuerystringData = QueryStringData;
                    // ViewBag.QueryStringData = QueryStringData;
                    return RedirectToAction("GetAttendanceSummary", "Report", new { area = "Reports" });
                }
                if (ReportId == 4)
                {
                    //DataHelper.QuerystringData = QueryStringData;
                    // ViewBag.QueryStringData = QueryStringData;
                    return RedirectToAction("GetLateReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 5)
                {
                    //DataHelper.QuerystringData = QueryStringData;
                    // ViewBag.QueryStringData = QueryStringData;
                    return RedirectToAction("GetAbsentReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 6)
                {
                    //DataHelper.QuerystringData = QueryStringData;
                    // ViewBag.QueryStringData = QueryStringData;
                    return RedirectToAction("GetHalfDayReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 7)
                {
                    //DataHelper.QuerystringData = QueryStringData;
                    // ViewBag.QueryStringData = QueryStringData;
                    return RedirectToAction("GetEarlyMinReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 8)
                {
                    //DataHelper.QuerystringData = QueryStringData;
                    // ViewBag.QueryStringData = QueryStringData;
                    return RedirectToAction("GetTimeOutReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 9)
                {
                    //DataHelper.QuerystringData = QueryStringData;
                    // ViewBag.QueryStringData = QueryStringData;
                    return RedirectToAction("GetTransaction", "Report", new { area = "Reports" });
                }
                if (ReportId == 10)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("Working", "Report", new { area = "Reports" });
                }
                if (ReportId == 11)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("OverTimeReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 12)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("ManualReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 13)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("getFullReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 14)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GetEmployeeRoaster", "Report", new { area = "Reports" });
                }
               

                if (ReportId == 15)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GeneratePaySlip", "Report", new { area = "Reports" });
                }
                if (ReportId == 1015)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("SalarySummaryReport", "Report", new { area = "Reports" });
                }

                if (ReportId == 1016)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GetLoanReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 1017)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GetAdvanceReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 2017)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GetLoanSummaryReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 3017)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GetSalaryTransferLetterReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 4017)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GetEmpIncomeTaxReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 5017)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GetFullAndFinalSettlementReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 6017)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GetSalaryIncrementReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 7018)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GetOverTimeReport", "Report", new { area = "Reports" });
                }
                if (ReportId == 8018)
                {
                    //DataHelper.QuerystringData = QueryStringData;
                
                    return RedirectToAction("GetTransactionWithLocation", "Report", new { area = "Reports" });
                }
                if (ReportId == 8019)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GetPaySlipMasterSheet", "Report", new { area = "Reports" });
                }

                if (ReportId == 8020)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GetDailyAttendace", "Report", new { area = "Reports" });
                }

                if (ReportId == 10018)
                {
                    //DataHelper.QuerystringData = QueryStringData;

                    return RedirectToAction("GetEmployeeHistoryReport", "Report", new { area = "Reports" });
                }


                // end
                string SelectedEmp = "";
                string[] Empid = QueryStringData[0].Split(',');

                foreach (var item in Empid)
                {
                    int id = int.Parse(item.ToString());
                    SelectedEmp = id + "," + SelectedEmp;
                }
                SelectedEmp = SelectedEmp.TrimEnd(',');
                abc = "," + SelectedEmp + ",";
                dateFrom = QueryStringData[1];
                dateTo = QueryStringData[2];
            }
            #region 
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            db.Database.CommandTimeout = 36000;
            var data = await Task.Run(() => db.Nstp_MonthlyAttendance(abc, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo)).OrderBy(a => a.AttDate).ToList());
            ViewBag.Data = data;
            var query = data.DistinctBy(q => new
            {
                q.EmpId,
                q.EmpCode,
                q.EmpName,
                q.DepartmentDesc,
                q.Total_Days,
                q.Weekends,
                q.Presents,
                q.Late,
                q.Overtime,
                q.HalfDay,
                q.WorkingDays,
                q.Gazetted,
                q.Absent,
                q.TotalLates,
                q.late_Deduction,
                q.WorkingHours,
                q.DesignationDesc,
            }).ToList();
            ViewBag.DataMaster = query;
            ViewBag.ViewName = "_Attandance";
            ViewBag.DateRange = "From " + Convert.ToDateTime(dateFrom).ToString("dd-MMM-yyy") + " To " + Convert.ToDateTime(dateTo).ToString("dd-MMM-yyy");
            return View();
            #endregion
        }

        [HttpPost]
        public ActionResult Generate(string data)
        {
            try
            {
                if(!String.IsNullOrEmpty(data))
                {
                    Session["ReportQuery"] = data.Split('?');

                    return Json(new { Status = "Success", URL = "/Reports/Report/Reporting" });
                

                }
                else
                {
                    return Json(new { Status = "Error" });
                }


            }
            catch (Exception ex)
            {

               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "Generate");
                return Json(new { Status = "Error" });
            }

        }
        public async Task<ActionResult> GeneratePaySlip()
        {
            try
            {
             //   string[] QueryStringData = DataHelper.QuerystringData;

                string[] QueryStringData = (string[])Session["ReportQuery"];

                string DateFrom = QueryStringData[1];
                string DateTo = QueryStringData[2];
                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                foreach (var item in sEmpids)
                {
                    employeeIds[index++] = Convert.ToInt64(item.Trim());
                }
                PaySlipModel psm = new PaySlipModel();

                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 36000;
                var data = await Task.Run(() => db.Nstp_MonthlyAttendancePaySlip(','+QueryStringData[0]+',', Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo)).OrderBy(x => Guid.NewGuid()).OrderBy(c => c.AttDate).ToList());
                ViewBag.Data = data;
               
                var tables = OldDB.GeneratePaySlip("," + QueryStringData[0] + ",", DateFrom, DateTo);

                var lateDeducts = new List<LateDeductViewModel>();
                foreach (DataRow item in tables.Tables[1].Rows)
                {
                    var newList = new LateDeductViewModel();
                    newList.deduct = (double)item["Deduct_Amount"];
                    newList.count = (int)item["Count"];
                    newList.empId = (long)item["EmpId"];

                    lateDeducts.Add(newList);

                }
                var leaves = new List<LeaveModel>();

                foreach(DataRow item in tables.Tables[2].Rows )
                {
                    var newLeave = new LeaveModel();
                    newLeave.EmpId = (long)item["EmpId"];
                    newLeave.LeaveId = (int)item["LeaveId"];
                    newLeave.LeaveAllowed = Convert.ToDouble(item["LeaveAllowed"].ToString().Trim());
                    newLeave.ApproveLeave = Convert.ToDouble(item["ApprovedLeave"].ToString().Trim());
                    newLeave.LeaveDesc = (string)item["LeaveDesc"];
                    leaves.Add(newLeave);
                }
                ViewBag.lates = lateDeducts;
                ViewBag.leaves = leaves;

                return View(tables);



            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "GeneratePaySlips");
               
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> GetEmployeeHistoryReport()
        {
            try
            {
                string[] QueryStringData = (string[])Session["ReportQuery"];

                string DateFrom = QueryStringData[1];
                string DateTo = QueryStringData[2];
                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                foreach (var item in sEmpids)
                {
                    employeeIds[index++] = Convert.ToInt64(item.Trim());
                }
                var tables = OldDB.GetEmployeeHistoryReport("," + string.Join(",",employeeIds[employeeIds.Length-1]) + ",", DateFrom, DateTo);
                List<int> yearArr = new List<int>();
                for (int i = 0; i < tables.Tables[1].Rows.Count; i++)
                {
                    if (!yearArr.Contains(Convert.ToInt32(tables.Tables[1].Rows[i]["years"]))) {
                        yearArr.Add(Convert.ToInt32(tables.Tables[1].Rows[i]["years"]));
                    }
                }
                ViewBag.years= yearArr;
                return View(tables);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetTransaction");
                throw;
            }

        }
        public async Task<ActionResult> GetPaySlipMasterSheet()
        {
            try
            {
                var model = new PaySlipMasterSheetModel();

              //  string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                string DateFrom = QueryStringData[1];
                string DateTo = QueryStringData[2];
                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                string SelectedEmp = "";
                foreach (var item in sEmpids)
                {
                    int id = int.Parse(item.ToString());
                    SelectedEmp = id + "," + SelectedEmp;
                }
                SelectedEmp = SelectedEmp.TrimEnd(',');
                SelectedEmp = "," + SelectedEmp + ",";
                var data = OldDB.GeneratePaySlip(SelectedEmp , DateFrom,DateTo);
                ViewBag.DataMaster = data.Tables[0];
               

                var lateDeducts = new List<LateDeductViewModel>();
                foreach (DataRow item in data.Tables[1].Rows)
                {
                    var newList = new LateDeductViewModel();
                    newList.deduct = (double)item["Deduct_Amount"];
                    newList.count = (int)item["Count"];
                    newList.empId = (long)item["EmpId"];

                    lateDeducts.Add(newList);

                }
                var leaves = new List<LeaveModel>();

                foreach (DataRow item in data.Tables[2].Rows)
                {
                    var newLeave = new LeaveModel();
                    newLeave.EmpId = (long)item["EmpId"];
                    newLeave.LeaveId = (int)item["LeaveId"];
                    newLeave.LeaveAllowed = Convert.ToDouble(item["LeaveAllowed"].ToString().Trim());
                    newLeave.ApproveLeave = Convert.ToDouble(item["ApprovedLeave"].ToString().Trim());
                    newLeave.LeaveDesc = (string)item["LeaveDesc"];
                    leaves.Add(newLeave);
                }
               
                ViewBag.leaves = leaves;
                ViewBag.DataMaster2 = lateDeducts;
                ViewBag.AllLates = await model.GetLates();
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                return View(data);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetTransaction");
                throw;
            }

        }

        public async Task<ActionResult> GetFullAndFinalSettlementReport()
        {
            try
            {
              //  string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                string DateFrom = QueryStringData[1];
                string DateTo = QueryStringData[2];
                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                foreach (var item in sEmpids)
                {
                    employeeIds[index++] = Convert.ToInt64(item.Trim());
                }
                var report = new EmpSettlementModel();
                IList<EmpSettlementModel> empSettlementReport = await report.GetFullAndFinalReport(employeeIds);

                //var psm = new PaySlipModel();
                //IList<PaySlipModel> paySlip = await psm.GetPaySlips(employeeIds)

                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                ViewBag.DataMaster = empSettlementReport;
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "GeneratePaySlips");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> GetLoanReport()
        {
            try
            {
              //  string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                DateTime DateFrom = Convert.ToDateTime(QueryStringData[1].Trim());
                DateTime DateTo = Convert.ToDateTime(QueryStringData[2].Trim());
                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                foreach (var item in sEmpids)
                {
                    employeeIds[index++] = Convert.ToInt64(item.Trim());
                }
                LoanModel lm = new LoanModel();
                PaySlipModel psm = new PaySlipModel();
                IList<LoanLedgerModel> loans = await lm.GetLoanReport(employeeIds, DateFrom, DateTo);// psm.GetPaySlips(employeeIds, DateFrom, DateTo);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                //ViewBag.DataMaster = loans;
                return View(loans);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "GetLoanReport");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> GetLoanSummaryReport()
        {
            try
            {
                // string[] QueryStringData = DataHelper.QuerystringData;

                string[] QueryStringData = (string[])Session["ReportQuery"];
                DateTime DateFrom = Convert.ToDateTime(QueryStringData[1].Trim());
                DateTime DateTo = Convert.ToDateTime(QueryStringData[2].Trim());
                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                foreach (var item in sEmpids)
                {
                    employeeIds[index++] = Convert.ToInt64(item.Trim());
                }
                LoanModel lm = new LoanModel();
                PaySlipModel psm = new PaySlipModel();
                IList<LoanLedgerModel> loans = await lm.GetLoanSummaryReport(employeeIds, DateFrom, DateTo);// psm.GetPaySlips(employeeIds, DateFrom, DateTo);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                //ViewBag.DataMaster = loans;
                return View(loans);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "GetLoanSummaryReport");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> GetSalaryIncrementReport()
        {
            try
            {
                //string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                DateTime DateFrom = Convert.ToDateTime(QueryStringData[1].Trim());
                DateTime DateTo = Convert.ToDateTime(QueryStringData[2].Trim());
                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                foreach (var item in sEmpids)
                {
                    employeeIds[index++] = Convert.ToInt64(item.Trim());
                }
               
                var sim = new SalaryIncrementModel();
                var incrementReport = await sim.GetSalaryIncrementReport(employeeIds, DateFrom, DateTo);// psm.GetPaySlips(employeeIds, DateFrom, DateTo);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                //ViewBag.DataMaster = loans;
                return View(incrementReport);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "GetSalaryIncrementReport");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> GetOverTimeReport()
        {
            try
            {
                // string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];

                DateTime DateFrom = Convert.ToDateTime(QueryStringData[1].Trim());
                DateTime DateTo = Convert.ToDateTime(QueryStringData[2].Trim());
                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                foreach (var item in sEmpids)
                {
                    employeeIds[index++] = Convert.ToInt64(item.Trim());
                }

                var sim = new OverTimeModel();
                var incrementReport = await sim.GetOTReport(employeeIds, DateFrom, DateTo);// psm.GetPaySlips(employeeIds, DateFrom, DateTo);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                //ViewBag.DataMaster = loans;
                return View(incrementReport);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "GetOverTimeReport");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> GetSalaryTransferLetterReport()
        {
            try
            {
                //string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                DateTime DateFrom = Convert.ToDateTime(QueryStringData[1].Trim());
                DateTime DateTo = Convert.ToDateTime(QueryStringData[2].Trim());

                var bankIdString = QueryStringData[4].Trim();
                
                int? BankId = String.IsNullOrEmpty(bankIdString) ? (int?)null:  Convert.ToInt32(bankIdString) ;

            
                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                foreach (var item in sEmpids)
                {
                    employeeIds[index++] = Convert.ToInt64(item.Trim());
                }
                var model = new SalaryTransferLetterModel();
               
                IList<SalaryTransferLetterModel> loans = await model.GetSalaryTransferLetterData(employeeIds, DateFrom, DateTo,BankId);// psm.GetPaySlips(employeeIds, DateFrom, DateTo);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];

                ViewBag.BankDetail = await model.GetBankModels();

                //ViewBag.DataMaster = loans;
                return View(loans);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "GetSalaryTransferLetterReport");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> GetEmpIncomeTaxReport()
        {
            try
            {
                //string[] QueryStringData = DataHelper.QuerystringData;

                string[] QueryStringData = (string[])Session["ReportQuery"];
                DateTime DateFrom = Convert.ToDateTime(QueryStringData[1].Trim());
                DateTime DateTo = Convert.ToDateTime(QueryStringData[2].Trim());

               

                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                foreach (var item in sEmpids)
                {
                    employeeIds[index++] = Convert.ToInt64(item.Trim());
                }
                var model = new EmpIncomeTaxReportModel();

                IList<EmpIncomeTaxReportModel> taxReport = await model.GetEmpTaxData(employeeIds, DateFrom, DateTo);// psm.GetPaySlips(employeeIds, DateFrom, DateTo);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];

              //  ViewBag.BankDetail = await model.GetBankModels();

                //ViewBag.DataMaster = loans;
                return View(taxReport);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "GetEmpIncomeTaxReport");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> GetAdvanceReport()
        {
            try
            {
                //string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];

                DateTime DateFrom = Convert.ToDateTime(QueryStringData[1].Trim());
                DateTime DateTo = Convert.ToDateTime(QueryStringData[2].Trim());
                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                foreach (var item in sEmpids)
                {
                    employeeIds[index++] = Convert.ToInt64(item.Trim());
                }
                AdvanceModel am = new AdvanceModel();

                IList<AdvanceReportModel> advances = await am.GetAdvanceReport(employeeIds, DateFrom, DateTo);// psm.GetPaySlips(employeeIds, DateFrom, DateTo);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                //ViewBag.DataMaster = loans;
                return View(advances);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "GetAdvanceReport");
                return Content("Something Goes Wrong");
            }
        }



        public async Task<ActionResult> SalarySummaryReport()
        {
            try
            {
                // string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                string DateFrom = QueryStringData[1];
                string DateTo = QueryStringData[2];
                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                foreach (var item in sEmpids)
                {
                    employeeIds[index++] = Convert.ToInt64(item.Trim());
                }
                PaySlipModel psm = new PaySlipModel();
                IList<PaySlipModel> payslips = await psm.GetPaySlips(employeeIds, DateFrom, DateTo);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                ViewBag.DataMaster = payslips;
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "SalarySummaryReport");
                return Content("Something Goes Wrong");
            }
        }
        // Created by moiez
        public ActionResult LeaveReport()
        {
            DataTable dt = TempData["LeaveReport"] as DataTable;

            if (dt == null)
            {
                return RedirectToAction("Index", "Report", new { area = "Reports" });
            }

            return View();
        }

        // Created by moiez
        public ActionResult GetLeaveReport()
        {
            try
            {
                string[] QueryStringData = null;
                //QueryStringData = DataHelper.QuerystringData;
               
                QueryStringData = (string[])Session["ReportQuery"];



              //  Session["ReportParameters"]

                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 36000;
                string empIDs = "," + QueryStringData[0] + ",";
                DataTable dt = OldDB.GetLeaveReport(empIDs, QueryStringData[1], QueryStringData[2]);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];

                return View(dt);

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetLeaveReport");
                throw;
            }

        }

        //created by moiez
        public ActionResult GetAttendanceSummary()
        {
            try
            {

                string[] QueryStringData = null;
                QueryStringData = (string[])Session["ReportQuery"];  //DataHelper.QuerystringData;
                string empIDs = "," + QueryStringData[0] + ",";
                DataTable dt = OldDB.GetAttendaceSummaryReport(empIDs, QueryStringData[1], QueryStringData[2]); 
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];


                return View(dt);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetLeaveReport");
                throw;
            }
        }

        // Created by moiez
        public async Task<ActionResult> GetLateReport()
        {
            try
            {
                ModelMonthlyRepot latemodel = new ModelMonthlyRepot();
                // string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 36000;
                string[] empIDs = QueryStringData[0].Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));
                string from = QueryStringData[1];
                string to = QueryStringData[2];
                var lateReport = await latemodel.getLateReport(from, to, empIDS);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                return View(lateReport);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetLeaveReport");
                throw;
            }

        }

        // Created by moiez
        public async Task<ActionResult> GetAbsentReport()
        {
            try
            {
                ModelMonthlyRepot AbsentReportModel = new ModelMonthlyRepot();
                // string[] QueryStringData = DataHelper.QuerystringData;

                string[] QueryStringData = (string[])Session["ReportQuery"];
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 36000;
                string[] empIDs = QueryStringData[0].Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));
                string from = QueryStringData[1];
                string to = QueryStringData[2];
                var AbsentReport = await AbsentReportModel.GetAbsentReport(from, to, empIDS);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                return View(AbsentReport);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetLeaveReport");
                throw;
            }

        }

        // Created by moiez
        public async Task<ActionResult> GetHalfDayReport()
        {
            try
            {
                ModelMonthlyRepot HalfDayModel = new ModelMonthlyRepot();
                //string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 36000;
                string[] empIDs = QueryStringData[0].Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));
                string from = QueryStringData[1];
                string to = QueryStringData[2];
                var HalfDayReport = await HalfDayModel.getHalfReport(from, to, empIDS);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                return View(HalfDayReport);


            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetHalfDayReport");
                throw;
            }

        }

        // Created by moiez
        public async Task<ActionResult> GetEarlyMinReport()
        {
            try
            {
                ModelMonthlyRepot HalfDayModel = new ModelMonthlyRepot();
                //string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];

                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 36000;
                string[] empIDs = QueryStringData[0].Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));
                string from = QueryStringData[1];
                string to = QueryStringData[2];
                var HalfDayReport = await HalfDayModel.GetEarlyMinReport(from, to, empIDS);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                return View(HalfDayReport);


            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetEarlyMinReport");
                throw;
            }

        }

        // Created by moiez
        public async Task<ActionResult> GetTimeOutReport()
        {
            try
            {
                ModelMonthlyRepot TimeOutModel = new ModelMonthlyRepot();
                //string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];

                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 36000;
                string[] empIDs = QueryStringData[0].Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));
                string from = QueryStringData[1];
                string to = QueryStringData[2];
                var TimeoutReport = await TimeOutModel.GetTimeOutReport(from, to, empIDS);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                return View(TimeoutReport);


            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetEarlyMinReport");
                throw;
            }

        }

        public async Task<ActionResult> GetTransaction()
        {
            try
            {
                ModelMonthlyRepot model = new ModelMonthlyRepot();
                //string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                string[] empIDs = QueryStringData[0].Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));
                string from = QueryStringData[1];
                string to = QueryStringData[2];
                var data = await model.GetTransactionReport(Convert.ToDateTime(from), Convert.ToDateTime(to), empIDS);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                return View(data);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetTransaction");
                throw;
            }

        }
        public async Task<ActionResult> GetTransactionWithLocation()
        {
            try
            {
                ModelMonthlyRepot model = new ModelMonthlyRepot();
                //string[] QueryStringData = DataHelper.QuerystringData;

                string[] QueryStringData = (string[])Session["ReportQuery"];
                string[] empIDs = QueryStringData[0].Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));
                string from = QueryStringData[1];
                string to = QueryStringData[2];
                var data = await model.GetTransactionWithLocationReport(Convert.ToDateTime(from), Convert.ToDateTime(to), empIDS);
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                return View(data);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetTransaction");
                throw;
            }

        }

       


        public async Task<ActionResult> trackMe(int id)
        {
            try
            {
                ModelMonthlyRepot model = new ModelMonthlyRepot();
                // string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                string[] empIDs = QueryStringData[0].Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));
                string from = QueryStringData[1];
                string to = QueryStringData[2];
                var data = await model.GetTransactionReport(Convert.ToDateTime(from), Convert.ToDateTime(to), id);
                return View(data);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "trackMe");
                return Content("something Goes Wrong");
            }


        }
        public JsonResult GetReportImageBase64()
        {
            try
            {
                var icon = (tbl_IconControl)Session["Icon"];
                return Json(new { Status = "Success", Data = Functions.GetBase64StringForImage(icon.Report) });

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "GetReportImageBase64");
                return Json(new { Status = "Error", Message = "Failed" });
            }
        }
        //created by moiez
        public async Task<ActionResult> Working()
        {
            try
            {
                ModelMonthlyRepot model = new ModelMonthlyRepot();
                // string[] QueryStringData = DataHelper.QuerystringData;

                string[] QueryStringData = (string[])Session["ReportQuery"];
                string[] empIDs = QueryStringData[0].Split(',');

                string from = QueryStringData[1];
                string to = QueryStringData[2];
                var data = await model.GetWorkingHours(QueryStringData[0], Convert.ToDateTime(from), Convert.ToDateTime(to));
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                return View(data);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "Working");
                throw;
            }
        }
        public async Task<ActionResult> OverTimeReport()
        {
            try
            {
                ModelMonthlyRepot model = new ModelMonthlyRepot();
                //string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                string[] empIDs = QueryStringData[0].Split(',');

                string from = QueryStringData[1];
                string to = QueryStringData[2];
                var data = await model.OverTime(QueryStringData[0], Convert.ToDateTime(from), Convert.ToDateTime(to));
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                return View(data);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "OverTimeReport");
                throw;
            }
        }
        public async Task<ActionResult> ManualReport()
        {
            try
            {
                ModelMonthlyRepot model = new ModelMonthlyRepot();
                // string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 36000;
                string[] empIDs = QueryStringData[0].Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));
                string from = QueryStringData[1];
                string to = QueryStringData[2];
                var AbsentReport = await model.ManualReport(from, to, ","+QueryStringData[0]+",");
                ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
                return View(AbsentReport);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetLeaveReport");
                throw;
            }

        }
        public ActionResult getFullReport()
        {

            //string[] QueryStringData = DataHelper.QuerystringData;
            string[] QueryStringData = (string[])Session["ReportQuery"];
            string from = QueryStringData[1];
            string to = QueryStringData[2];
            DateTime dateto = Convert.ToDateTime(to);
            dateto = dateto.AddDays(1).AddMilliseconds(-1);
            DataTable data = OldDB.AttendanceReport("," + QueryStringData[0] + ",", Convert.ToDateTime(from), dateto);
            ViewBag.data = data;
            ViewBag.DateRange1 = QueryStringData[1] + "," + QueryStringData[2];
            return View();


        }
        public ActionResult GetEmployeeRoaster()
        {
            //string[] QueryStringData = DataHelper.QuerystringData;

            string[] QueryStringData = (string[])Session["ReportQuery"];
            string from = QueryStringData[1];
            string to = QueryStringData[2];
            DateTime dateto = Convert.ToDateTime(to);
            dateto = dateto.AddDays(1).AddMilliseconds(-1);
            DataTable data = OldDB.EmployeeRoaster("," + QueryStringData[0] + ",", Convert.ToDateTime(from), dateto);
            ViewBag.data = null;
           ViewBag.data = data;
            ViewBag.DateRange = null;
            ViewBag.DateRange = QueryStringData[1] + "," + QueryStringData[2];
            return View();
        }
        public async Task<ActionResult> GetDailyAttendace()
        {
            try
            {
                ModelMonthlyRepot latemodel = new ModelMonthlyRepot();
                // string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 36000;
                string[] empIDs = QueryStringData[0].Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));
                string date = QueryStringData[1];
                var lateReport = await latemodel.DailyAttendance(date,  empIDS);
                ViewBag.DateRange = date;
                ViewBag.DataMaster = lateReport;
                ViewBag.ViewName = "_DailyAttendance";
                return View();
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetLeaveReport");
                throw;
            }
        }

        public async Task<ActionResult> GetEmployeeHistory()
        {
            try
            {
                var empHistory = new EmployeeHistoryModel();
                // string[] QueryStringData = DataHelper.QuerystringData;
                string[] QueryStringData = (string[])Session["ReportQuery"];
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 36000;
                string[] empIDs = QueryStringData[0].Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));
                string date = QueryStringData[1];
                var data = await empHistory.GetEmployeeHistoryReport(empIDS);
                ViewBag.DateRange = date;
                return View(data);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "Report", "GetEmployeeHistory");
                throw;
            }
        }

    }
}
