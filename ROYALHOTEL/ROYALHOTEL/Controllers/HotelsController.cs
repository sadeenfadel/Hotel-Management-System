﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ROYALHOTEL.Models;

namespace ROYALHOTEL.Controllers
{
    public class HotelsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public HotelsController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Hotels
        public async Task<IActionResult> Index()
        {
           

            return _context.Hotels != null ?
                       View(await _context.Hotels.ToListAsync()) :
                       Problem("Entity set 'ModelContext.Hotels'  is null.");
        }

        // GET: Hotels/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Hotelid == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Hotelid,Hotelname,ImageFile,Location,Description")] Hotel hotel)
        {
            

            if (!ModelState.IsValid)
            {
                if (hotel.ImageFile != null)
                {
                    // Handle image upload
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + hotel.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await hotel.ImageFile.CopyToAsync(fileStream);
                    }

                    hotel.Imagepath = fileName;
                }
                else
                {
                    // Set a default image if no image file is provided
                    hotel.Imagepath = "H1.jpg"; // Ensure this default image exists in the Images folder
                }

                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Hotelid,Hotelname,ImageFile,Location,Description")] Hotel hotel)
        {
            if (id != hotel.Hotelid)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    if (hotel.ImageFile != null)
                    {
                        // Handle image upload
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + hotel.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await hotel.ImageFile.CopyToAsync(fileStream);
                        }

                        hotel.Imagepath = fileName;
                    }
                    else
                    {
                        // Retain the existing image path if no new image is uploaded
                        _context.Entry(hotel).Property(x => x.Imagepath).IsModified = false;
                    }

                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.Hotelid))
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
            return View(hotel);
        }


        // GET: Hotels/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Hotelid == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Hotels == null)
            {
                return Problem("Entity set 'ModelContext.Hotels'  is null.");
            }
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Make sure this method is only defined once
        private bool HotelExists(decimal id)
        {
            return _context.Hotels.Any(e => e.Hotelid == id);
        }
    }
}
