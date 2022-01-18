using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class EmployeeProfileController : Controller
    {
        public EmployeeProfileController()
        {
            try
            {
                ViewBag.Country = DataHelper._getCountry();
                ViewBag.City = DataHelper._getCity();
                ViewBag.Designation = DataHelper._getDesignation();
                ViewBag.Department = DataHelper._getDepartment();
                ViewBag.MartialStatus = DataHelper._getMartialStatus();
                ViewBag.Company = DataHelper._getCompany();
                ViewBag.Grade = DataHelper._getGrade();
                ViewBag.AllEmployeeType = DataHelper._getEmployeeType();
                ViewBag.Employee = DataHelper._getEmployee();
                ViewBag.EmployeeStatus = DataHelper._getEmployeeStatus();


            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "Constructor");
            }

        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> ListAll()
        {
            try
            {

                ModelEmpProfile MD = new ModelEmpProfile();
                var model = await MD.GetEmployeeList();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        public async Task<ActionResult> LoadUserForm(int pageID, long Empid)
        {
            try
            {
                switch (pageID)
                {
                    case 1:
                        {
                            if (Empid > 0)
                            {
                                ModelEducation Edu = new ModelEducation();
                                ViewBag.Education = await Edu.Get_Education_By_Id(Empid);
                            }
                            return PartialView("_Education");
                        }
                    case 2:
                        {
                            if (Empid > 0)
                            {
                                ModelExperince exp = new ModelExperince();
                                ViewBag.Experince = await exp.Get_Experince_By_Id(Empid);
                            }

                            return PartialView("_Experince");
                        }
                    case 3:
                        {
                            if (Empid > 0)
                            {
                                ModelFamilyInfo Fam = new ModelFamilyInfo();
                                ViewBag.FamilyDetails = await Fam.Get_FamilyDetails_By_Id(Empid);
                            }
                            return PartialView("_FamilyInfo");
                        }
                    default:
                        return null;

                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "LoadUserForm");
                return Content("Something Goes Wrong");
            }

        }
        [HttpPost]
        public async Task<ActionResult> Save([Bind(Exclude = "MartialStatusId")] ModelEmpProfile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.EmpProfile_Save())
                    {
                        return Json(new { Status = "Success", Message = "Employee Profile has been successfully saved", URL = "/EmployeeProfile/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Employee Profile has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "Save");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<JsonResult> InsertExperince(List<EmpExperienceDetail> experinces)
        {
            try
            {


                //if (experinces.Select(a => a.Position).FirstOrDefault() != "No matching records found")
                //{
                using (PakOman_NedoEntities entities = new PakOman_NedoEntities())
                {
                    //Truncate Table to delete all old records.
                    await entities.Database.ExecuteSqlCommandAsync("delete EmpExperienceDetail where EmpId = " + DataHelper.Empid + "");

                    //Check for NULL.
                    if (experinces == null)
                    {
                        experinces = new List<EmpExperienceDetail>();
                    }

                    //Loop and insert records.
                    foreach (EmpExperienceDetail experince in experinces)
                    {
                        if (experince.Position != "No matching records found")
                        {
                            experince.CreatedBy = HttpContext.Session["UserName"].ToString();
                            experince.CreatedOn = DateTime.Now.ToString();
                            experince.EmpId = DataHelper.Empid;
                            entities.EmpExperienceDetails.Add(experince);
                        }
                    }
                    int insertedRecords = await entities.SaveChangesAsync();
                    return Json(insertedRecords);
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "InsertExperince");
                return Json(new { Status = "Error", Message = "Something Goes Wrong" });
            }


        }
        public async Task<JsonResult> InsertEducation(List<EmpEducationDetail> educations)
        {
            try
            {


                using (PakOman_NedoEntities entities = new PakOman_NedoEntities())
                {
                    //Truncate Table to delete all old records.
                    await entities.Database.ExecuteSqlCommandAsync("delete EmpEducationDetail where EmpId = " + DataHelper.Empid + "");

                    //Check for NULL.
                    if (educations == null)
                    {
                        educations = new List<EmpEducationDetail>();
                    }

                    //Loop and insert records.
                    foreach (EmpEducationDetail education in educations)
                    {
                        if (education.Degree != "No matching records found")
                        {
                            education.CreatedBy = HttpContext.Session["UserName"].ToString();
                            education.CreatedOn = DateTime.Now.ToString();
                            education.EmpId = DataHelper.Empid;
                            entities.EmpEducationDetails.Add(education);
                        }
                    }
                    int insertedRecords = await entities.SaveChangesAsync();
                    return Json(insertedRecords);
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "InsertEducation");
                return Json(new { Status = "Error", Message = "InsertEducation Goes Wrong" });
            }
        }

        public async Task<bool> ResetImei(long empId)
        {
            try
            {

                ModelEmpProfile MD = new ModelEmpProfile();
                var Result = await MD.ResetImei(empId);
                return Result;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "ResetImei");
                return false;
            }
        }

        public async Task<JsonResult> InsertFamily(List<EmpFamilyDetail> families)
        {
            try
            {


                using (PakOman_NedoEntities entities = new PakOman_NedoEntities())
                {
                    //Truncate Table to delete all old records.
                    await entities.Database.ExecuteSqlCommandAsync("delete EmpFamilyDetail where EmpId = " + DataHelper.Empid + "");

                    //Check for NULL.
                    if (families == null)
                    {
                        families = new List<EmpFamilyDetail>();
                    }

                    //Loop and insert records.
                    foreach (EmpFamilyDetail family in families)
                    {
                        if (family.Name != "No matching records found")
                        {
                            family.CreatedOn = DateTime.Now;
                            family.EmpId = DataHelper.Empid;
                            family.CreatedBy = HttpContext.Session["UserName"].ToString();
                            entities.EmpFamilyDetails.Add(family);
                        }
                    }
                    int insertedRecords = await entities.SaveChangesAsync();
                    return Json(insertedRecords);
                }


            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "InsertFamily");
                return Json(new { Status = "Error", Message = "InsertFamily Goes Wrong" });
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ModelEmpProfile MD = new ModelEmpProfile();
                var Result = await MD.Get_Employee_By_Id(id);
                DataHelper.Empid = id;
                return View(Result);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "Edit");
                return Content("Something Goes Wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Update(ModelEmpProfile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.UpdateEmployee())
                    {
                        return Json(new { Status = "Success", Message = "Employee Profile successfully updated.", URL = "/EmployeeProfile/ListAll" });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Employee Profile not successfully updated.Please try again" });
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "Update");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {


                ModelEmpProfile MD = new ModelEmpProfile();
                bool Result = await MD.DeleteEmployee(id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "Employee Successfully Deleted", JsonRequestBehavior.AllowGet });

                }
                else
                {
                    return Json(new { Status = "Error", Message = "Sory this Employee can't be deleted because there are some records maped on this Employee", JsonRequestBehavior.AllowGet });
                }


            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfileController", "DeleteEmployee");
                return Json(new { Status = "Error", Message = "Something Went Wrong", JsonRequestBehavior.AllowGet });

            }



        }
        [HttpPost]
        public async Task<ActionResult> CheckUserName(string UsrCode, int? EmpId)
        {
            try
            {
                UsrCode = UsrCode.Trim();
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                bool alreadyExist = false;
                if (EmpId != null)
                {
                    var IfExistAccept = await db.EmpMasters.Where(x => x.EmpCode == UsrCode && x.EmpId != EmpId).ToListAsync();
                    if (IfExistAccept.Count > 0)
                    {
                        alreadyExist = true;
                    }
                }
                else
                {
                    var IfExist = await db.EmpMasters.Where(x => x.EmpCode == UsrCode).ToListAsync();
                    if (IfExist.Count > 0)
                    {
                        alreadyExist = true;
                    }
                }

                if (alreadyExist == true)
                {
                    return Json(new { Status = "AlreadyExist", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "NotExist", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeProfile", "CheckUserName");
                return Content("Something Goes Wrong");
            }


        }
    }
}
