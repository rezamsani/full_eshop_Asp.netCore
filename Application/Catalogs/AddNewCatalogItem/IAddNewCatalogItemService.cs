using Application.Dtos;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogs.AddNewCatalogItem
{
    public interface IAddNewCatalogItemService
    {
        BaseDto<int> Execute(AddNewCatalogItemDto request);
    }
    public class AddNewCatalogItemService : IAddNewCatalogItemService
    {
        private readonly IDataBaseContext _context;
        private readonly IMapper _mapper;
        public AddNewCatalogItemService(IDataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public BaseDto<int> Execute(AddNewCatalogItemDto request)
        {
            var catalogItem = _mapper.Map<CatalogItem>(request);
            _context.catalogItems.Add(catalogItem);
            _context.SaveChanges();
            return new BaseDto<int>(true, new List<string> { "با موفقیت ثبت شد" }, catalogItem.Id);
        }
    }
    
    public class AddNewCatalogItemFeature_dto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
    }

    public class AddNewCatalogItemImage_Dto
    {
        public string Src { get; set; }
    }
}
