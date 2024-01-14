using Application.Dtos;
using Application.Interfaces.Contexts;
using AutoMapper;
using Common;
using Domain.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogs.CatalogTypeCrud
{
    public interface ICatalogTypeService
    {
        BaseDto<CatalogTypeDto> Add(CatalogTypeDto catalogType);
        BaseDto Remove(int Id);
        BaseDto<CatalogTypeDto> Edite(CatalogTypeDto ReqcatalogType);
        BaseDto<CatalogTypeDto> FindById(int Id);
        PaginetedItemDto<CatalogTypeListDto> GetList(int? parentId, int page, int pageSize);
    }
    public class CatalogTypeService : ICatalogTypeService
    {
        private readonly IDataBaseContext _context;
        private readonly IMapper _mapper;

        public CatalogTypeService(IDataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public BaseDto<CatalogTypeDto> Add(CatalogTypeDto ReqcatalogType)
        {
            var model = _mapper.Map<CatalogType>(ReqcatalogType);
            _context.catalogTypes.Add(model);
            _context.SaveChanges();
            return new BaseDto<CatalogTypeDto>
            (
                true,
                new List<string>() { $"تایپ {model.Type} با موفقیت در سیستم ثبت شد." },
                _mapper.Map<CatalogTypeDto>(model)
            );
        }

        public BaseDto<CatalogTypeDto> Edite(CatalogTypeDto catalogType)
        {
            var model = _context.catalogTypes.SingleOrDefault(p => p.Id == catalogType.Id);
            _mapper.Map(catalogType, model);
            _context.SaveChanges();
            return new BaseDto<CatalogTypeDto>
            (
                true,
                new List<string>() { $"تایپ {model.Type} با موفقیت در سیستم ویرایش شد." },
                _mapper.Map<CatalogTypeDto>(model)
            );
        }

        public BaseDto<CatalogTypeDto> FindById(int Id)
        {
            var data = _context.catalogTypes.Find(Id);
            var result = _mapper.Map<CatalogTypeDto>(data);
            return new BaseDto<CatalogTypeDto>
            (
                true,
                new List<string>() { "اطلاعات با موفقیت در یافت شد." },
                result
            );
        }

        public PaginetedItemDto<CatalogTypeListDto> GetList(int? parentId, int page, int pageSize)
        {
            int totalCount = 0;
            var model = _context.catalogTypes
                .Where(p => p.ParentCatalogTypeId == parentId)
                .PagedResult(page, pageSize, out totalCount);
            var result = _mapper.ProjectTo<CatalogTypeListDto>(model).ToList();
            return new PaginetedItemDto<CatalogTypeListDto>(page, pageSize, totalCount, result);
        }

        public BaseDto Remove(int Id)
        {
            var catalogType = _context.catalogTypes.Find(Id);
            _context.catalogTypes.Remove(catalogType);
            _context.SaveChanges();
            return new BaseDto
            (
                 true,
                 new List<string> { $"ایتم با موفقیت حذف شد" }
            );
        }
    }
    public class CatalogTypeDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? ParentCatalogTypeId { get; set; }
    }
    public class CatalogTypeListDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int SubTypeCount { get; set; }
    }

}
