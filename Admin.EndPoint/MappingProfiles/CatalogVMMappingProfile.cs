using Admin.EndPoint.Models.ViewModel.Catalog;
using Application.Catalogs.CatalogTypeCrud;
using AutoMapper;

namespace Admin.EndPoint.MappingProfiles
{
    public class CatalogVMMappingProfile : Profile
    {
        public CatalogVMMappingProfile()
        {
            CreateMap<CatalogTypeDto, CatalogTypeViewModel>().ReverseMap();
        }
    }
}
