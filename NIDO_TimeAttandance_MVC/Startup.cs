using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NIDO_TimeAttandance_MVC.Startup))]
namespace NIDO_TimeAttandance_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
           
            //app.MapSignalR();
        
        }
    }
}
