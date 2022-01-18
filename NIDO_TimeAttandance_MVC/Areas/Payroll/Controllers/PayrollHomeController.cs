using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Controllers
{
    [DashboardSession]
    public class PayrollHomeController : Controller
    {
        // GET: Payroll/Home
        public async Task<ActionResult> Index()
        {
            //Anas check in
            var empId = int.Parse(HttpContext.Session["UserId"].ToString());

            var data = await PayrollDashModel.GetUserDashboardData(empId);

            var totalLoan = data.Sum(c => c.LoanAmount);

            ViewBag.MonthList = await  PayrollDashModel.GetMonthlyData(empId);

            var totalLoanPaid = data.Sum(c => c.PaidLoan);
            

            var loanPercent = totalLoan == 0 ? 0 : (totalLoanPaid / totalLoan) * 100 ;

            ViewBag.LoanPercent =  int.Parse(loanPercent.ToString().Split('.')[0]);
            ViewBag.SalaryPostDate = await PayrollDashModel.LastSalaryPostedDate();
            return View(data);
            
        }


    
    }
}