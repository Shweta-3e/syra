using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Syra.Admin.Startup))]
namespace Syra.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }
        private void createRolesandUsers()
        {
            
        }
    }
}
