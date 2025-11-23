using AutoMapper;
using IDIMAdmin.Entity;
using IDIMAdmin.Extentions.File;
using IDIMAdmin.Models.Admin;
using IDIMAdmin.Models.Setup;
using IDIMAdmin.Models.User;

namespace IDIMAdmin
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            #region setup 
            CreateMap<GeneralInformationVm, GeneralInformation>();
            CreateMap<GeneralInformation, GeneralInformationVm>();
            CreateMap<UnitVm, SetupUnit>();
            CreateMap<SetupUnit, UnitVm>();
            #endregion

            #region security
            CreateMap<ApplicationVm, Application>();
            CreateMap<Application, ApplicationVm>();

            CreateMap<ActivityLogVm, ActivityLog>();
            CreateMap<ActivityLog, ActivityLogVm>();

            CreateMap<MenuVm, Menu>();
            CreateMap<Menu, MenuVm>()
                .ForMember(d => d.ApplicationDropdown, opts => opts.Ignore())
                .ForMember(d => d.ApplicationName, opts => opts.MapFrom(src => src.Application.ApplicationCode));

            CreateMap<RoleVm, Role>();
            CreateMap<Role, RoleVm>()
                .ForMember(d => d.ApplicationDropdown, opts => opts.Ignore())
                .ForMember(d => d.ApplicationName, opts => opts.MapFrom(src => src.Application.ApplicationCode));

            CreateMap<ImageSlideVm, SlideImage>();
            CreateMap<SlideImage, ImageSlideVm>()
                .ForMember(d => d.ImageFile, opts => opts.Ignore());

            CreateMap<UserPriviledgeVm, UserPrivilege>();
            CreateMap<UserPrivilege, UserPriviledgeVm>();

            CreateMap<UserVm, User>();
            CreateMap<User, UserVm>()
                .ForMember(d => d.RegimentNo, opts => opts.MapFrom(src => src.GeneralInformation.RegimentNo))
                .ForMember(d => d.UserType, opts => opts.MapFrom(src => src.UserType))
                .ForMember(d => d.Picture, opts => opts.MapFrom(src => src.GeneralInformation.Picture.ToThumbnail()));

            CreateMap<RegisterVm, User>();
            CreateMap<User, UserVm>()
                .ForMember(d => d.RegimentNo, opts => opts.MapFrom(src => src.GeneralInformation.RegimentNo))
                .ForMember(d => d.UserType, opts => opts.MapFrom(src => src.UserType))
                .ForMember(d => d.Picture, opts => opts.MapFrom(src => src.GeneralInformation.Picture.ToThumbnail()));
            
                

            #endregion
        }
    }
}