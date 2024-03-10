using AutoMapper;
using WemaCustomer.Application.Data;
using WemaCustomer.Application.Data.Models;
using WemaCustomer.Application.Features.Command;

namespace WemaCustomer.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCustomerRequest, Customer>();

        }
    }
}
