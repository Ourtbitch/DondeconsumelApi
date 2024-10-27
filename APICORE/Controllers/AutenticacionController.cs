using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using APICORE.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System.Text;
using System.Diagnostics.Eventing.Reader;


namespace APICORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly string secretkey;

        public AutenticacionController(IConfiguration config)
        {
            secretkey = config.GetSection("settings").GetSection("secretkey").ToString();
        }
        [HttpPost("validar")]
        public IActionResult Validar([FromBody] User request)
        {
            if(request.correo =="m@gmail.com" && request.clave == "123")
            {
                var keyBytes = Encoding.ASCII.GetBytes(secretkey);
                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.correo));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature),

                };

                var tokeHandler = new JwtSecurityTokenHandler();
                var tokenconfig = tokeHandler.CreateToken(tokenDescriptor);

                string tokencreado = tokeHandler.WriteToken(tokenconfig);

                return StatusCode(StatusCodes.Status200OK, new
                {
                    token = tokencreado
                });

                }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    token = ""
                });
            }
            
            
        }

    }
}
