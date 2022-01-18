using NIDO_TimeAttandance_MVC.Areas.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class ImportController : Controller
    {
        public async Task<ActionResult> ImportEmployee()
        {
            return View();
        }

        public async Task<ActionResult> ImportAttendance()
        {
            return View();
        }

        public async Task<ActionResult> ImportLeaves()
        {
            return View();
        }

        public async Task<ActionResult> ImportApprovedLeaves()
        {
            return View();
        }

        public async Task<ActionResult> AttendanceResult()
        {
            return View();
        }
        public async Task<ActionResult> EmployeeResult()
        {
            return View();
        }
        public async Task<ActionResult> LeavesResult()
        {
            return View();
        }
        public async Task<ActionResult> ApprovedLeavesResult()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaveEmployee(HttpPostedFileBase FileUpload)
        {
            try
            {
                var model = new ModelAttendance();
                var list = await model.ImportExcel(FileUpload);
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "SomeThing Goes Wrong Please check your file template";
                return View(new ImportExcelResult());
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveAttendance(HttpPostedFileBase FileUpload)
        {
            try
            {
                var model = new ModelAttendance();
                var list = await model.ImportExcel(FileUpload);
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "SomeThing Goes Wrong Please check your file template";
                return View(new ImportExcelResult());
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveLeaves(HttpPostedFileBase FileUpload)
        {
            try
            {
                var model = new ModelAttendance();
                var list = await model.ImportExcel(FileUpload);
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "SomeThing Goes Wrong Please check your file template";
                return View(new ImportExcelResult());
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveApprovedLeaves(HttpPostedFileBase FileUpload)
        {
            try
            {
                var model = new ModelAttendance();
                var list = await model.ImportExcel(FileUpload);
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "SomeThing Goes Wrong Please check your file template";
                return View(new ImportExcelResult());
            }
        }
    }
}