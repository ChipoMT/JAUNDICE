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
    [Authorize(Roles = "Nurse")]
    public class NeonatesController : Controller
    {
        private readonly JaundicedbContext _context;

        public NeonatesController(JaundicedbContext context)
        {
            _context = context;
        }

        // GET: Neonates
        public async Task<IActionResult> Index()
        {
            var jaundicedbContext = _context.Neonate.Include(n => n.Client);
            return View(await jaundicedbContext.ToListAsync());
        }

        // GET: Neonates/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neonate = await _context.Neonate
                .Include(n => n.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (neonate == null)
            {
                return NotFound();
            }

            return View(neonate);
        }

        // GET: Neonates/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id");
            return View();
        }

        // POST: Neonates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,BilirubunAtBirth,Dob,BirthWeight,GestationalAge,MothersFirstName,MothersMiddleName,MothersLastName,MothersNrc")] Neonate neonate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(neonate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", neonate.ClientId);
            return View(neonate);
        }

        // GET: Neonates/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neonate = await _context.Neonate.FindAsync(id);
            if (neonate == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", neonate.ClientId);
            return View(neonate);
        }

        // POST: Neonates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ClientId,BilirubunAtBirth,Dob,BirthWeight,GestationalAge,MothersFirstName,MothersMiddleName,MothersLastName,MothersNrc")] Neonate neonate)
        {
            if (id != neonate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(neonate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NeonateExists(neonate.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", neonate.ClientId);
            return View(neonate);
        }

        // GET: Neonates/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neonate = await _context.Neonate
                .Include(n => n.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (neonate == null)
            {
                return NotFound();
            }

            return View(neonate);
        }

        // POST: Neonates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var neonate = await _context.Neonate.FindAsync(id);
            _context.Neonate.Remove(neonate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NeonateExists(string id)
        {
            return _context.Neonate.Any(e => e.Id == id);
        }
    }
}
