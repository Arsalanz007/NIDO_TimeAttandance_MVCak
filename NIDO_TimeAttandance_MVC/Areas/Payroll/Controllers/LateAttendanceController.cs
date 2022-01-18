using Microsoft.Ajax.Utilities;
using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Controllers
{
    [DashboardSession]
    public class LateAttendanceController : Controller
    {
        // GET: Payroll/LateAttendance
        public LateAttendanceController()
        {
            //ViewBag.Employees = DataHelper._getEmployee();
            //ViewBag.LoanStatus = DataHelper._getLoanStatus();
        }
  
        public ActionResult Index()
        {
            return View();
        }
        
        public async Task<ActionResult> LateExempt()
        {
            clsEmployeeProfile model = new clsEmployeeProfile();
            int id = int.Parse(HttpContext.Session["UserId"].ToString());
            ViewBag.Employees = await model._GetEmployeePosting(id);
            return View();
        }

        [HttpPost]   
        public async Task<ActionResult> GetLateExemptDate(string data)
        {
            try
            {
                Session["LateExemptQuery"] = data.Split('?');
                return Json(new { Status = "Success", URL = "/Payroll/LateAttendance/GetLateRecord" });
            }
            catch (Exception ex)
            {

                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LateAttendanceController", "GetLateExemptDate");
                return Json(new { Status = "Error", Message = "Please try again", JsonRequestBehavior.AllowGet });
            }
        }
       
        public async Task<ActionResult> GetLateRecord()
        {
            try
            {
                var db = new PakOman_NedoEntities();
                var lm = new LateAttendanceModel();
                string[] QueryStringData = (string[])Session["LateExemptQuery"];
                string[] empIDs = QueryStringData[0].Split(',');
                var  dateFrom = DateTime.Parse(QueryStringData[1]);
                var dateTo = DateTime.Parse(QueryStringData[2]);
                var empId = QueryStringData[0];

                string from = QueryStringData[1];
                string to = QueryStringData[2];
                ViewBag.FromDate = from;
                ViewBag.ToDate = to;
                ViewBag.LateDeductList = await EmpLateDetails.GetLatePolicies();
                var data = db.tbl_EmpLateDetails.Where(c => empIDs.Contains(c.EmpId.Value.ToString()) && c.AttDate >= dateFrom && c.AttDate <= dateTo).ToList();

                var details = (from c in data
                               join y in db.EmpMasters on c.EmpId.Value equals y.EmpId
                               select new EmpLateDetails
                               {
                                   Id = c.Id,
                                   AttDate = c.AttDate,
                                   DeductId = c.DeductId.Value,
                                   EmpName = y.EmpName,
                                   EmpId = c.EmpId.Value,
                                  EmpCode = y.EmpCode,
                                   TimeIn = c.TimeIn,
                                   Deduct_Amount=c.Deduct_Amount,
                                   Remarks = c.Remarks,
                                   LatePolicyId = db.tbl_AllocatedLatePolicies
                                   
                                   .Where(a=>a.EmpId ==c.EmpId && c.AttDate >= a.LatePolicyStartDate && c.AttDate <= a.LatePolicyEndDate ).Select(q=>q.LatePolicyId).FirstOrDefault()

                               }).ToList();

                return View(details);
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ReportController", "Generate");
                return Json(new { Status = "Error", Message = "Please try again", JsonRequestBehavior.AllowGet });
            }


        }
        public async Task<ActionResult> DeleteLateAttendance(int id)
        {
            try
            {
                var lm = new LateAttendanceModel();
                bool result = await lm.Delete_LateAttendnace(id);
                if (result)
                {
                    return Json(new { Status = "Success", Message = "Late Attendance Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Late Attendance has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LateAttendanceController", "DeleteLateAttendance");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> DeleteLatePolicy(int id)
        {
            try
            {
                var lm = new LatePolicyMasterModel();
                bool result = await lm.Delete_LatePolicy(id);
                if (result)
                {
                    return Json(new { Status = "Success", Message = "Late Policy Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Late Policy has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LateAttendanceController", "DeleteLatePolicy");
                return Content("Something Goes Wrong");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var lm = new LateAttendanceModel();

                var lmModel = await lm.Get_LateAttendance_By_Id(id);
                return View(lmModel);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LateAttendanceController", "EditLateAttendance");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> UpdateLateAttendanceDetail(LateAttendanceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.Edit(model.Id);
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "LateAttendance Installment has been successfully saved", URL = ("/Payroll/LateAttendance/GetLatePolicy/" + model.LatePolicyId), JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "LateAttendance Installment has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LateAttendanceController", "UpdateLateAttendance");
                return Content("Something Goes Wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveLatePolicy(LatePolicyMasterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.Save())
                    {

                        return Json(new { Status = "Success", Message = "Late Policy has been successfully saved", URL = "/Payroll/LateAttendance/GetLatePolicy/" + model.LatePolicyId, JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Late Policy has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LateAttendanceController", "SaveLateAttendance");
                return Content("Something Goes Wrong");
            }

        }

        [HttpPost]
        public async Task<ActionResult> SaveLateAttendance(LateAttendanceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.Save())
                    {
                
                        return Json(new { Status = "Success", Message = "LateAttendance has been successfully saved", URL = "/Payroll/LateAttendance/GetLatePolicy/"+model.LatePolicyId, JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "LateAttendance has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LateAttendanceController", "SaveLateAttendance");
                return Content("Something Goes Wrong");
            }

        }
        [HttpPost]
        public async Task<ActionResult> AdjustLateDeduct(List<LateDeductAdjustViewModel> model)
            {
            try
            {
                var deductModel = new LateDeductAdjustViewModel();
               
                    if (await deductModel.AdjustEmpLateDetails(model))
                    {

                        return Json(new { Status = "Success", Message = "LateAttendance has been successfully saved", URL = "/Payroll/LateAttendance/LateExempt", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "LateAttendance has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                
             
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LateAttendanceController", "SaveLateAttendance");
                return Content("Something Goes Wrong");
            }

        }
        
        public async Task<ActionResult> ListAll()
        {
            //LateAttendanceModel aam = new LateAttendanceModel();
            LatePolicyMasterModel lm = new LatePolicyMasterModel();
            var lst =  await lm.GetAll();

            return View(lst);
        }

        public  ActionResult GetLatePolicy(int id)
        {
            LatePolicyMasterModel aam = new LatePolicyMasterModel();
            LatePolicyMasterModel lm =  aam.GetLatePolicy(id);

            return View(lm);
        }
        public ActionResult Create(int id)
        {
            LatePolicyMasterModel aam = new LatePolicyMasterModel();
            LatePolicyMasterModel lm =    aam.GetLatePolicy(id);
            ViewBag.LatePolicyId = lm.LatePolicyId;
            LateAttendanceModel lmh = new LateAttendanceModel();
            lmh.LatePolicyId = lm.LatePolicyId;
            return View(lmh);
        }

        public ActionResult CreateLatePolicy()
        {
            
            return View();
        }

    }
}