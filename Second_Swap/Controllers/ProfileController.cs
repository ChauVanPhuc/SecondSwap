﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Second_Swap.Models;
using Second_Swap.Service;
using Second_Swap.ViewModels;

namespace Second_Swap.Controllers
{

    [Authorize]
    public class ProfileController : Controller
    {

        private readonly SecondSwapContext _db;
        private readonly INotyfService _notyf;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileController(SecondSwapContext db, INotyfService notyf, IFileService fileService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _notyf = notyf;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
        }


        [Route("/Profile")]
        public async Task<IActionResult>  Index()
        {
            var account = _httpContextAccessor.HttpContext.Session.GetString("AccountId");
            if (account != null)
            {
                var profile = _db.Users
                    .Include(x => x.Address)
                    .ThenInclude(x => x.Province)
                    .Include(x => x.Address)
                    .ThenInclude(x => x.District)
                    .Include(x => x.Address)
                    .ThenInclude(x => x.Wards)
                    .Include(x => x.Products)
                    .ThenInclude(x => x.Category)
                    .ThenInclude(x => x.Brands)
                    .Include(x => x.Products)
                    .ThenInclude(x => x.ProductImages)
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == int.Parse(account));

                var order = _db.Orders
                            .Include(x => x.Product)
                            .Include(x=> x.Buyer)
                            .Where(x => x.BuyerId == int.Parse(account) && x.Status == "Processing")
                            .ToList();

                var orders = _db.Orders
                            .Include(o => o.Product)
                            .Include(o => o.Buyer)
                            .Where(o => o.Product.CreateBy == int.Parse(account) && o.Status == "Processing")
                            .ToList();

                var historyOrder = _db.Orders
                                    .Include(x => x.Product)
                                    .Include(x => x.Feedbacks)
                                    .Include(x => x.Buyer)
                                    .Where(x => x.BuyerId == int.Parse(account) )
                                    .ToList();

                // Tính tổng điểm trung bình của quality trong feedback của user đó
                var averageQuality = _db.Feedbacks
                                    .Where(f => f.Seller == int.Parse(account)) // Assuming the Feedback table has a UserId column
                                    .Average(f => f.Quality) ?? 0;

                ProfileDisplayModel model = new ProfileDisplayModel
                {
                    User = profile,
                    Order = orders,
                    HistoryOrder = historyOrder,
                    rate = (double)averageQuality
                };

                return View(model);
            }
            return Redirect("/Login");
        }

        [Route("/Profile/User/{id:}")]
        public async Task<IActionResult> ProfileUser(int id)
        {
            var user = _db.Users.AsNoTracking().SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                IEnumerable<Product> product = _db.Products.Include(x => x.ProductImages).Where(x => x.CreateBy == user.Id).ToList();

                ProfileUserDisplayModel model = new ProfileUserDisplayModel
                {
                    User = user,
                    Products = product
                };

                return View(model);
            }
        }
    }
}
