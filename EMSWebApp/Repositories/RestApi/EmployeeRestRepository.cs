using EMSWebApp.Models;
using EMSWebApp.ViewModel;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace EMSWebApp.Repositories.Api
{
    public class EmployeeRestRepository : IEmployeeRepository
    {
        /* string baseURL = "http://localhost:5176/api";
         HttpClient httpClient = new HttpClient(); */

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configs;
        private readonly string _baseURL;

        public EmployeeRestRepository(IConfiguration configs)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(httpClientHandler);
            _configs = configs;
            _baseURL = "http://localhost:5176/api"; // Corrected base URL
        }

        public EmployeeViewModel AddEmployees(EmployeeViewModel newEmployee, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Assuming 'token' is defined

            string data = JsonConvert.SerializeObject(newEmployee);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var queryParameters = new Dictionary<string, string>
            {
                { "First_Name", newEmployee.First_Name },
                { "Last_Name", newEmployee.Last_Name },
                { "Middle_Name", newEmployee.Middle_Name },
                { "Address", newEmployee.Address },
                { "DOB", newEmployee.DOB }
            };

            string queryString = string.Join("&", queryParameters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}")); // Fixed encoding of query parameters
            string fullURL = $"{_baseURL}/Employee?{queryString}";

            var response = _httpClient.PostAsync(fullURL, content).Result;
            if (response.IsSuccessStatusCode)
            {
                // Handle successful response here
            }
            return null;
        }

        public async Task DeleteEmployee(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync(_baseURL + "/Employee/?Id=" + id);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to delete selected employee. Error: " + response.StatusCode);
            }
        }

        public Employee GetEmployeeById(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Assuming 'token' is defined

            string fullURL = $"{_baseURL}/Employee/{id}";

            var response = _httpClient.GetAsync(fullURL).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                var employee = JsonConvert.DeserializeObject<Employee>(responseData);
                return employee;
            }
            else
            {
                return null;
            }
        }

        public List<Employee> GetEmployees(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Assuming 'token' is defined

            string fullURL = $"{_baseURL}/Employee";

            var response = _httpClient.GetAsync(fullURL).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                var employees = JsonConvert.DeserializeObject<List<Employee>>(responseData);
                return employees;
            }
            else
            {
               return null;
            }
        }

        public async Task<Employee> UpdateEmployee(int id, Employee newEmployee, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonConvert.SerializeObject(newEmployee);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            
            //http://localhost:5176/api/Employee?Id=1&First_Name=weewwew&Last_Name=ewew&Middle_Name=wewewew&Address=wewewew&DOB=wewewew
            
            string fullURL = _baseURL + "/Employee/?Id=" + newEmployee.Id + "&First_Name=" + newEmployee.First_Name + "&Last_Name=" + newEmployee.Last_Name + "&Middle_Name=" + newEmployee.Middle_Name + "&Address=" + newEmployee.Address + "&DOB=" + newEmployee.DOB;
            var response = await _httpClient.PutAsync(fullURL, content);

            if (response.IsSuccessStatusCode)
            {
                return newEmployee;
            }
            else
            {
                return null;
            }
        }

        /*
                public Employee DeleteEmployee(int id)
                {
                    var response = httpClient.DeleteAsync($"{baseURL}/Employee/{id}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var deletedEmployee = JsonConvert.DeserializeObject<Employee>(data);
                        return deletedEmployee;
                    }

                    return null;
                }


                public Employee GetEmployeeById(int id)
                {
                    var url = $"{baseURL}/Employee/{id}";

                    var response = httpClient.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var employee = JsonConvert.DeserializeObject<Employee>(data);
                        return employee;
                    }

                    return null;
                }


                public List<Employee> GetEmployees()
                {
                    var response = httpClient.GetAsync(baseURL + "/Employee").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;

                        List<Employee> emp = JsonConvert.DeserializeObject<List<Employee>>(data);
                        return emp;
                    }
                    return null; 
                }

                public Employee UpdateEmployee(int id, Employee newEmployee)
                {
                    var url = $"{baseURL}/Employee?Id={id}&First_Name={newEmployee.First_Name}&Last_Name={newEmployee.Last_Name}&Middle_Name={newEmployee.Middle_Name}&Address={newEmployee.Address}&DOB={newEmployee.DOB}";

                    var response = httpClient.PutAsync(url, null).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var updatedEmployee = JsonConvert.DeserializeObject<Employee>(data);
                        return updatedEmployee;
                    }

                    return null;
                }

                */

    }
}
