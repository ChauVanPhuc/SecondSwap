using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Second_Swap.Models;
using Second_Swap.Service;
using Second_Swap.ViewModels;

namespace Second_Swap.Controllers
{
    public class FeedbackController : Controller
    {

        private readonly SecondSwapContext _db;
        private readonly INotyfService _notyf;
        private readonly IFileService _fileService;
        private readonly IVnPayService _vnPayservice;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        public FeedbackController(SecondSwapContext db, INotyfService notyf, IFileService fileService,
            IVnPayService vnPayservice, IEmailService emailService, IUserService userService)
        {
            _db = db;
            _notyf = notyf;
            _fileService = fileService;
            _vnPayservice = vnPayservice;
            _emailService = emailService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }


        [Route("/FeedBack/AddFeedback")]
        public IActionResult AddFeedback(int id)
        {
            ViewData["OrderId"] = id;
            return View();
        }


        [Route("/FeedBack/AddFeedback")]
        [HttpPost]
        public IActionResult AddFeedback(FeedbackViewModel model,int id)
        {

            if (ModelState.IsValid)
            {
                string fileVideo = "";

                var ext = Path.GetExtension(model.FileVideo.FileName);
                var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv", ".flv" };

                if (allowedExtensions.Contains(ext))
                {
                    fileVideo = _fileService.SaveImage(model.FileVideo);
                }
                else
                {
                    _notyf.Error("Video, Invalid file type. Please upload a video file.");
                    ModelState.AddModelError("Video", "Invalid file type. Please upload a video file.");
                    return View();
                }

                var order = _db.Orders.Include(x => x.Product).FirstOrDefault(x => x.Id == id);
                if (order == null) return NotFound();

                Feedback feedback = new Feedback
                {
                    Comment = model.Comment,
                    Quality = model.Quality,
                    Video = fileVideo,
                    OrderId = order.Id,
                    Seller = order.Product.CreateBy
                };

                _db.Feedbacks.Add(feedback);
                _db.SaveChanges();

                _notyf.Success("Feedback Success");

                return Redirect("/");
            }

            _notyf.Error("Feedback Fail");

            return View();
        }
    }
}
