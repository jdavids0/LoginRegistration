using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using LoginRegistration.Models;

namespace LoginRegistration.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        HttpContext.Session.Clear();
        return View();
    }

    // *** registration route ***
    [HttpPost("user/register")]
    public IActionResult RegisterUser(User newUser)
    {
        if(ModelState.IsValid){
            // have now passed validations, need to check if email is unique
            if(_context.Users.Any(a => a.Email == newUser.Email))
            {
                // user is already in db
                ModelState.AddModelError("Email", "Email is already in use!");
                return View("Index");
            }
            // if user not in db, hash pw and add user to db
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("user", newUser.UserID);
            return RedirectToAction ("Success");
        } else {
            return View ("Index");
        }
    }

    // *** login routes
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("user/login")]
    public IActionResult LoginUser(LogUser loginUser)
    {
        if(ModelState.IsValid){
            // Step 1. find email in db; if not in db, throw error
            User userInDb = _context.Users.FirstOrDefault(a => a.Email == loginUser.LogEmail);
            if (userInDb == null){
                // there was no email in db, so throw error
                // say "invalid login attempt" rather than telling them what they get wrong
                ModelState.AddModelError("LogEmail", "Invalid login attempt");
                return View ("Login");
            }
            PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();

            var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.LogPassword);

            if (result == 0){
                // password was not correct
                ModelState.AddModelError("LogEmail", "Invalid login attempt");
                return View ("Login");
            } else {
                HttpContext.Session.SetInt32("user", userInDb.UserID);
                return RedirectToAction("Login");
            }
            
        }
        else {
            return View("Login");
        }
    }


    // *** success route ***
    [HttpGet("success")]
    public IActionResult Success()
    {
        // if no logged in user is in session (i.e., if sess user == null), redirect to registration page
        if(HttpContext.Session.GetInt32("user") == null){
            return RedirectToAction("Index");
        }
        // need (int) in front of sess here bc will be comparing UserID int
        User loggedInUser = _context.Users.FirstOrDefault(a => a.UserID == (int) HttpContext.Session.GetInt32("user"));
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
