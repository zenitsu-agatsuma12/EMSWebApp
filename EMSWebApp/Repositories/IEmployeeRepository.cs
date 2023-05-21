using EMSWebApp.Models;
using EMSWebApp.ViewModel;
using Microsoft.VisualBasic;

namespace EMSWebApp.Repositories
{
    public interface IEmployeeRepository
    {
        // get
        List<Employee> GetEmployees(string token);

        // get by id
        Employee GetEmployeeById(int id, string token);

        // add 
        EmployeeViewModel AddEmployees(EmployeeViewModel newEmployee, string token);

        // update 
        Task<Employee> UpdateEmployee(int id, Employee newEmployee, string token);

        // delete 
        Task DeleteEmployee(int id, string token);
    }
}
