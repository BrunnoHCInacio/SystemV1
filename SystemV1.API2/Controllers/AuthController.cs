using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels.Identity;
using SystemV1.Domain.Core.Interfaces.Services;

namespace SystemV1.API2.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(INotifier notifier,
                                 UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 IOptions<AppSettings> appSettings) : base(notifier)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterUser(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid)
            {
                return OkResult(ModelState);
            }

            var identityUser = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, registerUser.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Notify(error.Description);
                }
            }

            return OkResult(await GenerateToken(registerUser.Email));
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return OkResult(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, true);
            if (!result.Succeeded)
            {
                Notify("Usuário ou senha incorretos.");
            }

            return OkResult(await GenerateToken(loginViewModel.Email));
        }

        public async Task<string> GenerateToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}