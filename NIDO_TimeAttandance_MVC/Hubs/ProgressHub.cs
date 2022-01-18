using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace NIDO_TimeAttandance_MVC
{
    public class ProgressHub : Hub
    {
      public static List<UserConnection> uList = new List<UserConnection>();        
        public override Task OnConnected()
        {
            var us = new  UserConnection();
            us.UserName = Context.QueryString["UserName"].ToString();
            us.ConnectionID = Context.ConnectionId;
            uList.Add(us);
            return base.OnConnected();
        }
    }
    public class  UserConnection
    {
        public string UserName { set; get; }
        public string ConnectionID { set; get; }
    }

}