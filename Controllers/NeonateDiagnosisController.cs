using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JAUNDICE.CORE;
using Microsoft.AspNetCore.Authorization;


namespace JAUNDICE.WEB.Controllers
{
    [Authorize]
    public class NeonateDiagnosisController : Controller
    {
        private readonly JaundicedbContext _context;

        public NeonateDiagnosisController(JaundicedbContext context)
        {
            _context = context;
        }

        // GET: NeonateDiagnosis
        public async Task<IActionResult> Index()
        {
            var jaundicedbContext = _context.NeonateDiagnosis.Include(n => n.Disorder).Include(n => n.Neonate);
            return View(await jaundicedbContext.ToListAsync());
        }

        // GET: NeonateDiagnosis/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neonateDiagnosis = await _context.NeonateDiagnosis
                .Include(n => n.Disorder)
                .Include(n => n.Neonate)
                .FirstOrDefaultAsync(m => m.NeonateId == id);
            if (neonateDiagnosis == null)
            {
                return NotFound();
            }

            return View(neonateDiagnosis);
        }

        // GET: NeonateDiagnosis/Create
        [Authorize(Roles = "Doctor")]
        public IActionResult Create()
        {
            ViewData["DisorderId"] = new SelectList(_context.Disorder, "Id", "Id");
            ViewData["NeonateId"] = new SelectList(_context.Neonate, "Id", "Id");
            return View();
        }

        // POST: NeonateDiagnosis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NeonateId,DisorderId,NeedForFollowup,Treatment")] NeonateDiagnosis neonateDiagnosis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(neonateDiagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DisorderId"] = new SelectList(_context.Disorder, "Id", "Id", neonateDiagnosis.DisorderId);
            ViewData["NeonateId"] = new SelectList(_context.Neonate, "Id", "Id", neonateDiagnosis.NeonateId);
            return View(neonateDiagnosis);
        }

        // GET: NeonateDiagnosis/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neonateDiagnosis = await _context.NeonateDiagnosis.FindAsync(id);
            if (neonateDiagnosis == null)
            {
                return NotFound();
            }
            ViewData["DisorderId"] = new SelectList(_context.Disorder, "Id", "Id", neonateDiagnosis.DisorderId);
            ViewData["NeonateId"] = new SelectList(_context.Neonate, "Id", "Id", neonateDiagnosis.NeonateId);
            return View(neonateDiagnosis);
        }

        // POST: NeonateDiagnosis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("NeonateId,DisorderId,NeedForFollowup,Treatment")] NeonateDiagnosis neonateDiagnosis)
        {
            if (id != neonateDiagnosis.NeonateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(neonateDiagnosis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NeonateDiagnosisExists(neonateDiagnosis.NeonateId))
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
            ViewData["DisorderId"] = new SelectList(_context.Disorder, "Id", "Id", neonateDiagnosis.DisorderId);
            ViewData["NeonateId"] = new SelectList(_context.Neonate, "Id", "Id", neonateDiagnosis.NeonateId);
            return View(neonateDiagnosis);
        }

        // GET: NeonateDiagnosis/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neonateDiagnosis = await _context.NeonateDiagnosis
                .Include(n => n.Disorder)
                .Include(n => n.Neonate)
                .FirstOrDefaultAsync(m => m.NeonateId == id);
            if (neonateDiagnosis == null)
            {
                return NotFound();
            }

            return View(neonateDiagnosis);
        }

        // POST: NeonateDiagnosis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var neonateDiagnosis = await _context.NeonateDiagnosis.FindAsync(id);
            _context.NeonateDiagnosis.Remove(neonateDiagnosis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NeonateDiagnosisExists(string id)
        {
            return _context.NeonateDiagnosis.Any(e => e.NeonateId == id);
        }
    }
}
