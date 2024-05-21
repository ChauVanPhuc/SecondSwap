using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Second_Swap.Models;
using Second_Swap.Service;

namespace Second_Swap.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly SecondSwapContext _db;
        private readonly IFileService _fileService;
        private readonly INotyfService _notyf;

        public FeedbackController(SecondSwapContext db, IFileService fileService, INotyfService notyf)
        {
            _db = db;
            _fileService = fileService;
            _notyf = notyf;
        }

        [Route("/Admin/Feedback")]
        public IActionResult Index()
        {
            var feedback = _db.Feedbacks.Include(x => x.Order).ThenInclude(x => x.Product).ToList();
            return View(feedback);
        }

        [Route("/Admin/FeedbackDetail/{id:}")]
        public IActionResult Index(int id)
        {
            var feedback = _db.Feedbacks.Include(x => x.Order).ThenInclude(x => x.Product).FirstOrDefault(x => x.Id == id);
            return View(feedback);
        }
    }
}
