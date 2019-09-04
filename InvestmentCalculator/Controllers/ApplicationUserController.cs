using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InvestmentCalculator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InvestmentCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;

        }

        [HttpPost]
        [Route("Register")]
        //Post: /api/ApplicationUser/Register
        public async Task<Object> PostApplicationUser(ApplicationUserModel model)
        {

            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                CompanyName = model.CompanyName
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("Login")]
        //POST : /api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                //Get role assigned to the user
                // var role = await _userManager.GetRolesAsync(user);
                //IdentityOptions _options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString()),
                       // new Claim(_options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }

    

    [HttpGet]
    [Authorize]
    [Route("GetUserProfile")]
    //GET : /api/UserProfile
        public async Task<Object> GetUserProfile()
    {
        string userId = User.Claims.First(c => c.Type == "UserID").Value;
        var user = await _userManager.FindByIdAsync(userId);
        return new
        {
            user.CompanyName,
            user.Email,
            user.UserName
        };
    }


        [HttpGet]
        [Route("ListUsers")]
        public IActionResult GetListUsers()
        {
            List<ApplicationUserModel> users = new List<ApplicationUserModel>();
            foreach (var user in _userManager.Users)
            {
                users.Add(new ApplicationUserModel
                {
                    CompanyName = user.CompanyName,
                    Email = user.Email,
                    UserName = user.UserName
                });

            }
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUserByID/{id}")]
        public async Task<IActionResult> GetUserByID(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                return Ok(new
                {
                    user.CompanyName,
                    user.Email,
                    user.UserName
                });
            }

            return NotFound();
        }

        [HttpGet]
        [Route("GetUserByName/{username}")]
        public async Task<IActionResult> GetUserByName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(new
                {
                    user.CompanyName,
                    user.Email,
                    user.UserName
                });
            }

            return NotFound();

        }


        [HttpGet]
        [Route("EditUser/{id}")]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ApplicationUserModel updatedUser;

            if (user != null)
            {
                updatedUser = new ApplicationUserModel
                {
                    ID = user.Id,
                    CompanyName = user.CompanyName,
                    Email = user.Email,
                    UserName = user.UserName
                };

                return Ok(updatedUser);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("EditUser")]
        public async Task<IActionResult> UpdateUser(ApplicationUserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.ID);

            if (user != null)
            {
                user.CompanyName = model.CompanyName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(user);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return BadRequest(ModelState);


                }

            }

            return NotFound();
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("ForAdmin")]
        public string GetForAdmin()
        {
            return "Web method for Admin";
        }


    }



}