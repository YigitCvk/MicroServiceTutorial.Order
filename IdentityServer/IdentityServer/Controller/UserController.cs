using IdentityServer.DTOs;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Shared.Dtos;
namespace IdentityServer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDto signup)
        {
            var user = new ApplicationUser { UserName = signup.UserName, Email = signup.Email, City = signup.City };

            var result =await _userManager.CreateAsync(user, signup.Password);

            if (!result.Succeeded)
            {
                return BadRequest(Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400));
            }
            return NoContent();
        }
        //[HttpGet]
        //public async Task<IActionResult> GetUser()
        //{
        //    var userIdClaim =  User.Claims.FirstOrDefault(x=>x.Type == Jwt);
        //}
    }
}
