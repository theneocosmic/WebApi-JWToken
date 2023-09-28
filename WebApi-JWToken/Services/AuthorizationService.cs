using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi_JWToken.Helpers;
using WebApi_JWToken.Models;
using WebApi_JWToken.Models.Custom;

namespace WebApi_JWToken.Services
{
	public class AuthorizationService : IAuthorizationService
	{
		private readonly DbTestContext _context;
		private readonly IConfiguration _configuration;

		public AuthorizationService(DbTestContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}


		/// <summary>
		/// Este metodo crea el Token Principal
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		private string GenerateToken(string userId)
		{
			try
			{
				var key = _configuration.GetValue<string>("JwtSettings:key");
				var keyBytes = Encoding.ASCII.GetBytes(key);
				var claims = new ClaimsIdentity();
				claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));

				var credencialsToken = new SigningCredentials(
					new SymmetricSecurityKey(keyBytes),
					SecurityAlgorithms.HmacSha256Signature
					);

				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = claims,
					Expires = DateTime.UtcNow.AddMinutes(1),
					SigningCredentials = credencialsToken
				};

				var tokenHandler = new JwtSecurityTokenHandler();
				var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

				string createdToken = tokenHandler.WriteToken(tokenConfig);
				return createdToken;
			}
			catch (Exception e)
			{

				throw new Exception(e.Message);
			}
		}

		/// <summary>
		/// Este metodo crea el resfresh Token
		/// </summary>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		private string GenerateRefreshToken()
		{
			try
			{
				var byteArray = new byte[64];
				var refreshToken = "";

				using (var rng = RandomNumberGenerator.Create())
				{
					rng.GetBytes(byteArray);
					refreshToken = Convert.ToBase64String(byteArray);
				}

				return refreshToken;

			}
			catch (Exception e)
			{

				throw new Exception(e.Message);
			}
		}

		private async Task<AuthorizationResponse> SaveHistorialRefreshToken(
			int userId,
			string token,
			string refreshToken
			)
		{
			var historialRefreshToken = new HistorialRefreshToken
			{
				UserId = userId,
				Token = token,
				RefreshToken = refreshToken,
				CreationDate = DateTime.UtcNow,
				ExpirationDate = DateTime.UtcNow.AddMinutes(2)
			};

			await _context.HistorialRefreshTokens.AddAsync(historialRefreshToken);
			await _context.SaveChangesAsync();

			return new AuthorizationResponse
			{
				Token = token,
				RefreshToken = refreshToken,
				Result = true,
				Message = "Ok"
			};
		}

		public async Task<AuthorizationResponse> ReturnToken(AuthorizationRequest request)
		{
			var foundUser = _context.Users.FirstOrDefault(x =>
			x.Username == request.Username &&
			x.Password == request.Password);

			if (foundUser == null)
			{
				return await Task.FromResult<AuthorizationResponse>(null);
			}

			string createdToken = GenerateToken(foundUser.Username.ToString());
			string refreshToken = GenerateRefreshToken();

			//return new AuthorizationResponse() { Token = createdToken, RefreshToken = refreshToken, Result = true, Message = "Ok" };
			return await SaveHistorialRefreshToken(foundUser.UserId, createdToken, refreshToken);

		}

		public async Task<AuthorizationResponse> ReturnRefreshToken(RefreshTokenRequest refreshTokenRequest, int userId)
		{
			var foundRefreshToken = _context.HistorialRefreshTokens.FirstOrDefault(x =>
			x.Token == refreshTokenRequest.ExpiredToken &&
			x.RefreshToken == refreshTokenRequest.RefreshToken &&
			x.UserId == userId
			);

			if (foundRefreshToken == null)
				return new AuthorizationResponse { Result = false, Message = "RefreshToken not exist" };

			var createdRefreshToken = GenerateRefreshToken();
			var createdToken = GenerateToken(userId.ToString());

			return await SaveHistorialRefreshToken(userId, createdToken, createdRefreshToken);
		}
	}
}
