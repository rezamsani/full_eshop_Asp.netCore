using Application.Visitors.GetTodayReportService;
using Microsoft.AspNetCore.Mvc;

namespace Admin.EndPoint.Controllers
{
    public class VisitorController : Controller
    {
        private readonly IGetTodayReportService _getTodayReportService;
        public VisitorController(IGetTodayReportService getTodayReportService)
        {
            _getTodayReportService = getTodayReportService;
        }
        public IActionResult Index()
        {
            return View(_getTodayReportService.Execute());
        }
       
    }
}
