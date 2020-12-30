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
    public class OzelliksController : Controller
    {
        private readonly emlakoContext _context;

        public OzelliksController(emlakoContext context)
        {
            _context = context;
        }

        // GET: Ozelliks
        public async Task<IActionResult> Index()
        {
            var emlakoContext = _context.Ozellik.Include(o => o.Ev);
            return View(await emlakoContext.ToListAsync());
        }

        // GET: Ozelliks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ozellik = await _context.Ozellik
                .Include(o => o.Ev)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ozellik == null)
            {
                return NotFound();
            }

            return View(ozellik);
        }

        // GET: Ozelliks/Create
        public IActionResult Create()
        {
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi");
            return View();
        }

        // POST: Ozelliks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EvId,OzellikTipi")] Ozellik ozellik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ozellik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", ozellik.EvId);
            return View(ozellik);
        }

        // GET: Ozelliks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ozellik = await _context.Ozellik.FindAsync(id);
            if (ozellik == null)
            {
                return NotFound();
            }
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", ozellik.EvId);
            return View(ozellik);
        }

        // POST: Ozelliks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EvId,OzellikTipi")] Ozellik ozellik)
        {
            if (id != ozellik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ozellik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OzellikExists(ozellik.Id))
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
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", ozellik.EvId);
            return View(ozellik);
        }

        // GET: Ozelliks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ozellik = await _context.Ozellik
                .Include(o => o.Ev)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ozellik == null)
            {
                return NotFound();
            }

            return View(ozellik);
        }

        // POST: Ozelliks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ozellik = await _context.Ozellik.FindAsync(id);
            _context.Ozellik.Remove(ozellik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OzellikExists(int id)
        {
            return _context.Ozellik.Any(e => e.Id == id);
        }
    }
}
