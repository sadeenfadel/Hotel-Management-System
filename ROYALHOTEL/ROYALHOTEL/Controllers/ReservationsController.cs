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
    public class ReservationsController : Controller
    {
        private readonly ModelContext _context;

        public ReservationsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var rooms = _context.Rooms.ToList();
            foreach (var room in rooms)
            {
                var lastReservation = _context.Reservations
                    .Where(r => r.Roomid == room.Roomid)
                    .OrderByDescending(r => r.Checkoutdate)
                    .FirstOrDefault();

                if (lastReservation != null && lastReservation.Checkoutdate < DateTime.Now)
                {
                    room.Isavailable = "Y";
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
            }
            var modelContext = _context.Reservations.Include(r => r.Room).Include(r => r.User);
            return View(await modelContext.ToListAsync());

        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Reservationid == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create

        public IActionResult Book(decimal id)
        {
            ViewData["footer"] = _context.Footers.ToList();

            // Fetch the logged-in user's ID from the session
            var userId = HttpContext.Session.GetInt32("Userid");

            if (userId == null)
            {
                return RedirectToAction("UserLogin", "LoginandRegister"); // Redirect to login if the user is not logged in
            }

            var user = _context.Users.FirstOrDefault(u => u.Userid == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Fetch the room based on the ID
            var room = _context.Rooms.FirstOrDefault(r => r.Roomid == id);

            if (room == null)
            {
                return NotFound("Room not found.");
            }

            // Pass user and room information to the view
            ViewData["UserFname"] = user.Userfname;
            ViewData["UserLname"] = user.Userlname;
            ViewData["Roomtype"] = room.Roomtype;
            ViewData["Roomid"] = room.Roomid;

            return View();
        }


        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(decimal id, [Bind("Checkindate,Checkoutdate")] Reservation reservation)
        {
            // Fetch the logged-in user's ID from the session
            var userId = HttpContext.Session.GetInt32("Userid");

            if (userId == null)
            {
                return RedirectToAction("UserLogin", "LoginandRegister"); // Redirect to login if the user is not logged in
            }

            // Fetch the user and room details
            var user = _context.Users.FirstOrDefault(u => u.Userid == userId);
            var room = _context.Rooms.FirstOrDefault(r => r.Roomid == id);

            if (user == null || room == null)
            {
                return NotFound("User or Room not found.");
            }

            // Check if the room is available for the selected dates
            var conflictingReservation = _context.Reservations
                .Where(r => r.Roomid == id &&
                            r.Checkindate < reservation.Checkoutdate &&
                            r.Checkoutdate > reservation.Checkindate)
                .FirstOrDefault();

            if (conflictingReservation != null)
            {
                ModelState.AddModelError("", "Room is already reserved for the selected dates.");

                // Re-populate ViewData with user and room details if reservation fails
                ViewData["UserFname"] = user.Userfname;
                ViewData["UserLname"] = user.Userlname;
                ViewData["Roomtype"] = room.Roomtype;
                ViewData["Roomid"] = room.Roomid;

                return View(reservation);
            }

            // Save the reservation details
            reservation.Userid = user.Userid;
            reservation.Roomid = room.Roomid;
            reservation.Reservationdate = DateTime.Now;

            // Add the reservation to the context
            _context.Add(reservation);

            // Mark the room as unavailable
            room.Isavailable = "N";
            _context.Update(room);

            await _context.SaveChangesAsync();

            return RedirectToAction("Create", "Payments", new { reservationId = reservation.Reservationid });
        }


        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomtype", reservation.Roomid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userfname", reservation.Userid);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Reservationid,Userid,Roomid,Reservationdate,Checkindate,Checkoutdate")] Reservation reservation)
        {
            if (id != reservation.Reservationid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Reservationid))
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
            ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomid", reservation.Roomid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", reservation.Userid);
            return View(reservation);
        }


        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Reservationid == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'ModelContext.Reservations'  is null.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(decimal id)
        {
            return (_context.Reservations?.Any(e => e.Reservationid == id)).GetValueOrDefault();
        }
    }
}







