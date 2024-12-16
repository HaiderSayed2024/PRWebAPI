using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using PRWebAPI.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;



namespace PRWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UsersController : ControllerBase
    {
        private readonly PRContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;     

        public UsersController(PRContext dbContext, IConfiguration configuration, IEmailService emailservice)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _emailService = emailservice;          
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            if (_dbContext.tblUsers == null)
            {
                return BadRequest("User does not exists.");
            }
            return await _dbContext.tblUsers.ToListAsync();
        }

        [Authorize]
        [HttpGet("GetUsersByID")]
        public ActionResult<Users> GetUsersByID(int id)
        {
            var objUser = _dbContext.tblUsers.FirstOrDefault(x => x.ID == id);
            if (objUser == null)
            {
                return BadRequest("User does not exists.");
            }
            else
            {
                return Ok(objUser);
            }
        }

        [HttpPost("Registration")]
        public ActionResult<UserDTO> Registration(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var objUser = _dbContext.tblUsers.FirstOrDefault(x => x.Email == userDTO.Email);
            if (objUser == null)
            { 

                var user = new Users
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    Password = userDTO.Password,
                    UserType = userDTO.Usertype
                };
                _dbContext.tblUsers.Add(user);
                ////_dbContext.tblUsers.Add(new Users
                ////{
                ////    FirstName = userDTO.FirstName,
                ////    LastName = userDTO.LastName,
                ////    Email = userDTO.Email,
                ////    Password = userDTO.Password,
                ////    UserType = userDTO.Usertype
                ////});


              _dbContext.SaveChanges();
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", user.ID.ToString()),
                    new Claim("Email", user.Email.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var sigIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: sigIn
                    );
                string tokenvalue = new JwtSecurityTokenHandler().WriteToken(token);
                var confirmationLink = Url.Action("ConfirmEmail", "Users", new { userId = user.ID, token = tokenvalue }, Request.Scheme);
                bool emailResponse = SendEmail(user.Email, confirmationLink);
                if (emailResponse)                   
                return Ok("User Registered Successfully");

            }
            else
            {
                return BadRequest("User already exists with same Email Address.");
            }
            return Ok("User Registered Successfully"); 
        }



        [HttpGet("ConfirmEmail")]
        public  ActionResult ConfirmEmail(string token, string email)
        {
            if (token == null || email == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("Login")]
        public ActionResult<LoginDTO> Login(LoginDTO loginDTO)
        {
            var objUser = _dbContext.tblUsers.FirstOrDefault(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password);
            if (objUser != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", objUser.ID.ToString()),
                    new Claim("Email", objUser.Email.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var sigIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: sigIn
                    );
                string tokenvalue = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { Token = tokenvalue, objUser });
            }
            else
            {
                return BadRequest("User does not exists.");
            }
        }


        [NonAction]
        public bool SendEmail(string userEmail, string emlCnfrmtnLink)
        {
            try
            {
                Mailrequest mailrequest = new Mailrequest();
                //userEmail
                mailrequest.ToEmail = "myselfhaider2022@gmail.com";
                mailrequest.Subject = "Welcome to Raabta Application";
                // mailrequest.Body = "Thanks for Joining Us";
                mailrequest.Body = emlCnfrmtnLink;
                _emailService.SendEmailAsync(mailrequest);
                return true;
            }
            catch(Exception ex)   {
                throw;
            }
        }
    }
}
