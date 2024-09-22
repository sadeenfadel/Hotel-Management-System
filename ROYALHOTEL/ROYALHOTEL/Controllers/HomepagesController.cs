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
    public class HomepagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public HomepagesController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Homepages
        public async Task<IActionResult> Index()
        {
              return _context.Homepages != null ? 
                          View(await _context.Homepages.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Homepages'  is null.");
        }

        // GET: Homepages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages
                .FirstOrDefaultAsync(m => m.Homepageid == id);
            if (homepage == null)
            {
                return NotFound();
            }

            return View(homepage);
        }

        // GET: Homepages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Homepages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Homepageid,Paragraph,ImageFile,Greeting")] Homepage homepage)
        {
            if (ModelState.IsValid)
            {
                if (homepage.ImageFile != null)
                {
                    //1.Get root path
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2.create variable to store file name must be unique

                    string fileName = Guid.NewGuid().ToString() + "_" + homepage.ImageFile.FileName;

                    //3.create the path of image ~/Images/filename

                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    //4.upload image to folder images

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homepage.ImageFile.CopyToAsync(fileStream);
                    }

                    homepage.Imagepath = fileName;


                }



                _context.Add(homepage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homepage);
        }

        // GET: Homepages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages.FindAsync(id);
            if (homepage == null)
            {
                return NotFound();
            }
            return View(homepage);
        }

        // POST: Homepages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Homepageid,Paragraph,ImageFile,Greeting")] Homepage homepage)
        {
            if (id != homepage.Homepageid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (homepage.ImageFile != null)
                {
                    //1.Get root path
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2.create variable to store file name must be unique

                    string fileName = Guid.NewGuid().ToString() + "_" + homepage.ImageFile.FileName;

                    //3.create the path of image ~/Images/filename

                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    //4.upload image to folder images

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homepage.ImageFile.CopyToAsync(fileStream);
                    }

                    homepage.Imagepath = fileName;


                }










                try
                {
                    _context.Update(homepage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomepageExists(homepage.Homepageid))
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
            return View(homepage);
        }

        // GET: Homepages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages
                .FirstOrDefaultAsync(m => m.Homepageid == id);
            if (homepage == null)
            {
                return NotFound();
            }

            return View(homepage);
        }

        // POST: Homepages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Homepages == null)
            {
                return Problem("Entity set 'ModelContext.Homepages'  is null.");
            }
            var homepage = await _context.Homepages.FindAsync(id);
            if (homepage != null)
            {
                _context.Homepages.Remove(homepage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomepageExists(decimal id)
        {
          return (_context.Homepages?.Any(e => e.Homepageid == id)).GetValueOrDefault();
        }
    }
}
