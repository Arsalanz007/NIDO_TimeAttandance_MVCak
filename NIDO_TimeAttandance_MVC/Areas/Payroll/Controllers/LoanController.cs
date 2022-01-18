using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;
using NIDO_TimeAttandance_MVC.Models;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Controllers
{   
    [DashboardSession]
    public class LoanController : Controller
    {
        public LoanController()
        {
            ViewBag.Employees = DataHelper._getEmployee();
            ViewBag.LoanStatus = DataHelper._getLoanStatus();
        }
        // GET: Payroll/Loan
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> DeleteLoan(int id)
        {
            try
            {
                LoanModel lm = new LoanModel();
                bool result = await lm.DeleteLoan(id);
                if (result)
                {
                    return Json(new { Status = "Success", Message = "Loan Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Loan has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LoanController", "DeleteLoan");
                return Content("Something Goes Wrong");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                LoanModel lm = new LoanModel();
                
                var lmModel = await lm.Get_Loan_By_Id(id);
                return View(lmModel);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LoanController", "EditLoan");
                return Content("Something Goes Wrong");
            }
        }


        public async Task<ActionResult> SaveDescription(LoanModel model)
        {
            try
            {
                var lmModel = await model.SaveDescription();

                return        lmModel ? 
                              Json(new { Status = "Success", Message = "Description Successfully Updated" ,Url= "/Payroll/Loan/ListAll", JsonRequestBehavior.AllowGet }) :
                              Json(new { Status = "Error", Message = "Description has not been successfully Updated.Please try again", JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LoanController", "SaveDescription");
                return Json(new { Status = "Error", Message = "Description has not been successfully Updated.Please try again", JsonRequestBehavior.AllowGet });
            }
        }
        public async Task<ActionResult> InsertLoanDetail(LoanDetailModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.InsertLoanDetail();
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "Loan Installment has been successfully saved", URL = ("/Payroll/Loan/Edit/" + model.LoanId.ToString()), JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Loan Installment has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "403")
                {
                    return Json(new { Status = "Error", Message = "Cannot Add New Loan Installment.", JsonRequestBehavior.AllowGet });
                }
                return Content("Something Goes Wrong");
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AllowanceController", "UpdateAllowance");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> UpdateLoanDetail(LoanDetailModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.EditLoanDetail();
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "Loan Installment has been successfully saved", URL = ( "/Payroll/Loan/Edit/" + model.LoanId.ToString()), JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Loan Installment has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AllowanceController", "UpdateAllowance");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> CreateLoanDetail(int loanId)
        {
            try
            {
                var lm = new LoanDetailModel();
                lm.LoanId = loanId;
                return View(lm);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LoanController", "EditLoanDetail");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> EditLoanDetail(int detailId)
        {
            try
            {
                LoanModel lm = new LoanModel();

                var lmModel = await lm.Get_Loan_Detail_By_Id(detailId);
                return View(lmModel);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LoanController", "EditLoanDetail");
                return Content("Something Goes Wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveLoan(LoanModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SaveLoan())
                    {
                        //return RedirectToAction("Edit", "Loan", new { area = "Payroll" ,id=model.LoanId});
                        return Json(new { Status = "Success", Message = "Loan has been successfully saved", URL = "/Payroll/Loan/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Laon has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LoanController", "SaveLoan");
                return Content("Something Goes Wrong");
            }

        }
        public async Task<ActionResult> ListAll()
        {
            LoanModel aam = new LoanModel();
            IList<LoanModel> lst = await aam.GetAll();

            return View(lst);
        }
        public ActionResult Create()
        {


            return View();
        }




    }
}