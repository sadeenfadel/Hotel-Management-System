using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ROYALHOTEL.Models;

namespace ROYALHOTEL.Controllers
{
    public class UserController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public UserController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }
        public IActionResult Index()
        {
            ViewBag.Name = HttpContext.Session.GetString("UserNAME");
            var id = HttpContext.Session.GetInt32("Userid");
            var UserId = _context.Users.Where(x => x.Userid == id).SingleOrDefault();
            ViewBag.LName = UserId.Userlname;
            ViewBag.ProfileImagePath = UserId?.Imagepath ?? "cust1.jpg";


           
            ViewData["footer"] = _context.Footers.ToList();
            ViewData["home"] = _context.Homepages.ToList();
            var hotels = _context.Hotels.ToList();
            ViewData["features"] = _context.Features.ToList();
            ViewData["about"] = _context.Aboutus.ToList();
            var test = _context.Testimonials1s.Include(t => t.User).ToList();

            ViewData["blog"] = _context.Bloggings.ToList();
            var tuple = Tuple.Create<IEnumerable<Hotel>,IEnumerable<Testimonials1>>(hotels,test);
           
            return View(tuple);

        }


        public IActionResult GetHotelByRoomId(int id)
        {
            ViewData["footer"] = _context.Footers.ToList();
            var rooms = _context.Rooms.Where(x => x.Hotelid == id).ToList();
            return View(rooms);
        }

        public IActionResult About()
        {
            ViewData["footer"] = _context.Footers.ToList();
            var about = _context.Aboutus.ToList();

            return View(about);
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





        /*     public IActionResult AddTestimonial()
             {
                 ViewData["footer"] = _context.Footers.ToList();
                 var userId = HttpContext.Session.GetInt32("Userid");
                 var user = _context.Users.FirstOrDefault(u => u.Userid == userId);

                 ViewBag.UserName = $"{user?.Userfname} {user?.Userlname}";
                 ViewBag.HotelList = new SelectList(_context.Hotels, "Hotelid", "Hotelname");
                 return View();
             }


             [HttpPost]
             [ValidateAntiForgeryToken]
             public async Task<IActionResult> AddTestimonial([Bind("Userfname,Userlname,Email,Hotelid,ImageFile,Testimonialtext")] Testimonials1 testimonial)
             {
                 if (ModelState.IsValid)
                 {
                     // Save the image file if uploaded
                     if (testimonial.ImageFile != null)
                     {
                         string wwwRootPath = _webHostEnviroment.WebRootPath;
                         string fileName = Guid.NewGuid().ToString() + "_" + testimonial.ImageFile.FileName;
                         string path = Path.Combine(wwwRootPath, "Images", fileName);

                         using (var fileStream = new FileStream(path, FileMode.Create))
                         {
                             await testimonial.ImageFile.CopyToAsync(fileStream);
                         }

                         testimonial.Imagepath = fileName;
                     }

                     var userId = HttpContext.Session.GetInt32("Userid");
                     testimonial.Userid = userId ?? 0; // Assign the user ID

                     testimonial.Datecreated = DateTime.Now;
                     testimonial.Rating = false; // Initially set to false until approved by the admin

                     _context.Add(testimonial);
                     await _context.SaveChangesAsync();

                     return RedirectToAction(nameof(Index)); // Or any other action after submission
                 }

                 ViewBag.HotelList = new SelectList(_context.Hotels, "Hotelid", "Hotelname", testimonial.Hotelid);
                 return View(testimonial);
             }


             */
        /*      public IActionResult AddTestimonial()
              {
                  ViewData["footer"] = _context.Footers.ToList();
                  ViewBag.HotelList = new SelectList(_context.Hotels, "Hotelid", "Hotelname");
                  // Assuming you have logic to get the logged-in user's details, adjust as needed.
                  var userId = HttpContext.Session.GetInt32("Userid");
                  var user = _context.Users.FirstOrDefault(u => u.Userid == userId);

                  if (user != null)
                  {
                      ViewBag.UserName = $"{user.Userfname} {user.Userlname}";
                  }

                  return View();
              }

              [HttpPost]
              [ValidateAntiForgeryToken]
              public async Task<IActionResult> AddTestimonial([Bind("Testimonialid,Userfname,Userlname,Email,Hotelid,ImageFile,Testimonialtext")] Testimonials1 testimonial)
              {
                  if (!ModelState.IsValid)
                  {
                      // Save the image file if uploaded
                      if (testimonial.ImageFile != null)
                      {
                          string wwwRootPath = _webHostEnviroment.WebRootPath;
                          string fileName = Guid.NewGuid().ToString() + "_" + testimonial.ImageFile.FileName;
                          string path = Path.Combine(wwwRootPath + "/Images", fileName);

                          using (var fileStream = new FileStream(path, FileMode.Create))
                          {
                              await testimonial.ImageFile.CopyToAsync(fileStream);
                          }

                          testimonial.Imagepath = fileName;
                      }

                      var userId = HttpContext.Session.GetInt32("Userid");
                      if (userId.HasValue)
                      {
                          testimonial.Userid = userId.Value; // Assign the user ID
                      }
                      else
                      {
                          ModelState.AddModelError("", "User ID is required");
                          return View(testimonial);
                      }

                      testimonial.Datecreated = DateTime.Now;
                      testimonial.Rating = false; // Initially set to false until approved by the admin

                      _context.Add(testimonial);
                      await _context.SaveChangesAsync();

                      return RedirectToAction(nameof(Index)); // Or any other action after submission
                  }

                  ViewBag.HotelList = new SelectList(_context.Hotels, "Hotelid", "Hotelname", testimonial.Hotelid);
                  return View(testimonial);
              }




              */





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

          public IActionResult ProfileView()
            {

            ViewData["footer"] = _context.Footers.ToList();

            var userId = HttpContext.Session.GetInt32("Userid");
                if (userId == null)
                {
                    return RedirectToAction("UserLogin", "LoginandRegister");
                }
            // Convert userId to decimal
            decimal userDecimalId = Convert.ToDecimal(userId);

            var user = _context.Users.Find(userDecimalId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
            }

        [HttpGet]
        public async Task<IActionResult> ProfileEdit()
        {
            ViewData["footer"] = _context.Footers.ToList();
            var userId = HttpContext.Session.GetInt32("Userid");
            if (userId == null)
            {
                return RedirectToAction("UserLogin", "LoginandRegister");
            }
            // Convert userId to decimal
            decimal userDecimalId = Convert.ToDecimal(userId);

            var user = await _context.Users.FindAsync(userDecimalId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileEdit([Bind("Userid,Userfname,Userlname,Email,ImageFile")] User user)
        {
            if (!ModelState.IsValid)
            {
                if (user.ImageFile != null)
                {
                    // Handle image upload
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await user.ImageFile.CopyToAsync(fileStream);
                    }

                    user.Imagepath = fileName;
                }

                _context.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("ProfileView");
            }

            return View(user);
        }






        [HttpGet]
        public IActionResult Search()
        {
            ViewData["footer"] = _context.Footers.ToList();
            var modelContext= _context.Hotels.ToList();
            return View(modelContext); 
        }






        public IActionResult Invoice()//user view
        {
            ViewData["footer"] = _context.Footers.ToList();
            return View();
        }

    }
}
