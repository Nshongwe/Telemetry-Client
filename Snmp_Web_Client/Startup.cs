using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Snmp_Web_Client.Startup))]
namespace Snmp_Web_Client
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
           // ConfigureAuth(app);
        }
    }
}
