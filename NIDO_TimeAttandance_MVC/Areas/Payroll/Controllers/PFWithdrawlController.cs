using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System.Threading.Tasks;
namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Controllers
{
    [DashboardSession]
    public class PFWithdrawlController : Controller
    {
        public PFWithdrawlController()
        {
            ViewBag.Employees = DataHelper._getEmployeePF();
        }
        // GET: Payroll/PFWithdrawl
        public ActionResult Index()
        {

            return View();
        }

        public async Task<ActionResult> ListAll()
        {
            try
            {
                PFWithdrawlModel pfm = new PFWithdrawlModel();
                var lst = await pfm.GetAll();
                return View(lst);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "PFWithdrawlController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        public ActionResult Create()
        {


            return View();
        }
        [HttpPost]
        public ActionResult GetPF(int EmpId)
        {
            try
            {
                
                
                PFWithdrawlModel pfm = new PFWithdrawlModel();
                //var lst =  pfm.GetPF(Convert.ToInt32(EmpId.Trim()));
                var lst = pfm.GetPF(EmpId);
                if (lst == null)
                    return Json(new { Message = "No Data Found" });
                return Json(new {Message="OK", EmployeeContribution = lst.EmployeeContributedAmount.GetValueOrDefault(),EmployerContribution=lst.EmployerContributedAmount.GetValueOrDefault() });
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "PFWithdrawlController", "GetPF");
                return Content("Something Goes Wrong");
            }
            
            
        }

        [HttpPost]
        public async Task<ActionResult> WithdrawPF(PFWithdrawlModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SavePFWithdrawel())
                    {
                        return Json(new { Status = "Success", Message = "Provident Fund withdrawed successfully ", URL = "/Payroll/PFWithdrawl/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Provident Fund is not  Withdrawed successfully .Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "PFWithdrawlController", "WithdrawPF");
                return Content("Something Goes Wrong");
            }

        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                PFWithdrawlModel am = new PFWithdrawlModel();
                var alModel = await am.Get_PFWithdrawl_By_Id(id);
                return View(alModel);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "PFWithdrawlController", "EditPFWithdrawl");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> DeletePFWithdrawl(int id)
        {
            try
            {
                PFWithdrawlModel am = new PFWithdrawlModel();
                bool result = await am.DeletePFWithdrawel(id);
                if (result)
                {
                    return Json(new { Status = "Success", Message = "PFWithdrawel Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "PFWithDrawel has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "PFWithdrawlController", "DeletePFWithdrawl");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Update(PFWithdrawlModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.EditPFWithdrawl();
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "PF Withdrawel has been successfully saved", URL = "/Payroll/PFWithdrawl/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "PF Withdrawel has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "PFWithdrawlController", "UpdatePFWithdrawl");
                return Content("Something Goes Wrong");
            }
        }
    }
}