using LinqToExcel;
using NIDO_TimeAttandance_MVC.Areas.User.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelExcelImport
    {
        public List<AnonymoueModel> DepartList { get; set; } = new List<AnonymoueModel>();
        public List<AnonymoueModel> DesigList { get; set; }     = new List<AnonymoueModel>();
        public List<AnonymoueModel> CityList { get; set; }      = new List<AnonymoueModel>();
        public List<AnonymoueModel> GradeList { get; set; }     = new List<AnonymoueModel>();
        public List<AnonymoueModel> CompanyList { get; set; } = new List<AnonymoueModel>();


        public async Task<ImportExcelResult> ImportExcel(HttpPostedFileBase FileUpload)
        {
            var db = new PakOman_NedoEntities();

            var result = new ImportExcelResult();
            var totalEmpCount = 0;
            var rejectEmpList = new List<EmpExcelModel>();
            var acceptEmpList = new List<EmpExcelModel>();
            List<string> data = new List<string>();

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (FileUpload != null)
                    {
                        if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {

                            string filename = FileUpload.FileName;
                            string targetpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");
                            var filePath = targetpath + DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss") + filename;
                            FileUpload.SaveAs(filePath);
                            string pathToExcelFile = filePath;



                            var excelFile = new ExcelQueryFactory(pathToExcelFile);
                            var sheetNames = excelFile.GetWorksheetNames();

                            foreach (var sheetName in sheetNames)
                            {

                                var artistAlbums = from a in excelFile.Worksheet<EmpExcelModel>(sheetName) select a;

                                totalEmpCount += artistAlbums.Count();

                                if (artistAlbums.Count() > 0)
                                {
                                    var empCodeList = db.EmpMasters.AsNoTracking().Select(c => c.EmpCode).ToList();
                                    DepartList = db.DepartmentMasters.AsNoTracking().Select(c => new AnonymoueModel { Id = c.DepartmentId, Name = c.DepartmentDesc.ToLower() }).ToList();
                                    DesigList = db.DesignationMasters.AsNoTracking().Select(c => new AnonymoueModel { Id = c.DesignationId, Name = c.DesignationDesc.ToLower() }).ToList();
                                    CityList = db.CityMasters.AsNoTracking().Select(c => new AnonymoueModel { Id = c.CityId, Name = c.CityDesc.ToLower() }).ToList();
                                    GradeList = db.GradeMasters.AsNoTracking().Select(c => new AnonymoueModel { Id = c.GradeId, Name = c.GradeDesc.ToLower() }).ToList();
                                    CompanyList = db.CompanyMasters.AsNoTracking().Select(c => new AnonymoueModel { Id = c.CompanyId, Name = c.CompanyDesc.ToLower() }).ToList();

                                    foreach (var a in artistAlbums)
                                    {
                                        if (!String.IsNullOrWhiteSpace(a.EmpCode))
                                        {
                                            int? departId, desginId, cityId, gradeId, companyId;

                                            if (!empCodeList.Contains(a.EmpCode))
                                            {
                                                departId = null;
                                                desginId = null;
                                                cityId = null;
                                                gradeId = null;
                                                companyId = null;

                                                departId = string.IsNullOrEmpty(a.Department) ? 0 : await SaveDepartment(a.Department.Trim());
                                                if (departId == 0)
                                                {
                                                    throw new InvalidDataException();
                                                }

                                                desginId = string.IsNullOrEmpty(a.Designation) ? 0 : await SaveDesignation(a.Designation.Trim());
                                                if (desginId == 0)
                                                {
                                                    throw new InvalidDataException();
                                                }

                                                //if (cityList.Any(c => c.CityDesc.Contains(a.City.Trim().ToLower())))
                                                //{
                                                //    cityId = cityList.Where(c => c.CityDesc.Contains(a.City.Trim().ToLower())).FirstOrDefault().CityId;
                                                //}
                                                //else
                                                //{
                                                //    if (!String.IsNullOrEmpty(a.City))
                                                //    {
                                                //        var cityModel = new CityMaster();
                                                //        cityModel.CityDesc = a.City.Trim();
                                                //        cityModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                                                //        cityModel.CreatedOn = DateTime.Now;
                                                //        cityModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                                                //        cityModel.EditedOn = DateTime.Now;
                                                //        db.CityMasters.Add(cityModel);
                                                //        await db.SaveChangesAsync();
                                                //        cityId = cityModel.CityId;
                                                //        cityList.Add(new { CityId = desginId.Value, CityDesc = a.City.Trim() });
                                                //    }



                                                //}

                                                //if (gradeList.Any(c => c.GradeDesc.Contains(a.Grade.Trim().ToLower())))
                                                //{
                                                //    gradeId = gradeList.Where(c => c.GradeDesc.Contains(a.Grade.Trim().ToLower())).FirstOrDefault().GradeId;
                                                //}
                                                //else
                                                //{
                                                //    if (!String.IsNullOrEmpty(a.Grade))
                                                //    {
                                                //        var gradeModel = new GradeMaster();
                                                //        gradeModel.GradeDesc = a.Grade.Trim();
                                                //        gradeModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                                                //        gradeModel.CreatedOn = DateTime.Now;
                                                //        gradeModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                                                //        gradeModel.EditedOn = DateTime.Now;
                                                //        db.GradeMasters.Add(gradeModel);
                                                //        await db.SaveChangesAsync();
                                                //        gradeId = gradeModel.GradeId;
                                                //        gradeList.Add(new { GradeId = gradeId.Value, GradeDesc = a.Grade.Trim() });
                                                //    }



                                                //}
                                                //if (companyList.Any(c => c.CompanyDesc.Contains(a.Company.ToLower())))
                                                //{
                                                //    companyId = companyList.Where(c => c.CompanyDesc.Contains(a.Company.Trim().ToLower())).FirstOrDefault().CompanyId;
                                                //}
                                                //else
                                                //{
                                                //    if (!String.IsNullOrEmpty(a.Company))
                                                //    {
                                                //        var companyModel = new CompanyMaster();
                                                //        companyModel.CompanyDesc = a.Company.Trim();
                                                //        companyModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                                                //        companyModel.CreatedOn = DateTime.Now;
                                                //        companyModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                                                //        companyModel.EditedOn = DateTime.Now;
                                                //        db.CompanyMasters.Add(companyModel);
                                                //        await db.SaveChangesAsync();
                                                //        gradeId = companyModel.CompanyId;
                                                //        companyList.Add(new { CompanyId = companyId.Value, CompanyDesc = a.Company.Trim() });
                                                //    }



                                                //}

                                                var empModel = new EmpMaster();
                                                empModel.EmpCode = a.EmpCode;
                                                empModel.EmpName = a.Name;
                                                empModel.DepartmentId = departId;
                                                empModel.DesignationID = desginId;
                                                empModel.CityID = cityId;
                                                empModel.GradeId = gradeId;
                                                empModel.EmployeeTypeID = db.EmployeeTypes.Where(c => c.EmployeeTypeDsc == "Permanent").Select(c => c.ID).FirstOrDefault();
                                                empModel.CompanyId = companyId;
                                                empModel.CreatedOn = DateTime.Now.ToString();
                                                empModel.CreatedBy = "System";
                                                empModel.EditedOn = DateTime.Now.ToString();
                                                empModel.EditedBy = "System";

                                                db.EmpMasters.Add(empModel);
                                                await db.SaveChangesAsync();

                                                var user = new UserMaster
                                                {
                                                    UserName = empModel.EmpName.Split(' ')[0].ToLower(),
                                                    UserPwd = EncryptDecrypt.EncryptEX("123", true),
                                                    CreatedBy = "System",
                                                    CreatedOn = DateTime.Now,
                                                    EditedBy = "System",
                                                    EditedOn = DateTime.Now,
                                                    IsActive = true,
                                                    Empid = empModel.EmpId,
                                                    IsSuperUser = false,
                                                    RoleID = 2

                                                };
                                                db.UserMasters.Add(user);
                                                await db.SaveChangesAsync();


                                                acceptEmpList.Add(a);
                                            }
                                            else
                                            {
                                                rejectEmpList.Add(a);
                                            }
                                        }

                                    }
                                }


                            }



                        }
                        //deleting excel file from folder  

                        result.AcceptEmpList = acceptEmpList;
                        result.RejectEmpList = rejectEmpList;
                        result.AcceptEmpCount = acceptEmpList.Count;
                        result.RejectEmpCount = rejectEmpList.Count;
                        result.TotalEmpCount = totalEmpCount;
                        transaction.Commit();
                        return result;
                    }
                    else
                    {

                        return result;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAttendance", "Import");
                    throw;
                }
            }


        }
        public async Task<ImportExcelResult> SaveAttendanceDataExcel(HttpPostedFileBase FileUpload)
        {
            var db = new PakOman_NedoEntities();

            var result = new ImportExcelResult();
            var rejectEmpList = new List<EmpExcelModel>();
            var acceptEmpList = new List<EmpExcelModel>();
            List<string> data = new List<string>();
            var totalEmpCount = 0;

            try
            {
                if (FileUpload != null)
                {
                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                    if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        var filename = FileUpload.FileName;
                        var targetpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");
                        var filePath = targetpath + DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss") + filename;
                        FileUpload.SaveAs(filePath);
                        var pathToExcelFile = filePath;

                        var excelFile = new ExcelQueryFactory(pathToExcelFile);
                        var sheetNames = excelFile.GetWorksheetNames();

                        foreach (var sheetName in sheetNames)
                        {

                            var artistAlbums = from a in excelFile.Worksheet<EmpAttendanceExcelModel>(sheetName) select a;

                            totalEmpCount += artistAlbums.Count();

                            if (artistAlbums.Count() > 0)
                            {
                                var attendanceLogList = new List<AttendanceLogMaster>();

                                foreach (var a in artistAlbums)
                                {
                                    if (a.EmpCode != null)
                                    {
                                        var checktime = new DateTime();
                                        if (DateTime.TryParse(a.Date + " " + a.Time, out checktime))
                                        {
                                            attendanceLogList.Add(new AttendanceLogMaster
                                            {
                                                AcNo = a.EmpCode,
                                                CheckTime = checktime,
                                                InOutTypeId = a.InOutType.Trim().ToLower() == "in" ? 1 : 2,
                                                IsProcessed = 0,
                                                MachineNo = 1,
                                                IsManual = false,
                                                CreatedDate = DateTime.Now,
                                                CreatedBy = "System",
                                                Remarks = "Import From Excel"

                                            });
                                            result.AcceptEmpAttendList.Add(a);
                                            result.AcceptEmpCount++;
                                        }
                                        else
                                        {
                                            result.RejectEmpAttendList.Add(a);
                                            result.RejectEmpCount++;
                                        }
                                    }

                                }
                                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        db.AttendanceLogMasters.AddRange(attendanceLogList);
                                        await db.SaveChangesAsync();
                                        transaction.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAttendance", "Import");
                                        throw;
                                    }
                                }
                            }


                        }



                    }

                    return result;
                }
                else
                {

                    return result;
                }
            }
            catch (Exception ex)
            {

                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAttendance", "Import");
                throw;
            }

        }
        public async Task<ImportExcelResult> SaveExcelLeaveApproveData(HttpPostedFileBase FileUpload)
        {
            var db = new PakOman_NedoEntities();

            var result = new ImportExcelResult();
            var rejectEmpList = new List<EmpExcelModel>();
            var acceptEmpList = new List<EmpExcelModel>();
            var totalEmpCount = 0;
            var filePath = "";
            List<string> data = new List<string>();

            try
            {
                if (FileUpload != null)
                {
                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                    if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        var filename = FileUpload.FileName;
                        var targetpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");
                        filePath = targetpath + DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss") + filename;
                        FileUpload.SaveAs(filePath);
                        var pathToExcelFile = filePath;

                        var excelFile = new ExcelQueryFactory(pathToExcelFile);
                        var sheetNames = excelFile.GetWorksheetNames();

                        var leaveList = db.LeaveSetups.AsNoTracking().ToList().Select(c => new { id = c.ID, desc = c.LeaveDsc.ToLower() });

                        foreach (var sheetName in sheetNames)
                        {
                            var artistAlbums = from a in excelFile.Worksheet<EmpLeaveApproveExcelModel>(sheetName) select a;

                            totalEmpCount += artistAlbums.Count();

                            if (artistAlbums.Count() > 0)
                            {
                                var leaveApprovalist = new List<LeaveApproval>();

                                foreach (var a in artistAlbums)
                                {
                                    if (a.EmpCode != null)
                                    {
                                        var emp = await db.EmpMasters.Where(c => c.EmpCode == a.EmpCode).FirstOrDefaultAsync();

                                        // If there is no employee on this empcode then we reject this row and move forward
                                        if (emp == null)
                                        {
                                            result.RejectLeaveApproveList.Add(a, "There is no employee of Emp Code : " + a.EmpCode);
                                            result.RejectEmpCount++;
                                            continue;
                                        }

                                        var leaveStartDate = new DateTime();
                                        var leaveEndDate = new DateTime();

                                        bool isLeaveStartDateCorrect = DateTime.TryParse(a.LeaveStartDate, out leaveStartDate);
                                        bool isLeaveEndDateCorrect = DateTime.TryParse(a.LeaveEndDate, out leaveEndDate);

                                        if (isLeaveStartDateCorrect && isLeaveEndDateCorrect)
                                        {
                                            if (leaveStartDate > leaveEndDate)
                                            {
                                                result.RejectLeaveApproveList.Add(a, "Leave end date must be greater than start date");
                                                result.RejectEmpCount++;
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            result.RejectLeaveApproveList.Add(a, "Please Check Leave Start Date and Leave End Date.");
                                            result.RejectEmpCount++;
                                            continue;
                                        }
                                        var leave = leaveList.Where(c => c.desc.Contains(a.LeaveType.ToLower())).FirstOrDefault();

                                        if (leave == null)
                                        {
                                            result.RejectLeaveApproveList.Add(a, "Invalid Leave Type.");
                                            result.RejectEmpCount++;
                                            continue;
                                        }
                                        var leaveApprovals = new List<LeaveApproval>();

                                        for (DateTime i = leaveStartDate; i <= leaveEndDate; i = i.AddDays(1))
                                        {
                                            leaveApprovalist.Add(new LeaveApproval
                                            {
                                                EmpId = emp.EmpId,
                                                LeaveDate = i,
                                                CreatedOn = DateTime.Now,
                                                LeaveID = leave.id,
                                                Remarks = "Import From Excel",
                                                TranDate = DateTime.Now,
                                                CreatedBy = HttpContext.Current.Session["UserName"].ToString(),

                                            });
                                            result.AcceptLeaveApproveList.Add(a);
                                            result.AcceptEmpCount++;

                                        }
                                    }

                                }
                                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        db.LeaveApprovals.AddRange(leaveApprovalist);
                                        await db.SaveChangesAsync();
                                        transaction.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAttendance", "Import");
                                        throw;
                                    }
                                }
                            }


                        }



                    }

                    return result;
                }
                else
                {

                    return result;
                }
            }
            catch (Exception ex)
            {

                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAttendance", "Import");
                throw;
            }

            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

        }
        public async Task<ImportExcelResult> SaveExcelLeaveData(HttpPostedFileBase FileUpload)
        {
            var db = new PakOman_NedoEntities();

            var result = new ImportExcelResult();
            var rejectEmpList = new List<EmpExcelModel>();
            var acceptEmpList = new List<EmpExcelModel>();
            var totalEmpCount = 0;
            var filePath = "";
            List<string> data = new List<string>();

            try
            {
                if (FileUpload != null)
                {
                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                    if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        var filename = FileUpload.FileName;
                        var targetpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");
                        filePath = targetpath + DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss") + filename;
                        FileUpload.SaveAs(filePath);
                        var pathToExcelFile = filePath;

                        var excelFile = new ExcelQueryFactory(pathToExcelFile);
                        var sheetNames = excelFile.GetWorksheetNames();

                        var sickLeave = await db.LeaveSetups.AsNoTracking().Where(c => c.LeaveDsc.Contains("Sick")).FirstOrDefaultAsync();
                        var casualLeave = await db.LeaveSetups.AsNoTracking().Where(c => c.LeaveDsc.Contains("Casual")).FirstOrDefaultAsync();
                        var annualLeave = await db.LeaveSetups.AsNoTracking().Where(c => c.LeaveDsc.Contains("Annual")).FirstOrDefaultAsync();

                        foreach (var sheetName in sheetNames)
                        {
                            var artistAlbums = from a in excelFile.Worksheet<EmpLeaveRecordExcelModel>(sheetName) select a;

                            totalEmpCount += artistAlbums.Count();

                            if (artistAlbums.Count() > 0)
                            {
                                var leaveMapList = new List<LeaveMapMaster>();

                                foreach (var a in artistAlbums)
                                {
                                    if (a.EmpCode != null)
                                    {
                                        var emp = await db.EmpMasters.Where(c => c.EmpCode == a.EmpCode).FirstOrDefaultAsync();

                                        // If there is no employee on this empcode then we reject this row and move forward
                                        if (emp == null)
                                        {
                                            result.RejectEmpLeaveList.Add(a, "There is no employee of Emp Code : " + a.EmpCode);
                                            result.RejectEmpCount++;
                                            continue;
                                        }

                                        var joiningDate = new DateTime();
                                        var confirmationDate = new DateTime();
                                        var leaveStartDate = new DateTime();

                                        bool isJoiningDateCorrect = DateTime.TryParse(a.JoiningDate, out joiningDate);
                                        bool isConfirmationDateCorrect = DateTime.TryParse(a.JoiningDate, out confirmationDate);
                                        bool isLeaveStartDateCorrect = DateTime.TryParse(a.JoiningDate, out leaveStartDate);

                                        double sickLeaveCnt = 0;
                                        double casualLeaveCnt = 0;
                                        double annualLeaveCnt = 0;

                                        double.TryParse(a.Sick, out sickLeaveCnt);
                                        double.TryParse(a.Casual, out casualLeaveCnt);
                                        double.TryParse(a.Annual, out annualLeaveCnt);


                                        if (isJoiningDateCorrect && isConfirmationDateCorrect && isLeaveStartDateCorrect)
                                        {
                                            var leaveDetailList = new List<LeaveMapDetail>();

                                            if (sickLeave != null && sickLeaveCnt > 0)
                                                leaveDetailList.Add(new LeaveMapDetail
                                                {
                                                    LeaveID = sickLeave.ID,
                                                    LeaveAllowed = sickLeaveCnt
                                                });

                                            if (casualLeave != null && casualLeaveCnt > 0)
                                                leaveDetailList.Add(new LeaveMapDetail
                                                {
                                                    LeaveID = casualLeave.ID,
                                                    LeaveAllowed = casualLeaveCnt
                                                });

                                            if (annualLeave != null && annualLeaveCnt > 0)
                                                leaveDetailList.Add(new LeaveMapDetail
                                                {
                                                    LeaveID = annualLeave.ID,
                                                    LeaveAllowed = annualLeaveCnt
                                                });

                                            if (leaveDetailList.Count == 0)
                                            {
                                                result.RejectEmpLeaveList.Add(a, "There is no leave assign or this employee all leaves equal to zero.");
                                                result.RejectEmpCount++;
                                                continue;
                                            }

                                            if (db.LeaveMapMasters.Any(c => c.EmpID == emp.EmpId && leaveStartDate >= c.StartDate && leaveStartDate <= c.EndDate))
                                            {
                                                result.RejectEmpLeaveList.Add(a, "There is already leave assign between this leave date.");
                                                result.RejectEmpCount++;
                                                continue;
                                            }

                                            leaveMapList.Add(new LeaveMapMaster
                                            {
                                                EmpID = emp.EmpId,
                                                StartDate = leaveStartDate,
                                                EndDate = leaveStartDate.AddYears(1),
                                                LeaveCodes = "Sc Al ca",
                                                Year = leaveStartDate.Year,
                                                LeaveMapDetails = leaveDetailList,
                                                CreatedDate = DateTime.Now.ToString(),
                                                CreatedBy = "Import From Excel",

                                            });
                                            result.AcceptEmpLeaveList.Add(a);
                                            result.AcceptEmpCount++;

                                            emp.ConfirmDate = confirmationDate;
                                        }
                                        else
                                        {

                                            result.RejectEmpLeaveList.Add(a, "Please Check Joining Date, Confirmation date and Leave Start Date.");
                                            result.RejectEmpCount++;
                                            continue;
                                        }
                                    }

                                }
                                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        db.LeaveMapMasters.AddRange(leaveMapList);
                                        await db.SaveChangesAsync();
                                        transaction.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAttendance", "Import");
                                        throw;
                                    }
                                }
                            }


                        }



                    }

                    return result;
                }
                else
                {

                    return result;
                }
            }
            catch (Exception ex)
            {

                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAttendance", "Import");
                throw;
            }

            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

        }

        async Task<int> SaveDepartment(string departName)
        {
            using (var db = new PakOman_NedoEntities())
            {
                int departId = 0;
                try
                {
                    if (DepartList.Any(c => c.Name.Contains(departName)))
                    {
                        departId = DepartList.Where(c => c.Name.Contains(departName)).FirstOrDefault().Id;
                    }

                    else
                    {
                        var departmentModel = new DepartmentMaster
                        {
                            DepartmentDesc = departName,
                            CreatedBy = HttpContext.Current.Session["UserName"].ToString(),
                            CreatedOn = DateTime.Now,
                            EditedBy = HttpContext.Current.Session["UserName"].ToString(),
                            EditedOn = DateTime.Now
                        };
                        db.DepartmentMasters.Add(departmentModel);
                        await db.SaveChangesAsync();
                        DepartList.Add(new AnonymoueModel { Id = departId, Name = departName });
                        departId = departmentModel.DepartmentId;
                    }
                    
                    return departId;

                }
                catch (Exception)
                {
                    return departId;
                }
            }
        }

        async Task<int> SaveDesignation(string desigName)
        {
            using (var db = new PakOman_NedoEntities())
            {
                int designId = 0;
                try
                {

                    if (DesigList.Any(c => c.Name.Contains(desigName)))
                    {
                        designId = DesigList.Where(c => c.Name.Contains(desigName)).FirstOrDefault().Id;
                    }
                    else
                    {

                            var designationModel = new DesignationMaster
                            {
                                DesignationDesc = desigName,
                                CreatedBy = HttpContext.Current.Session["UserName"].ToString(),
                                CreatedOn = DateTime.Now,
                                EditedBy = HttpContext.Current.Session["UserName"].ToString(),
                                EditedOn = DateTime.Now
                            };
                            db.DesignationMasters.Add(designationModel);
                            await db.SaveChangesAsync();
                            DesigList.Add(new AnonymoueModel { Id = designId, Name = desigName });
                            designId = designationModel.DesignationId;

                    }
                    return designId;

                }
                catch (Exception)
                {
                    return designId;
                }
            }
        }

    }

    public class AnonymoueModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }
}