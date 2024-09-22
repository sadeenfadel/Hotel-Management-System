using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ROYALHOTEL.Models;

namespace ROYALHOTEL.Controllers
{
    public class Testimonials1Controller : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Testimonials1Controller(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Testimonials1
        public async Task<IActionResult> Index()
            
        {
            ViewData["footer"] = _context.Footers.ToList();
            var modelContext = _context.Testimonials1s.Include(t => t.Hotel).Include(t => t.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Testimonials1/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            ViewData["footer"] = _context.Footers.ToList();
            if (id == null || _context.Testimonials1s == null)
            {
                return NotFound();
            }

            var testimonials1 = await _context.Testimonials1s
                .Include(t => t.Hotel)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonials1 == null)
            {
                return NotFound();
            }

            return View(testimonials1);
        }

        // GET: Testimonials1/Create
        public IActionResult Create()
        {
            ViewData["footer"] = _context.Footers.ToList();
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelname");
            
            var userId = HttpContext.Session.GetInt32("Userid");
            if (userId == null)
            {
                // Handle the case where the user is not logged in (redirect, error message, etc.)
                return RedirectToAction("Login", "LoginandRegister");
            }

            var user = _context.Users.FirstOrDefault(u => u.Userid == userId);
            if (user != null)
            {
                ViewBag.UserName = $"{user.Userfname} {user.Userlname}";
            }

            return View();
        }

        // POST: Testimonials1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Hotelid,ImageFile,Testimonialtext,Datecreated")] Testimonials1 testimonials1)
        {
            if (!ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("Userid");
                if (userId == null)
                {
                    return Unauthorized(); // Handle unauthenticated user
                }

                testimonials1.Userid = (decimal)userId;
                testimonials1.Rating = null; // Set status to Pending

                try
                {
                    if (testimonials1.ImageFile != null)
                    {
                        //1.Get root path
                        string wwwRootPath = _webHostEnvironment.WebRootPath;

                        //2.create variable to store file name must be unique

                        string fileName = Guid.NewGuid().ToString() + "_" + testimonials1.ImageFile.FileName;

                        //3.create the path of image ~/Images/filename

                        string path = Path.Combine(wwwRootPath + "/Images", fileName);

                        //4.upload image to folder images

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await testimonials1.ImageFile.CopyToAsync(fileStream);
                        }

                        testimonials1.Imagepath = fileName;


                    }
                    _context.Add(testimonials1);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "User");
                }
                catch (DbUpdateException ex)
                {
                    // Log the exception and handle errors
                    Console.WriteLine(ex.InnerException?.Message);
                    ModelState.AddModelError("", "An error occurred while saving the testimonial. Please try again.");
                }
            }

            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelname", testimonials1.Hotelid);
            return View(testimonials1);
        }


        // GET: Testimonials1/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewData["footer"] = _context.Footers.ToList();
            if (id == null || _context.Testimonials1s == null)
            {
                return NotFound();
            }

            var testimonials1 = await _context.Testimonials1s.FindAsync(id);
            if (testimonials1 == null)
            {
                return NotFound();
            }
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelname", testimonials1.Hotelid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userfname", testimonials1.Userid);
            return View(testimonials1);
        }

        // POST: Testimonials1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Testimonialid,Userid,Hotelid,Imagepath,Testimonialtext,Rating,Datecreated")] Testimonials1 testimonials1)
        {
            if (id != testimonials1.Testimonialid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonials1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Testimonials1Exists(testimonials1.Testimonialid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelname", testimonials1.Hotelid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userfname", testimonials1.Userid);
            return View(testimonials1);
        }

        // GET: Testimonials1/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            ViewData["footer"] = _context.Footers.ToList();
            if (id == null || _context.Testimonials1s == null)
            {
                return NotFound();
            }

            var testimonials1 = await _context.Testimonials1s
                .Include(t => t.Hotel)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonials1 == null)
            {
                return NotFound();
            }

            return View(testimonials1);
        }

        // POST: Testimonials1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonials1s == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials1s'  is null.");
            }
            var testimonials1 = await _context.Testimonials1s.FindAsync(id);
            if (testimonials1 != null)
            {
                _context.Testimonials1s.Remove(testimonials1);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Testimonials1Exists(decimal id)
        {
          return (_context.Testimonials1s?.Any(e => e.Testimonialid == id)).GetValueOrDefault();
        }
    }
}
