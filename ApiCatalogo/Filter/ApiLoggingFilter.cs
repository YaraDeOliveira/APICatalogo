﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Filter
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;
        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }
        // executa antes da Action
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("");
        }
        // executa depois da Action
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("");
        }
    }
}
