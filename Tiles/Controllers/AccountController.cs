using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Tiles.Models;
using Tiles.Models.Data;

namespace Tiles.Controllers
{
    public class AccountController : Controller
    {
        public UsersRepository UsersRepository { get; }
        public IConfiguration Configuration { get; }

        public AccountController(UsersRepository usersRepository, IConfiguration config)
        {
            UsersRepository = usersRepository;
            Configuration = config;
        }

        

        [HttpGet]
        public IActionResult Signin()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Signin(LoginData model)
        {
            await Task.Delay(1500);

            if (!ModelState.IsValid)
                return View(model);


            var user = await UsersRepository.GetUser(model.Login);
            if (user == null)
            {
                ModelState.AddModelError("", "Login or password error");
                return View(model);
            }

            await Authenticate(model);

            return Redirect("~/");
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }


        [HttpGet]
        public async Task<IActionResult> Register(string token, User user)
        {
            await Task.Delay(5000);

            if (string.IsNullOrEmpty(token))
                return BadRequest();

            if (token != Configuration.GetValue<string>("RootToken"))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            await UsersRepository.SaveUser(user);

            return Redirect("~/");
        }


        private async Task Authenticate(LoginData model)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, model.Login)
            };

            var id = new ClaimsIdentity(claims, "ApplicaionCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}