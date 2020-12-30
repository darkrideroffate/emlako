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
    public class EsyasController : Controller
    {
        private readonly emlakoContext _context;

        public EsyasController(emlakoContext context)
        {
            _context = context;
        }

        // GET: Esyas
        public async Task<IActionResult> Index()
        {
            var emlakoContext = _context.Esya.Include(e => e.Ev);
            return View(await emlakoContext.ToListAsync());
        }

        // GET: Esyas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var esya = await _context.Esya
                .Include(e => e.Ev)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (esya == null)
            {
                return NotFound();
            }

            return View(esya);
        }

        // GET: Esyas/Create
        public IActionResult Create()
        {
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi");
            return View();
        }

        // POST: Esyas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EvId,EsyaTipi")] Esya esya)
        {
            if (ModelState.IsValid)
            {
                _context.Add(esya);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", esya.EvId);
            return View(esya);
        }

        // GET: Esyas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var esya = await _context.Esya.FindAsync(id);
            if (esya == null)
            {
                return NotFound();
            }
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", esya.EvId);
            return View(esya);
        }

        // POST: Esyas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EvId,EsyaTipi")] Esya esya)
        {
            if (id != esya.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(esya);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EsyaExists(esya.Id))
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
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", esya.EvId);
            return View(esya);
        }

        // GET: Esyas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var esya = await _context.Esya
                .Include(e => e.Ev)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (esya == null)
            {
                return NotFound();
            }

            return View(esya);
        }

        // POST: Esyas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var esya = await _context.Esya.FindAsync(id);
            _context.Esya.Remove(esya);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EsyaExists(int id)
        {
            return _context.Esya.Any(e => e.Id == id);
        }
    }
}
