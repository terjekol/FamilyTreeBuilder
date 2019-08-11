using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeBuilder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FamilyTreeBuilder.ModelsGenerated;
using Microsoft.AspNetCore.Authorization;

namespace FamilyTreeBuilder.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private readonly FamilyTreeContext _context;

        public PeopleController(FamilyTreeContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            var familyTreeContext = _context.Person.Include(p => p.FatherNavigation).Include(p => p.MotherNavigation);
            return View(await familyTreeContext.ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .Include(p => p.FatherNavigation)
                .Include(p => p.MotherNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            var children = _context.Person.Where(p => p.Mother == id || p.Father == id).ToList();
            var viewModel = new PersonViewModel { Person = person, Children = children };

            return View(viewModel);

        }

        // GET: People/Create
        public IActionResult Create()
        {
            SavePotentialParentsAndSexesInViewData();
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,BirthDate,DeathDate,IsMale,Father,Mother,DataOwnerId")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            SavePotentialParentsAndSexesInViewData(person);
            return View(person);
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
            SavePotentialParentsAndSexesInViewData(person);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,BirthDate,DeathDate,IsMale,Father,Mother,DataOwnerId")] Person person)
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
            SavePotentialParentsAndSexesInViewData(person);
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
                .Include(p => p.FatherNavigation)
                .Include(p => p.MotherNavigation)
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
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.Id == id);
        }

        private void SavePotentialParentsAndSexesInViewData(Person person = null)
        {
            object mapPerson(Person p)
            {
                var name = p.FirstName ?? string.Empty;
                if (p.LastName != null) name += " " + p.LastName;
                if (p.BirthDate != null) name += " (born " + p.BirthDate.Value.ToShortDateString() + ")";
                return new
                {
                    Id = p.Id,
                    Name = name
                };
            }

            var sexes = new object[]
            {
                new {Value = (bool?)null, Text=string.Empty},
                new {Value=(bool?)true, Text="Male"},
                new {Value=(bool?)false, Text="Female"},
            };
            var potentialFathers = _context.Person.Where(p => p.IsMale == true).Select(mapPerson).ToList();
            var potentialMothers = _context.Person.Where(p => p.IsMale == false).Select(mapPerson).ToList();
            potentialMothers.Insert(0, new { Id = (int?)null, Name = string.Empty });
            potentialFathers.Insert(0, new { Id = (int?)null, Name = string.Empty });
            ViewData["Father"] = new SelectList(potentialFathers, "Id", "Name", person?.Father);
            ViewData["Mother"] = new SelectList(potentialMothers, "Id", "Name", person?.Mother);
            ViewData["Sexes"] = new SelectList(sexes, "Value", "Text", person?.IsMale);
        }

    }
}
