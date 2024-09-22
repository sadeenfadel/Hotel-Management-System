using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ROYALHOTEL.Models;

namespace ROYALHOTEL.Controllers
{
    public class FootersController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public FootersController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Footers
        public async Task<IActionResult> Index()
        {
              return _context.Footers != null ? 
                          View(await _context.Footers.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Footers'  is null.");
        }

        // GET: Footers/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Footers == null)
            {
                return NotFound();
            }

            var footer = await _context.Footers
                .FirstOrDefaultAsync(m => m.Footerid == id);
            if (footer == null)
            {
                return NotFound();
            }

            return View(footer);
        }

        // GET: Footers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Footers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Footerid,About,Links,Newsletter,ImageFile")] Footer footer)
        {
            if (ModelState.IsValid)
            {
                if (footer.ImageFile != null)
                {
                    //1.Get root path
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2.create variable to store file name must be unique

                    string fileName = Guid.NewGuid().ToString() + "_" + footer.ImageFile.FileName;

                    //3.create the path of image ~/Images/filename

                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    //4.upload image to folder images

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await footer.ImageFile.CopyToAsync(fileStream);
                    }

                    footer.Imagepath = fileName;


                }






                _context.Add(footer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(footer);
        }

        // GET: Footers/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Footers == null)
            {
                return NotFound();
            }

            var footer = await _context.Footers.FindAsync(id);
            if (footer == null)
            {
                return NotFound();
            }
            return View(footer);
        }

        // POST: Footers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Footerid,About,Links,Newsletter,ImageFile")] Footer footer)
        {
            if (id != footer.Footerid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (footer.ImageFile != null)
                {
                    //1.Get root path
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2.create variable to store file name must be unique

                    string fileName = Guid.NewGuid().ToString() + "_" + footer.ImageFile.FileName;

                    //3.create the path of image ~/Images/filename

                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    //4.upload image to folder images

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await footer.ImageFile.CopyToAsync(fileStream);
                    }

                    footer.Imagepath = fileName;


                }









                try
                {
                    _context.Update(footer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FooterExists(footer.Footerid))
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
            return View(footer);
        }

        // GET: Footers/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Footers == null)
            {
                return NotFound();
            }

            var footer = await _context.Footers
                .FirstOrDefaultAsync(m => m.Footerid == id);
            if (footer == null)
            {
                return NotFound();
            }

            return View(footer);
        }

        // POST: Footers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Footers == null)
            {
                return Problem("Entity set 'ModelContext.Footers'  is null.");
            }
            var footer = await _context.Footers.FindAsync(id);
            if (footer != null)
            {
                _context.Footers.Remove(footer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FooterExists(decimal id)
        {
          return (_context.Footers?.Any(e => e.Footerid == id)).GetValueOrDefault();
        }
    }
}
