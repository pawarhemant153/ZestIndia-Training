using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_in_.NET.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetEmployees(
            [FromHeader(Name = "Authorization")]
            string authHeader)
        {

            return Ok(new List<string>
            {
                "Hemant",
                "Rahul",
                "Amit"
            });
        }
    }
}
