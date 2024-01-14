using Admin.EndPoint.Models.ViewModel.Catalog;
using Application.Catalogs.CatalogTypeCrud;
using AutoMapper;
using Domain.Catalogs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.EndPoint.Controllers
{
    public class CatalogTypeController : Controller
    {
        // GET: CatalogTypeController
        private readonly ICatalogTypeService _catalogTypeService;
        private readonly IMapper _mapper;
        public List<String> Message { get; set; } = new List<string>();
        public CatalogTypeController(ICatalogTypeService catalogTypeService, IMapper mapper)
        {
            _catalogTypeService = catalogTypeService;
            _mapper = mapper;
        }
        public ActionResult Index(int? parentId, int page = 1, int pageSize = 100)
        {
            return View(_catalogTypeService.GetList(parentId, page, pageSize));
        }

        // GET: CatalogTypeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CatalogTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CatalogTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CatalogTypeViewModel catalogTypeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var model = _mapper.Map<CatalogTypeDto>(catalogTypeViewModel);
            var result = _catalogTypeService.Add(model);
            if(result.IsSuccess)
            {
                return RedirectToAction(nameof(Index), new { parentId = catalogTypeViewModel.ParentCatalogTypeId});
            }
            else
            {
                //string message = ""; This Is Another Methoud
                //foreach (var item in result.Messages)
                //{
                //    message += item;
                //    message += " ";
                //}
                //TempData["ErrorMessage"] = message;
                TempData["ErrorMessage"] = result.Messages;
                return View();
            }
        }

        // GET: CatalogTypeController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _catalogTypeService.FindById(id);
            if (model.IsSuccess)
            {
                var result = _mapper.Map<CatalogTypeViewModel>(model.Data);
                return View(result);
            }
            TempData["Messages"] = model.Messages;
            return View();
        }

        // POST: CatalogTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,CatalogTypeViewModel catalogTypeViewModel)
        {
            var model = _mapper.Map<CatalogTypeDto>(catalogTypeViewModel);
            var result = _catalogTypeService.Edite(model);
            if (result.IsSuccess)
            {
                TempData["Messages"] = result.Messages;
                return View();
            }
            TempData["Messages"] = result.Messages;
            return View();
        }

        // GET: CatalogTypeController/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _catalogTypeService.FindById(id);
            if (model.IsSuccess)
            {
                var result = _mapper.Map<CatalogTypeViewModel>(model.Data);
                return View(result);
            }
            TempData["Messages"] = model.Messages;
            return View();
        }

        // POST: CatalogTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var result = _catalogTypeService.Remove(id);
            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Messages"] = result.Message;
                return View();
            }
        }
    }
}
