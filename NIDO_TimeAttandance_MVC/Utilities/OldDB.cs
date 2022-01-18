using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Utilities
{
    public static class OldDB
    {
        public static DataTable RunStoredProc(int ManagerID)
        {
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
            using (var cmd = new SqlCommand("Nstp_GetManagerTeamAttendance", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpID", SqlDbType.Int).Value = ManagerID;
                da.Fill(table);
            }
            return table;
        }

        //Created By Moiz
        public static DataTable GetTeamYearlyLateAttendace(int ManagerID, int Mode)
        {
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
            using (var cmd = new SqlCommand("[Nstp_Get_Yearly_Team_LateAttendance_ByUser]", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = ManagerID;
                cmd.Parameters.Add("@Mode", SqlDbType.Int).Value = Mode;
                da.Fill(table);
            }
            return table;
        }
        //Created By Moiz
        public static DataTable GetTea(int ManagerID, int Mode)
        {
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
            using (var cmd = new SqlCommand("[Nstp_Get_Yearly_Team_LateAttendance_ByUser]", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = ManagerID;
                cmd.Parameters.Add("@Mode", SqlDbType.Int).Value = Mode;
                da.Fill(table);
            }
            return table;
        }

        //Created By Moiez
        public static DataTable GetLeaveReport(string EmpIds, string from, string to)
        {
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
            using (var cmd = new SqlCommand("[Nstp_Get_LeaveReport]", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpIDs", SqlDbType.Text).Value = EmpIds;
                cmd.Parameters.Add("@dateFrom", SqlDbType.VarChar).Value = from;
                cmd.Parameters.Add("@dateTo", SqlDbType.VarChar).Value = to;
                da.Fill(table);
            }
            return table;
        }

        //Created By Anas
        public static DataSet GeneratePaySlip(string EmpIds, string from, string to)
        {
            try
            {

                var table = new DataTable();
                var ds = new DataSet();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
                using (var cmd = new SqlCommand("[stp_GeneratePaySlip]", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 36000000;
                    cmd.Parameters.Add("@EmpIDs", SqlDbType.Text).Value = EmpIds;
                    cmd.Parameters.Add("@dateFrom", SqlDbType.VarChar).Value = from;
                    cmd.Parameters.Add("@dateTo", SqlDbType.VarChar).Value = to;
                    da.Fill(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }

        //Created By Anas
        public static DataSet GetEmployeeHistoryReport(string EmpIds, string from, string to)
        {
            try
            {

                var table = new DataTable();
                var ds = new DataSet();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
                using (var cmd = new SqlCommand("[Nstp_Employement_History]", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 36000;
                    cmd.Parameters.Add("@EmpIds", SqlDbType.Text).Value = EmpIds;
                    cmd.Parameters.Add("@startDate", SqlDbType.VarChar).Value = from;
                    cmd.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = to;
                    da.Fill(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }

        //Created By Moiez
        public static DataTable GetAttendaceSummaryReport(string EmpIds, string from, string to)
        {
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
            using (var cmd = new SqlCommand("[Nstp_AttendanceSummary]", con))

            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpIDs", SqlDbType.Text).Value = EmpIds;
                cmd.Parameters.Add("@dateFrom", SqlDbType.VarChar).Value = from;
                cmd.Parameters.Add("@dateTo", SqlDbType.VarChar).Value = to;
                da.Fill(table);
            }

            return table;
        }
        public static DataTable AttendanceReport(string EmpIds, DateTime from, DateTime to)
        {
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
            using (var cmd = new SqlCommand("[Nstp_AttendanceByID]", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpIDs", SqlDbType.Text).Value = EmpIds;
                cmd.Parameters.Add("@dateFrom", SqlDbType.VarChar).Value = from;
                cmd.Parameters.Add("@dateTo", SqlDbType.VarChar).Value = to;
                da.Fill(table);
            }
            return table;
        }
        public static DataTable EmployeeRoaster(string EmpIds, DateTime from, DateTime to)
        {
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
            using (var cmd = new SqlCommand("[Nstp_GetRoaster]", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpIDs", SqlDbType.Text).Value = EmpIds;
                cmd.Parameters.Add("@dateFrom", SqlDbType.VarChar).Value = from;
                cmd.Parameters.Add("@dateTo", SqlDbType.VarChar).Value = to;
                da.Fill(table);
            }
            return table;
        }
    }
}