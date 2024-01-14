using Application.Catalogs.UriComposer;
using Application.Dtos;
using Application.Interfaces.Contexts;
using Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogs.GetCatalogIItemPLP
{
   public interface IGetCatalogIItemPLPService
    {
        PaginetedItemDto<CatalogPLPDto> Execute(int page, int pageSize);
    }

    public class GetCatalogIItemPLPService : IGetCatalogIItemPLPService
    {
        private readonly IDataBaseContext context;
        private readonly IUriComposerService uriComposerService;

        public GetCatalogIItemPLPService(IDataBaseContext context
            , IUriComposerService uriComposerService)
        {
            this.context = context;
            this.uriComposerService = uriComposerService;
        }
        public PaginetedItemDto<CatalogPLPDto> Execute(int page, int pageSize)
        {
            int rowCount = 0;
            var data = context.catalogItems
                .Include(p => p.CatalogItemImages)
                .OrderByDescending(p => p.Id)
                .PagedResult(page, pageSize, out rowCount)
                .Select(p => new CatalogPLPDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Rate = 4,
                    Image = uriComposerService
                    .ComposeImageUri(p.CatalogItemImages.FirstOrDefault().Src),
                }).ToList();
            return new PaginetedItemDto<CatalogPLPDto>(page, pageSize, rowCount, data);
        }
    }


    public class CatalogPLPDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public byte Rate { get; set; }
    }
}
