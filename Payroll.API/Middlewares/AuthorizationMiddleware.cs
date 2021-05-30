using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Payroll.API.Common;
using Payroll.API.Models;
using Payroll.Bll.Core.Models.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Payroll.API.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly IEnumerable<ApiKey> _appKey;
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next, IOptionsMonitor<AppConfiguration> configuration)
        {
            _next = next;
            _appKey = configuration.CurrentValue.ApiKey;

        }

        public async Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;
            if (_appKey != null)
            {
                //ignore the header key check if the request is the swagger page
                if (!context.Request.Path.ToString().ToLower().Contains("swagger"))
                {
                    if (headers.Keys.Contains(ApiKeyConstants.HeaderName))
                    {
                        ApiKey apiCaller = _appKey
                            .Where(x => x.Key == headers[ApiKeyConstants.HeaderName])
                            .FirstOrDefault();

                        if (apiCaller == null)
                        {
                            await WriteUnauthorizationAsync(context);
                            return;
                        }
                    }
                    else
                    {
                        await WriteUnauthorizationAsync(context);
                        return;
                    }
                }
            }

            await _next.Invoke(context);
        }

        private async Task WriteUnauthorizationAsync(HttpContext context)
        {
            context.Response.StatusCode = 403;
            var problemDetails = new UnauthorizedProblemDetails();
            await context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
        }

        private bool CheckMatching(string path, string key)
        {
            switch (key)
            {
                case "support":
                    Regex rgx = new Regex(@"^\/api\/v1\/accounts\/.*\/reviews\/.*$");
                    return rgx.IsMatch(path);

                default:
                    return true;
            }
        }
    }
}
