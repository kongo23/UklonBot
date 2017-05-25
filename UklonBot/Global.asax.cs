using System.Web.Http;
using UklonBot.Infrastructure;

namespace UklonBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutofacConfig.ConfigureContainer();
        }
    }
}
