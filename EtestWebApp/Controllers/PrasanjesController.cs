using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EtestWebApp.Data;
using EtestWebApp.Models;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EtestWebApp.Controllers
{
    public class PrasanjesController : Controller
    {
        private readonly EtestContext _context;

        public PrasanjesController(EtestContext context)
        {
            _context = context;
        }

        // GET: Prasanjes
        public async Task<IActionResult> Index()
        {
            var etestContext = _context.Prasanjes.Include(p => p.TidNavigation);
            return View(await etestContext.ToListAsync());
        }

        // GET: Prasanjes/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var prasanje = await _context.Prasanjes
                .Include(p => p.TidNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prasanje == null)
            {
                return NotFound();
            }

            return View(prasanje);
        }

        // GET: Prasanjes/Create
        public IActionResult Create(int? tid)
        {
            ViewData["Tid"] = tid;
            return View();
        }

        // POST: Prasanjes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int tid, [Bind("Id,Tekst,Tip")] Prasanje prasanje)
        {
            if (ModelState.IsValid)
            {
                prasanje.Tid = tid;
                _context.Add(prasanje);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Create", "Odgovors", new {pid = prasanje.Id});
        }

        // GET: Prasanjes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prasanje = await _context.Prasanjes.FindAsync(id);
            if (prasanje == null)
            {
                return NotFound();
            }
            ViewData["Tid"] = new SelectList(_context.Tests, "Id", "Id", prasanje.Tid);
            return View(prasanje);
        }

        // POST: Prasanjes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tid,Tekst,Tip")] Prasanje prasanje)
        {
            if (id != prasanje.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prasanje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrasanjeExists(prasanje.Id))
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
            ViewData["Tid"] = new SelectList(_context.Tests, "Id", "Id", prasanje.Tid);
            return View(prasanje);
        }

        // GET: Prasanjes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prasanje = await _context.Prasanjes
                .Include(p => p.TidNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prasanje == null)
            {
                return NotFound();
            }
            return View(prasanje);
        }

        // POST: Prasanjes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prasanje = await _context.Prasanjes.FindAsync(id);
            if (prasanje != null)
            {
                _context.Prasanjes.Remove(prasanje);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrasanjeExists(int id)
        {
            return _context.Prasanjes.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Quiz(int? page, int? tid, int? tt)
        {
            int pageSize = 1;
            var total = (int)Math.Ceiling(_context.Prasanjes.Count(q => q.Tid == tid) / (double)pageSize);
            int pageNumber = (page ?? 1);
            ViewBag.TotalPages = total;


            var test = await _context.Tests
                .FirstOrDefaultAsync(t => t.Id == tid);

            if (page > ViewBag.TotalPages)
            {
                if (!test.Tip) 
                    return RedirectToAction("End", "Kodgs", new {ktid = tt});
                else 
                    return RedirectToAction("Tnx", "Kodgs");
            }

            var korisnik = await _context.Korisniks
                .FirstOrDefaultAsync(k => k.Username == User.Identity.Name);

            ViewBag.PageNumber = pageNumber;
            ViewBag.TestName = test.Ime;
            ViewBag.TestId = tid;
            ViewBag.tt = tt;
            
            var questionsWithAnswers = await _context.Prasanjes
                .Include(q => q.Odgovors)
                .AsNoTracking()
                .Where(q => q.Tid == tid)
                .OrderBy(q => q.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(questionsWithAnswers);
        }
    }
}
