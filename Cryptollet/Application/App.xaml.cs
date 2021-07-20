using Autofac;
using Cryptollet.Common.Database;
using Cryptollet.Common.Models;
using Cryptollet.Modules.Loading;
using System.Reflection;
using Xamarin.Forms;

namespace Cryptollet
{
    public partial class App : Application
    {

        public static IContainer Container;

        public App()
        {
            InitializeComponent();
            //class used for registration
            var builder = new ContainerBuilder();
            //scan and register all classes in the assembly
            var dataAccess = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(dataAccess)
                   .AsImplementedInterfaces()
                   .AsSelf();
            builder.RegisterType<Repository<Transaction>>().As<IRepository<Transaction>>();

            //get container
            Container = builder.Build();
            //set first page
            MainPage = Container.Resolve<LoadingView>();
        }
    }
}
