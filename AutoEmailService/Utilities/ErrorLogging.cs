using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoEmailService.Utilities
{
    public static class ErrorLogging
    {
        

        public static bool Add(string ErrorMsg,string InnerMSg, DateTime ErrorDate, string UserName,string ControllerName,string MethodName)
        {
            try
            {
                NedoDb db = new NedoDb();
                ErrorLogMaster errorLog = new ErrorLogMaster();
                errorLog.ControllerName = ControllerName;
                errorLog.ErrorDateTime = ErrorDate;
                errorLog.ErrorMessage = ErrorMsg;
                errorLog.MethodName = MethodName;
                errorLog.UserName = UserName;
                db.ErrorLogMasters.Add(errorLog);
                db.SaveChanges();
            }
            catch 
            {
                
                throw;
            }
            return false;

        }

        internal static void Add(string message, DateTime now, object p, string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}