using ClosedXML.Excel;
using ExcelDataReader;
using homework.Domain.Models;
using homework.Repository.Interface;
using homework.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace homework.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMovieService movieService;
        public readonly IPasswordHasher<User> _passwordHasher;

        public AdminController(UserManager<User> userManager, IUserRepository userRepository, ITicketRepository ticketRepository, IMovieService movieService, IPasswordHasher<User> passwordHasher)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            this.movieService = movieService;
            _passwordHasher = passwordHasher;
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
        public IActionResult ImportUsers(IFormFile userTable)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                userTable.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read()) //Each row of the file
                    {
                        string email = reader.GetValue(0).ToString();
                        string passsword = reader.GetValue(1).ToString();
                        string role = reader.GetValue(2).ToString();

                        var user = new User();
                        user.Email = email;
                        user.UserName = email;
                        user.NormalizedEmail = email.ToUpper();
                        user.NormalizedUserName = email.ToUpper();

                        var hashedPassword = _passwordHasher.HashPassword(user, passsword);
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        user.PasswordHash = hashedPassword;

                        _userRepository.Insert(user);

                        _userManager.AddToRoleAsync(user, role);
                    }
                }
            }


            return Redirect("/Admin");
        }

        [HttpPost]
        public IActionResult ExportTickets(List<Guid> ticketIds)
        {
            var tickets = ticketIds.Select(id => _ticketRepository.Get(id))
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                //< td > @i </ td >
                //< td > @ticket.User.UserName </ td >
                //< td > @ticket.Screaning.Movie.Name </ td >
                //< td > @ticket.Screaning.Movie.Genre </ td >
                //< td > @ticket.Screaning.Date.ToShortDateString() </ td >

                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "#";
                worksheet.Cell(currentRow, 2).Value = "User Name";
                worksheet.Cell(currentRow, 2).Value = "Movie";
                worksheet.Cell(currentRow, 2).Value = "Genre";
                worksheet.Cell(currentRow, 2).Value = "Date";
                foreach (var ticket in tickets)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = ticket.User.UserName;
                    worksheet.Cell(currentRow, 2).Value = ticket.Screaning.Movie.Name;
                    worksheet.Cell(currentRow, 3).Value = ticket.Screaning.Movie.Genre;
                    worksheet.Cell(currentRow, 4).Value = ticket.Screaning.Date.ToShortDateString();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "users.xlsx");
                }
            }
        }
    }
}
