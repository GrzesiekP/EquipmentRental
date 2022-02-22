using EquipmentRental.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EquipmentRental.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserController(
            IUserService userService, 
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration)
        {
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("authenticate")]
        public IActionResult Authorize([FromBody] LoginData loginData)
        {
            var response = _userService.Authenticate(loginData);
            // w user service:
            // get user and autenticate
            //g get user roles
            // add claims 
            // add roles as claims
            // create new sumetric key with secret key from config
            // create new JWT token https://medium.com/c-sharp-progarmming/asp-net-core-5-jwt-authentication-tutorial-with-example-api-aa59e80d02da
            // return token

            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterUser registerData)
        {
            // check if user exists
            
            // create new user

            return Ok();
        }
    }
}