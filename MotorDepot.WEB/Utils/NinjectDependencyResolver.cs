using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using MotorDepot.BLL.Abstract;
using MotorDepot.BLL.Services;
using Ninject;

namespace MotorDepot.WEB.Utils
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            Bind();
        }
        

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void Bind()
        {
            kernel.Bind<IAuthService>().To<AuthService>();
            kernel.Bind<IVehicleService>().To<VehicleService>();
            kernel.Bind<IVoyageService>().To<VoyageService>();
        }

        
    }
}