using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Second_Swap.Models;
using Second_Swap.Service;

namespace Second_Swap.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {


        private readonly SecondSwapContext _db;
        private readonly IFileService _fileService;
        private readonly INotyfService _notyf;

        public DashboardController(SecondSwapContext db, IFileService fileService, INotyfService notyf)
        {
            _db = db;
            _fileService = fileService;
            _notyf = notyf;
        }

        [Route("/Admin/Dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
