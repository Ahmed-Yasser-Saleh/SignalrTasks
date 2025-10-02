
using signalrTask.ViewModels;
using signalrTask.ViewModels.Account;
using signalrTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MVC_Test.Controllers
{
    public class AccountController : Controller
    {
        UserManager<Employee> userManager;
        SignInManager<Employee> signmanager;

        public AccountController(UserManager<Employee> userManager, SignInManager<Employee> signmanager)
        {
            this.userManager = userManager;
            this.signmanager = signmanager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> SaveRegister(RegisterDTO Rg)
        {
            if (ModelState.IsValid)
            {
                    var user = await userManager.FindByNameAsync(Rg.username);
                    if (user != null)
                    {
                        // return BadRequest(new { Status = 400, ErrorMassege = "Register failed, There is Admin with the same username" });\
                        ModelState.AddModelError("Username", "Username Exist");
                        return View("Register");
                    }
                    var user2 = await userManager.FindByEmailAsync(Rg.email);
                    if (user2 != null)
                    {
                        //return BadRequest(new { Status = 400, ErrorMassege = "Register failed, There is Admin with the same email" });
                        ModelState.AddModelError("Email", "Email Exist");
                        return View("Register");
                    }
                    var emp = new Employee
                    {
                        UserName = Rg.username,
                        Email = Rg.email
                    };

                    var res = await userManager.CreateAsync(emp, Rg.password);
                    if (res.Succeeded)
                    {
                            //return Ok(new { Status = "Admin registered successfully." });
                            ViewBag.SuccessMessage = "User registered successfully.";
                            return View("Login");
                    }

                    else
                    {
                        return View("Register");
                        // return BadRequest(new { Status = 400, ErrorMassege = "Create failed: " + string.Join(", ", res.Errors.Select(e => e.Description)) });
                    }
                }
            else
                return View("Register");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO cs)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(cs.email);
                if (user == null)
                // return Unauthorized(new { Status = 401, ErrorMassege = "Unauthorized" });
                {
                    ViewBag.UnauthorizedMessage = "Email Not Exist";
                    return View();
                }
                var checkpw = await userManager.CheckPasswordAsync(user, cs.password);
                if(!checkpw)
                {
                    ViewBag.UnauthorizedMessage = "invalid Account";
                    return View();
                }
                #region generate Cooky
                 //var rs = signmanager.PasswordSignInAsync(user.UserName, cs.password, false, false).Result; // هنا ال identity بيضيف الكليم اوتوماتيك زي اليوزر نيم والاميل
                List<Claim> claims = new List<Claim>(); //add custom claims
                claims.Add(new Claim("UserEmail", user.Email));
                await signmanager.SignInWithClaimsAsync(user, false, claims); //هنا بيضيف الكليم بتاعتك زائد بتاعت ال identity
                #endregion
         
                return View("Chat", user);    
           
                   // return BadRequest(new { Status = 400, ErrorMassege = "Login Failed" });
            }
            else
                return View();
            //return Ok(); //BadRequest(ModelState);
        }

        [HttpGet]
        [Authorize(Roles = "Customer,Admin")]
        public IActionResult Logout()
        {
            // 1- مسح الكوكي الأساسي اللي عمله Identity
            signmanager.SignOutAsync();

            // 2- لو عندك كوكي مخصص (إنت عملته بنفسك)
            if (Request.Cookies.ContainsKey("MyCustomCookie"))
            {
                Response.Cookies.Delete("MyCustomCookie");
            }
            return  View("Login");
        }
    }
}
