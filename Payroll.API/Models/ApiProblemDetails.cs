using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.API.Models
{
    public class ApiProblemDetails : ProblemDetails
    {
        public ApiProblemDetails()
        {
            Title = "Internal Server Error - API";
            Detail = "An unexpected error occurred.";
            Status = 500;
            Type = "https://httpstatuses.com/500";
        }
    }
}
