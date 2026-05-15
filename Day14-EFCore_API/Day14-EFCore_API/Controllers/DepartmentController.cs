using Day14_EFCore_API.Data;
using Day14_EFCore_API.DTOs;
using Day14_EFCore_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Day14_EFCore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext db;
        public DepartmentController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetDepartments()
        {
            // Fetch department data and map entity to DTO
            var departments = db.Departments
                .Select(d => new DepartmentDto
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName
                }).ToList();

            // Check whether department list is empty
            if (departments.Count == 0)
            {
                return NotFound("No Departments Found");
            }

            return Ok(departments);
        }

        [HttpGet("{id}")]
        public IActionResult GetDepartmentById(int id)
        {
            // Fetch department by id and map entity to DTO
            var data = db.Departments
                .Where(d => d.DepartmentId == id)
                .Select(d => new DepartmentDto
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName
                }).FirstOrDefault();

            // Check whether department exists or not
            if (data == null)
            {
                return NotFound("Department not found");
            }

            return Ok(data);
        }

        [HttpPost]
        public IActionResult AddDepartment(DepartmentDto dto)
        {
            try
            {
                // Check whether incoming model data is valid or not
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check whether department already exists in database
                bool exists = db.Departments
                    .Any(d => d.DepartmentName.Trim().ToLower()
                           == dto.DepartmentName.Trim().ToLower());

                // If department already exists then return 409 Conflict
                if (exists)
                {
                    return Conflict("Department Already Exits");
                }

                // Convert DTO object into Department entity object
                Department department = new Department()
                {
                    DepartmentName = dto.DepartmentName
                };

                // Add department object into Departments table
                db.Departments.Add(department);
                db.SaveChanges();

                return Ok("Department Added Succesfully");

            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("bulk")]
        public IActionResult AddDepartments(List<DepartmentDto> departments)
        {
            try
            {
                // Check whether department list is null or empty
                if (departments == null || departments.Count == 0)
                {
                    return BadRequest("Department List Cannot be Empty");
                }

                // Create new list to store valid departments
                List<Department> newDepartments = new List<Department>();

                // Loop through each department from request
                foreach (var dto in departments)
                {
                    // Check duplicate department in database
                    bool exists = db.Departments
                        .Any(d => d.DepartmentName.Trim().ToLower()
                               == dto.DepartmentName.Trim().ToLower());

                    // Add only non-duplicate departments
                    if (!exists)
                    {
                        newDepartments.Add(new Department()
                        {
                            DepartmentName = dto.DepartmentName
                        });
                    }
                }

                // Add multiple departments into databas
                db.Departments.AddRange(newDepartments);
                db.SaveChanges();
                return Ok("Departments Added Succesfully");
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, DepartmentDto dto)
        {
            try
            {
                // Check whether incoming model data is valid or not
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Find department by id
                var department = db.Departments
                    .FirstOrDefault(d => d.DepartmentId == id);

                // Check whether department exists or not
                if (department == null)
                {
                    return NotFound("Department Not Found");
                }

                // Check duplicate department name
                // Ignore current department id while checking duplicate
                bool exists = db.Departments
                    .Any(d => d.DepartmentName.Trim().ToLower()
                           == dto.DepartmentName.Trim().ToLower()
                           && d.DepartmentId != id);

                // If duplicate exists then return conflict response
                if (exists)
                {
                    return Conflict("Department Already Exists");
                }

                // Update department data
                department.DepartmentName = dto.DepartmentName;

                db.SaveChanges();

                // Return success response
                return Ok("Department Updated Successfully");
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            try
            {
                // Find department by id
                var department = db.Departments
                    .FirstOrDefault(d => d.DepartmentId == id);

                // Check whether department exists or not
                if (department == null)
                {
                    return NotFound("Department Not Found");
                }

                // Remove department from database
                db.Departments.Remove(department);

                // Save changes into database
                db.SaveChanges();

                // Return success response
                return Ok("Department Deleted Successfully");
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, ex.Message);
            }
        }


    }   
}

