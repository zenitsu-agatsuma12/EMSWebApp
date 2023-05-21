using EMSWebApp.Models;
using EMSWebApp.Repositories;
using EMSWebApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace EMSWebApp.Controllers
{
    public class EmployeeController : Controller
    {
        IEmployeeRepository _repo;

        public EmployeeController(IEmployeeRepository repo)
        {
            this._repo = repo;
        }
        // Get All
        public IActionResult GetEmployees()
        {
            string token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                // Handle the case when the token is not available
                return RedirectToAction("Login", "Account");
            }

            var contacts =  _repo.GetEmployees(token);

            return View(contacts);
        }

        // Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel newEmployee)
        {
            if (ModelState.IsValid)
            {
                var token = HttpContext.Session.GetString("JWToken");
                var info = _repo.AddEmployees(newEmployee, token);
                return RedirectToAction("GetEmployees");
            }
            ViewData["Message"] = "Data is not valid to create the Employees";
            return View();
        }

        // Update

        [HttpGet]
        public IActionResult Update(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var wishlist = _repo.GetEmployeeById(id, token);
            if (wishlist == null)
                return NotFound();

            return View(wishlist);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Employee updatedEmployee)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (id != updatedEmployee.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var getEmp = await _repo.UpdateEmployee(id, updatedEmployee, token);
                if (getEmp != null)
                {
                    if (getEmp.Id != null)
                    {
                        return RedirectToAction(nameof(GetEmployees), new { id = getEmp.Id, token });
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to update the selected employee";
                        return View(updatedEmployee);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to update the selected employee";
                    return View(updatedEmployee);
                }
            }

            return View(updatedEmployee);
        }

        // Get by ID

        public IActionResult Details(int id)
        {

            var token = HttpContext.Session.GetString("JWToken");
            var emp = _repo.GetEmployeeById(id, token);

            if (emp is null)
                return NotFound();

            return View(emp);
        }

        public IActionResult Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            _repo.DeleteEmployee(id, token);
            return RedirectToAction("GetEmployees");
        }

    }
}
