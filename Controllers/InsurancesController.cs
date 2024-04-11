using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectApp.Data;
using ProjectApp.Models;

namespace ProjectApp.Controllers
{
    public class InsurancesController(ApplicationDbContext context) : Controller
    {

        private readonly ApplicationDbContext _context = context;


        // GET: Insurances
        public async Task<IActionResult> Index()
        {
            // Pro administrátora zobrazí všechny pojištění
            if (User.IsInRole("Admin"))
            {
                return View(await _context.Insurance.Include(i => i.Person).ToListAsync());
            }
            // Jinak zobrazí pouze pojištění náležící lidem v přihlášeném účtu
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var insurancesForUser = await _context.Insurance.Include(i => i.Person)
                    .Where(i => i.Person.IdentityUserId == userId).ToListAsync();
                return View(insurancesForUser);
            }
        }


        // GET: Insurances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // GET: Insurances/Create
        public async Task<IActionResult> Create(int personId)

        {
            ViewBag.PersonId = personId;

            // Načtení jména osoby na základě personId a uložení do ViewData
            var person = await _context.Person.SingleOrDefaultAsync(i => i.Id == personId);
            ViewData["PersonName"] = person?.Jmeno; // Místo FullName použijte příslušnou vlastnost s celým jménem osoby

            return View();
        }


        // POST: Insurances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int personId,[Bind("Id,Castka,PlatnostOd,PlatnostDo,PredmetPojisteni,Typ,PersonId")] Insurance insurance)
        {


            // Načtení instance Person z databáze na základě personId
            var person = await _context.Person.SingleOrDefaultAsync(i => i.Id == personId);

            // Přiřazení Id osoby k vlastnosti PersonId v pojištění
            insurance.PersonId = personId;

            // Zkontrolujte, zda byla osoba nalezena
            if (person == null)
            {
                return NotFound("Osoba nenalezena"); // Pokud osoba nebyla nalezena, vrátíme 404
            }

            if (!ModelState.IsValid)
            {
                // Výpis chyb do konzole pro ladění
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Console.WriteLine($"Chyba: {error.ErrorMessage}");
                    }
                }

                // Vyhodit výjimku s odpovídající zprávou
                throw new InvalidOperationException("Některá pole nejsou ve správném formátu.");
            }

            // Pokud jsou data v pořádku, přidáme pojištění a uložíme změny
            _context.Add(insurance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Insurances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurance.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }

            // Přidání PersonId do ViewData, aby bylo dostupné v pohledu
            ViewData["PersonId"] = insurance.PersonId;

            return View(insurance);
        }

        // POST: Insurances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Castka,PlatnostOd,PlatnostDo,PredmetPojisteni,Typ,PersonId")] Insurance insurance)
        {
            if (id != insurance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insurance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceExists(insurance.Id))
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
            return View(insurance);
        }






        // GET: Insurances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // POST: Insurances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insurance = await _context.Insurance.FindAsync(id);
            if (insurance != null)
            {
                _context.Insurance.Remove(insurance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceExists(int id)
        {
            return _context.Insurance.Any(e => e.Id == id);
        }
    }
}
