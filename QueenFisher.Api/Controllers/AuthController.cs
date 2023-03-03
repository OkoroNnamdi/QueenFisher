using MailKit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueenFisher.Core.DTO;
using QueenFisher.Core.Interfaces.IServices;
using QueenFisher.Core.Utilities;
using System.Security.Claims;
using QueenFisher.Core.Interfaces;

namespace QueenFisher.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _mailService;
        

        public AuthController(IAuthService authService, IEmailService mailService)
        {
            _authService = authService;
            _mailService = mailService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO user)
        {
            var response = await _authService.Register(user);
            if (response.Succeeded)
            {
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { email = user.Email, token = response.Data }, Request.Scheme);
                var emailRequest = new EmailMessage(new string[] { user.Email }, "Email confirmation", $"use this link to confirm your email {confirmationLink}");
                await _mailService.SendEmailAsync(emailRequest);    
                return Ok(response.Message);
            }
            return BadRequest(response);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var login = await _authService.Login(model);
            if (login.Succeeded == false) return Unauthorized(login);
            return Ok(login);
        }
        [Authorize]
        [HttpGet("Refresh-Token")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = await _authService.RefreshToken();
            if (token.Succeeded == false) return BadRequest(token);
            return Ok(token);
        }
        [HttpPost("Change-Password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            var result = await _authService.ChangePassword(model);
            if (result.ToString().Contains("login")) return Unauthorized(result);
            if (result.ToString().Contains("character")) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var result = await _authService.ForgottenPassword(model);
            return Ok(result);
        }
        [HttpPost("Reset-Update-Password")]
        public async Task<IActionResult> ResetUpdatePassword([FromBody] UpdatePasswordDTO model)
        {
            var result = await _authService.ResetPassword(model);
            return Ok(result);
        }

        [HttpGet("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var response = await _authService.Confirmemail(email, token);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return Ok(response);
        }

        //[HttpPost("GoogleLogin")]
        //public async Task<IActionResult> GoogleLogin()
        //{
        //    await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
        //    {
        //        RedirectUri = Url.Action("GoogleResponse")
        //    });
        //    return Ok();
        //}
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action(nameof(GoogleResponse), "Auth", null, Request.Scheme, null)
            });

            return Ok();
        }


        //[HttpGet("GoogleResponse")]
        //public async Task<IActionResult> GoogleResponse()
        //{
        //    // var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        //    var claims = result.Principal.Identities
        //        .FirstOrDefault().Claims.Select(claims => new
        //        {
        //            claims.Issuer,
        //            claims.OriginalIssuer,
        //            claims.Type,
        //            claims.Value
        //        });
        //    return Ok(claims);
        //}

        [HttpGet("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
            {
                // Handle authentication failure
                return BadRequest();
            }

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;

            // Use the email to identify the user and create a new user account if necessary
            // ...

            return Ok($"The email {email} was signed in successfully ");
        }

        [HttpPut("signout")]
        public async Task<IActionResult> Signout()
        {
            await _authService.Signout();
            return Ok();
        }

    }
}
