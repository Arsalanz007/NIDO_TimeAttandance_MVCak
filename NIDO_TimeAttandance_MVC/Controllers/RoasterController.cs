using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class RoasterController : Controller
    {
        public RoasterController()
        {
            try
            {
                ViewBag.Shifts = DataHelper._getAllShift();
                ViewBag.ShiftData = DataHelper._GetShifts();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoasterController", "Constructor");
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> UpdateName(int id,string sd)
        {
            try
            {
                ModelRoaster MD = new ModelRoaster();
                var result = await MD.UpdateRoasterName(id, sd);
                if (result)
                {
                    return RedirectToAction("ListAll");
                    
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Roaster name has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                }
                

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoasterController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelRoaster MD = new ModelRoaster();
                var model = await MD.GetRoaster();
                return View(model);

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoasterController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetRoasters(int id)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Configuration.ProxyCreationEnabled = false;
                var events = await Task.Run(() => db.ShiftScheduleGeneralMasters.
                Join(db.ShiftScheduleGeneralDetails,
                o => o.PKId, od => od.MID,
                (o, od) => new
                {
                    o.PKId,
                    o.ScheduleDesc,
                    od.Remarks,
                    od.ShiftMaster.ShiftDesc,
                    od.AttDate,
                    od.BarColor,
                    o.ScheduleCode,
                }).Where(m => m.PKId == id));
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoasterController", "GetRoasters");
                return new JsonResult { Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveRoaster(ModelRoaster model)
        {
            try
            {
                string Holidays = Request["txtHolidays"];
                string Shiftdays = Request["txtShiftdays"];

                if (ModelState.IsValid)
                {
                    if (await model.Roaster_Save(Holidays, Shiftdays))
                    {
                        return Json(new { Status = "Success", Message = "Roaster has been successfully saved", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Roaster has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoasterController", "SaveRoaster");
                return new JsonResult { Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public async Task<ActionResult> Update(ModelRoaster model)
        {
            try
            {
                string Holidays = Request["txtHolidaysUpdate"];
                string Shiftdays = Request["txtShiftdays"];
                string comma = String.Empty;
                if (Holidays == "" || Holidays == null || Holidays == string.Empty)
                {

                }
                else
                {
                    comma = Holidays.Substring(Holidays.Length - 1, 1);
                    if (comma == ",")
                    {
                        Holidays = Holidays.Remove(Holidays.Length - 1, 1);
                    }
                }
                if (Shiftdays == "" || Shiftdays == null || Shiftdays == string.Empty)
                {

                }
                else
                {
                    comma = Shiftdays.Substring(Holidays.Length - 1, 1);
                    if (comma == ",")
                    {
                        Shiftdays = Shiftdays.Remove(Shiftdays.Length - 1, 1);
                    }
                }
                if (ModelState.IsValid)
                {
                    PakOman_NedoEntities _db = new PakOman_NedoEntities();
                    var DeleteData = _db.ShiftScheduleGeneralDetails.Where(m => m.AttDate >= model.Startdate && m.AttDate <= model.EndDate && m.MID == model.MID).ToList();
                    if (DeleteData != null)
                    {
                        _db.ShiftScheduleGeneralDetails.RemoveRange(DeleteData);
                        await _db.SaveChangesAsync();
                    }
                    if (await model.Roaster_Save(Holidays, Shiftdays))
                    {

                        return Json(new { Status = "Success", Message = "Roaster has been successfully updated", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Roaster has not been successfully updated.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoasterController", "Update");
                return new JsonResult { Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetRoastersInfo(int id)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Configuration.ProxyCreationEnabled = false;
                var events = await Task.Run(() => (from m in db.ShiftScheduleGeneralMasters
                                                   join d in db.ShiftScheduleGeneralDetails on m.PKId equals d.MID
                                                   where m.PKId == id
                                                   select new
                                                   {
                                                       m.PKId,
                                                       m.ScheduleDesc,
                                                       m.ScheduleCode,
                                                       d.ShiftMaster.ShiftDesc,
                                                       d.ShiftID,

                                                   }).Distinct());
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoasterController", "GetRoastersInfo");
                return new JsonResult { Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        //public async Task < ActionResult> DeleteRoaster(int id)
        //{
        //    try
        //    {


        //    ModelRoaster MD = new ModelRoaster();
        //    bool Result = await MD.DeleteRoaster(id);
        //    if (Result == true)
        //    {
        //            return Json(new { Status = "Success", Message = " Roaster Successfully Deleted", JsonRequestBehavior.AllowGet });

        //    }
        //    else
        //    {
        //            return Json(new { Status = "Error", Message = "Sory this Roaster can't be deleted because there are Employees maped on this Roaster", JsonRequestBehavior.AllowGet });


        //    }

        //    }
        //    catch (Exception ex)
        //    {

        //       ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoasterController", "DeleteRoaster");
        //        return Json(new { Status = "Error", Message = "Something Went Wrong", JsonRequestBehavior.AllowGet });


        //    }
        //}
    }
}