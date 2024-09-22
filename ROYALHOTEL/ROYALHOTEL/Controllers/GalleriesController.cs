using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ROYALHOTEL.Models;

namespace ROYALHOTEL.Controllers
{
    public class GalleriesController : Controller
    {
        private readonly ModelContext _context2;
        private readonly IWebHostEnvironment _webHostEnviroment;

   
        public GalleriesController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context2 = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Galleries
        public async Task<IActionResult> Index()
        {
              return _context2.Galleries != null ? 
                          View(await _context2.Galleries.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Galleries'  is null.");
        }

        // GET: Galleries/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context2.Galleries == null)
            {
                return NotFound();
            }

            var gallery = await _context2.Galleries
                .FirstOrDefaultAsync(m => m.Galleryid == id);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }

        // GET: Galleries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Galleries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Galleryid,ImageFile")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {

                if (gallery.ImageFile != null)
                {
                    //1.Get root path
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2.create variable to store file name must be unique

                    string fileName = Guid.NewGuid().ToString() + "_" + gallery.ImageFile.FileName;

                    //3.create the path of image ~/Images/filename

                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    //4.upload image to folder images

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await gallery.ImageFile.CopyToAsync(fileStream);
                    }

                    gallery.Imagepath = fileName;


                }










                _context2.Add(gallery);
                await _context2.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gallery);
        }

        // GET: Galleries/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context2.Galleries == null)
            {
                return NotFound();
            }

            var gallery = await _context2.Galleries.FindAsync(id);
            if (gallery == null)
            {
                return NotFound();
            }
            return View(gallery);
        }

        // POST: Galleries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Galleryid,ImageFile")] Gallery gallery)
        {
            if (id != gallery.Galleryid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (gallery.ImageFile != null)
                {
                    //1.Get root path
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2.create variable to store file name must be unique

                    string fileName = Guid.NewGuid().ToString() + "_" + gallery.ImageFile.FileName;

                    //3.create the path of image ~/Images/filename

                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    //4.upload image to folder images

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await gallery.ImageFile.CopyToAsync(fileStream);
                    }

                    gallery.Imagepath = fileName;


                }









                try
                {
                    _context2.Update(gallery);
                    await _context2.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleryExists(gallery.Galleryid))
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
            return View(gallery);
        }

        // GET: Galleries/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context2.Galleries == null)
            {
                return NotFound();
            }

            var gallery = await _context2.Galleries
                .FirstOrDefaultAsync(m => m.Galleryid == id);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }

        // POST: Galleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context2.Galleries == null)
            {
                return Problem("Entity set 'ModelContext.Galleries'  is null.");
            }
            var gallery = await _context2.Galleries.FindAsync(id);
            if (gallery != null)
            {
                _context2.Galleries.Remove(gallery);
            }
            
            await _context2.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalleryExists(decimal id)
        {
          return (_context2.Galleries?.Any(e => e.Galleryid == id)).GetValueOrDefault();
        }
    }
}
