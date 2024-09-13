using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EtestWebApp.Data;
using EtestWebApp.Models;
using System.Security.Cryptography;

namespace EtestWebApp.Controllers
{
    public class KtsController : Controller
    {
        private readonly EtestContext _context;

        public KtsController(EtestContext context)
        {
            _context = context;
        }

        // GET: Kts
        public async Task<IActionResult> Index()
        {
            var etestContext = _context.Kts.Include(k => k.KidNavigation).Include(k => k.TidNavigation);
            return View(await etestContext.ToListAsync());
        }

        // GET: Kts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kt = await _context.Kts
                .Include(k => k.KidNavigation)
                .Include(k => k.TidNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kt == null)
            {
                return NotFound();
            }

            return View(kt);
        }

        // GET: Kts/Create
        public IActionResult Create()
        {
            ViewData["Kid"] = new SelectList(_context.Korisniks, "Id", "Id");
            ViewData["Tid"] = new SelectList(_context.Tests, "Id", "Id");
            return View();
        }

        // POST: Kts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kid,Tid,Datum,Score")] Kt kt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Kid"] = new SelectList(_context.Korisniks, "Id", "Id", kt.Kid);
            ViewData["Tid"] = new SelectList(_context.Tests, "Id", "Id", kt.Tid);
            return View(kt);
        }

        // GET: Kts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kt = await _context.Kts.FindAsync(id);
            if (kt == null)
            {
                return NotFound();
            }
            ViewData["Kid"] = new SelectList(_context.Korisniks, "Id", "Id", kt.Kid);
            ViewData["Tid"] = new SelectList(_context.Tests, "Id", "Id", kt.Tid);
            return View(kt);
        }

        // POST: Kts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kid,Tid,Datum,Score")] Kt kt)
        {
            if (id != kt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KtExists(kt.Id))
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
            ViewData["Kid"] = new SelectList(_context.Korisniks, "Id", "Id", kt.Kid);
            ViewData["Tid"] = new SelectList(_context.Tests, "Id", "Id", kt.Tid);
            return View(kt);
        }

        // GET: Kts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kt = await _context.Kts
                .Include(k => k.KidNavigation)
                .Include(k => k.TidNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kt == null)
            {
                return NotFound();
            }

            return View(kt);
        }

        // POST: Kts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kt = await _context.Kts.FindAsync(id);
            if (kt != null)
            {
                _context.Kts.Remove(kt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KtExists(int id)
        {
            return _context.Kts.Any(e => e.Id == id);
        }

        public async Task<IActionResult> LSc()
        {
            var korisnik = await _context.Korisniks
                .FirstOrDefaultAsync(k => k.Username == User.Identity.Name);
            ViewBag.username = User.Identity.Name;
            var records = await _context.Kts
                .Include(k => k.TidNavigation)
                .Where(k => k.Kid == korisnik.Id)
                .ToListAsync();

            if (!records.Any())
            {
                return NotFound("Немате решено ниту еден тест.");
            }

            return View(records);
        }

        public async Task<IActionResult> Stats(int id)
        {

            var test = await _context.Tests
                .FirstOrDefaultAsync(t => t.Id == id);

            ViewBag.Ime = test.Ime;
            var records = await _context.Kts
                .Include(k => k.TidNavigation)
                .Where(k => k.Tid == id)
                .ToListAsync();

            if (!records.Any())
            {
                return NotFound("Уште нема податоци за избраниот тест.");
            }

            return View(records);
        }

        [HttpPost]
        public async Task<IActionResult> Pocetok(int tid)
        {
            var test = await _context.Tests
                .FirstOrDefaultAsync(t => t.Id == tid);
            var korisnik = await _context.Korisniks
                .FirstOrDefaultAsync(k => k.Username == User.Identity.Name);
            var Ktexists = await _context.Kts.FirstOrDefaultAsync(kt => kt.Kid == korisnik.Id && kt.Tid == tid);
            if (Ktexists != null)
            {
                return NotFound("Веќе сте го решиле одбраниот тест.");
            }
            else
            {
                var kt = new Kt
                {
                    Kid = korisnik.Id,
                    Tid = tid,
                    Datum = DateTime.Now,
                    Score = 0
                };

                _context.Kts.Add(kt);
                await _context.SaveChangesAsync();
                return RedirectToAction("Quiz", "Prasanjes", new { tid = tid, tt = kt.Id });
            }
        }
    }
}