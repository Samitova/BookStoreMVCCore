using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs.Attributes;

namespace BookStore.Web.Controllers
{    
    public class ErrorController : Controller
    {
        private readonly ILogger _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>(); 
            switch (statusCode)
            { 
                case 404:
                    ViewBag.ErrorMessage = "Sorry the resource you requested could not be found";
                    _logger.LogWarning($"404 Error Occured. Path is {statusCodeResult.OriginalPath} and Quary string is {statusCodeResult.OriginalQueryString}.");
                    break;
            }
            return View("NotFound");
        }

        [Route("Error")]
        [AllowAnonymous]        
        public IActionResult Error() 
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError($"The path is {exceptionDetails.Path} threw an exception {exceptionDetails.Error}");          
            return View("Error");
        }
    }
}
