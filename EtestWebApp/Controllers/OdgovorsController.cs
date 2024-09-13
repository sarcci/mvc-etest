using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EtestWebApp.Data;
using EtestWebApp.Models;

namespace EtestWebApp.Controllers
{
    public class OdgovorsController : Controller
    {
        private readonly EtestContext _context;

        public OdgovorsController(EtestContext context)
        {
            _context = context;
        }

        // GET: Odgovors
        public async Task<IActionResult> Index()
        {
            var etestContext = _context.Odgovors.Include(o => o.PidNavigation);
            return View(await etestContext.ToListAsync());
        }

        // GET: Odgovors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odgovor = await _context.Odgovors
                .Include(o => o.PidNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (odgovor == null)
            {
                return NotFound();
            }

            return View(odgovor);
        }

        // GET: Odgovors/Create
        public IActionResult Create(int? pid)
        {
            ViewData["Pid"] = pid;
            return View();
        }

        // POST: Odgovors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int pid, List<Odgovor> Odgovor)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in Odgovor)
                {
                    item.Pid = pid;
                    _context.Add(item);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Odgovor);
        }

        // GET: Odgovors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odgovor = await _context.Odgovors.FindAsync(id);
            if (odgovor == null)
            {
                return NotFound();
            }
            ViewData["Pid"] = new SelectList(_context.Prasanjes, "Id", "Id", odgovor.Pid);
            return View(odgovor);
        }

        // POST: Odgovors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tekst,Pid,True")] Odgovor odgovor)
        {
            if (id != odgovor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(odgovor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OdgovorExists(odgovor.Id))
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
            ViewData["Pid"] = new SelectList(_context.Prasanjes, "Id", "Id", odgovor.Pid);
            return View(odgovor);
        }

        // GET: Odgovors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odgovor = await _context.Odgovors
                .Include(o => o.PidNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (odgovor == null)
            {
                return NotFound();
            }

            return View(odgovor);
        }

        // POST: Odgovors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var odgovor = await _context.Odgovors.FindAsync(id);
            if (odgovor != null)
            {
                _context.Odgovors.Remove(odgovor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OdgovorExists(int id)
        {
            return _context.Odgovors.Any(e => e.Id == id);
        }
    }
}
