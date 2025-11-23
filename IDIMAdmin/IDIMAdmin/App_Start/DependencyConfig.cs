using System;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;

using AutoMapper;

using IDIMAdmin.Services.Admin;
using IDIMAdmin.Services.Setup;
using IDIMAdmin.Services.User;

namespace IDIMAdmin
{
	public static class DependencyConfig
    {
        private static readonly Lazy<IContainer> Container = new Lazy<IContainer>(() =>
        {
            var builder = new ContainerBuilder();

            Configure(builder);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return container;
        });

        public static IContainer GetConfiguredContainer()
        {
            return Container.Value;
        }

        private static void Configure(ContainerBuilder builder)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;

                // Auto-mapper profiles
                cfg.AddProfile<MapperConfig>();
            });

            builder.Register(c => mapperConfig.CreateMapper())
                .Named<IMapper>("IDIM")
                .SingleInstance();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());

            #region setup
            builder.Register(c => new GeneralInformationService(c.ResolveNamed<IMapper>("IDIM"))).As<IGeneralInformationService>().InstancePerLifetimeScope();
            builder.Register(c => new UnitService(c.ResolveNamed<IMapper>("IDIM"))).As<IUnitService>().InstancePerLifetimeScope();
            #endregion

            #region admin
            builder.Register(c => new DashboardService(c.ResolveNamed<IMapper>("IDIM"))).As<IDashboardService>().InstancePerLifetimeScope();
            #endregion

            #region security            
            builder.Register(c => new ApplicationService(c.ResolveNamed<IMapper>("IDIM"))).As<IApplicationService>().InstancePerLifetimeScope();
            builder.Register(c => new UserApplicationService(c.ResolveNamed<IMapper>("IDIM"))).As<IUserApplicationService>().InstancePerLifetimeScope();
            builder.Register(c => new UserService(c.ResolveNamed<IMapper>("IDIM"))).As<IUserService>().InstancePerLifetimeScope();
            builder.Register(c => new DeviceService(c.ResolveNamed<IMapper>("IDIM"))).As<IDeviceService>().InstancePerLifetimeScope();
            builder.Register(c => new MenuService(c.ResolveNamed<IMapper>("IDIM"))).As<IMenuService>().InstancePerLifetimeScope();
            builder.Register(c => new RoleService(c.ResolveNamed<IMapper>("IDIM"))).As<IRoleService>().InstancePerLifetimeScope();
            builder.Register(c => new UserPriviledgeService(c.ResolveNamed<IMapper>("IDIM"))).As<IUserPriviledgeService>().InstancePerLifetimeScope();
            builder.Register(c => new RoleMenuPermissionService(c.ResolveNamed<IMapper>("IDIM"))).As<IRoleMenuPermissionService>().InstancePerLifetimeScope();
            builder.Register(c => new UserRolePermissionService(c.ResolveNamed<IMapper>("IDIM"))).As<IUserRolePermissionService>().InstancePerLifetimeScope();
            builder.Register(c => new ActivityLogService(c.ResolveNamed<IMapper>("IDIM"))).As<IActivityLogService>().InstancePerLifetimeScope();
            builder.Register(c => new ImageSlideService(c.ResolveNamed<IMapper>("IDIM"))).As<IImageSlideService>().InstancePerLifetimeScope();
            #endregion
        }
    }
}
