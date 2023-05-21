using EMSWebApp.Repositories;
using EMSWebApp.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EMSWebApp.Controllers
{
    public class AccountController : Controller
    {
        public IAccountRepository _repo { get; }

        public AccountController(IAccountRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel userViewModel)
        {
            userViewModel.UserName = userViewModel.Email; // Assign Email to UserName
            ModelState.Remove("UserName");
            if (ModelState.IsValid)
            {
                var result = await _repo.SignUpUserAsync(userViewModel);
                if (result)
                {
                    return RedirectToAction("login");
                }
            }
            return View(userViewModel);
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                // login activity -> cookie [Roles and Claims]
                var result = await _repo.SignInUserAsync(userViewModel);
                //login cookie and transfter to the client 
                if (result is not null)
                {
                    // add token to session 
                    HttpContext.Session.SetString("JWToken", result);

                    return RedirectToAction("GetEmployees", "Employee");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Credentials");
            }
            return View(userViewModel);
        }

        public IActionResult Logout()
        {
            // Clear the session or any other authentication-related data
            HttpContext.Session.Clear();

            // Redirect to the login page or any other desired page
            return RedirectToAction("Login", "Account");
        }

        private bool IsUserAuthenticated()
        {
            // Check if the token exists in the session

            return HttpContext.Session.GetString("JWToken") != null;
        }

    }
}
