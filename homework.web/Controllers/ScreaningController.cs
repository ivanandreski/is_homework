﻿using System;
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
    public class ScreaningController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IScreaningService _screaningService;

        public ScreaningController(ApplicationDbContext context, IScreaningService screaning)
        {
            _context = context;
            _screaningService = screaning;
        }

        // GET: Screaning
        public IActionResult Index(DateTime date)
        {
            var screanings = _screaningService.FindAll();
            if (date.Year != 1)
            {
                screanings = _screaningService.FindAllFiltered(date);
            }
            
            List<int> availableTickets = new List<int>();
            foreach(var screaning in screanings)
            {
                int num = _screaningService.FindAvailableTicketsForScreaning(screaning.Id);
                availableTickets.Add(num);
            }
            ViewData["AvailableTickets"] = availableTickets;

            return View(screanings);
        }

        // GET: Screaning/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screaning = _screaningService.FindById(id);
            if (screaning == null)
                return NotFound();

            return View(screaning);
        }

        // GET: Screaning/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            List<Movie> allMovies = _context.Movies.ToList();
            ViewData["Movies"] = allMovies;

            return View();
        }

        // POST: Screaning/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("Date,MaxTickets,Price,MovieId,Id")] Screaning screaning)
        {
            if (ModelState.IsValid)
            {
                _screaningService.Create(screaning);

                return RedirectToAction(nameof(Index));
            }
            return View(screaning);
        }

        // GET: Screaning/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screaning = _screaningService.FindById(id);
            if (screaning == null)
            {
                return NotFound();
            }
            List<Movie> allMovies = _context.Movies.ToList();
            ViewData["Movies"] = allMovies;

            return View(screaning);
        }

        // POST: Screaning/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, [Bind("Date,MaxTickets,Price,MovieId,Id")] Screaning screaning)
        {
            if (id != screaning.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _screaningService.Update(screaning);

                return RedirectToAction(nameof(Index));
            }
            return View(screaning);
        }

        // GET: Screaning/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screaning = _screaningService.FindById(id);
            if (screaning == null)
            {
                return NotFound();
            }

            return View(screaning);
        }

        // POST: Screaning/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _screaningService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
