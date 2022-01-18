using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using System.Linq;
namespace NIDO_TimeAttandance_MVC
{
    public class NotificationComponents
    {   
        //here we will add a function for register notification (will add sql dependency)
        public void RegisterNotification(long EmpID)
        {
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            string SqlCommand = @"SELECT [NotifyFor],[NotificationDetail],[NotificationLink],[NotificationTitle] FROM [dbo].[tbl_Notifications] where  NotifyFor= @EmpID and IsSeen=0";
            // you notice here i have added table name like this [dbo].[Contacts] its mandatory when you use sql dependency
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(SqlCommand, con);
                cmd.Parameters.AddWithValue("@EmpID", EmpID);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();

                }
                cmd.Notification = null;
                //SqlDependency sqlDep = new SqlDependency(cmd);
                //sqlDep.OnChange += sqlDep_OnChange;
                
                //using (SqlDataReader reader = cmd.ExecuteReader())
                //{
                   
                //}
          
                // we must have to execute the coomand here 

               

            }
        }

        private void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;
                //from here we will send notification message to client
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                //new


                notificationHub.Clients.All.notify("added");
                //re-register notification 
                RegisterNotification(DataHelper.SessionEmpID);
            }
        }

    }
}