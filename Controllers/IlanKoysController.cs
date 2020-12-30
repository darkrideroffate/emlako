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
    public class IlanKoysController : Controller
    {
        private readonly emlakoContext _context;

        public IlanKoysController(emlakoContext context)
        {
            _context = context;
        }

        // GET: IlanKoys
        public async Task<IActionResult> Index()
        {
            var emlakoContext = _context.IlanKoy.Include(i => i.Ev).Include(i => i.Ofis);
            return View(await emlakoContext.ToListAsync());
        }

        // GET: IlanKoys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ilanKoy = await _context.IlanKoy
                .Include(i => i.Ev)
                .Include(i => i.Ofis)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ilanKoy == null)
            {
                return NotFound();
            }

            return View(ilanKoy);
        }

        // GET: IlanKoys/Create
        public IActionResult Create()
        {
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi");
            ViewData["OfisId"] = new SelectList(_context.Ofis, "Id", "Name");
            return View();
        }

        // POST: IlanKoys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EvId,OfisId")] IlanKoy ilanKoy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ilanKoy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", ilanKoy.EvId);
            ViewData["OfisId"] = new SelectList(_context.Ofis, "Id", "Name", ilanKoy.OfisId);
            return View(ilanKoy);
        }

        // GET: IlanKoys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ilanKoy = await _context.IlanKoy.FindAsync(id);
            if (ilanKoy == null)
            {
                return NotFound();
            }
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", ilanKoy.EvId);
            ViewData["OfisId"] = new SelectList(_context.Ofis, "Id", "Name", ilanKoy.OfisId);
            return View(ilanKoy);
        }

        // POST: IlanKoys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EvId,OfisId")] IlanKoy ilanKoy)
        {
            if (id != ilanKoy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ilanKoy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IlanKoyExists(ilanKoy.Id))
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
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", ilanKoy.EvId);
            ViewData["OfisId"] = new SelectList(_context.Ofis, "Id", "Name", ilanKoy.OfisId);
            return View(ilanKoy);
        }

        // GET: IlanKoys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ilanKoy = await _context.IlanKoy
                .Include(i => i.Ev)
                .Include(i => i.Ofis)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ilanKoy == null)
            {
                return NotFound();
            }

            return View(ilanKoy);
        }

        // POST: IlanKoys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ilanKoy = await _context.IlanKoy.FindAsync(id);
            _context.IlanKoy.Remove(ilanKoy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IlanKoyExists(int id)
        {
            return _context.IlanKoy.Any(e => e.Id == id);
        }
    }
}
