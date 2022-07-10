using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using homework.Domain.Models;
using homework.Repository;
using homework.Service.Interface;
using Microsoft.AspNetCore.Authorization;

namespace homework.web.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMovieService _movieService;
        private readonly IScreaningService _screaningService;

        public MovieController(ApplicationDbContext context, IMovieService movieService, IScreaningService screaningService)
        {
            _context = context;
            _movieService = movieService;
            _screaningService = screaningService;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Movie/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _movieService.FindById(id);
            if (movie == null)
            {
                return NotFound();
            }

            var screanings = _screaningService.FindAll().Where(s => s.MovieId.Equals(id)).Where(s => s.Date >= DateTime.Today).ToList();
            List<int> availableTickets = new List<int>();
            foreach (var screaning in screanings)
            {
                int num = _screaningService.FindAvailableTicketsForScreaning(screaning.Id);
                availableTickets.Add(num);
            }
            ViewData["AvailableTickets"] = availableTickets;
            ViewData["Screanings"] = screanings;

            return View(movie);
        }

        // GET: Movie/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("Name,Genre,Id")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _movieService.Create(movie);

                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _movieService.FindById(id);

            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, [Bind("Name,Genre,Id")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _movieService.Update(movie);

                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _movieService.FindById(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _movieService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
