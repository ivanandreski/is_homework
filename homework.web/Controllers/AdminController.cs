using homework.Domain.Models;
using homework.Repository.Interface;
using homework.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace homework.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMovieService movieService;

        public AdminController(UserManager<User> userManager, IUserRepository userRepository, ITicketRepository ticketRepository, IMovieService movieService)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            this.movieService = movieService;
        }

        [HttpGet]
        public IActionResult Index(string userError, string fileError, string genre)
        {
            var user = _userRepository.Get(User.Identity.Name);
            if (!string.IsNullOrEmpty(userError))
                ViewData["userError"] = userError;
            if (!string.IsNullOrEmpty(fileError))
                ViewData["fileError"] = fileError;
            if (!string.IsNullOrEmpty(genre))
                ViewData["Tickets"] = _ticketRepository.GetAll()
                    .Where(t => t.Screaning.Movie.Genre.Equals(genre))
                    .ToList();
            else
                ViewData["Tickets"] = _ticketRepository.GetAll().ToList();

            ViewData["Genres"] = movieService.GetAllGenres();

            return View();
        }

        [HttpPost]
        public IActionResult AddToRole(string userName, string role)
        {
            if (string.IsNullOrEmpty(userName)) return Redirect("/Admin?userError=User%20Name%20must%20not%20be%20empty!");

            var user = _userRepository.Get(userName);
            if (user == null) return Redirect("/Admin?userError=User%20does%20not%20exist!");

            _userManager.AddToRoleAsync(user, role);

            return Redirect("/Admin");
        }

        [HttpPost]
        public IActionResult ImportUsers()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult ExportTickets(List<Guid> ticketIds)
        {
            var tickets = ticketIds.Select(id => _ticketRepository.Get(id))
                .ToList();

            throw new NotImplementedException();
        }
    }
}
