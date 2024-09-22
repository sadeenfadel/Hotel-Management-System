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

    public class InvoicesController : Controller
    {
        private readonly ModelContext _context;
        

        public InvoicesController(ModelContext context)
        {
            _context = context;
            
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            
            var modelContext = _context.Invoices.Include(i => i.Reservation).Include(i => i.User);
            return View(await modelContext.ToListAsync());
        }

        public async Task<IActionResult> Details(decimal? id)
        {
            ViewData["footer"] = _context.Footers.ToList();
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Reservation)
                    .ThenInclude(r => r.Room) // Ensure Room is included
                .Include(i => i.User) // Ensure User is included
                .FirstOrDefaultAsync(m => m.Invoiceid == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        public async Task<IActionResult> Details2(decimal? id)
        {
            
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Reservation)
                    .ThenInclude(r => r.Room) // Ensure Room is included
                .Include(i => i.User) // Ensure User is included
                .FirstOrDefaultAsync(m => m.Invoiceid == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create(decimal? reservationId, decimal? userId)
        {
            ViewData["footer"] = _context.Footers.ToList();

            if (reservationId.HasValue && userId.HasValue)
            {
                var reservation = _context.Reservations
                    .Include(r => r.Room)
                    .FirstOrDefault(r => r.Reservationid == reservationId);

                if (reservation == null)
                {
                    return NotFound();
                }

                // Generate invoice content based on reservation details
                string invoiceContent = $"Invoice for Reservation ID: {reservation.Reservationid}\n" +
                                        $"Room: {reservation.Room.Roomtype}\n" +
                                        $"Check-in Date: {reservation.Checkindate:yyyy-MM-dd}\n" +
                                        $"Check-out Date: {reservation.Checkoutdate:yyyy-MM-dd}";

                var invoice = new Invoice
                {
                    Reservationid = reservationId,
                    Userid = userId,
                    Invoicedate = DateTime.Now,
                    Invoicecontent = invoiceContent
                };

                return View(invoice); // Ensure the view expects an Invoice model
            }

            ViewData["Reservationid"] = new SelectList(_context.Reservations, "Reservationid", "Reservationid");
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid");
            return View();
        }



        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Invoiceid,Reservationid,Userid,Invoicedate,Invoicecontent")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyInvoices"); // Redirect to the list of invoices or another relevant page
            }

            // If the model state is not valid, repopulate the select lists
            ViewData["Reservationid"] = new SelectList(_context.Reservations, "Reservationid", "Reservationid", invoice.Reservationid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", invoice.Userid);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
       /* public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["Reservationid"] = new SelectList(_context.Reservations, "Reservationid", "Reservationid", invoice.Reservationid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", invoice.Userid);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Invoiceid,Reservationid,Userid,Invoicedate,Invoicecontent")] Invoice invoice)
        {
            if (id != invoice.Invoiceid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Invoiceid))
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
            ViewData["Reservationid"] = new SelectList(_context.Reservations, "Reservationid", "Reservationid", invoice.Reservationid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", invoice.Userid);
            return View(invoice);
        }
       */
        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            ViewData["footer"] = _context.Footers.ToList();
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Reservation)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Invoiceid == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Invoices == null)
            {
                return Problem("Entity set 'ModelContext.Invoices'  is null.");
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(decimal id)
        {
            return (_context.Invoices?.Any(e => e.Invoiceid == id)).GetValueOrDefault();
        }




        /*  public async Task<IActionResult> MyInvoices()
          {
              ViewData["footer"] = _context.Footers.ToList();
              // Assuming you have a way to get the logged-in user's ID
              decimal? userId = HttpContext.Session.GetInt32("Userid");

              if (userId == null)
              {
                  return Unauthorized();
              }

              var userInvoices = await _context.Invoices
                  .Include(i => i.Reservation)
                  .ThenInclude(r => r.Room) // Assuming you want to include room details
                  .Include(i => _context.Users) // Ensure that you include the user details
                  .Where(i => i.Userid == userId)
                  .ToListAsync();

              return View(userInvoices);
          }
        */


        // GET: Invoices/MyInvoices
        public async Task<IActionResult> MyInvoices()
        {
            ViewData["footer"] = _context.Footers.ToList();

            // Get the logged-in user's ID. You may need to adjust this according to how you handle authentication
            decimal? userId = HttpContext.Session.GetInt32("Userid");
            if (userId == null)
            {
                return Unauthorized(); // Redirect to an error page or login page
            }

            var userInvoices = await _context.Invoices
                .Include(i => i.Reservation)
                .ThenInclude(r => r.Room)
                .Include(i => i.User)
                .Where(i => i.Userid == userId)
                .ToListAsync();

            return View(userInvoices);
        }





    }
}

        
    
















