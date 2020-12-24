using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoggingDemo.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger _logger;
        public ErrorController(ILogger<ErrorController> logger)
        {
            this._logger = logger;
        }
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var execptionDetails = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    _logger.LogWarning($"404 Error Occured. Path ={execptionDetails.OriginalPath}" +
                        $"and the query string ={execptionDetails.OriginalQueryString}");
                    break;
                default:
                    break;
            }
            return View();
        }
        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
          
            var execptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError($"The Execption path {execptionDetails.Path} threw an exception {execptionDetails.Error}");
            ViewBag.ExceptionPath = execptionDetails.Path;
            ViewBag.ExceptionMessage = execptionDetails.Error;
            ViewBag.StackTrace = execptionDetails.Error.StackTrace;
            return View("Errors");
        }
    }
}