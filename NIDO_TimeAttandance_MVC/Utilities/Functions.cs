using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace NIDO_TimeAttandance_MVC
{
    public class Functions
    {        
        public static void SendProgress(string progressMessage, int progressCount, int totalItems,string UserName)
        {
            //IN ORDER TO INVOKE SIGNALR FUNCTIONALITY DIRECTLY FROM SERVER SIDE WE MUST USE THIS
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            //CALCULATING PERCENTAGE BASED ON THE PARAMETERS SENT
            
            var percentage = (progressCount * 100) / totalItems;           
            var users = ProgressHub.uList.Where(a => a.UserName == UserName);
            if (users.Any())
            {

                //send msg to specific client 
                hubContext.Clients.Client(users.Last().ConnectionID).AddProgress(progressMessage, percentage + "%");
            } 
        }
        public static string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
    }
}