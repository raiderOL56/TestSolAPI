using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using TestSolAPI.Helper;
using TestSolAPI.Models;
using TestSolAPI.Models.ViewModels;

namespace TestSolAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettins;
        public AuthController(JwtSettings jwtSettings)
        {
            _jwtSettins = jwtSettings;
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        public IActionResult GetToken(AccountUser accountUser)
        {
            try
            {
                User user = JwtHelper.GenerateToken(new User()
                {
                    Username = accountUser.Username,
                    Id = 1,
                    GuidId = Guid.NewGuid()
                }, _jwtSettins);

                return Ok(user);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        public string SaludarAdmin()
        {
            return "Hola, administrador";
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, User")]
        public string SaludarUsuario()
        {
            return "Hola, usuario";
        }
    }
}