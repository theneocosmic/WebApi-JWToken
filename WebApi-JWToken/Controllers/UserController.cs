using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using WebApi_JWToken.Filter;
using WebApi_JWToken.Helpers;
using WebApi_JWToken.Models.Custom;
using WebApi_JWToken.Services;

namespace WebApi_JWToken.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[TypeFilter(typeof(ExceptionManagerFilter))]

	public class UserController : ControllerBase
	{
		private readonly IAuthorizationService _authorizationService;

		public UserController(IAuthorizationService authorizationService)
		{
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// This end point authenticate users and return Token and Refresh Token
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Authenticate")]
		public async Task<IActionResult> Authenticate([FromBody] AuthorizationRequest request)
		{
			var authorizationResult = await _authorizationService.ReturnToken(request);
			if (authorizationResult == null)
			{
				return Unauthorized();
			}

			return Ok(authorizationResult);
		}

		/// <summary>
		/// This endpoint returns an new Token based on refreshToken key, with out user credentials.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("GetRefreshToken")]
		public async Task<IActionResult> GetRefreshToken([FromBody] RefreshTokenRequest request)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var supposedExpiredToken = tokenHandler.ReadJwtToken(request.ExpiredToken);

			if (supposedExpiredToken.ValidTo > DateTime.UtcNow)
				return BadRequest(new AuthorizationResponse { Result = false, Message = "Token not expired yet." });


			string userId = supposedExpiredToken.Claims.First(x =>
			x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();

			var authorizationResponse = await _authorizationService.ReturnRefreshToken(request, int.Parse(userId));

			if (authorizationResponse.Result)
				return Ok(authorizationResponse);
			else
				return BadRequest(authorizationResponse);
		}
	}
}
