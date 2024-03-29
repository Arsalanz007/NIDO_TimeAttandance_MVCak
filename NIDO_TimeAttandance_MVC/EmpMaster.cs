//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NIDO_TimeAttandance_MVC
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmpMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmpMaster()
        {
            this.AttendancePostings = new HashSet<AttendancePosting>();
            this.Emp_FullAndFinalSettlement = new HashSet<Emp_FullAndFinalSettlement>();
            this.EmpEducationDetails = new HashSet<EmpEducationDetail>();
            this.EmpExperienceDetails = new HashSet<EmpExperienceDetail>();
            this.EmpFamilyDetails = new HashSet<EmpFamilyDetail>();
            this.tbl_AllocatedEOBI = new HashSet<tbl_AllocatedEOBI>();
            this.tbl_AllocatedProvidentFund = new HashSet<tbl_AllocatedProvidentFund>();
            this.tbl_SalaryIncrement_Posting = new HashSet<tbl_SalaryIncrement_Posting>();
            this.EmpScheduleMasters = new HashSet<EmpScheduleMaster>();
            this.LeaveApprovals = new HashSet<LeaveApproval>();
            this.LeaveMapMasters = new HashSet<LeaveMapMaster>();
            this.tbl_AllocatedBonuses = new HashSet<tbl_AllocatedBonuses>();
            this.tbl_AllocatedDeductions = new HashSet<tbl_AllocatedDeductions>();
            this.tbl_AllocatedLatePolicies = new HashSet<tbl_AllocatedLatePolicies>();
            this.tbl_AllocatedSecurityDeposit = new HashSet<tbl_AllocatedSecurityDeposit>();
            this.tbl_ExemptMaster = new HashSet<tbl_ExemptMaster>();
            this.tbl_Manager = new HashSet<tbl_Manager>();
            this.tbl_PFWithdrawlDetail = new HashSet<tbl_PFWithdrawlDetail>();
            this.tbl_Request = new HashSet<tbl_Request>();
            this.tbl_SalaryPostingMaster = new HashSet<tbl_SalaryPostingMaster>();
            this.tbl_SecurityDepositPosting = new HashSet<tbl_SecurityDepositPosting>();
            this.UserMasters = new HashSet<UserMaster>();
            this.tbl_AdvanceMaster = new HashSet<tbl_AdvanceMaster>();
            this.tbl_LoanMaster = new HashSet<tbl_LoanMaster>();
            this.tbl_AllocatedAllowances = new HashSet<tbl_AllocatedAllowances>();
        }
    
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpTempAdd { get; set; }
        public string EmpPermAdd { get; set; }
        public string HomeTel { get; set; }
        public string MobileNo { get; set; }
        public string EmailAdd { get; set; }
        public string DOB { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string AppointmentDate { get; set; }
        public string MotherTounge { get; set; }
        public Nullable<int> MartialStatusId { get; set; }
        public string CNICNo { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> GradeId { get; set; }
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
        public Nullable<bool> Ismodified { get; set; }
        public string BloodGroup { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public Nullable<int> ReportingToId { get; set; }
        public Nullable<int> DivisionHeadId { get; set; }
        public string ERPNo { get; set; }
        public bool LateAllow { get; set; }
        public bool HalfdayAllow { get; set; }
        public Nullable<int> Driving { get; set; }
        public Nullable<int> Passport { get; set; }
        public string EOBI { get; set; }
        public Nullable<int> EmpBackupSupervisor { get; set; }
        public Nullable<long> CountryID { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> DesignationID { get; set; }
        public string EmpImg { get; set; }
        public string Imei { get; set; }
        public Nullable<double> BasicSallary { get; set; }
        public Nullable<double> DailyWage { get; set; }
        public Nullable<int> BankId { get; set; }
        public string Bank_AccountNo { get; set; }
        public Nullable<int> EmpStatusId { get; set; }
        public Nullable<bool> IsConfirmAssgin { get; set; }
        public Nullable<System.DateTime> TrailStartDate { get; set; }
        public Nullable<System.DateTime> TrailEndDate { get; set; }
        public Nullable<double> SecurityDepositOP { get; set; }
        public Nullable<System.DateTime> AppointedStartDate { get; set; }
        public Nullable<System.DateTime> AppointedEndDate { get; set; }
        public Nullable<System.DateTime> ConfirmDate { get; set; }
        public Nullable<bool> IsLeaveDeduct { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AttendancePosting> AttendancePostings { get; set; }
        public virtual CityMaster CityMaster { get; set; }
        public virtual CompanyMaster CompanyMaster { get; set; }
        public virtual CountryMaster CountryMaster { get; set; }
        public virtual DepartmentMaster DepartmentMaster { get; set; }
        public virtual DesignationMaster DesignationMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Emp_FullAndFinalSettlement> Emp_FullAndFinalSettlement { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmpEducationDetail> EmpEducationDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmpExperienceDetail> EmpExperienceDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmpFamilyDetail> EmpFamilyDetails { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
        public virtual tbl_BankMaster tbl_BankMaster { get; set; }
        public virtual tbl_EmployeeStatus tbl_EmployeeStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AllocatedEOBI> tbl_AllocatedEOBI { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AllocatedProvidentFund> tbl_AllocatedProvidentFund { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SalaryIncrement_Posting> tbl_SalaryIncrement_Posting { get; set; }
        public virtual GradeMaster GradeMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmpScheduleMaster> EmpScheduleMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaveApproval> LeaveApprovals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaveMapMaster> LeaveMapMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AllocatedBonuses> tbl_AllocatedBonuses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AllocatedDeductions> tbl_AllocatedDeductions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AllocatedLatePolicies> tbl_AllocatedLatePolicies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AllocatedSecurityDeposit> tbl_AllocatedSecurityDeposit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ExemptMaster> tbl_ExemptMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Manager> tbl_Manager { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PFWithdrawlDetail> tbl_PFWithdrawlDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Request> tbl_Request { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SalaryPostingMaster> tbl_SalaryPostingMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SecurityDepositPosting> tbl_SecurityDepositPosting { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserMaster> UserMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AdvanceMaster> tbl_AdvanceMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LoanMaster> tbl_LoanMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AllocatedAllowances> tbl_AllocatedAllowances { get; set; }
    }
}
