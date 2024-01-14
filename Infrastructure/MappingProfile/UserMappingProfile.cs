using Application.Catalogs.CatalogItemServices;
using Application.User;
using AutoMapper;
using Domain.Catalogs;
using Domain.Entities;
using Domain.Order;
using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MappingProfile
{
    public class UserMappingProfile:Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserAddress, UserAddressDto>().ReverseMap();
            CreateMap<UserAddressAddDto, UserAddress>().ReverseMap();
            CreateMap<UserAddress, Address>().ReverseMap();
            
            
        }
    }
}
