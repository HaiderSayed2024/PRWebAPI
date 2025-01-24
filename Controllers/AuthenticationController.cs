using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRWebAPI.Models.Authentication.SignUp;
using PRWebAPI.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using PRWebAPI.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace PRWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly PRContext _dbContext;

        public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, IEmailService emailService, PRContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
            _dbContext = dbContext;
        }

        [NonAction]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Register registerUser, string role)
        {
            //check user exists
            var userExists = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExists != null)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "User already exists!" }
                    );
            }
            var user = new IdentityUser { UserName = registerUser.Email, Email = registerUser.Email };

            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(
                       StatusCodes.Status500InternalServerError,
                       new Response { Status = "Error", Message = "User Failed to be create!" }
                       );
                }
                // Add role
                await _userManager.AddToRoleAsync(user, role);

                //Add token to verify email
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, Email = user.Email }, Request.Scheme);
                bool emailResponse = SendEmail(user.Email, "Confirm your email", $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>link</a>");
                if (emailResponse)
                {
                    return StatusCode(
                        StatusCodes.Status200OK,
                        new Response { Status = "Success", Message = "User registered successfully!" }
                        );
                }
                else
                {
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "We are experiencing issues sending confirmation emails. Please try again later.!" }
                        );
                }
            }
            else
            {
                return StatusCode(
                               StatusCodes.Status500InternalServerError,
                               new Response { Status = "Error", Message = "Role does not exists!" }
                               );
            }
            //return BadRequest(result.Errors);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return StatusCode(
                               StatusCodes.Status500InternalServerError,
                               new Response { Status = "Error", Message = "Invalid token or user ID!" }
                               );
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(
                                    StatusCodes.Status200OK,
                                    new Response { Status = "Success", Message = "Email verified successfully!" }
                                     );
                }
            }
            return StatusCode(
                              StatusCodes.Status500InternalServerError,
                              new Response { Status = "Error", Message = "User does not exists!" }
                              );

        }

        [NonAction]
        public bool SendEmail(string userEmail, string subject, string link)
        {
            try
            {
                Mailrequest mailrequest = new Mailrequest();
                //userEmail
                mailrequest.ToEmail = userEmail;
                mailrequest.Subject = $"Welcome to Raabta Application -  {subject} ";
                // mailrequest.Body = "Thanks for Joining Us";
                mailrequest.Body = link;
                _emailService.SendEmailAsync(mailrequest);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            //check valid user
            var objUser = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (objUser != null && await _userManager.CheckPasswordAsync(objUser, loginDTO.Password))
            {


                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Email", objUser.Email.ToString())
                };

                var userRoles = await _userManager.GetRolesAsync(objUser);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                //generate the token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var sigIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    authClaims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: sigIn
                    );
                string tokenvalue = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { Token = tokenvalue, expiration = token.ValidTo, objUser });
            }
            else
            {
                return BadRequest("User does not exists.");
            }
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            //check user exists
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);
                bool emailResponse = SendEmail(email, "Forgot Password Link", $"Please reset your password by clicking this link: <a href='{forgotPasswordLink}'>link</a>");
                if (emailResponse)
                {
                    return StatusCode(
                        StatusCodes.Status200OK,
                        new Response { Status = "Success", Message = $"Password change request is sent on your email {user.Email}. Please open your email and click the link" }
                        );
                }
                else
                {
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "We are experiencing issues sending Password change request emails. Please try again later.!" }
                        );
                }
            }
            else
            {
                return StatusCode(
                               StatusCodes.Status500InternalServerError,
                               new Response { Status = "Error", Message = "User does not exists!" }
                               );
            }
        }

        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return Ok(new { model });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            //check user exists
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }
                return StatusCode(
                       StatusCodes.Status200OK,
                       new Response { Status = "Success", Message = "Password has changed!" }
                       );
            }

            else
            { 
            return StatusCode(
                           StatusCodes.Status400BadRequest,
                           new Response { Status = "Error", Message = "Could not able to sent the link, please try again later!" });
             }
        }
    }
}
