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
    public class EvsController : Controller
    {
        private readonly emlakoContext _context;

        public EvsController(emlakoContext context)
        {
            _context = context;
        }

        // GET: Evs
        public async Task<IActionResult> Index()
        {
            var emlakoContext = _context.Ev.Include(e => e.Adres).Include(e => e.EvSahibi);
            return View(await emlakoContext.ToListAsync());
        }

        // GET: Evs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ev = await _context.Ev
                .Include(e => e.Adres)
                .Include(e => e.EvSahibi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ev == null)
            {
                return NotFound();
            }

            return View(ev);
        }

        // GET: Evs/Create
        public IActionResult Create()
        {
            ViewData["AdresId"] = new SelectList(_context.Adres, "Id", "Il");
            ViewData["EvSahibiId"] = new SelectList(_context.EvSahibi, "Id", "Ad");
            return View();
        }

        // POST: Evs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EvSahibiId,Name,EvTipi,KiraFiyati,AdresId")] Ev ev)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ev);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdresId"] = new SelectList(_context.Adres, "Id", "Il", ev.AdresId);
            ViewData["EvSahibiId"] = new SelectList(_context.EvSahibi, "Id", "Ad", ev.EvSahibiId);
            return View(ev);
        }

        // GET: Evs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ev = await _context.Ev.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }
            ViewData["AdresId"] = new SelectList(_context.Adres, "Id", "Il", ev.AdresId);
            ViewData["EvSahibiId"] = new SelectList(_context.EvSahibi, "Id", "Ad", ev.EvSahibiId);
            return View(ev);
        }

        // POST: Evs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EvSahibiId,Name,EvTipi,KiraFiyati,AdresId")] Ev ev)
        {
            if (id != ev.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ev);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvExists(ev.Id))
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
            ViewData["AdresId"] = new SelectList(_context.Adres, "Id", "Il", ev.AdresId);
            ViewData["EvSahibiId"] = new SelectList(_context.EvSahibi, "Id", "Ad", ev.EvSahibiId);
            return View(ev);
        }

        // GET: Evs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ev = await _context.Ev
                .Include(e => e.Adres)
                .Include(e => e.EvSahibi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ev == null)
            {
                return NotFound();
            }

            return View(ev);
        }

        // POST: Evs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ev = await _context.Ev.FindAsync(id);
            _context.Ev.Remove(ev);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvExists(int id)
        {
            return _context.Ev.Any(e => e.Id == id);
        }
    }
}
