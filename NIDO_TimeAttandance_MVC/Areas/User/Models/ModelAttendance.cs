using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data;
using LinqToExcel;
using System.Data.Entity.Validation;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;

//using Excel = Microsoft.Office.Interop.Excel;

namespace NIDO_TimeAttandance_MVC.Areas.User.Models
{
    public class ModelAttendance
    {
        public long ID { get; set; }
        public Nullable<long> EmpID { get; set; }
        public Nullable<int> LeaveID { get; set; }
        public Nullable<int> LateDeductID { get; set; }
        public Nullable<System.DateTime> LeaveStartDate { get; set; }
        public Nullable<System.DateTime> LeaveEndDate { get; set; }
        public string Reason { get; set; }
        public Nullable<int> InOutTypeID { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
        public Nullable<System.TimeSpan> AttendanceTime { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ReqTrackingID { get; set; }
        public bool IsApproved { get; set; }
        public string ReqStatus { get; set; }
        public string Code { get; set; }
        public string LeaveName { get; set; }
        public int Hlevel { get; set; }
        public long tblManager_ID { get; set; }
        public string Empname { get; set; }


        public async Task<bool> SaveManaualRequest()
        {
            try
            {
                string RequestType = "";
                using (PakOman_NedoEntities db = new PakOman_NedoEntities())
                {
                    #region tblReqest
                    tbl_Request obj = new tbl_Request();

                    var Empid = long.Parse(HttpContext.Current.Session["UserId"].ToString());
                    var Exist = await (from x in db.tbl_Request
                                       where x.EmpID == Empid && x.Code == Code && x.AttendanceDate == AttendanceDate && x.AttendanceTime == AttendanceTime && x.InOutTypeID == InOutTypeID
                                       select x).FirstOrDefaultAsync();
                    if (Exist != null)
                    {
                        throw new InvalidOperationException("403");
                    }

                    if (Code == "M")
                    {
                        obj.AttendanceDate = AttendanceDate;
                        obj.AttendanceTime = AttendanceTime;
                        obj.InOutTypeID = InOutTypeID;
                        RequestType = "Manual Request";
                    }
                    else if (Code == "D")
                    {
                        var empId = long.Parse(HttpContext.Current.Session["UserId"].ToString());
                        var assignLeave = db.LeaveMapMasters.Any(c => c.EmpID == empId);

                        if (!assignLeave)
                            throw new System.InvalidOperationException("500"); // Cannot perform this task
                        obj.LeaveEndDate = LeaveEndDate;
                        obj.LeaveID = LeaveID;
                        obj.LeaveStartDate = LeaveStartDate;
                        Code = "D";
                        RequestType = "Late Deduct Request";

                    }

                    else
                    {
                        var empId = long.Parse(HttpContext.Current.Session["UserId"].ToString());
                        var assignLeave = db.LeaveMapMasters.Any(c => c.EmpID == empId);

                        if (!assignLeave)
                            throw new System.InvalidOperationException("500"); // Cannot perform this task
                        obj.LeaveEndDate = LeaveEndDate;
                        obj.LeaveID = LeaveID;
                        obj.LeaveStartDate = LeaveStartDate;
                        Code = "L";
                        RequestType = "Leave Request";

                    }
                    obj.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    obj.CreatedDate = DateTime.Now;
                    obj.EmpID = long.Parse(HttpContext.Current.Session["UserId"].ToString());
                    obj.Reason = Reason;
                    obj.ReqStatus = "Initiated";
                    obj.ReqTrackingID = TrackingID(Code);
                    obj.Code = Code;
                    db.tbl_Request.Add(obj);
                    await db.SaveChangesAsync();
                    ReqTrackingID = obj.ReqTrackingID;
                    #endregion
                    #region tblPendingRequest
                    tbl_PendingRequestMaster objM = new tbl_PendingRequestMaster();
                    objM.CreatedBy = obj.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    objM.CreatedDate = DateTime.Now;
                    objM.RequestID = obj.ID;
                    db.tbl_PendingRequestMaster.Add(objM);
                    await db.SaveChangesAsync();
                    #endregion

                    #region tblPedingDetail                             

                    long UserID = long.Parse(HttpContext.Current.Session["UserId"].ToString());

                    //check requester is in levelcount(managerial level par he ya ni)
                    var IsManagerLevel = await db.tbl_Manager.Where(x => x.ManagerID == UserID).FirstOrDefaultAsync();

                    // agar requester managerial level ka hua
                    if (IsManagerLevel != null)
                    {
                        int id = int.Parse(UserID.ToString());
                        var data = await Task.Run(() => db.Nstp_GetManagers_Manager(id).ToList());
                        if (data.Count > 0)
                        {
                            var query = data.GroupBy(r => r.ID)
                            .Select(grp => new
                            {
                                ID = grp.Key,
                                Min = grp.Min(t => t.LevelCount)
                            }).FirstOrDefault();
                            Hlevel = int.Parse(query.Min.ToString());
                            tblManager_ID = query.ID;
                            Empname = data.Select(m => m.EmpName).FirstOrDefault();
                        }

                    }
                    //agar normal Employee he to
                    else
                    {
                        var ManagerDetail = await (from ep in db.EmpMasters
                                                   join m in db.tbl_Manager on ep.DepartmentId equals m.DepartmentID
                                                   join lev in db.tbl_ManagerLevel on m.LevelID equals lev.ID
                                                   where ep.EmpId == UserID
                                                   orderby lev.LevelCount
                                                   select new
                                                   {
                                                       m.ID,
                                                       lev.LevelCount,
                                                       ep.EmpName,
                                                   }).ToListAsync();
                        if (ManagerDetail.Count > 0)
                        {
                            var query = ManagerDetail.GroupBy(r => r.ID)
                                                        .Select(grp => new
                                                        {
                                                            ID = grp.Key,
                                                            Min = grp.Min(t => t.LevelCount),
                                                        }).FirstOrDefault();
                            Hlevel = int.Parse(query.Min.ToString());
                            tblManager_ID = query.ID;
                            Empname = ManagerDetail.Select(m => m.EmpName).FirstOrDefault();
                        }
                    }
                    if (tblManager_ID > 0)
                    {
                        tbl_PendingRequestDetail objD = new tbl_PendingRequestDetail();
                        objD.HLevel = Hlevel;
                        objD.tblManager_ID = tblManager_ID;
                        objD.MID = objM.ID;
                        objD.CreatedDate = DateTime.Now;
                        db.tbl_PendingRequestDetail.Add(objD);
                        await db.SaveChangesAsync();
                    }
                    #endregion

                    #region InsertInNotificationTable
                    var ManagerID = await db.tbl_Manager.Where(x => x.ID == tblManager_ID).FirstOrDefaultAsync();
                    if (ManagerID != null)
                    {
                        //notification                      
                        DataHelper.InsertNotification(RequestType, "There is a new " + RequestType + " of " + Empname + " for approval.", "/Management/ListAllRequest/", ManagerID.ManagerID);
                        var emailBody = "";
                        if (Code == "M")
                        {
                            var type = obj.InOutTypeID == 1 ? "In" : "Out";
                            emailBody = "There is a new " + RequestType + " of " + Empname + " for approval. The Type of request is " + type + " Date : " + obj.AttendanceDate.Value.ToString("dd/MM/yyyy") + " Time : " + obj.AttendanceTime;

                        }
                        else
                        {
                            var leaves = db.LeaveSetups.Where(c => c.ID == LeaveID).Select(c => c.LeaveDsc).FirstOrDefault();
                            emailBody = "There is a new " + RequestType + " of " + Empname + " for approval. The type of leave is " + leaves + ".Duration of Leave : " + obj.LeaveStartDate.Value.ToString("dd/MM/yyyy") + "-" + obj.LeaveEndDate.Value.ToString("dd/MM/yyyy");

                        }
                        if (ManagerID.EmpMaster.EmailAdd != null || ManagerID.EmpMaster.EmailAdd != "")
                        {
                            EmailSending.SendEmail("Manual Request Status", emailBody, 4, ManagerID.EmpMaster.EmailAdd);
                        }
                    }
                    #endregion

                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAttendance", "SaveManaualRequest");
                throw;
            }
        }

        public async Task<IList<ModelAttendance>> GetManualRequest()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    long ID = long.Parse(HttpContext.Current.Session["UserId"].ToString());
                    IList<ModelAttendance> List = await (from q in _db.tbl_Request
                                                         where q.EmpID == ID && q.IsApproved == false && q.IsCancel == false && q.IsRejected == false
                                                         orderby q.AttendanceDate
                                                         select new ModelAttendance
                                                         {
                                                             AttendanceDate = q.AttendanceDate,
                                                             AttendanceTime = q.AttendanceTime,
                                                             ID = q.ID,
                                                             InOutTypeID = q.InOutTypeID,
                                                             IsApproved = q.IsApproved,
                                                             Reason = q.Reason,
                                                             ReqStatus = q.ReqStatus,
                                                             ReqTrackingID = q.ReqTrackingID,
                                                             Code = q.Code,
                                                             LeaveEndDate = q.LeaveEndDate,
                                                             LeaveStartDate = q.LeaveStartDate,
                                                             LeaveName = q.LeaveSetup.LeaveDsc,
                                                         }).ToListAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAttendance", "GetManualRequest");
                return null;
            }


        }
        public async Task<IList<ModelAttendance>> GetAllRequest()
        {
            try
            {

                using (var _db = new PakOman_NedoEntities())
                {
                    long ID = long.Parse(HttpContext.Current.Session["UserId"].ToString());
                    IList<ModelAttendance> List = await (from q in _db.tbl_Request
                                                         where q.EmpID == ID && q.CreatedDate.Year == DateTime.Now.Year
                                                         orderby q.CreatedDate descending
                                                         select new ModelAttendance
                                                         {
                                                             ID = q.ID,
                                                             ReqStatus = q.ReqStatus,
                                                             ReqTrackingID = q.ReqTrackingID,
                                                             Code = q.Code,
                                                             CreatedDate = q.CreatedDate,
                                                         }).ToListAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAttendance", "GetAllRequest");
                return null;
            }


        }

        public string TrackingID(string Code)
        {
            string tracking = "";
            Random r = new Random();
            tracking = Code.ToString() + "-" + DateTime.Now.Date.ToString("ddMMyyy") + "-" + r.Next(3, 1000) + "-" + HttpContext.Current.Session["UserId"].ToString();
            return tracking;
        }

        public async Task<ImportExcelResult> ImportExcel(HttpPostedFileBase FileUpload)
        {
            var db = new PakOman_NedoEntities();

            var result = new ImportExcelResult();
            var totalEmpCount = 0;
            var rejectEmpList = new List<EmpExcelModel>();
            var acceptEmpList = new List<EmpExcelModel>();
            List<string> data = new List<string>();
            try
            {
                if (FileUpload != null)
                {
                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
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
                                var empCodeList = db.EmpMasters.Select(c => c.EmpCode).ToList();
                                var departmentList = db.DepartmentMasters.Select(c => new { DepartmentId = c.DepartmentId, DepartmentDesc = c.DepartmentDesc.ToLower() }).ToList();
                                var designationList = db.DesignationMasters.Select(c => new { DesignationId = c.DesignationId, DesignationDesc = c.DesignationDesc.ToLower() }).ToList();
                                var cityList = db.CityMasters.Select(c => new { CityId = c.CityId, CityDesc = c.CityDesc.ToLower() }).ToList();
                                var gradeList = db.GradeMasters.Select(c => new { GradeId = c.GradeId, GradeDesc = c.GradeDesc.ToLower() }).ToList();
                                var companyList = db.CompanyMasters.Select(c => new { CompanyId = c.CompanyId, CompanyDesc = c.CompanyDesc.ToLower() }).ToList();
                                //artistAlbums.Any(c=>c.GetType().GetProperty("Department"))

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

                                            if (departmentList.Any(c => c.DepartmentDesc.Contains(a.Department.Trim().ToLower())))
                                            {

                                                departId = departmentList.Where(c => c.DepartmentDesc.Contains(a.Department.Trim().ToLower())).FirstOrDefault().DepartmentId;
                                            }
                                            else
                                            {
                                                if (!String.IsNullOrEmpty(a.Department))
                                                {
                                                    var departmentModel = new DepartmentMaster();
                                                    departmentModel.DepartmentDesc = a.Department.Trim();
                                                    departmentModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                                                    departmentModel.CreatedOn = DateTime.Now;
                                                    departmentModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                                                    departmentModel.EditedOn = DateTime.Now;
                                                    db.DepartmentMasters.Add(departmentModel);
                                                    await db.SaveChangesAsync();
                                                    departId = departmentModel.DepartmentId;
                                                    departmentList.Add(new { DepartmentId = departId.Value, DepartmentDesc = a.Department.Trim() });
                                                }

                                            }

                                            if (designationList.Any(c => c.DesignationDesc.Contains(a.Designation.Trim().ToLower())))
                                            {
                                                desginId = designationList.Where(c => c.DesignationDesc.Contains(a.Designation.Trim().ToLower())).FirstOrDefault().DesignationId;
                                            }
                                            else
                                            {
                                                if (!String.IsNullOrEmpty(a.Designation))
                                                {
                                                    var designationModel = new DesignationMaster();
                                                    designationModel.DesignationDesc = a.Designation.Trim();
                                                    designationModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                                                    designationModel.CreatedOn = DateTime.Now;
                                                    designationModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                                                    designationModel.EditedOn = DateTime.Now;
                                                    db.DesignationMasters.Add(designationModel);
                                                    await db.SaveChangesAsync();
                                                    desginId = designationModel.DesignationId;
                                                    designationList.Add(new { DesignationId = desginId.Value, DesignationDesc = a.Designation.Trim() });

                                                }

                                            }

                                            if (cityList.Any(c => c.CityDesc.Contains(a.City.Trim().ToLower())))
                                            {
                                                cityId = cityList.Where(c => c.CityDesc.Contains(a.City.Trim().ToLower())).FirstOrDefault().CityId;
                                            }
                                            else
                                            {
                                                if (!String.IsNullOrEmpty(a.City))
                                                {
                                                    var cityModel = new CityMaster();
                                                    cityModel.CityDesc = a.City.Trim();
                                                    cityModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                                                    cityModel.CreatedOn = DateTime.Now;
                                                    cityModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                                                    cityModel.EditedOn = DateTime.Now;
                                                    db.CityMasters.Add(cityModel);
                                                    await db.SaveChangesAsync();
                                                    cityId = cityModel.CityId;
                                                    cityList.Add(new { CityId = desginId.Value, CityDesc = a.City.Trim() });
                                                }



                                            }

                                            if (gradeList.Any(c => c.GradeDesc.Contains(a.Grade.Trim().ToLower())))
                                            {
                                                gradeId = gradeList.Where(c => c.GradeDesc.Contains(a.Grade.Trim().ToLower())).FirstOrDefault().GradeId;
                                            }
                                            else
                                            {
                                                if (!String.IsNullOrEmpty(a.Grade))
                                                {
                                                    var gradeModel = new GradeMaster();
                                                    gradeModel.GradeDesc = a.Grade.Trim();
                                                    gradeModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                                                    gradeModel.CreatedOn = DateTime.Now;
                                                    gradeModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                                                    gradeModel.EditedOn = DateTime.Now;
                                                    db.GradeMasters.Add(gradeModel);
                                                    await db.SaveChangesAsync();
                                                    gradeId = gradeModel.GradeId;
                                                    gradeList.Add(new { GradeId = gradeId.Value, GradeDesc = a.Grade.Trim() });
                                                }



                                            }
                                            if (companyList.Any(c => c.CompanyDesc.Contains(a.Company.ToLower())))
                                            {
                                                companyId = companyList.Where(c => c.CompanyDesc.Contains(a.Company.Trim().ToLower())).FirstOrDefault().CompanyId;
                                            }
                                            else
                                            {
                                                if (!String.IsNullOrEmpty(a.Company))
                                                {
                                                    var companyModel = new CompanyMaster();
                                                    companyModel.CompanyDesc = a.Company.Trim();
                                                    companyModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                                                    companyModel.CreatedOn = DateTime.Now;
                                                    companyModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                                                    companyModel.EditedOn = DateTime.Now;
                                                    db.CompanyMasters.Add(companyModel);
                                                    await db.SaveChangesAsync();
                                                    gradeId = companyModel.CompanyId;
                                                    companyList.Add(new { CompanyId = companyId.Value, CompanyDesc = a.Company.Trim() });
                                                }



                                            }

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
                            var isDeleted = false;
                            var empc = "";
                            if (artistAlbums.Count() > 0)
                            {
                                var attendanceLogList = new List<AttendanceLogMaster>();
                                int month=-1, year=-1;
                                foreach (var a in artistAlbums)
                                {
                                    if (a.EmpCode != null)
                                    {
                                        month = -1;
                                        year = -1;
                                        DateTime dtin = DateTime.MinValue;
                                        DateTime dtout = DateTime.MinValue;

                                        if (DateTime.TryParse(a.Date, out dtin))
                                        {
                                            month = dtin.Month;
                                            year = dtin.Year;
                                        }
                                        if (month != -1 && year != -1)
                                        {
                                            //if(a.EmpCode == "72")
                                            //{
                                            //    var i = 0;
                                            //}
                                            var dtdata = db.AttendanceLogMasters.Where(x => x.CheckTime.Month == month && x.CheckTime.Year == year && x.AcNo == a.EmpCode).ToList();
                                            if (dtdata != null)
                                            {
                                                db.AttendanceLogMasters.RemoveRange(dtdata);
                                                await db.SaveChangesAsync();
                                            }
                                        }
                                    }
                                }
                                
                                

                                    foreach (var a in artistAlbums)
                                {
                                    if (a.EmpCode != null)
                                    {
                                        //if (!string.IsNullOrEmpty(a.ClockIn) || !string.IsNullOrEmpty(a.ClockOut) && isDeleted == false && empc != a.EmpCode)
                                        //{

                                        //    var dtdata = db.AttendanceLogMasters.Where(x => x.CheckTime.Month == 8 && x.CheckTime.Year == 2020 && x.AcNo == a.EmpCode).ToList();
                                        //    db.AttendanceLogMasters.RemoveRange(dtdata);
                                        //    isDeleted = true;
                                        //}
                                        var checktime = new DateTime();
                                        if (a.ClockIn != string.Empty)
                                        {
                                            //if (DateTime.TryParse(a.Date + " " + a.ClockIn, out checktime))
                                            if (DateTime.TryParse(a.Date, out checktime))
                                            {
                                                checktime = Convert.ToDateTime(checktime.ToString("dd/MMM/yyyy ") + a.ClockIn);
                                                var dd = db.AttendanceLogMasters.Where(x => x.CheckTime == checktime && x.AcNo == a.EmpCode).FirstOrDefault();
                                                if (dd != null)
                                                {
                                                    db.AttendanceLogMasters.Remove(dd);
                                                }
                                                attendanceLogList.Add(new AttendanceLogMaster
                                                {
                                                    AcNo = a.EmpCode,
                                                    CheckTime = checktime,
                                                    InOutTypeId = 1,
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


                                        if (a.ClockOut != string.Empty)
                                        {
                                            if (DateTime.TryParse(a.Date , out checktime))
                                            //if (DateTime.TryParse(a.Date + " " + a.ClockOut, out checktime))
                                            {
                                                checktime = Convert.ToDateTime(checktime.ToString("dd/MMM/yyyy ") + a.ClockOut);
                                                var dd = db.AttendanceLogMasters.Where(x => x.CheckTime == checktime && x.AcNo == a.EmpCode).FirstOrDefault();
                                                if (dd != null)
                                                {
                                                    db.AttendanceLogMasters.Remove(dd);
                                                }
                                                attendanceLogList.Add(new AttendanceLogMaster
                                                {
                                                    AcNo = a.EmpCode,
                                                    CheckTime = checktime,
                                                    InOutTypeId = 2,
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
                                var leaveMapList2020 = new List<LeaveMapMaster>();
                                var leaveMapList2019 = new List<LeaveMapMaster>();

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
                                        var leaveStartDate2020 = new DateTime();
                                       // var leaveStartDate2019 = new DateTime();

                                        bool isJoiningDateCorrect = DateTime.TryParse(a.JoiningDate, out joiningDate);
                                        bool isConfirmationDateCorrect = DateTime.TryParse(a.ConfirmationDate, out confirmationDate);
                                        bool isLeaveStartDateCorrect = DateTime.TryParse(a.LeaveStartDate, out leaveStartDate);
                                        bool isLeaveStartDateCorrect2020 = DateTime.TryParse(a.LeaveStartDate2020, out leaveStartDate2020);
                                        //bool isLeaveStartDateCorrect2019 = DateTime.TryParse(a.LeaveStartDate2019, out leaveStartDate2019);

                                        double sickLeaveCnt = 0;
                                        double casualLeaveCnt = 0;
                                        double annualLeaveCnt = 0;

                                        double sickLeaveCnt2020 = 0;
                                        double casualLeaveCnt2020 = 0;
                                        double annualLeaveCnt2020 = 0;

                                        //double sickLeaveCnt2019 = 0;
                                        //double casualLeaveCnt2019 = 0;
                                        //double annualLeaveCnt2019 = 0;

                                        double.TryParse(a.Sick, out sickLeaveCnt);
                                        double.TryParse(a.Casual, out casualLeaveCnt);
                                        double.TryParse(a.Annual, out annualLeaveCnt);

                                        double.TryParse(a.Sick2020, out sickLeaveCnt2020);
                                        double.TryParse(a.Casual2020, out casualLeaveCnt2020);
                                        double.TryParse(a.Annual2020, out annualLeaveCnt2020);

                                       // double.TryParse(a.Sick2019, out sickLeaveCnt2019);
                                       // double.TryParse(a.Casual2019, out casualLeaveCnt2019);
                                       // double.TryParse(a.Annual2019, out annualLeaveCnt2019);


                                        if (isJoiningDateCorrect && isConfirmationDateCorrect && isLeaveStartDateCorrect2020 && isLeaveStartDateCorrect )
                                        {
                                            var leaveDetailList = new List<LeaveMapDetail>();
                                            var leaveDetailList2019 = new List<LeaveMapDetail>();

                                            //if (sickLeave != null && sickLeaveCnt > 0)
                                            //    leaveDetailList.Add(new LeaveMapDetail
                                            //    {
                                            //        LeaveID = sickLeave.ID,
                                            //        LeaveAllowed = sickLeaveCnt
                                            //    });


                                            leaveDetailList.Add(new LeaveMapDetail
                                                {
                                                    LeaveID = sickLeave.ID,
                                                    LeaveAllowed = sickLeaveCnt2020
                                                });

                                            
                                                leaveDetailList.Add(new LeaveMapDetail
                                                {
                                                    LeaveID = casualLeave.ID,
                                                    LeaveAllowed = casualLeaveCnt2020
                                                });

                                      
                                                leaveDetailList.Add(new LeaveMapDetail
                                                {
                                                    LeaveID = annualLeave.ID,
                                                    LeaveAllowed = annualLeaveCnt2020
                                                });

                                            leaveDetailList2019.Add(new LeaveMapDetail
                                            {
                                                LeaveID = sickLeave.ID,
                                                LeaveAllowed = sickLeaveCnt
                                            });


                                            leaveDetailList2019.Add(new LeaveMapDetail
                                            {
                                                LeaveID = casualLeave.ID,
                                                LeaveAllowed = casualLeaveCnt
                                            });


                                            leaveDetailList2019.Add(new LeaveMapDetail
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
                                            //var lm = db.LeaveMapMasters.Where(c => c.EmpID == emp.EmpId && c.Year == leaveStartDate2020.Year);
                                            var lm = db.LeaveMapMasters.Where(c => c.EmpID == emp.EmpId);
                                            if (lm != null)
                                            {
                                                foreach (var ll in lm)
                                                {
                                                    db.LeaveMapDetails.RemoveRange(ll.LeaveMapDetails);
                                                }

                                                //foreach (LeaveMapDetail lmd in lm.LeaveMapDetails)
                                                //{
                                                //    lm.LeaveMapDetails.Remove(lmd);
                                                //}
                                                db.LeaveMapMasters.RemoveRange(lm);
                                                await db.SaveChangesAsync();
                                            }
                                            //if (lm != null)
                                            //{
                                            //    //foreach(LeaveMapDetail lmd in lm.LeaveMapDetails)
                                            //    //{
                                            //    //    lm.LeaveMapDetails.Remove(lmd);
                                            //    //}

                                            //    db.LeaveMapDetails.RemoveRange(lm.LeaveMapDetails);
                                            //    db.LeaveMapMasters.Remove(lm);
                                            //    await db.SaveChangesAsync();
                                            //}


                                            //if (db.LeaveMapMasters.Any(c => c.EmpID == emp.EmpId && leaveStartDate2020 >= c.StartDate && leaveStartDate2020 <= c.EndDate))
                                            //{
                                            //    result.RejectEmpLeaveList.Add(a, "There is already leave assign between this leave date.");
                                            //    result.RejectEmpCount++;
                                            //    continue;
                                            //}

                                            leaveMapList2020.Add(new LeaveMapMaster
                                            {
                                                EmpID = emp.EmpId,
                                                StartDate = leaveStartDate2020,
                                                EndDate = leaveStartDate2020.AddYears(1).AddDays(-1),
                                                LeaveCodes = "Sc Al ca",
                                                Year = leaveStartDate2020.Year,
                                                LeaveMapDetails = leaveDetailList,
                                                CreatedDate = DateTime.Now.ToString(),
                                                CreatedBy = "Import From Excel",

                                            });
                                            if (leaveStartDate2020.ToShortDateString() != leaveStartDate.ToShortDateString())
                                            {

                                                leaveMapList2019.Add(new LeaveMapMaster
                                                {
                                                    EmpID = emp.EmpId,
                                                    StartDate = leaveStartDate,
                                                    EndDate = leaveStartDate2020.AddDays(-1),
                                                    LeaveCodes = "Sc Al ca",
                                                    Year = (leaveStartDate2020.Year == 2021 ? 2020 : 2019),
                                                    LeaveMapDetails = leaveDetailList2019,
                                                    CreatedDate = DateTime.Now.ToString(),
                                                    CreatedBy = "Import From Excel",

                                                });
                                            }
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

                                        //if (isJoiningDateCorrect && isConfirmationDateCorrect && isLeaveStartDateCorrect2019)
                                        //{
                                        //    var leaveDetailList2019 = new List<LeaveMapDetail>();

                                        //    //if (sickLeave != null && sickLeaveCnt > 0)
                                        //    //    leaveDetailList.Add(new LeaveMapDetail
                                        //    //    {
                                        //    //        LeaveID = sickLeave.ID,
                                        //    //        LeaveAllowed = sickLeaveCnt
                                        //    //    });


                                        //    leaveDetailList2019.Add(new LeaveMapDetail
                                        //    {
                                        //        LeaveID = sickLeave.ID,
                                        //        LeaveAllowed = sickLeaveCnt2019
                                        //    });


                                        //    leaveDetailList2019.Add(new LeaveMapDetail
                                        //    {
                                        //        LeaveID = casualLeave.ID,
                                        //        LeaveAllowed = casualLeaveCnt2019
                                        //    });


                                        //    leaveDetailList2019.Add(new LeaveMapDetail
                                        //    {
                                        //        LeaveID = annualLeave.ID,
                                        //        LeaveAllowed = annualLeaveCnt2019
                                        //    });

                                        //    if (leaveDetailList2019.Count == 0)
                                        //    {
                                        //        result.RejectEmpLeaveList.Add(a, "There is no leave assign or this employee all leaves equal to zero.");
                                        //        result.RejectEmpCount++;
                                        //        continue;
                                        //    }
                                        //    var lm = db.LeaveMapMasters.Where(c => c.EmpID == emp.EmpId && c.Year == leaveStartDate2019.Year);
                                        //    if (lm != null)
                                        //    {
                                        //        foreach(var ll in lm)
                                        //        {
                                        //            db.LeaveMapDetails.RemoveRange(ll.LeaveMapDetails);
                                        //        }
                                                
                                        //        //foreach (LeaveMapDetail lmd in lm.LeaveMapDetails)
                                        //        //{
                                        //        //    lm.LeaveMapDetails.Remove(lmd);
                                        //        //}
                                        //        db.LeaveMapMasters.RemoveRange(lm);
                                        //        await db.SaveChangesAsync();
                                        //    }
                                        //    //if (db.LeaveMapMasters.Any(c => c.EmpID == emp.EmpId && leaveStartDate2020 >= c.StartDate && leaveStartDate2020 <= c.EndDate))
                                        //    //{
                                        //    //    result.RejectEmpLeaveList.Add(a, "There is already leave assign between this leave date.");
                                        //    //    result.RejectEmpCount++;
                                        //    //    continue;
                                        //    //}

                                        //    leaveMapList2019.Add(new LeaveMapMaster
                                        //    {
                                        //        EmpID = emp.EmpId,
                                        //        StartDate = leaveStartDate2019,
                                        //        EndDate = leaveStartDate2019.AddYears(1).AddDays(-1),
                                        //        LeaveCodes = "Sc Al ca",
                                        //        Year = leaveStartDate2019.Year,
                                        //        LeaveMapDetails = leaveDetailList2019,
                                        //        CreatedDate = DateTime.Now.ToString(),
                                        //        CreatedBy = "Import From Excel",

                                        //    });
                                        //    result.AcceptEmpLeaveList.Add(a);
                                        //    result.AcceptEmpCount++;

                                        //    emp.ConfirmDate = confirmationDate;
                                        //}
                                        //else
                                        //{

                                        //    result.RejectEmpLeaveList.Add(a, "Please Check Joining Date, Confirmation date and Leave Start Date.");
                                        //    result.RejectEmpCount++;
                                        //    continue;
                                        //}

                                    }

                                }
                                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        db.LeaveMapMasters.AddRange(leaveMapList2020);
                                        await db.SaveChangesAsync();

                                        db.LeaveMapMasters.AddRange(leaveMapList2019);
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
        //public async Task<ImportExcelResult> ImportExcel(HttpPostedFileBase FileUpload)
        //{
        //    var db = new PakOman_NedoEntities();

        //    var result = new ImportExcelResult();
        //    var totalEmpCount = 0;
        //    var rejectEmpList = new List<EmpExcelModel>();
        //    var acceptEmpList = new List<EmpExcelModel>();
        //    List<string> data = new List<string>();
        //    try
        //    {
        //        if (FileUpload != null)
        //        {
        //            // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
        //            if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //            {

        //                var app = new Excel.Application();


        //                string filename = FileUpload.FileName;
        //                string targetpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");
        //                var filePath = targetpath + DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss") + filename;
        //                FileUpload.SaveAs(filePath);
        //                string pathToExcelFile = filePath;

        //                var workbook = app.Workbooks.Open(pathToExcelFile);

        //                for (int i = 1; i <= workbook.Worksheets.Count; i++)
        //                {
        //                    var sheetNo = "Sheet" + i;

        //                    var connectionString = "";
        //                    if (filename.EndsWith(".xls"))
        //                    {
        //                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
        //                    }
        //                    else if (filename.EndsWith(".xlsx"))
        //                    {
        //                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
        //                    }

        //                    var adapter = new OleDbDataAdapter("SELECT * FROM [" + sheetNo + "$]", connectionString);
        //                    var ds = new DataSet();

        //                    adapter.Fill(ds, "ExcelTable");

        //                    DataTable dtable = ds.Tables["ExcelTable"];

        //                    string sheetName = sheetNo;

        //                    var excelFile = new ExcelQueryFactory(pathToExcelFile);


        //                    var artistAlbums = from a in excelFile.Worksheet<EmpExcelModel>(sheetName) select a;

        //                    totalEmpCount += artistAlbums.Count();

        //                    if (artistAlbums.Count() > 0)
        //                    {
        //                        var empCodeList = db.EmpMasters.Select(c => c.EmpCode).ToList();
        //                        var departmentList = db.DepartmentMasters.Select(c => new { c.DepartmentId, c.DepartmentDesc }).ToList();
        //                        var designationList = db.DesignationMasters.Select(c => new { c.DesignationId, c.DesignationDesc }).ToList();
        //                        var cityList = db.CityMasters.Select(c => new { c.CityId, c.CityDesc }).ToList();
        //                        var gradeList = db.GradeMasters.Select(c => new { c.GradeId, c.GradeDesc }).ToList();
        //                        var companyList = db.CompanyMasters.Select(c => new { c.CompanyId, c.CompanyDesc }).ToList();
        //                        //artistAlbums.Any(c=>c.GetType().GetProperty("Department"))

        //                        foreach (var a in artistAlbums)
        //                        {
        //                            int? departId, desginId, cityId, gradeId, companyId;

        //                            if (!empCodeList.Contains(a.EmpCode))
        //                            {
        //                                departId = null;
        //                                desginId = null;
        //                                cityId = null;
        //                                gradeId = null;
        //                                companyId = null;

        //                                if (departmentList.Any(c => c.DepartmentDesc == a.Department))
        //                                {
        //                                    departId = departmentList.Where(c => c.DepartmentDesc == a.Department).FirstOrDefault().DepartmentId;
        //                                }
        //                                else
        //                                {
        //                                    if (!String.IsNullOrEmpty(a.Department))
        //                                    {
        //                                        var departmentModel = new DepartmentMaster();
        //                                        departmentModel.DepartmentDesc = a.Department;
        //                                        departmentModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
        //                                        departmentModel.CreatedOn = DateTime.Now;
        //                                        departmentModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
        //                                        departmentModel.EditedOn = DateTime.Now;
        //                                        db.DepartmentMasters.Add(departmentModel);
        //                                        await db.SaveChangesAsync();
        //                                        departId = departmentModel.DepartmentId;
        //                                        departmentList.Add(new { DepartmentId = departId.Value, DepartmentDesc = a.Department });
        //                                    }

        //                                }

        //                                if (designationList.Any(c => c.DesignationDesc == a.Designation))
        //                                {
        //                                    desginId = designationList.Where(c => c.DesignationDesc == a.Designation).FirstOrDefault().DesignationId;
        //                                }
        //                                else
        //                                {
        //                                    if (!String.IsNullOrEmpty(a.Designation))
        //                                    {
        //                                        var designationModel = new DesignationMaster();
        //                                        designationModel.DesignationDesc = a.Designation;
        //                                        designationModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
        //                                        designationModel.CreatedOn = DateTime.Now;
        //                                        designationModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
        //                                        designationModel.EditedOn = DateTime.Now;
        //                                        db.DesignationMasters.Add(designationModel);
        //                                        await db.SaveChangesAsync();
        //                                        desginId = designationModel.DesignationId;
        //                                        designationList.Add(new { DesignationId = desginId.Value, DesignationDesc = a.Designation });

        //                                    }

        //                                }

        //                                if (cityList.Any(c => c.CityDesc == a.City))
        //                                {
        //                                    cityId = cityList.Where(c => c.CityDesc == a.City).FirstOrDefault().CityId;
        //                                }
        //                                else
        //                                {
        //                                    if (!String.IsNullOrEmpty(a.City))
        //                                    {
        //                                        var cityModel = new CityMaster();
        //                                        cityModel.CityDesc = a.City;
        //                                        cityModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
        //                                        cityModel.CreatedOn = DateTime.Now;
        //                                        cityModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
        //                                        cityModel.EditedOn = DateTime.Now;
        //                                        db.CityMasters.Add(cityModel);
        //                                        await db.SaveChangesAsync();
        //                                        cityId = cityModel.CityId;
        //                                        cityList.Add(new { CityId = desginId.Value, CityDesc = a.City });
        //                                    }



        //                                }

        //                                if (gradeList.Any(c => c.GradeDesc == a.Grade))
        //                                {
        //                                    gradeId = gradeList.Where(c => c.GradeDesc == a.Grade).FirstOrDefault().GradeId;
        //                                }
        //                                else
        //                                {
        //                                    if (!String.IsNullOrEmpty(a.Grade))
        //                                    {
        //                                        var gradeModel = new GradeMaster();
        //                                        gradeModel.GradeDesc = a.Grade;
        //                                        gradeModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
        //                                        gradeModel.CreatedOn = DateTime.Now;
        //                                        gradeModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
        //                                        gradeModel.EditedOn = DateTime.Now;
        //                                        db.GradeMasters.Add(gradeModel);
        //                                        await db.SaveChangesAsync();
        //                                        gradeId = gradeModel.GradeId;
        //                                        gradeList.Add(new { GradeId = gradeId.Value, GradeDesc = a.Grade });
        //                                    }



        //                                }
        //                                if (companyList.Any(c => c.CompanyDesc == a.Company))
        //                                {
        //                                    companyId = companyList.Where(c => c.CompanyDesc == a.Company).FirstOrDefault().CompanyId;
        //                                }
        //                                else
        //                                {
        //                                    if (!String.IsNullOrEmpty(a.Company))
        //                                    {
        //                                        var companyModel = new CompanyMaster();
        //                                        companyModel.CompanyDesc = a.Company;
        //                                        companyModel.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
        //                                        companyModel.CreatedOn = DateTime.Now;
        //                                        companyModel.EditedBy = HttpContext.Current.Session["UserName"].ToString();
        //                                        companyModel.EditedOn = DateTime.Now;
        //                                        db.CompanyMasters.Add(companyModel);
        //                                        await db.SaveChangesAsync();
        //                                        gradeId = companyModel.CompanyId;
        //                                        companyList.Add(new { CompanyId = companyId.Value, CompanyDesc = a.Company });
        //                                    }



        //                                }

        //                                var empModel = new EmpMaster();
        //                                empModel.EmpCode = a.EmpCode;
        //                                empModel.EmpName = a.Name;
        //                                empModel.DepartmentId = departId;
        //                                empModel.DesignationID = desginId;
        //                                empModel.CityID = cityId;
        //                                empModel.GradeId = gradeId;
        //                                empModel.EmployeeTypeID = 6;
        //                                empModel.CompanyId = companyId;
        //                                empModel.CreatedOn = DateTime.Now.ToString();
        //                                empModel.CreatedBy = "System";
        //                                empModel.EditedOn = DateTime.Now.ToString();
        //                                empModel.EditedBy = "System";

        //                                db.EmpMasters.Add(empModel);
        //                                await db.SaveChangesAsync();

        //                                var user = new UserMaster
        //                                {
        //                                    UserName = empModel.EmpName,
        //                                    UserPwd = EncryptDecrypt.EncryptEX("123", true),
        //                                    CreatedBy = "System",
        //                                    CreatedOn = DateTime.Now,
        //                                    EditedBy = "System",
        //                                    EditedOn = DateTime.Now,
        //                                    IsActive = true,
        //                                    Empid = empModel.EmpId,
        //                                    IsSuperUser = false,
        //                                    RoleID = 2

        //                                };
        //                                db.UserMasters.Add(user);
        //                                await db.SaveChangesAsync();


        //                                acceptEmpList.Add(a);
        //                            }
        //                            else
        //                            {
        //                                rejectEmpList.Add(a);
        //                            }
        //                        }
        //                    }


        //                }



        //            }
        //            //deleting excel file from folder  

        //            result.AcceptEmpList = acceptEmpList;
        //            result.RejectEmpList = rejectEmpList;
        //            result.AcceptEmpCount = acceptEmpList.Count;
        //            result.RejectEmpCount = rejectEmpList.Count;
        //            result.TotalEmpCount = totalEmpCount;
        //            return result;
        //        }
        //        else
        //        {

        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //       ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAttendance", "Import");
        //        return null;
        //        throw;
        //    }

        //}
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


    }
}