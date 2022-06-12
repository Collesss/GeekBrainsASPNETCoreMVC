using AutoMapper;
using Repository.Models;
using Store.Models;

namespace Store.AutoMapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddProductViewModel, Product>();
            CreateMap<RemoveProductViewModel, Product>();
        }
    }
}
