using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ROYALHOTEL.Models;
using Newtonsoft.Json;


namespace ROYALHOTEL.Controllers
{
    public class ReportingsController : Controller
    {
        private readonly ModelContext _context;

        public ReportingsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Reportings
        public async Task<IActionResult> Index()
        {
            // Retrieve profits by year
            var yearlyProfits = await _context.Reportings
                .GroupBy(r => r.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    Profit = g.Sum(r => r.Revenue - r.Totalexpenses)
                })
                .ToListAsync();

            // Prepare data for the chart
            var chartData = new
            {
                Labels = yearlyProfits.Select(p => p.Year.ToString()).ToArray(),
                Data = yearlyProfits.Select(p => p.Profit).ToArray()
            };

            ViewData["ChartData"] = JsonConvert.SerializeObject(chartData);

            // Other existing code
            int totalBookedRooms = _context.Rooms.Count(x => x.Isavailable == "N");
            decimal totalRevenue = _context.Rooms
                .Where(x => x.Isavailable == "N")
                .Sum(x => x.Price);
            decimal totalExpenses = _context.Reportings.Sum(r => r.Totalexpenses) ?? 0;
            decimal profitOrLoss = totalRevenue - totalExpenses;
            var groupedRevenue = _context.Rooms
                .Where(x => x.Isavailable == "N")
                .GroupBy(x => x.Roomtype)
                .Select(g => new
                {
                    RoomType = g.Key,
                    Revenue = g.Sum(x => x.Price)
                })
                .ToList();

            ViewData["BOOKED"] = totalBookedRooms;
            ViewData["REVENUE"] = totalRevenue;
            ViewData["PROFIT_OR_LOSS"] = profitOrLoss;
            ViewData["GROUPED_REVENUE"] = groupedRevenue;

            var reportings = await _context.Reportings.ToListAsync();
            return reportings != null ? View(reportings) : Problem("Entity set 'ModelContext.Reportings' is null.");
        }


        // GET: Reportings/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Reportings == null)
            {
                return NotFound();
            }

            var reporting = await _context.Reportings
                .FirstOrDefaultAsync(m => m.Reportid == id);
            if (reporting == null)
            {
                return NotFound();
            }

            return View(reporting);
        }

        // GET: Reportings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reportings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Reportid,Year,Totalroomsbooked,Pricepernight,Totalexpenses,Revenue,Netprofit,Profitorloss")] Reporting reporting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reporting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reporting);
        }

        // GET: Reportings/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Reportings == null)
            {
                return NotFound();
            }

            var reporting = await _context.Reportings.FindAsync(id);
            if (reporting == null)
            {
                return NotFound();
            }
            return View(reporting);
        }

        // POST: Reportings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Reportid,Year,Totalroomsbooked,Pricepernight,Totalexpenses,Revenue,Netprofit,Profitorloss")] Reporting reporting)
        {
            if (id != reporting.Reportid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reporting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportingExists(reporting.Reportid))
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
            return View(reporting);
        }

        // GET: Reportings/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Reportings == null)
            {
                return NotFound();
            }

            var reporting = await _context.Reportings
                .FirstOrDefaultAsync(m => m.Reportid == id);
            if (reporting == null)
            {
                return NotFound();
            }

            return View(reporting);
        }

        // POST: Reportings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Reportings == null)
            {
                return Problem("Entity set 'ModelContext.Reportings'  is null.");
            }
            var reporting = await _context.Reportings.FindAsync(id);
            if (reporting != null)
            {
                _context.Reportings.Remove(reporting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportingExists(decimal id)
        {
            return (_context.Reportings?.Any(e => e.Reportid == id)).GetValueOrDefault();
        }
    }
}