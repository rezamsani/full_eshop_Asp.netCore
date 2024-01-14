using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User
{
    public interface IUserAddressServices
    {
        List<UserAddressDto> GetUserAddresses(string userId);
        void AddNewAddress(UserAddressAddDto address);
    }
    public class UserAddressServices : IUserAddressServices
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;

        public UserAddressServices(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void AddNewAddress(UserAddressAddDto address)
        {
            var data = mapper.Map<UserAddress>(address);
            context.userAddresses.Add(data);
            context.SaveChanges();
        }

        public List<UserAddressDto> GetUserAddresses(string userId)
        {
            var Address = context.userAddresses.Where(p => p.UserId == userId);
            var data = mapper.Map<List<UserAddressDto>>(Address);
            return data;
        }
    }
    public class UserAddressDto
    {
        public string Id { get; set; }
        public string State { get;  set; }
        public string PostaCode { get; }
        public string City { get;  set; }
        public string ZipCode { get;  set; }
        public string PostalAddress { get;  set; }
        public string ReciverName { get;  set; }
    }
    public class UserAddressAddDto
    {
        public string UserId { get; set; }
        public string State { get;  set; }
        public string PostaCode { get; }
        public string City { get;  set; }
        public string ZipCode { get;  set; }
        public string PostalAddress { get;  set; }
        public string ReciverName { get;  set; }
    }
}
