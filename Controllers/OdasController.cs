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
    public class OdasController : Controller
    {
        private readonly emlakoContext _context;

        public OdasController(emlakoContext context)
        {
            _context = context;
        }

        // GET: Odas
        public async Task<IActionResult> Index()
        {
            var emlakoContext = _context.Oda.Include(o => o.Ev);
            return View(await emlakoContext.ToListAsync());
        }

        // GET: Odas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oda = await _context.Oda
                .Include(o => o.Ev)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oda == null)
            {
                return NotFound();
            }

            return View(oda);
        }

        // GET: Odas/Create
        public IActionResult Create()
        {
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi");
            return View();
        }

        // POST: Odas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EvId,OdaTipi")] Oda oda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(oda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", oda.EvId);
            return View(oda);
        }

        // GET: Odas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oda = await _context.Oda.FindAsync(id);
            if (oda == null)
            {
                return NotFound();
            }
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", oda.EvId);
            return View(oda);
        }

        // POST: Odas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EvId,OdaTipi")] Oda oda)
        {
            if (id != oda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OdaExists(oda.Id))
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
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", oda.EvId);
            return View(oda);
        }

        // GET: Odas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oda = await _context.Oda
                .Include(o => o.Ev)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oda == null)
            {
                return NotFound();
            }

            return View(oda);
        }

        // POST: Odas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oda = await _context.Oda.FindAsync(id);
            _context.Oda.Remove(oda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OdaExists(int id)
        {
            return _context.Oda.Any(e => e.Id == id);
        }
    }
}
