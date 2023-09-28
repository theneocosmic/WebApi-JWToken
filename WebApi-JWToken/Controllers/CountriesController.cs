using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_JWToken.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountriesController : ControllerBase
	{
		/// <summary>
		/// This endpoint returns a list of countries. To use this endpoint you must provide the bearer token.
		/// </summary>
		/// <returns></returns>
		[Authorize]
		[HttpGet]
		[Route("List")]
		public async Task<IActionResult> List()
		{
			var lstCountries = await Task.FromResult(new List<string> { "France", "Mexico", "United State", "Marrakesh", "Italy", "Spain", "Belize", "China" });
			return Ok(lstCountries);
		}
	}
}
