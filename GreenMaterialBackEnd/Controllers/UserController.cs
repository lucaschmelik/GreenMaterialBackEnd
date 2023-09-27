using DB;
using GreenMaterialBackEnd.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenMaterialBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GreenMaterialContext _context;

        public UserController(GreenMaterialContext context) { _context = context; }

        [HttpGet]
        public ActionResult GetUsers()
        {
            try
            {
                return Ok(_context.users.ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetUsersById(int id)
        {
            try
            {
                return Ok(_context.users.FirstOrDefault(user => user.id == id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ByEmailPassword")]
        public ActionResult GetUserByEmailPassword(string email, string password)
        {
            var user = new UserResponse();

            try
            {
                var userFound = _context.users.FirstOrDefault(user => user.email == email && user.password == password);

                if (userFound != null)
                {
                    user.id = userFound.id;
                    user.email = email;
                    user.firstName = userFound.firstName;
                    user.lastName = userFound.lastName;
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult PostUser([FromBody] User user)
        {
            try
            {
                _context.users.Add(user);
                _context.SaveChanges();
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
