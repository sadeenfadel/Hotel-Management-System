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
    public class AboutusController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public AboutusController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Aboutus
        public async Task<IActionResult> Index()
        {
              return _context.Aboutus != null ? 
                          View(await _context.Aboutus.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Aboutus'  is null.");
        }

        // GET: Aboutus/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Aboutus == null)
            {
                return NotFound();
            }

            var aboutu = await _context.Aboutus
                .FirstOrDefaultAsync(m => m.Aboutusid == id);
            if (aboutu == null)
            {
                return NotFound();
            }

            return View(aboutu);
        }

        // GET: Aboutus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aboutus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Aboutusid,ImageFile,Content")] Aboutu aboutu)
        {
            if (ModelState.IsValid)
            {
                if (aboutu.ImageFile != null)
                {
                    //1.Get root path
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2.create variable to store file name must be unique

                    string fileName = Guid.NewGuid().ToString() + "_" + aboutu.ImageFile.FileName;

                    //3.create the path of image ~/Images/filename

                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    //4.upload image to folder images

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutu.ImageFile.CopyToAsync(fileStream);
                    }

                    aboutu.Imagepath = fileName;


                }



                _context.Add(aboutu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutu);
        }

        // GET: Aboutus/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Aboutus == null)
            {
                return NotFound();
            }

            var aboutu = await _context.Aboutus.FindAsync(id);
            if (aboutu == null)
            {
                return NotFound();
            }
            return View(aboutu);
        }

        // POST: Aboutus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Aboutusid,ImageFile,Content")] Aboutu aboutu)
        {
            if (id != aboutu.Aboutusid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (aboutu.ImageFile != null)
                {
                    //1.Get root path
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2.create variable to store file name must be unique

                    string fileName = Guid.NewGuid().ToString() + "_" + aboutu.ImageFile.FileName;

                    //3.create the path of image ~/Images/filename

                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    //4.upload image to folder images

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutu.ImageFile.CopyToAsync(fileStream);
                    }

                    aboutu.Imagepath = fileName;


                }






                try
                {
                    _context.Update(aboutu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutuExists(aboutu.Aboutusid))
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
            return View(aboutu);
        }

        // GET: Aboutus/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Aboutus == null)
            {
                return NotFound();
            }

            var aboutu = await _context.Aboutus
                .FirstOrDefaultAsync(m => m.Aboutusid == id);
            if (aboutu == null)
            {
                return NotFound();
            }

            return View(aboutu);
        }

        // POST: Aboutus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Aboutus == null)
            {
                return Problem("Entity set 'ModelContext.Aboutus'  is null.");
            }
            var aboutu = await _context.Aboutus.FindAsync(id);
            if (aboutu != null)
            {
                _context.Aboutus.Remove(aboutu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutuExists(decimal id)
        {
          return (_context.Aboutus?.Any(e => e.Aboutusid == id)).GetValueOrDefault();
        }
    }
}
