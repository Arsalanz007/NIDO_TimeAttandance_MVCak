using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Data.Entity;
//using static NIDO_TimeAttandance_MVC.DataHelper;
using NIDO_TimeAttandance_MVC;
using System.Data.Entity;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelEmpProfile
    {
        #region Properties
        public bool UpdateOnlyPic { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpTempAdd { get; set; }
        public string EmpPermAdd { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public string HomeTel { get; set; }
        public string MobileNo { get; set; }
        public string EmailAdd { get; set; }
        public string DOB { get; set; }
        public DateTime? TrailStartDate { get; set; }
        public DateTime? TrailEndDate { get; set; }
        public DateTime? AppointedStartDate { get; set; }
        public DateTime? AppointedEndDate { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string AppointmentDate { get; set; }
        public string MotherTounge { get; set; }
        public int? MartialStatusId { get; set; }
        public string CNICNo { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? DesignationId { get; set; }
        public string Designation { get; set; }
        public int? GradeId { get; set; }
        public string GradeDsc { get; set; }
        public string Ref1Name { get; set; }
        public string Ref1Add { get; set; }
        public string Ref1HomeTel { get; set; }
        public string Ref1OfficeTel { get; set; }
        public string Ref1MobileNo { get; set; }
        public string Ref1EmailAdd { get; set; }
        public string Ref2Name { get; set; }
        public string Ref2Add { get; set; }
        public string Ref2HomeTel { get; set; }
        public string Ref2OfficeTel { get; set; }
        public string Ref2MobileNo { get; set; }
        public string Ref2EmailAdd { get; set; }
        public string EmgName { get; set; }
        public string EmgAdd { get; set; }
        public string EmgHomeTel { get; set; }
        public string EmgOfficeTel { get; set; }
        public string EmgMobileNo { get; set; }
        public string EmgEmailAdd { get; set; }
        public bool ActiveInActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public string EditedOn { get; set; }
        public long EmployeeTypeID { get; set; }
        public string DOE { get; set; }
        public string ExitRemarks { get; set; }
        public bool IsAllowAccess { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool IsOTAllow { get; set; }
        public bool Ismodified { get; set; }
        public string BloodGroup { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public int ReportingToId { get; set; }
        public int DivisionHeadId { get; set; }
        public string ERPNo { get; set; }
        public bool LateAllow { get; set; }
        public bool HalfdayAllow { get; set; }
        public int Driving { get; set; }
        public int Passport { get; set; }
        public string EOBI { get; set; }
        public int EmpBackupSupervisor { get; set; }
        public long CountryID { get; set; }
        public string CountryName { get; set; }
        public string EmpImg { get; set; }
        [UIHint("File")]
        public ProfilePhoto ProfilePhotoImage { get; set; }
        public string path = string.Empty;
        public string fileName;
        public string urlpath;
        public string ImageUrl_thumb { get; set; }
        public double? BasicSallary { get; set; }
        public double? DailyWage { get; set; }
        public string EmployeeType { get; set; }
        public int? BankId { get; set; }
        public string BankAccountNo { get; set; }
        public int? EmployeeStatusId { get; set; }
        public string IMEI { get; set; }
        public bool? IsLeaveDeduct { get; set; }
        public DateTime? ConfirmationDate { get; set; }



        #endregion
        public bool UploadFile()
        {
            string fileSavePath = string.Empty;
            string tempPath = string.Empty;
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            if (true)
            {
                fileName = DateTime.Now.ToString("yyyyMMdd") + "_" + EmpCode + "_" + Path.GetFileName(ProfilePhotoImage.File.FileName);
                path = System.Web.HttpContext.Current.Server.MapPath("~");
                path += "\\Uploads\\ProfilePic\\" + "User_" + EmpCode + "\\";
                urlpath = "/uploads/ProfilePic/" + "User_" + EmpCode + "/";
            }
            else
            {

            }
            if (!Directory.Exists(path))
            {
                try { Directory.CreateDirectory(path); }
                catch (Exception ex)
                {
                   ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmpProfile", "UploadFile");
                }
            }
            try
            {
                ProfilePhotoImage.File.SaveAs(path + fileName);
                // Upload Image Thumbnail
                WebImage img = new WebImage(ProfilePhotoImage.File.InputStream);
                if (img.Width > 240)
                    img.Resize(240, 240);
                try
                {
                    img.Save(path + "thumb_" + fileName);
                    ImageUrl_thumb = urlpath + "thumb_" + fileName;
                    EmpImg = urlpath + fileName;
                    return true;
                }
                catch (Exception ex)
                {
                   ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmpProfile", "UploadFile");
                    return false;
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmpProfile", "UploadFile");
                return false;
            }
        }
        public async Task<IList<ModelEmpProfile>> GetEmployeeList()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelEmpProfile> List = await (from q in _db.EmpMasters
                                                         select new ModelEmpProfile
                                                         {
                                                             EmpId = q.EmpId,
                                                             EmpCode = q.EmpCode,
                                                             EmpName = q.EmpName,
                                                             Designation = q.DesignationMaster.DesignationDesc,
                                                             DepartmentName = q.DepartmentMaster.DepartmentDesc,
                                                             CompanyName = q.CompanyMaster.CompanyDesc,
                                                             CityName = q.CityMaster.CityDesc,
                                                             CountryName = q.CountryMaster.CountryName,
                                                             MobileNo = q.MobileNo,
                                                             ActiveInActive = q.ActiveInActive,
                                                             IsOTAllow = q.IsOTAllow,
                                                             EmpImg = q.EmpImg,
                                                             Gender = q.Gender,
                                                             BasicSallary = q.BasicSallary,
                                                             DailyWage = q.DailyWage,
                                                             EmployeeType = q.EmployeeType.EmployeeTypeDsc
                                                         }
                                            ).ToListAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmpProfile", "GetEmployeeList");
                return null; ;
            }


        }
        public async Task<bool> EmpProfile_Save()
        {
            try
            {
                if (ProfilePhotoImage != null)
                {
                    if (!UploadFile())
                        return false;
                }

                using (PakOman_NedoEntities _Db = new PakOman_NedoEntities())
                {
                    EmpMaster ObjEmp = new EmpMaster();
                    ObjEmp.ActiveInActive = ActiveInActive;
                    ObjEmp.AppointmentDate = AppointmentDate;
                    ObjEmp.BloodGroup = BloodGroup;
                    ObjEmp.CityID = CityId;
                    ObjEmp.CNICNo = CNICNo;
                    ObjEmp.CompanyId = CompanyId;
                    ObjEmp.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjEmp.CreatedOn = DateTime.Now.ToString();
                    ObjEmp.DepartmentId = DepartmentId;
                    ObjEmp.DesignationID = DesignationId;
                    ObjEmp.DOB = DOB;
                    ObjEmp.DOE = DOE;
                    ObjEmp.EmailAdd = EmailAdd;
                    ObjEmp.EmpTempAdd = EmpTempAdd;
                    ObjEmp.EmgAdd = EmgAdd;
                    ObjEmp.EmgEmailAdd = EmgEmailAdd;
                    ObjEmp.EmgHomeTel = EmgHomeTel;
                    ObjEmp.EmgMobileNo = EmgMobileNo;
                    ObjEmp.EmgName = EmgName;
                    ObjEmp.EmgOfficeTel = EmgOfficeTel;
                    ObjEmp.EmpCode = EmpCode;
                    ObjEmp.EmpImg = EmpImg;
                    ObjEmp.EmployeeTypeID = EmployeeTypeID;
                    ObjEmp.EmpName = EmpName;
                    ObjEmp.EmpPermAdd = EmpPermAdd;
                    ObjEmp.ExitRemarks = ExitRemarks;
                    ObjEmp.FatherName = FatherName;
                    ObjEmp.Gender = Gender;
                    ObjEmp.GradeId = GradeId;
                    ObjEmp.HalfdayAllow = HalfdayAllow;
                    ObjEmp.HomeTel = HomeTel;
                    ObjEmp.IsAllowAccess = IsAllowAccess;
                    ObjEmp.IsOTAllow = IsOTAllow;
                    ObjEmp.LateAllow = LateAllow;
                    ObjEmp.MartialStatusId = MartialStatusId;
                    ObjEmp.MobileNo = MobileNo;
                    ObjEmp.MotherTounge = MotherTounge;
                    ObjEmp.Nationality = Nationality;
                    ObjEmp.Password = Password;
                    ObjEmp.Ref1Add = Ref1Add;
                    ObjEmp.Ref1EmailAdd = Ref1EmailAdd;
                    ObjEmp.Ref1HomeTel = Ref1HomeTel;
                    ObjEmp.Ref1MobileNo = Ref1MobileNo;
                    ObjEmp.Ref1Name = Ref1Name;
                    ObjEmp.Ref1OfficeTel = Ref1OfficeTel;
                    ObjEmp.Ref2Add = Ref2Add;
                    ObjEmp.Ref2EmailAdd = Ref2EmailAdd;
                    ObjEmp.Ref2HomeTel = Ref2HomeTel;
                    ObjEmp.Ref2MobileNo = Ref2MobileNo;
                    ObjEmp.Ref2Name = Ref2Name;
                    ObjEmp.Ref2OfficeTel = Ref2OfficeTel;
                    ObjEmp.Religion = Religion;
                    ObjEmp.UserId = UserId;
                    ObjEmp.EmpStatusId = EmployeeStatusId;
                    ObjEmp.TrailStartDate = TrailStartDate;
                    ObjEmp.TrailEndDate = TrailEndDate;
                    ObjEmp.AppointedStartDate = AppointedStartDate;
                    ObjEmp.AppointedEndDate = AppointedEndDate;
                    ObjEmp.IsLeaveDeduct = true;

                    if (EmployeeStatusId == 3)
                        ObjEmp.IsConfirmAssgin = true;

                    _Db.EmpMasters.Add(ObjEmp);
                    await _Db.SaveChangesAsync();
                    DataHelper.Empid = ObjEmp.EmpId;

                    var manager = _Db.tbl_Manager.Where(c => c.DepartmentID == ObjEmp.DepartmentId).FirstOrDefault();
                    if(manager != null)
                    {
                    var email = DataHelper._getEmpEmails(manager.ManagerID);
                    if(email != null)
                        EmailSending.SendEmail("New Employee Added", "New Employee Added", 7, email,ObjEmp.EmpId);
                    }

                }
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmpProfile", "EmpProfile_Save");
                return false;
            }

        }
        public async Task<bool> DeleteEmployee(int ID)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();

            _db.EmpEducationDetails.RemoveRange(_db.EmpEducationDetails.Where(c => c.EmpId == ID));
            _db.EmpExperienceDetails.RemoveRange(_db.EmpExperienceDetails.Where(c => c.EmpId == ID));
            _db.EmpFamilyDetails.RemoveRange(_db.EmpFamilyDetails.Where(c => c.EmpId == ID));
            var Mastdata = await _db.EmpMasters.Where(x => x.EmpId == ID).FirstOrDefaultAsync();
            try
            {
                if (Mastdata != null)
                {
                    _db.EmpMasters.Remove(Mastdata);
                    await _db.SaveChangesAsync();

                }
                return true;


            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmpProfile", "DeleteEmployee");
                return false;

            }
        }

        public async Task<ModelEmpProfile> Get_Employee_By_Id(long Id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelEmpProfile List = await (from q in _db.EmpMasters
                                                  where q.EmpId == Id
                                                  select new ModelEmpProfile
                                                  {

                                                      EmpId = q.EmpId,
                                                      ActiveInActive = q.ActiveInActive,
                                                      AppointmentDate = q.AppointmentDate,
                                                      BloodGroup = q.BloodGroup,
                                                      CityId = q.CityID,
                                                      CNICNo = q.CNICNo,
                                                      CompanyId = q.CompanyId,
                                                      DepartmentId = q.DepartmentId,
                                                      DesignationId = q.DesignationID,
                                                      DOB = q.DOB,
                                                      DOE = q.DOE,
                                                      EmailAdd = q.EmailAdd,
                                                      EmpTempAdd = q.EmpTempAdd,
                                                      EmgAdd = q.EmgAdd,
                                                      EmgEmailAdd = q.EmgEmailAdd,
                                                      EmgHomeTel = q.EmgHomeTel,
                                                      EmgMobileNo = q.EmgMobileNo,
                                                      EmgName = q.EmgName,
                                                      EmgOfficeTel = q.EmgOfficeTel,
                                                      EmpCode = q.EmpCode,
                                                      EmpImg = q.EmpImg,
                                                      EmployeeTypeID = q.EmployeeTypeID,
                                                      EmpName = q.EmpName,
                                                      EmpPermAdd = q.EmpPermAdd,
                                                      ExitRemarks = q.ExitRemarks,
                                                      FatherName = q.FatherName,
                                                      Gender = q.Gender,
                                                      GradeId = q.GradeId,
                                                      HalfdayAllow = q.HalfdayAllow,
                                                      HomeTel = q.HomeTel,
                                                      IsAllowAccess = q.IsAllowAccess,
                                                      IsOTAllow = q.IsOTAllow,
                                                      LateAllow = q.LateAllow,
                                                      MartialStatusId = q.MartialStatusId,
                                                      MobileNo = q.MobileNo,
                                                      MotherTounge = q.MotherTounge,
                                                      Nationality = q.Nationality,
                                                      Password = q.Password,
                                                      Ref1Add = q.Ref1Add,
                                                      Ref1EmailAdd = q.Ref1EmailAdd,
                                                      Ref1HomeTel = q.Ref1HomeTel,
                                                      Ref1MobileNo = q.Ref1MobileNo,
                                                      Ref1Name = q.Ref1Name,
                                                      Ref1OfficeTel = q.Ref1OfficeTel,
                                                      Ref2Add = q.Ref2Add,
                                                      Ref2EmailAdd = q.Ref2EmailAdd,
                                                      Ref2HomeTel = q.Ref2HomeTel,
                                                      Ref2MobileNo = q.Ref2MobileNo,
                                                      Ref2Name = q.Ref2Name,
                                                      Ref2OfficeTel = q.Ref2OfficeTel,
                                                      Religion = q.Religion,
                                                      UserId = q.UserId,
                                                      BasicSallary = q.BasicSallary,
                                                      DailyWage = q.DailyWage,
                                                      BankId = q.BankId,
                                                      BankAccountNo = q.Bank_AccountNo,
                                                      EmployeeStatusId = q.EmpStatusId,
                                                      IMEI = q.Imei,
                                                      TrailStartDate = q.TrailStartDate,
                                                      TrailEndDate = q.TrailEndDate,
                                                      AppointedStartDate = q.AppointedStartDate,
                                                      AppointedEndDate = q.AppointedEndDate,
                                                      IsLeaveDeduct = q.IsLeaveDeduct==null?false:true,
                                                      ConfirmationDate = q.ConfirmDate
                                                      
                                                  }
                                            ).SingleOrDefaultAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmpProfile", "Get_Employee_By_Id");
                return null;
            }

        }
        public async Task<bool> ResetImei(long empId)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.EmpMasters.Where(x => x.EmpId == empId).FirstOrDefaultAsync();
            try
            {

                if (data != null)
                {

                    data.Imei = null;
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmpProfile", "ResetImei");
                return false;

            }
        }
        public async Task<bool> UpdateEmployeeSalaryInfo()
        {

            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.EmpMasters.Where(x => x.EmpId == this.EmpId).FirstOrDefaultAsync();
            try
            {

                if (data != null)
                {

                    data.BasicSallary = BasicSallary;
                    data.DailyWage = DailyWage;
                    data.EmployeeTypeID = EmployeeTypeID;
                    data.BankId = BankId;
                    data.Bank_AccountNo = BankAccountNo;
                    data.UserId = UserId;
                    data.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.EditedOn = DateTime.Now.ToString();
                    data.IsLeaveDeduct = IsLeaveDeduct.HasValue ? false : true;
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmpProfile", "UpdateEmployeeSalaryInfo");
                return false;

            }
        }



        public async Task<bool> UpdateEmployee()
        {
            if (ProfilePhotoImage != null)
            {
                if (!UploadFile())
                    return false;
            }
            else
            {
                EmpImg = DataHelper.EmpimgUrl;
            }
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.EmpMasters.Where(x => x.EmpId == this.EmpId).FirstOrDefaultAsync();
            var lastStatus = data.ActiveInActive;
            try
            {
                bool EmpStatusFlag = false;

                if (data != null)
                {

                    //only update userpicture from userprofile
                    if (UpdateOnlyPic == true)
                    {
                        data.EmpImg = EmpImg;
                        _db.SaveChanges();
                        return true;
                    }
                    data.ActiveInActive = ActiveInActive;
                    data.AppointmentDate = AppointmentDate;
                    data.BloodGroup = BloodGroup;
                    data.CityID = CityId;
                    data.CNICNo = CNICNo;
                    data.CompanyId = CompanyId;
                    data.DepartmentId = DepartmentId;
                    data.DesignationID = DesignationId;
                    data.DOB = DOB;
                    data.DOE = DOE;
                    data.EmailAdd = EmailAdd;
                    data.EmpTempAdd = EmpTempAdd;
                    data.EmgAdd = EmgAdd;
                    data.EmgEmailAdd = EmgEmailAdd;
                    data.EmgHomeTel = EmgHomeTel;
                    data.EmgMobileNo = EmgMobileNo;
                    data.EmgName = EmgName;
                    data.EmgOfficeTel = EmgOfficeTel;
                    data.EmpCode = EmpCode;
                    data.EmpImg = EmpImg;
                    data.EmployeeTypeID = EmployeeTypeID;
                    data.EmpName = EmpName;
                    data.EmpPermAdd = EmpPermAdd;
                    data.ExitRemarks = ExitRemarks;
                    data.FatherName = FatherName;
                    data.Gender = Gender;
                    data.GradeId = GradeId;
                    data.HalfdayAllow = HalfdayAllow;
                    data.HomeTel = HomeTel;
                    data.IsAllowAccess = IsAllowAccess;
                    data.IsOTAllow = IsOTAllow;
                    data.LateAllow = LateAllow;
                    data.MartialStatusId = MartialStatusId;
                    data.MobileNo = MobileNo;
                    data.MotherTounge = MotherTounge;
                    data.Nationality = Nationality;
                    data.Password = Password;
                    data.Ref1Add = Ref1Add;
                    data.Ref1EmailAdd = Ref1EmailAdd;
                    data.Ref1HomeTel = Ref1HomeTel;
                    data.Ref1MobileNo = Ref1MobileNo;
                    data.Ref1Name = Ref1Name;
                    data.Ref1OfficeTel = Ref1OfficeTel;
                    data.Ref2Add = Ref2Add;
                    data.Ref2EmailAdd = Ref2EmailAdd;
                    data.Ref2HomeTel = Ref2HomeTel;
                    data.Ref2MobileNo = Ref2MobileNo;
                    data.Ref2Name = Ref2Name;
                    data.Ref2OfficeTel = Ref2OfficeTel;
                    data.Religion = Religion;
                    data.UserId = UserId;
                    data.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.EditedOn = DateTime.Now.ToString();
                    data.TrailStartDate = TrailStartDate;
                    data.TrailEndDate = TrailEndDate;
                    data.AppointedStartDate = AppointedStartDate;
                    data.AppointedEndDate = AppointedEndDate;
                    data.ConfirmDate = ConfirmationDate;

                    if (EmployeeStatusId == 3 && !data.IsConfirmAssgin.HasValue)
                    {
                        data.IsConfirmAssgin = true;
                        EmpStatusFlag = true;
                    }

                    data.EmpStatusId = EmployeeStatusId;

                    await _db.SaveChangesAsync();

                    DataHelper.Empid = data.EmpId;

                    var managerObj = _db.tbl_Manager.Where(c => c.DepartmentID == data.DepartmentId).FirstOrDefault();

                    if(managerObj != null)
                    {
                        var managerId = managerObj.ManagerID;
                        var email = DataHelper._getEmpEmails(managerId);
                        if (!lastStatus && data.ActiveInActive)
                        {
                            if (email != null)
                            {
                                EmailSending.SendEmail("Employee InActive", "Employee Disabled", 6, email, data.EmpId);
                            }
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelEmpProfile", "UpdateEmployee");
                return false;

            }
        }
        public static IList<SelectListItem> _getBankAccount()
        {
            var _db = new PakOman_NedoEntities();
            IList<SelectListItem> items = (from q in _db.tbl_BankMaster
                                           select new SelectListItem { Text = q.Bank_Name, Selected = false, Value = q.Id.ToString() }).ToList();
            return items;
        }


    }
    public class ProfilePhoto
    {
        public string FooterText { get; set; }
        [Required(ErrorMessage = "Please select file.")]
        //[FileTypes("jpg,jpeg,png", ErrorMessage = "Please Select Only Jpeg|Jpg|Png file types")]
        public HttpPostedFileBase File { get; set; }
    }
}