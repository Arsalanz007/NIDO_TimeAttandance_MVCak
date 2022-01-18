using AutoEmailService.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Web;


namespace AutoEmailService
{

    public static class DataHelper
    {
        public static string Data { get; set; }
        public static long Empid { get; set; }
        public static string MsgSuccess { get; set; }
        public static string EmpimgUrl { get; set; }
        public static long SessionEmpID { get; set; }
        public static string SessionUserName { get; set; }
        public static string[] QuerystringData { get; set; }
        public static IList<string> emailsList { get; set; }
        public static IList<long> EmpList { get; set; }
        public static bool IsNearExpire { get; set; }
        public static int DaysExpireRemaining { get; set; }
        public static bool IsAnyNoticiation { get; set; }
        public static string MessageForNotication { get; set; }
        public static string DateFrom { get; set; }
        public static string Dateto { get; set; }
        public static tbl_IconControl IconSession { get; set; }


        public static NedoDb _db = new NedoDb();





        //created  by moiez
        public static IList<string> _getEmpEmails(long[] empids)
        {
            IList<string> items = (from x in _db.EmpMasters
                                   where empids.Contains(x.EmpId) && x.EmailAdd != null
                                   select x.EmailAdd).ToList();
            return items;

        }
        public static IList<string> _getEmpEmailsWithID(long[] empids)
        {
            IList<string> items = (from x in _db.EmpMasters
                                   where empids.Contains(x.EmpId) && x.EmailAdd != null
                                   select x.EmailAdd + "," + x.EmpId.ToString()).ToList();
            return items;

        }

    }
}


