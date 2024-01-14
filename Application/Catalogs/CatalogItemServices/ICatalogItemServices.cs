using Application.Dtos;
using Application.Interfaces.Contexts;
using AutoMapper;
using Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogs.CatalogItemServices
{
    public interface ICatalogItemService
    {
        List<CatalogBrandDto> GetBrand();
        List<ListCatalogTypeDto> GetCatalogType();
        PaginetedItemDto<CatalogItemListItemDto> GetCatalogList(int page, int pageSize);
    }
    public class CatalogItemService : ICatalogItemService
    {

        private readonly IDataBaseContext context;
        private readonly IMapper mapper;

        public CatalogItemService(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public List<CatalogBrandDto> GetBrand()
        {
            var brands = context.catalogBrands
           .OrderBy(p => p.Brand).Take(500).ToList();

            var data = mapper.Map<List<CatalogBrandDto>>(brands);
            return data;
        }

        public PaginetedItemDto<CatalogItemListItemDto> GetCatalogList(int page, int pageSize)
        {
            int rowCount = 0;
            var data = context.catalogItems
                .Include(p => p.CatalogType)
                .ThenInclude(p => p.ParentCatalogType).ThenInclude(p => p.ParentCatalogType.ParentCatalogType)
                .Include(p => p.CatalogBrand)
                .ToPaged(page, pageSize, out rowCount)
                .OrderByDescending(p => p.Id)
                 //var result = mapper.Map<List<CatalogItemListItemDto>>(data);
            .Select(p => new CatalogItemListItemDto
            {
                Id = p.Id,
                Brand = p.CatalogBrand.Brand,
                Type = $"{p?.CatalogType.Type ?? ""} - {p?.CatalogType?.ParentCatalogType?.Type ?? ""}-{p?.CatalogType?.ParentCatalogType?.ParentCatalogType?.Type ?? ""}",
                AvailableStock = p.AvailableStock,
                MaxStockThreshold = p.MaxStockThreshold,
                RestockThreshold = p.RestockThreshold,
                Name = p.Name,
                Price = p.Price,
            }).ToList();

            return new PaginetedItemDto<CatalogItemListItemDto>(page, page, rowCount, data);

        }

        public List<ListCatalogTypeDto> GetCatalogType()
        {
            var types = context.catalogTypes
               .Include(p => p.ParentCatalogType)
               //.Include(p => p.ParentCatalogType)
               .ThenInclude(p => p.ParentCatalogType.ParentCatalogType)
                .Include(p => p.SubType)
                .Where(p => p.ParentCatalogTypeId != null)
                .Where(p => p.SubType.Count == 0)
                 .Select(p => new { p.Id, p.Type, p.ParentCatalogType, p.SubType })
                                .ToList()
                .Select(p => new ListCatalogTypeDto
                {
                    Id = p.Id,
                    Type = $"{p?.Type ?? ""} - {p?.ParentCatalogType?.Type ?? ""} - {p?.ParentCatalogType?.ParentCatalogType?.Type ?? ""}"
                }).ToList();
            return types;
        }
    }
    public class CatalogBrandDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
    }
    public class ListCatalogTypeDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
    public class CatalogItemListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public int AvailableStock { get; set; }
        public int RestockThreshold { get; set; }
        public int MaxStockThreshold { get; set; }
    }
}
