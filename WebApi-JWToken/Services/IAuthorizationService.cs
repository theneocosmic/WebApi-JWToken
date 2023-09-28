using WebApi_JWToken.Models.Custom;

namespace WebApi_JWToken.Services
{
	public interface IAuthorizationService
	{
		Task<AuthorizationResponse> ReturnToken(AuthorizationRequest request);
		Task<AuthorizationResponse> ReturnRefreshToken(RefreshTokenRequest refreshTokenRequest, int userId);

	}
}
