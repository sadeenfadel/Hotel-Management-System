using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ROYALHOTEL.Models;

namespace ROYALHOTEL.Controllers
{

    public class AdminController1 : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public AdminController1(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }



        public IActionResult Index()
        {

            var id = HttpContext.Session.GetInt32("ADMINid");
            var UserId = _context.Users.Where(x => x.Userid == id).SingleOrDefault();
            ViewBag.LName = UserId.Userlname;
            ViewBag.ProfileImagePath = UserId?.Imagepath ?? "cust1.jpg";


            ViewBag.Name = HttpContext.Session.GetString("UserNAME");
            ViewBag.numofhotels = _context.Hotels.Count();
            ViewBag.numofusers = _context.Users.Count();
            ViewBag.numavarooms = _context.Rooms.Count(r => r.Isavailable == "Y");
            ViewBag.unavaliablerooms = _context.Rooms.Count(r => r.Isavailable == "N");


            var adminRoleId = _context.Roles.FirstOrDefault(r => r.Rolename == "Admin").Roleid;
            var userRoleId = _context.Roles.FirstOrDefault(r => r.Rolename == "User").Roleid;

            // Get the count of regular users (Users with Role "User")
            int userCount = _context.Logins.Where(l => l.Roleid == userRoleId).Count();

            // Get the count of admins (Users with Role "Admin")
            int adminCount = _context.Logins.Where(l => l.Roleid == adminRoleId).Count();



            // Pass data to the view using ViewBag
            ViewBag.UserCount = userCount;
            ViewBag.AdminCount = adminCount;




            var Users = _context.Users.ToList();
            var Role = _context.Roles.ToList();
            var Login = _context.Logins.ToList();

            ViewData["hotels"] = _context.Hotels.ToList();
            ViewData["reports"] = _context.Reportings.ToList();
            ViewBag.res = _context.Reservations.ToList();

            var multiData = from u in Users
                            join l in Login on u.Userid equals l.Userid
                            join r in Role on l.Roleid equals r.Roleid
                            select new JoinTable { User = u, Role = r, Login = l };


            ViewData["MultiData"] = multiData.ToList();



            var pendingTestimonials = _context.Testimonials1s
       .Include(t => t.User)
       .Include(t => t.Hotel)
       .Where(t => t.Rating == null)
       .ToList();

            ViewBag.PendingCount = pendingTestimonials.Count;


            return View(UserId);
        }



        // GET: Admin/ViewProfile
        public async Task<IActionResult> ViewProfile()
        {
            
            
            var id = HttpContext.Session.GetInt32("ADMINid");

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Userid == id);
           
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Returns a view with user profile data
        }



        // GET: Admin/EditProfile
        public async Task<IActionResult> EditProfile(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Ensure 'id' is of type 'decimal'
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(decimal id, [Bind("Userid,Userfname,Userlname,Email,ImageFile")] User user)
        {
            if (id != user.Userid)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    if (user.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await user.ImageFile.CopyToAsync(fileStream);
                        }

                        user.Imagepath = fileName;
                    }
                    else
                    {
                        _context.Entry(user).Property(x => x.Imagepath).IsModified = false;
                    }

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Userid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ViewProfile", new { id = user.Userid });
            }
            return View(user);
        }


        private bool UserExists(decimal id)
        {
            return _context.Users.Any(e => e.Userid == id);
        }



        public async Task<IActionResult> PendingTestimonials()
        {
            var pendingTestimonials = _context.Testimonials1s
                .Where(t => t.Rating == null)
                .Include(t => t.Hotel)
                .Include(t => t.User);
            return View(await pendingTestimonials.ToListAsync());
        }



        [HttpPost]
        public async Task<IActionResult> AcceptTestimonial(decimal id)
        {
            var testimonial = await _context.Testimonials1s.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }

            testimonial.Rating = true; // Set status to Approved
            _context.Update(testimonial);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(PendingTestimonials));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectTestimonial(decimal id)
        {
            // Find the testimonial by ID
            var testimonial = await _context.Testimonials1s.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound(); // Handle not found scenario
            }

            // Remove the testimonial from the context
            _context.Testimonials1s.Remove(testimonial);

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception and handle errors
                Console.WriteLine(ex.InnerException?.Message);
                ModelState.AddModelError("", "An error occurred while deleting the testimonial. Please try again.");
            }

            // Redirect to the index or any other appropriate page
            return RedirectToAction(nameof(Index));
        }






        public async Task<IActionResult> DisplayTestimonials()
        {
            var acceptedTestimonials = await _context.Testimonials1s
                .Include(t => t.Hotel)
                .Include(t => t.User)
                .Where(t => t.Rating == true) // Only accepted testimonials
                .ToListAsync();

            return View(acceptedTestimonials);
        }


        [HttpGet]
        public IActionResult Search()
        {
            // Fetch all reservations with associated User and Room data
            var modelContext = _context.Reservations.Include(u => u.User).Include(u => u.Room).ToList();

            // Calculate the number of available rooms
            ViewBag.numavarooms = modelContext.Count(r => r.Room.Isavailable == "Y");

            return View(modelContext);
        }

        [HttpPost]
        public IActionResult Search(DateTime? startDate, DateTime? endDate)
        {
            var modelContext = _context.Reservations.Include(u => u.User).Include(u => u.Room).AsQueryable();



            // If both startDate and endDate are null, return the full list
            if (!startDate.HasValue && !endDate.HasValue)
            {

                return View(modelContext.ToList());
            }

            // If only startDate is provided, filter for reservations starting on or after that date
            if (startDate.HasValue && !endDate.HasValue)
            {
                modelContext = modelContext.Where(x => x.Checkindate.Value.Date >= startDate.Value.Date);
            }

            // If only endDate is provided, filter for reservations ending on or before that date
            if (!startDate.HasValue && endDate.HasValue)
            {
                modelContext = modelContext.Where(x => x.Checkoutdate.Value.Date <= endDate.Value.Date);
            }

            // If both dates are provided, filter for reservations within that date range
            if (startDate.HasValue && endDate.HasValue)
            {
                modelContext = modelContext.Where(x => x.Checkindate.Value.Date >= startDate.Value.Date &&
                                                       x.Checkoutdate.Value.Date <= endDate.Value.Date);
            }

            // Calculate the number of available rooms after filtering
            ViewBag.numavarooms = modelContext.Count(r => r.Room.Isavailable == "Y");

            // Convert the query result to a list and return it to the view
            return View(modelContext.ToList());
        }

        




    }
}
