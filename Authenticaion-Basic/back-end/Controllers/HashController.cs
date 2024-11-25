using ApiWuthHush.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AuthApiApplication;
namespace ApiWuthHush.Controllers
{
    [Route("/")]
    [ApiController]
    public class HashController : ControllerBase
    {
        private readonly UsersServices usersServices;

        public HashController(UsersServices usersServices)
        {
            this.usersServices = usersServices;
        }

        [HttpGet("")]
        public string Runing()
        {
            return "server now runing insert swagger text in url to folow it";
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserModel userModel)
        {
            var users = await usersServices.Register(userModel.username, userModel.password);
            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserModel request)
        {
            var user = await usersServices.Login(request.username, request.password);
            if (user == null)
                return Unauthorized("Invalid username or password");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,request.username),
            };

            var identity = new ClaimsIdentity(claims);

            var princpal = new ClaimsPrincipal(identity);

            var token = Tokens.GenerateTokens(princpal.Claims.ToList());

            var cookieOption = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddMinutes(30),
                SameSite = SameSiteMode.None
            };

         

            Response.Cookies.Append("AuthToken",token,cookieOption);

            return Ok(new {Message = "you login success",Token = token});
        }

        [HttpGet(ApiEndPoints.Autentication.GetAuth)]
        [Authorize]
        public IActionResult GetAuth()
        {
            return Ok($"you authorize {User?.Identity?.Name}");
        }
        [HttpGet(ApiEndPoints.Autentication.Logout)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");

            return Ok("You Logout!");
        }

        [HttpGet(ApiEndPoints.Autentication.GetAllUsers)]
        public async Task <IActionResult> GetAllUsers()
        {
            var res = await usersServices.GetUsers();

            return Ok(new {Users = res});
        }
        [HttpGet(ApiEndPoints.Autentication.RemoveUser)]
        public async Task <IActionResult> RemoveUser([FromRoute]string username)
        {
           var success =  await usersServices.DeleteUsers(username);
            if (success)
                return Ok($"user {username} is remove");
            else return BadRequest("removed faild!");
        }
        [HttpGet("getusersnames")]
        public async Task<IActionResult> GetUsersByNames()
        {
            var users = await usersServices.GetUsersNames();

            return Ok(users);
        }

       



    }
    
}
