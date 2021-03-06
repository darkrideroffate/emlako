﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Emlakkko;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Emlakkko.Controllers
{
    public class FotografsController : Controller
    {
        private readonly emlakoContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        

        public FotografsController(emlakoContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }
        // POST: Fotografs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fotograf fotograf)
        {

            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(fotograf);
                Fotograf fotograf1 = new Fotograf
                {
                    EvId = fotograf.EvId,
                    File = uniqueFileName,
                };
                _context.Add(fotograf1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", fotograf.EvId);
            return View(fotograf);
            
        }

        // GET: Fotografs
        public async Task<IActionResult> Index()
        {
            var emlakoContext = _context.Fotograf.Include(f => f.Ev);
            return View(await emlakoContext.ToListAsync());
        }

        // GET: Fotografs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fotograf = await _context.Fotograf
                .Include(f => f.Ev)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fotograf == null)
            {
                return NotFound();
            }

            return View(fotograf);
        }

        // GET: Fotografs/Create
        public IActionResult Create()
        {
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi");
            return View();
        }

        

        // GET: Fotografs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fotograf = await _context.Fotograf.FindAsync(id);
            if (fotograf == null)
            {
                return NotFound();
            }
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", fotograf.EvId);
            return View(fotograf);
        }

        // POST: Fotografs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EvId,File")] Fotograf fotograf)
        {
            if (id != fotograf.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fotograf);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FotografExists(fotograf.Id))
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
            ViewData["EvId"] = new SelectList(_context.Ev, "Id", "EvTipi", fotograf.EvId);
            return View(fotograf);
        }

        // GET: Fotografs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fotograf = await _context.Fotograf
                .Include(f => f.Ev)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fotograf == null)
            {
                return NotFound();
            }

            return View(fotograf);
        }

        // POST: Fotografs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fotograf = await _context.Fotograf.FindAsync(id);
            _context.Fotograf.Remove(fotograf);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FotografExists(int id)
        {
            return _context.Fotograf.Any(e => e.Id == id);
        }
        private string UploadedFile(Fotograf model)
        {
            string uniqueFileName = null;

            if (model.tempPath != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.tempPath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.tempPath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        
    }
 
}
