using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Repositories;
using Ninject.Modules;

namespace MotorDepot.BLL.Utils
{
    public class ServiceModule : NinjectModule
    {
        private String connectionString;

        public ServiceModule(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public override void Load()
        {
            Bind<MotorDepotContext>().ToSelf().WithConstructorArgument(connectionString);
            Bind<IMotorDepotUnitOfWork>().To<MotorDepotUnitOfWork>();
        }
    }
}
