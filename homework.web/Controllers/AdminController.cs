using homework.Domain.Models;
using homework.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace homework.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public AdminController(UserManager<User> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index(string userError, string fileError)
        {
            var user = _userRepository.Get(User.Identity.Name);
            if (!string.IsNullOrEmpty(userError))
                ViewData["userError"] = userError;
            if (!string.IsNullOrEmpty(fileError))
                ViewData["fileError"] = fileError;

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
    }
}
