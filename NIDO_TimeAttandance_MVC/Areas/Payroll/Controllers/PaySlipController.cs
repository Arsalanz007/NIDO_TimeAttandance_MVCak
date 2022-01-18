using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;

using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Controllers
{
    [DashboardSession]
    public class PaySlipController : Controller
    {

        public async Task<ActionResult> Index()
        {
            try
            {
                clsEmployeeProfile model = new clsEmployeeProfile();
                ViewBag.Employees = await model._GetEmployeePosting();
                return View();
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SalaryPostingController", "Index");
                return Content("Something Goes Wrong");
            }
        }
        //public async Task<ActionResult> GeneratePaySlip(Array EmpId, string DateFrom, string DateTo)
        public async Task<ActionResult> GeneratePaySlip(string data)
        {
            try
            {
                Array EmpId=null; string DateFrom=null; string DateTo=null;

                string[] QueryStringData = data.Split('?');

                DateFrom = QueryStringData[1];
                DateTo = QueryStringData[2];
                string[] sEmpids = QueryStringData[0].Split(new char[] { ',' });
                long[] employeeIds = new long[sEmpids.Length];
                int index = 0;
                foreach (var item in sEmpids)
                {
                    employeeIds[index++] = Convert.ToInt64(item.Trim());
                }

                PaySlipModel psm = new PaySlipModel();
                IList<PaySlipModel> payslips= await psm.GetPaySlips(employeeIds, DateFrom, DateTo);

                //foreach(PaySlipModel model in payslips)
                //{
                //    foreach(PaySlipTaxModel tmodel in model.Taxes)
                //    {
                //        model.Deductions.Add(new PaySlipDeductionModel { DeductionName = tmodel.TaxName, DeductionAmount = tmodel.TaxAmount });
                //    }
                //}

                return View(payslips);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "PaySlipController", "GetPaySlips");
                return Content("Something Goes Wrong");
            }
        }
    }
}