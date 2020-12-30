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
    public class EvSahibisController : Controller
    {
        private readonly emlakoContext _context;

        public EvSahibisController(emlakoContext context)
        {
            _context = context;
        }

        // GET: EvSahibis
        public async Task<IActionResult> Index()
        {
            return View(await _context.EvSahibi.ToListAsync());
        }

        // GET: EvSahibis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evSahibi = await _context.EvSahibi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evSahibi == null)
            {
                return NotFound();
            }

            return View(evSahibi);
        }

        // GET: EvSahibis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EvSahibis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ad,Soyad,Telefon")] EvSahibi evSahibi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evSahibi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(evSahibi);
        }

        // GET: EvSahibis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evSahibi = await _context.EvSahibi.FindAsync(id);
            if (evSahibi == null)
            {
                return NotFound();
            }
            return View(evSahibi);
        }

        // POST: EvSahibis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Soyad,Telefon")] EvSahibi evSahibi)
        {
            if (id != evSahibi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evSahibi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvSahibiExists(evSahibi.Id))
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
            return View(evSahibi);
        }

        // GET: EvSahibis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evSahibi = await _context.EvSahibi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evSahibi == null)
            {
                return NotFound();
            }

            return View(evSahibi);
        }

        // POST: EvSahibis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evSahibi = await _context.EvSahibi.FindAsync(id);
            _context.EvSahibi.Remove(evSahibi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvSahibiExists(int id)
        {
            return _context.EvSahibi.Any(e => e.Id == id);
        }
    }
}
