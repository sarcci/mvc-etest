using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EtestWebApp.Data;
using EtestWebApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EtestWebApp.Controllers
{
    public class KodgsController : Controller
    {
        private readonly EtestContext _context;

        public KodgsController(EtestContext context)
        {
            _context = context;
        }

        // GET: Kodgs
        public async Task<IActionResult> Index()
        {
            var etestContext = _context.Kodgs.Include(k => k.Kt).Include(k => k.OidNavigation).Include(k => k.PidNavigation);
            return View(await etestContext.ToListAsync());
        }

        public ActionResult Tnx()
        {
            return View();
        }

        // GET: Kodgs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kodg = await _context.Kodgs
                .Include(k => k.Kt)
                .Include(k => k.OidNavigation)
                .Include(k => k.PidNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kodg == null)
            {
                return NotFound();
            }

            return View(kodg);
        }

        // GET: Kodgs/Create
        public IActionResult Create()
        {
            ViewData["Ktid"] = new SelectList(_context.Kts, "Id", "Id");
            ViewData["Oid"] = new SelectList(_context.Odgovors, "Id", "Id");
            ViewData["Pid"] = new SelectList(_context.Prasanjes, "Id", "Id");
            return View();
        }

        // POST: Kodgs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int page, int tid, int Ktid, int Pid, int? Oid)
        {
            if (Oid == null)
                return RedirectToAction("Quiz", "Prasanjes", new { page = page, tid = tid, tt = Ktid });

            var kexists = await _context.Kodgs
                .FirstOrDefaultAsync(k => k.Ktid == Ktid && k.Pid == Pid);

            if (kexists == null)
            {
                var kodg = new Kodg
                {
                    Ktid = Ktid,
                    Pid = Pid,
                    Oid = (int)Oid
                };
                _context.Add(kodg);
            }

            else if (Oid != kexists.Oid)
            {
                kexists.Oid = (int)Oid;
                _context.Update(kexists);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Quiz", "Prasanjes", new {page = page, tid = tid, tt = Ktid});
        }

        // GET: Kodgs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kodg = await _context.Kodgs.FindAsync(id);
            if (kodg == null)
            {
                return NotFound();
            }
            ViewData["Ktid"] = new SelectList(_context.Kts, "Id", "Id", kodg.Ktid);
            ViewData["Oid"] = new SelectList(_context.Odgovors, "Id", "Id", kodg.Oid);
            ViewData["Pid"] = new SelectList(_context.Prasanjes, "Id", "Id", kodg.Pid);
            return View(kodg);
        }

        // POST: Kodgs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ktid,Pid,Oid")] Kodg kodg)
        {
            if (id != kodg.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kodg);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KodgExists(kodg.Id))
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
            ViewData["Ktid"] = new SelectList(_context.Kts, "Id", "Id", kodg.Ktid);
            ViewData["Oid"] = new SelectList(_context.Odgovors, "Id", "Id", kodg.Oid);
            ViewData["Pid"] = new SelectList(_context.Prasanjes, "Id", "Id", kodg.Pid);
            return View(kodg);
        }

        // GET: Kodgs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kodg = await _context.Kodgs
                .Include(k => k.Kt)
                .Include(k => k.OidNavigation)
                .Include(k => k.PidNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kodg == null)
            {
                return NotFound();
            }

            return View(kodg);
        }

        // POST: Kodgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kodg = await _context.Kodgs.FindAsync(id);
            if (kodg != null)
            {
                _context.Kodgs.Remove(kodg);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KodgExists(int id)
        {
            return _context.Kodgs.Any(e => e.Id == id);
        }

        public async Task<IActionResult> End(int ktid)
        {
            var kodgs = await _context.Kodgs
                .Include(k => k.OidNavigation)
                .Include(k => k.PidNavigation)
                .Where(k => k.Ktid == ktid)
                .ToListAsync();

            var score = 0;
            ViewBag.Score = score;

            if (kodgs == null || !kodgs.Any())
            {
                return View();
            }

            foreach (var kodg in kodgs)
            {
                if (kodg.OidNavigation.True == true)
                {
                    score += 1;
                }
            }
            var dd = await _context.Kts
                .FirstAsync(k => k.Id == ktid);
            dd.Score = score;
            _context.Update(dd);
            await _context.SaveChangesAsync();
            ViewBag.Score = score;
            return View();
        }
    }
}
