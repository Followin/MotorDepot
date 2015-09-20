using System;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using MotorDepot.BLL.Abstract;
using MotorDepot.BLL.Utils;
using MotorDepot.WEB.Utils;

namespace MotorDepot.WEB
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private IAuthService _service;

        public MvcApplication()
        {
            _service = DependencyResolver.Current.GetService<IAuthService>();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelMetadataProviders.Current = new MyMetadataProvider();
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ModelValidatorProviders.Providers.Add(new MyValidationProvider());

            ClientDataTypeModelValidatorProvider.ResourceClassKey = "ValidationMessages";
            DefaultModelBinder.ResourceClassKey = "ValidationMessages";

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperBLLProfile>();
                cfg.AddProfile<AutoMapperWebProfile>();
            });
        }

        protected void Session_Start()
        {
            var httpContext = HttpContext.Current;

            if (!httpContext.User.Identity.IsAuthenticated) return;

            var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;

            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
            Int32 id;
            if (!Int32.TryParse(ticket.UserData, out id))
            {
                FormsAuthentication.SignOut();
                return;
            }

            var user = _service.GetUserInfo(id);
            if (user != null) return;

            FormsAuthentication.SignOut();
            httpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
        }

        protected void Application_AuthenticateRequest()
        {
            var request = HttpContext.Current;

            var authCookie = request.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;

            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
            Int32 id;
            if (!Int32.TryParse(ticket.UserData, out id)) return;
            var user = _service.GetUserInfo(id);
            if (user == null) return;

            var identity = new GenericIdentity(ticket.Name);
            request.User = new GenericPrincipal(identity, new[] {user.Role.Name});
        }
    }
}
