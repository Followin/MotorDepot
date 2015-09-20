using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MotorDepot.BLL.DTO;
using MotorDepot.WEB.Models;
using MotorDepot.WEB.Extensions;

namespace MotorDepot.WEB.Utils
{
    public class AutoMapperWebProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<RegisterDriverLicenseViewModel, DriverLicenseDTO>()
                .ForMember(x => x.VehicleClassIds, _ => _.MapFrom(x => x.VehicleClasses))
                .ForMember(x => x.VehicleClasses, _ => _.Ignore());
            Mapper.CreateMap<RegisterDriverViewModel, DriverDTO>();
            Mapper.CreateMap<RegisterUserViewModel, UserDTO>();

            Mapper.CreateMap<CreateDriveViewModel, DriveDTO>();
            Mapper.CreateMap<DimensionsViewModel, DimensionsDTO>();
            Mapper.CreateMap<CreateVehicleViewModel, VehicleDTO>()
                .ForMember(x => x.Photo, _ => _.MapFrom(x => new ImageDTO
                {
                    Type = x.Photo.ContentType,
                    Bytes = x.Photo.InputStream.ReadBytes(x.Photo.ContentLength)
                }));

            Mapper.CreateMap<EditVehicleViewModel, VehicleDTO>()
                .ForMember(x => x.Photo, _ => _.Ignore());
                


            Mapper.CreateMap<ImageDTO, ImageViewModel>();
            Mapper.CreateMap<VehicleClassDTO, VehicleClassViewModel>();
            Mapper.CreateMap<DimensionsDTO, DimensionsViewModel>();
            Mapper.CreateMap<DriveDTO, DriveViewModel>();
            Mapper.CreateMap<FuelTypeDTO, FuelTypeViewModel>();
            Mapper.CreateMap<VehicleDTO, VehicleViewModel>();

            Mapper.CreateMap<DimensionsViewModel, DimensionsDTO>();
            Mapper.CreateMap<VoyageLifeCycleViewModel, VoyageLifeCycleDTO>();
            Mapper.CreateMap<VoyagePointViewModel, VoyagePointDTO>();
            Mapper.CreateMap<CreateVoyageViewModel, VoyageDTO>()
                .ForMember(x => x.RequestedStartTime, _ => _.MapFrom(x =>
                    x.RequestedStartDate.AddHours(x.RequestedStartTime.Hour).AddMinutes(x.RequestedStartTime.Minute)))
                .ForMember(x => x.RequestedEndTime, _ => _.MapFrom(x =>
                    x.RequestedEndDate.AddHours(x.RequestedEndTime.Hour).AddMinutes(x.RequestedEndTime.Minute)));

            Mapper.CreateMap<VoyagePointDTO, VoyagePointViewModel>();
            Mapper.CreateMap<DriverDTO, RegisterDriverViewModel>();
            Mapper.CreateMap<VoyageLifeCycleDTO, VoyageLifeCycleViewModel>();
            Mapper.CreateMap<VoyageDTO, VoyageViewModel>();
            Mapper.CreateMap<DriverLicenseDTO, RegisterDriverLicenseViewModel>();

            Mapper.CreateMap<VehicleDTO, EditVehicleViewModel>()
                .ForMember(x => x.Photo, _ => _.Ignore())
                .ForMember(x => x.VehicleClassId, _ => _.MapFrom(x => x.Class.Id));
            Mapper.CreateMap<DriveDTO, CreateDriveViewModel>()
                .ForMember(x => x.FuelTypeId, _ => _.MapFrom(x => x.FuelTypeId));

            Mapper.CreateMap<DriverLicenseDTO, DriverLicenseViewModel>();
            Mapper.CreateMap<DriverDTO, DriverViewModel>();
            Mapper.CreateMap<RoleDTO, RoleViewModel>();
            Mapper.CreateMap<UserDTO, UserViewModel>();

        }
    }
}