using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nxt.Common;
using Nxt.Common.Models;
using Nxt.Entities.Dtos.Account;
using Nxt.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Nxt.API.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JWTConfiguration _jwtConfiguration;

        public AccountController(IUserService userService, IOptions<JWTConfiguration> jwtConfiguration)
        {
            _userService = userService;
            _jwtConfiguration = jwtConfiguration.Value;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterInput model)
        {
            var result = await _userService.RegisterAsync(model);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync(TokenRequestInput model)
        {
            var result = await _userService.GetTokenAsync(model);
            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _userService.RefreshTokenAsync(refreshToken);
            if (!string.IsNullOrEmpty(response.RefreshToken))
                SetRefreshTokenInCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("send-password-reset-link")]
        public async Task<IActionResult> SendPasswordResetLinkAsync(string email)
        {
            var result = await _userService.SendPasswordResetLinkAsync(email);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsyc(string userId, string code)
        {
            var result = await _userService.ConfirmEmailAsyc(userId, code);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordInput input)
        {
            var result = await _userService.ResetPasswordAsync(input);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("send-email-verification-link")]
        public async Task<IActionResult> SendEmailVerificationLinkAsync(string email)
        {
            var result = await _userService.SendEmailVerificationLinkAsync(email);
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordInput input)
        {
            var result = await _userService.ChangePasswordAsync(input);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenInput model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = await _userService.RevokeToken(token);

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        [HttpPost("tokens/{id}")]
        public async Task<IActionResult> GetUserRefreshTokensAsync(string id)
        {
            var refreshTokens = await _userService.GetUserRefreshTokensAsync(id);
            return Ok(refreshTokens);
        }

        #region User Management

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("manage/user/{id}")]
        public async Task<IActionResult> GetAllUsersAsync(string id)
        {
            var result = await _userService.GetUserAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("manage/add-role")]
        public async Task<IActionResult> AddRoleAsync(RoleInput input)
        {
            var result = await _userService.AddRoleAsync(input);
            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("manage/remove-role")]
        public async Task<IActionResult> RemoveRoleAsync(RoleInput input)
        {
            var result = await _userService.RemoveRoleAsync(input);
            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("manage/user/{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, ApplicationUserInput input)
        {
            var result = await _userService.UpdateUserAsync(id, input);
            return Ok(result);
        }

        #endregion

        #region Private Methods

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(_jwtConfiguration.RefreshTokenExpiryDurationInDays),
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        #endregion
    }
}
