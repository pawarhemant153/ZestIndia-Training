using Day14_EFCore_API.Data;
using Day14_EFCore_API.DTOs;
using Day14_EFCore_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Day14_EFCore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext db;

        public EmployeeController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            // Fetch employee data with related department data
            // Include() is used for eager loading
            var employees = db.Employees.Include(e => e.Department)

                // Convert Employee entity into EmployeeDto
                .Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.EmployeeName,
                    Salary = e.Salary,
                    DepartmentName = e.Department.DepartmentName
                }).ToList();

            return Ok(employees);
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeDto dto)
        {

            // Get department id using department name
            // Get department id using department name
            int departmentId = db.Departments
                .Where(d => d.DepartmentName == dto.DepartmentName)
                .Select(d => d.DepartmentId)
                .FirstOrDefault();

            // Convert DTO object into Employee entity object
            Employee employee = new Employee()
            {
                EmployeeName = dto.EmployeeName,
                Salary = dto.Salary,
                DepartmentId = departmentId
            };

            db.Employees.Add(employee);
            db.SaveChanges();

            return Ok("Employee Added Succesfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, EmployeeDto dto)
        {
            // Find employee by id
            var employee = db.Employees
                .FirstOrDefault(e => e.EmployeeId == id);

            // Check whether employee exists or not
            if (employee == null)
            {
                return NotFound("Employee Not Found");
            }

            // Get department id using department name
            int departmentId = db.Departments
                .Where(d => d.DepartmentName == dto.DepartmentName)
                .Select(d => d.DepartmentId)
                .FirstOrDefault();

            // Update employee details
            employee.EmployeeName = dto.EmployeeName;
            employee.Salary = dto.Salary;
            employee.DepartmentId = departmentId;

            db.SaveChanges();

            // Return success response
            return Ok("Employee Updated Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            // Find employee by id
            var employee = db.Employees
                .FirstOrDefault(e => e.EmployeeId == id);

            // Check whether employee exists or not
            if (employee == null)
            {
                return NotFound("Employee Not Found");
            }

            // Remove employee from database
            db.Employees.Remove(employee);

            db.SaveChanges();

            // Return success response
            return Ok("Employee Deleted Successfully");
        }
    }
}
