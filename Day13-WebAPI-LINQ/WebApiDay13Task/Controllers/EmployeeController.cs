using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDay13Task.DTOs;
using WebApiDay13Task.Models;

namespace WebApiDay13Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        static List<Employee> employees = new List<Employee>()
        {
            new Employee() {Id =1, Name="Pratik Pawar",Department="IT",Salary=25000},
            new Employee() {Id =2, Name="Sahil Shinde", Department="HR", Salary=20000}
        };

        [HttpGet]
        public IActionResult GetEmployees()
        {
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id) 
        {
            Employee emp = employees.FirstOrDefault(x => x.Id == id);

            if(emp == null)
            {
                return NotFound("Employee Not Found");
            }

            return Ok(emp);
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeDTO dto)
        {
            Employee emp = new Employee()
            {
                Id = employees.Count + 1,
                Name = dto.Name,

                Department = dto.Department,
                Salary = 30000
            };

            employees.Add(emp);
            return Created("", emp);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmplyee(int id, EmployeeDTO dto)
        {
            Employee emp = employees.FirstOrDefault(x => x.Id == id);

            if(emp == null)
            {
                return NotFound();
            }

            emp.Name = dto.Name;
            emp.Department = dto.Department;

            return Ok(emp);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var emp = employees.FirstOrDefault(x => x.Id == id);
            
            if( emp == null )
            {
                return NotFound();
            }

            employees.Remove(emp);
            return Ok("Deleted Succesfully");
        }

        [HttpGet("sorted")]
        public IActionResult SortedEmployees()
        {
            var data = employees.OrderByDescending( x=> x.Salary).ToList();
            return Ok(data);    
        }

        [HttpGet("department/{department}")]
        public IActionResult GetEmployeeByDepartment(string department)
        {
            var data = employees.Where(x=> x.Department.ToLower() == department.ToLower()).ToList();
            return Ok(data);
        }


    }
}
