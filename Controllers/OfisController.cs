using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Emlakkko;

namespace Emlakkko.Controllers
{
    public class OfisController : Controller
    {
        private readonly emlakoContext _context;

        public OfisController(emlakoContext context)
        {
            _context = context;
        }

        // GET: Ofis
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ofis.ToListAsync());
        }

        // GET: Ofis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofis = await _context.Ofis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ofis == null)
            {
                return NotFound();
            }

            return View(ofis);
        }

        // GET: Ofis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ofis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Ofis ofis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ofis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ofis);
        }

        // GET: Ofis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofis = await _context.Ofis.FindAsync(id);
            if (ofis == null)
            {
                return NotFound();
            }
            return View(ofis);
        }

        // POST: Ofis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Ofis ofis)
        {
            if (id != ofis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ofis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfisExists(ofis.Id))
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
            return View(ofis);
        }

        // GET: Ofis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofis = await _context.Ofis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ofis == null)
            {
                return NotFound();
            }

            return View(ofis);
        }

        // POST: Ofis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ofis = await _context.Ofis.FindAsync(id);
            _context.Ofis.Remove(ofis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfisExists(int id)
        {
            return _context.Ofis.Any(e => e.Id == id);
        }
    }
}
