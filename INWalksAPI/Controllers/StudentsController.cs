using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace INWalksAPI.Controllers
{
    //https://localhost:port/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            return Ok();
        }

    }
}
