using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ROYALHOTEL.Models;

namespace ROYALHOTEL.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ModelContext _context;

        public PaymentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Payments.Include(p => p.Reservation);
            return View(await modelContext.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Reservation)
                .FirstOrDefaultAsync(m => m.Paymentid == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

   
     
        // GET: Payments/Create
        public IActionResult Create(decimal reservationId)
        {
            ViewData["footer"] = _context.Footers.ToList();
            ViewData["Reservationid"] = new SelectList(_context.Reservations, "Reservationid", "Reservationid");

            // Create a new Payment instance and set the Reservationid
            var payment = new Payment
            {
                Reservationid = reservationId // Automatically set the reservation ID
            };

            // Pass the payment model to the view
            return View(payment);
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Step 1: Validate Credit Card Information
        private bool ValidateCreditCard(string cardNumber, string expiryDate)
        {
            // Basic check to ensure the card number is 16 digits
            if (cardNumber.Length != 16 || !cardNumber.All(char.IsDigit))
                return false;

            // Parse expiry date (expected format: MM/YY)
            if (DateTime.TryParseExact(expiryDate, "MM/yy", null, DateTimeStyles.None, out DateTime expDate))
            {
                // Check if the card is expired
                if (expDate < DateTime.Now)
                    return false;
            }
            else
            {
                return false;
            }

            // Additional checks like Luhn's algorithm can be added here

            return true;
        }

        // Step 2: Simulate Card Balance (in real scenarios, this would be fetched from a payment gateway)
        private decimal GetCardBalance(string cardNumber)
        {
            // For simplicity, let's assume all cards have a balance of 1000.00
            return 1000.00m;
        }

        // Step 3: Check if the Payment Can Be Processed
        private bool CanProcessPayment(string cardNumber, decimal amountToPay)
        {
            // Get the card's balance
            var balance = GetCardBalance(cardNumber);

            // Check if the balance is sufficient for the payment
            return balance >= amountToPay;
        }

        // Step 4: Implement Payment Validation in the Controller Action Method
        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Paymentid,Reservationid,Paymentmethod,Cardholdername,Creditcardnumber,Expirydate,Paymentdate,Amount")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                // Validate credit card information
                if (!ValidateCreditCard(payment.Creditcardnumber, payment.Expirydate))
                {
                    ViewData["footer"] = _context.Footers.ToList();
                    ModelState.AddModelError("", "Invalid credit card details.");
                    return View(payment);
                }

                // Check if the user can pay the required amount
                if (!CanProcessPayment(payment.Creditcardnumber, payment.Amount))
                {
                    ViewData["footer"] = _context.Footers.ToList();
                    ModelState.AddModelError("", "Insufficient funds to complete the transaction.");
                    return View(payment);
                }

                // Save the payment
                _context.Add(payment);
                await _context.SaveChangesAsync();

                // Retrieve the userId from the reservation
                var reservation = await _context.Reservations
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.Reservationid == payment.Reservationid);

                if (reservation != null && reservation.User != null)
                {
                    decimal userId = reservation.User.Userid;

                    // Redirect to the Invoice Create action with the reservationId and userId
                    return RedirectToAction("Create", "Invoices", new { reservationId = payment.Reservationid, userId = userId });
                }

                // If user or reservation is not found, handle the error
                ModelState.AddModelError("", "Unable to find the associated reservation or user.");
            }

            ViewData["Reservationid"] = new SelectList(_context.Reservations, "Reservationid", "Reservationid", payment.Reservationid);
            return View(payment);
        }


        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["Reservationid"] = new SelectList(_context.Reservations, "Reservationid", "Reservationid", payment.Reservationid);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Paymentid,Reservationid,Paymentmethod,Cardholdername,Creditcardnumber,Expirydate,Paymentdate,Amount")] Payment payment)
        {
            if (id != payment.Paymentid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Paymentid))
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
            ViewData["Reservationid"] = new SelectList(_context.Reservations, "Reservationid", "Reservationid", payment.Reservationid);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Reservation)
                .FirstOrDefaultAsync(m => m.Paymentid == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Payments == null)
            {
                return Problem("Entity set 'ModelContext.Payments'  is null.");
            }
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(decimal id)
        {
          return (_context.Payments?.Any(e => e.Paymentid == id)).GetValueOrDefault();
        }
    }
}
