using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi_JWToken.Helpers;

namespace WebApi_JWToken.Filter
{
	public class ExceptionManagerFilter : IExceptionFilter
	{
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly IModelMetadataProvider _metadataProvider;

		public ExceptionManagerFilter(IWebHostEnvironment hostingEnvironment, IModelMetadataProvider metadataProvider)
		{
			_hostingEnvironment = hostingEnvironment;
			_metadataProvider = metadataProvider;
		}

		public void OnException(ExceptionContext context)
		{
			var exceptionType = context.Exception;

			context.Result = new JsonResult("Something be wrong! "
			 + _hostingEnvironment.ApplicationName
			+ "  Exception type: " + context.Exception.GetType()
			);
		}
	}
}
