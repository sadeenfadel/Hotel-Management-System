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
    public class BloggingsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public BloggingsController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Bloggings
        public async Task<IActionResult> Index()
        {
              return _context.Bloggings != null ? 
                          View(await _context.Bloggings.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Bloggings'  is null.");
        }

        // GET: Bloggings/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Bloggings == null)
            {
                return NotFound();
            }

            var blogging = await _context.Bloggings
                .FirstOrDefaultAsync(m => m.Blogid == id);
            if (blogging == null)
            {
                return NotFound();
            }

            return View(blogging);
        }

        // GET: Bloggings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bloggings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Blogid,ImageFile,Heading,Datecreated,Content")] Blogging blogging)
        {
            if (ModelState.IsValid)
            {

                if (blogging.ImageFile != null)
                {
                    //1.Get root path
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2.create variable to store file name must be unique

                    string fileName = Guid.NewGuid().ToString() + "_" + blogging.ImageFile.FileName;

                    //3.create the path of image ~/Images/filename

                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    //4.upload image to folder images

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await blogging.ImageFile.CopyToAsync(fileStream);
                    }

                    blogging.Imageurl = fileName;


                }


                _context.Add(blogging);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogging);
        }

        // GET: Bloggings/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Bloggings == null)
            {
                return NotFound();
            }

            var blogging = await _context.Bloggings.FindAsync(id);
            if (blogging == null)
            {
                return NotFound();
            }
            return View(blogging);
        }

        // POST: Bloggings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Blogid,ImageFile,Heading,Datecreated,Content")] Blogging blogging)
        {
            if (id != blogging.Blogid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (blogging.ImageFile != null)
                {
                    //1.Get root path
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2.create variable to store file name must be unique

                    string fileName = Guid.NewGuid().ToString() + "_" + blogging.ImageFile.FileName;

                    //3.create the path of image ~/Images/filename

                    string path = Path.Combine(wwwRootPath + "/Images", fileName);

                    //4.upload image to folder images

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await blogging.ImageFile.CopyToAsync(fileStream);
                    }

                    blogging.Imageurl = fileName;


                }






                try
                {
                    _context.Update(blogging);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BloggingExists(blogging.Blogid))
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
            return View(blogging);
        }

        // GET: Bloggings/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Bloggings == null)
            {
                return NotFound();
            }

            var blogging = await _context.Bloggings
                .FirstOrDefaultAsync(m => m.Blogid == id);
            if (blogging == null)
            {
                return NotFound();
            }

            return View(blogging);
        }

        // POST: Bloggings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Bloggings == null)
            {
                return Problem("Entity set 'ModelContext.Bloggings'  is null.");
            }
            var blogging = await _context.Bloggings.FindAsync(id);
            if (blogging != null)
            {
                _context.Bloggings.Remove(blogging);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BloggingExists(decimal id)
        {
          return (_context.Bloggings?.Any(e => e.Blogid == id)).GetValueOrDefault();
        }
    }
}
