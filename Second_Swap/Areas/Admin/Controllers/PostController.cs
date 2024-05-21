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
    public class PostController : Controller
    {
        private readonly SecondSwapContext _db;
        private readonly IFileService _fileService;
        private readonly INotyfService _notyf;

        public PostController(SecondSwapContext db, IFileService fileService, INotyfService notyf)
        {
            _db = db;
            _fileService = fileService;
            _notyf = notyf;
        }

        [Route("/Admin/Post")]
        public IActionResult Index()
        {
            var post = _db.Products.Include(x => x.Category).Include(x => x.ProductImages).Where(x => x.Censorship == "Approved").ToList();

            return View(post);
        }


        [Route("/Admin/Censorship")]
        public IActionResult Censorship()
        {
            var post = _db.Products.Include(x => x.ProductImages).Where(x => x.Censorship == "Processing").ToList();

            return View(post);
        }


        [Route("/Admin/Approved")]
        public IActionResult Approved(int id)
        {
            try
            {
                var post = _db.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == id);

                if (post != null) return RedirectToAction("Index");

                post.Censorship = "Approved";

                _db.Products.Update(post);
                _db.SaveChanges();

                _notyf.Success("Approved Successs");

                return RedirectToAction("Censorship");
            }
            catch
            {
                _notyf.Success("Approved Fail");

                return RedirectToAction("Censorship");
            }
        }

        [Route("/Admin/Reject")]
        public IActionResult Reject(int id)
        {
            try
            {
                var post = _db.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == id);

                if(post != null) return RedirectToAction("Index");

                post.Censorship = "Reject";

                _db.Products.Update(post);
                _db.SaveChanges();

                _notyf.Success("Reject Successs");

                return RedirectToAction("Censorship");
            }
            catch
            {
                _notyf.Success("Approved Fail");

                return RedirectToAction("Censorship");
            }

        }

    }
}
