using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ROYALHOTEL.Models;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace ROYALHOTEL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

       

            public IActionResult Index()
            {

            ViewData["footer"] = _context.Footers.ToList();
            var hotels = _context.Hotels.ToList();
            ViewData["features"] = _context.Features.ToList();
            ViewData["about"] = _context.Aboutus.ToList();
            ViewData["blog"] = _context.Bloggings.ToList();

            var approvedTestimonials = _context.Testimonials1s
              .Include(t => t.User)
              .Include(t => t.Hotel)
              .Where(t => t.Rating == true)
              .ToList();
            ViewBag.ApprovedTestimonials = approvedTestimonials;
            return View(hotels);

            }


        public IActionResult GetHotelByRoomId(int id)
        {
            ViewData["footer"] = _context.Footers.ToList();
            var rooms= _context.Rooms.Where(x=>x.Hotelid == id ).ToList();
            return View(rooms);
        }

        public IActionResult About()
        {
            ViewData["footer"] = _context.Footers.ToList();
            var about=_context.Aboutus.ToList();

            return View( about);
        }

        public IActionResult Hotel(string searchQuery)
        {
            ViewData["footer"] = _context.Footers.ToList();
            var hotels = _context.Hotels.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                hotels = hotels.Where(h => h.Hotelname.Contains(searchQuery) || h.Location.Contains(searchQuery));
            }

            return View("Hotel", hotels.ToList());
        }

        public IActionResult Gallery()
        {
            ViewData["footer"] = _context.Footers.ToList();
            var gallery = _context.Galleries.ToList();

            return View(gallery);
        }


        public IActionResult Testimonials()
        {
            var approvedTestimonials = _context.Testimonials1s
              .Include(t => t.User)
              .Include(t => t.Hotel)
              .Where(t => t.Rating == true)
              .ToList();
            ViewBag.ApprovedTestimonials = approvedTestimonials;
            ViewData["footer"] = _context.Footers.ToList();
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["footer"] = _context.Footers.ToList();
            var contact = _context.Contactus.FirstOrDefault();

            if (contact == null)
            {
                // Handle the case where no contact information is available
                return NotFound();
            }

            return View(contact);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
