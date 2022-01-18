using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



    namespace NIDO_TimeAttandance_MVC.Areas.User.Models
    {
        public class EmpExcelModel
        {
            public string EmpCode { get; set; }
            public string Name { get; set; }
            public string Department { get; set; }
            public string Designation { get; set; }
            public string City { get; set; }
            public string Grade { get; set; }
            public string Company { get; set; }
        }
        public class EmpAttendanceExcelModel
        {
            public string EmpCode { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string InOutType { get; set; }
        public string ClockIn { get; set; }
            public string ClockOut { get; set; }


        }
        public class EmpLeaveRecordExcelModel
        {
            public string EmpCode { get; set; }
            public string JoiningDate { get; set; }
            public string ConfirmationDate { get; set; }
            public string LeaveStartDate { get; set; }
            public string Sick { get; set; }
            public string Casual { get; set; }
            public string Annual { get; set; }
        public string LeaveStartDate2020 { get; set; }
        public string Sick2020 { get; set; }
        public string Casual2020 { get; set; }
        public string Annual2020 { get; set; }

        public string LeaveStartDate2019 { get; set; }
        public string Sick2019 { get; set; }
        public string Casual2019 { get; set; }
        public string Annual2019 { get; set; }


    }
        public class EmpLeaveApproveExcelModel
        {
            public string EmpCode { get; set; }
            public string LeaveStartDate { get; set; }
            public string LeaveEndDate { get; set; }
            public string LeaveType { get; set; }

        }
        public class ImportExcelResult
        {
            public List<EmpExcelModel> AcceptEmpList { get; set; } = new List<EmpExcelModel>();
            public List<EmpExcelModel> RejectEmpList { get; set; } = new List<EmpExcelModel>();
            public List<EmpAttendanceExcelModel> AcceptEmpAttendList { get; set; } = new List<EmpAttendanceExcelModel>();
            public List<EmpAttendanceExcelModel> RejectEmpAttendList { get; set; } = new List<EmpAttendanceExcelModel>();
            public List<EmpLeaveRecordExcelModel> AcceptEmpLeaveList { get; set; } = new List<EmpLeaveRecordExcelModel>();
            public List<EmpLeaveApproveExcelModel> AcceptLeaveApproveList { get; set; } = new List<EmpLeaveApproveExcelModel>();
            public Dictionary<EmpLeaveRecordExcelModel, string> RejectEmpLeaveList { get; set; } = new Dictionary<EmpLeaveRecordExcelModel, string>();
            public Dictionary<EmpLeaveApproveExcelModel, string> RejectLeaveApproveList { get; set; } = new Dictionary<EmpLeaveApproveExcelModel, string>();
            public int AcceptEmpCount { get; set; }
            public int RejectEmpCount { get; set; }
            public int TotalEmpCount { get; set; }

        }
        //public class ImportExcelResult
        //{
        //    public List<EmpExcelModel> AcceptEmpList { get; set; } = new List<EmpExcelModel>();
        //    public List<EmpExcelModel> RejectEmpList { get; set; } = new List<EmpExcelModel>();
        //    public List<EmpAttendanceExcelModel> AcceptEmpAttendList { get; set; } = new List<EmpAttendanceExcelModel>();
        //    public List<EmpAttendanceExcelModel> RejectEmpAttendList { get; set; } = new List<EmpAttendanceExcelModel>();
        //    public List<EmpLeaveRecordExcelModel> AcceptEmpLeaveList { get; set; } = new List<EmpLeaveRecordExcelModel>();
        //    public List<EmpLeaveApproveExcelModel> AcceptLeaveApproveList { get; set; } = new List<EmpLeaveApproveExcelModel>();
        //    public Dictionary<EmpLeaveRecordExcelModel, string> RejectEmpLeaveList { get; set; } = new Dictionary<EmpLeaveRecordExcelModel, string>();
        //    public Dictionary<EmpLeaveApproveExcelModel, string> RejectLeaveApproveList { get; set; } = new Dictionary<EmpLeaveApproveExcelModel, string>();
        //    public int AcceptEmpCount { get; set; }
        //    public int RejectEmpCount { get; set; }
        //    public int TotalEmpCount { get; set; }

        //}

    }

