using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectApp.Data;
using ProjectApp.Models;

namespace ProjectApp.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;


        public PeopleController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Pro administrátora zobrazí všechny lidi
            if (User.IsInRole("Admin"))
            {
                return View(await _context.Person.ToListAsync());
            }
            // Jinak zobrazí pouze lidi spadající pod přihlášený účet
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var peopleForUser = await _context.Person.Where(p => p.IdentityUserId == userId).ToListAsync();
                return View(peopleForUser);
            }
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Jmeno,Prijmeni,TelefoniCislo,Email,Mesto,Ulice,CisloPopisne,PSC")] Person person)
        {
            // Získání aktuálně přihlášeného uživatele
            var currentUser = await userManager.GetUserAsync(User);
            person.IdentityUserId = currentUser.Id;

            var currentUserEmail = User.Identity.Name;
            person.Email = currentUserEmail;

            if (!ModelState.IsValid)
            {
                // Pokud validace selže, vrátíme pohled s chybami validace
                return View(person);
            }

            // Normalizace PSČ na formát '123 45'
            person.PSC = NormalizePSC(person.PSC);

            // Pokud je vše v pořádku, přidáme osobu do kontextu a uložíme změny
            _context.Add(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Metoda pro normalizaci PSČ
        private string NormalizePSC(string psc)
        {
            // Odstranění mezer a přidání mezery po prvních 3 číslicích (pokud je třeba)
            psc = psc.Replace(" ", "");
            if (psc.Length > 3)
            {
                psc = psc.Insert(3, " ");
            }
            return psc;
        }



        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            ViewData["OriginalIdentityUserId"] = person.IdentityUserId; // Předání do pohledu

            return View(person);
        }

        // POST: People/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Jmeno,Prijmeni,TelefoniCislo,Email,Mesto,Ulice,CisloPopisne,PSC,IdentityUserId")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            return View(person);
        }





        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person != null)
            {
                _context.Person.Remove(person);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}
