using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MotorDepot.BLL.DTO;
using MotorDepot.DAL.Entities;

namespace MotorDepot.BLL.Utils
{
    

    public class AutoMapperBLLProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DimensionsDTO, Dimensions>();
            Mapper.CreateMap<DriveDTO, Drive>();
            Mapper.CreateMap<DriverDTO, Driver>();
            Mapper.CreateMap<DriverLicenseDTO, DriverLicense>()
                .ForMember(x => x.VehicleClasses, _ => _.Ignore());
            Mapper.CreateMap<FuelTypeDTO, FuelType>();
            Mapper.CreateMap<RoleDTO, Role>();
            Mapper.CreateMap<UserDTO, User>();
            Mapper.CreateMap<VehicleClassDTO, VehicleClass>();
            Mapper.CreateMap<VehicleDTO, Vehicle>();
            Mapper.CreateMap<VoyageDTO, Voyage>();
            Mapper.CreateMap<VoyageLifeCycleDTO, VoyageLifeCycle>();
            Mapper.CreateMap<VoyagePointDTO, VoyagePoint>();
            Mapper.CreateMap<DriverVoyageRequestDTO, DriverVoyageRequest>();
            Mapper.CreateMap<ImageDTO, Image>();

            Mapper.CreateMap<Dimensions, DimensionsDTO>();
            Mapper.CreateMap<Drive, DriveDTO>();
            Mapper.CreateMap<Driver, DriverDTO>();
            Mapper.CreateMap<DriverLicense, DriverLicenseDTO>()
                .ForMember(x => x.VehicleClassIds, _ => _.MapFrom(x => x.VehicleClasses.Select(vClass => vClass.Id)));
            Mapper.CreateMap<FuelType, FuelTypeDTO>();
            Mapper.CreateMap<Role, RoleDTO>();
            Mapper.CreateMap<User, UserDTO>();
            Mapper.CreateMap<VehicleClass, VehicleClassDTO>();
            Mapper.CreateMap<Vehicle, VehicleDTO>();
            Mapper.CreateMap<Voyage, VoyageDTO>();
            Mapper.CreateMap<VoyageLifeCycle, VoyageLifeCycleDTO>();
            Mapper.CreateMap<VoyagePoint, VoyagePointDTO>();
            Mapper.CreateMap<DriverVoyageRequest, DriverVoyageRequestDTO>();
            Mapper.CreateMap<Image, ImageDTO>();
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }
    }
}
