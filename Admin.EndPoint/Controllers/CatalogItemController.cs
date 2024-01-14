using Application.Catalogs.AddNewCatalogItem;
using Application.Catalogs.CatalogItemServices;
using Application.Catalogs.CatalogTypeCrud;
using Application.Dtos;
using AutoMapper;
using Infrastructure.ExternalApi.ImageServer;
using MessagePack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Admin.EndPoint.Controllers
{
    public class CatalogItemController : Controller
    {
        private readonly IAddNewCatalogItemService _addNewCatalogItemService;
        private readonly ICatalogItemService _catalogItemService;
        private readonly IMapper _mapper;
        private readonly IImageUploadService _imageUploadService;
        public List<String> Message { get; set; } = new List<string>();
        public CatalogItemController(IAddNewCatalogItemService addNewCatalogItemService,
            ICatalogItemService catalogItemService,
            IImageUploadService imageUploadService,
            IMapper mapper)
        {
            _addNewCatalogItemService = addNewCatalogItemService;
            _catalogItemService = catalogItemService;
            _mapper = mapper;
            _imageUploadService = imageUploadService;
        }
        // GET: CatalogItemController
        public ActionResult Index(int page=1, int pageSize=20)
        {
            return View(_catalogItemService.GetCatalogList(page,pageSize));
        }

        // GET: CatalogItemController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CatalogItemController/Create
        public ActionResult Create()
        {
            ViewBag.CatalogType = new SelectList(_catalogItemService.GetCatalogType(), "Id", "Type");
            ViewBag.CatalogBrand = new SelectList(_catalogItemService.GetBrand(), "Id", "Brand");
            return View();
        }

        // POST: CatalogItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddNewCatalogItemDto request)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new JsonResult(new BaseDto<int>(false, allErrors.Select(p => p.ErrorMessage).ToList(), 0));
            }
            List<IFormFile> Files = new List<IFormFile>();
            for (int i = 0; i < Request.Form.Files.Count(); i++)
            {
                var file = Request.Form.Files[i];
                Files.Add(file);
            }
            
            List<AddNewCatalogItemImage_Dto> images = new List<AddNewCatalogItemImage_Dto>();
            if (Files.Count > 0)
            {
                UploadDto imageUploded = _imageUploadService.Upload(Files);
                foreach (var item in imageUploded.FileNameAddress)
                {
                    images.Add(new AddNewCatalogItemImage_Dto { Src = item });
                }
            }
            request.Images = images;
            var resultService = _addNewCatalogItemService.Execute(request);
            return new JsonResult(resultService);
        }

        // GET: CatalogItemController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CatalogItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CatalogItemController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CatalogItemController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
