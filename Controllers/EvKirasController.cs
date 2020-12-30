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
    public class EvKirasController : Controller
    {
        private readonly emlakoContext _context;

        public EvKirasController(emlakoContext context)
        {
            _context = context;
        }

        // GET: EvKiras
        public async Task<IActionResult> Index()
        {
            var emlakoContext = _context.EvKira.Include(e => e.Ev).Include(e => e.Kiraci).Include(e => e.Personel);
            return View(await emlakoContext.ToListAsync());
        }

        // GET: EvKiras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evKira = await _context.EvKira
                .Include(e => e.Ev)
                .Include(e => e.Kiraci)
                .Include(e => e.Personel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evKira == null)
            {
                return NotFound();
            }

            return View(evKira);
        }

        // GET: EvKiras/Create
        public IActionResult Create()
        {
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi");
            ViewData["KiraciId"] = new SelectList(_context.Kiraci, "Id", "Ad");
            ViewData["PersonelId"] = new SelectList(_context.Personel, "Id", "Id");
            return View();
        }

        // POST: EvKiras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PersonelId,EvId,KiraciId,KiraFiyati,Sure")] EvKira evKira)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evKira);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", evKira.EvId);
            ViewData["KiraciId"] = new SelectList(_context.Kiraci, "Id", "Ad", evKira.KiraciId);
            ViewData["PersonelId"] = new SelectList(_context.Personel, "Id", "Id", evKira.PersonelId);
            return View(evKira);
        }

        // GET: EvKiras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evKira = await _context.EvKira.FindAsync(id);
            if (evKira == null)
            {
                return NotFound();
            }
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", evKira.EvId);
            ViewData["KiraciId"] = new SelectList(_context.Kiraci, "Id", "Ad", evKira.KiraciId);
            ViewData["PersonelId"] = new SelectList(_context.Personel, "Id", "Id", evKira.PersonelId);
            return View(evKira);
        }

        // POST: EvKiras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PersonelId,EvId,KiraciId,KiraFiyati,Sure")] EvKira evKira)
        {
            if (id != evKira.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evKira);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvKiraExists(evKira.Id))
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
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", evKira.EvId);
            ViewData["KiraciId"] = new SelectList(_context.Kiraci, "Id", "Ad", evKira.KiraciId);
            ViewData["PersonelId"] = new SelectList(_context.Personel, "Id", "Id", evKira.PersonelId);
            return View(evKira);
        }

        // GET: EvKiras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evKira = await _context.EvKira
                .Include(e => e.Ev)
                .Include(e => e.Kiraci)
                .Include(e => e.Personel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evKira == null)
            {
                return NotFound();
            }

            return View(evKira);
        }

        // POST: EvKiras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evKira = await _context.EvKira.FindAsync(id);
            _context.EvKira.Remove(evKira);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvKiraExists(int id)
        {
            return _context.EvKira.Any(e => e.Id == id);
        }
    }
}
