using Payroll.Dal.Interfaces;
using Reviews.Dal.Adaptors;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Payroll.Dal.Adaptors
{
    public class LocationApiClient : ApiClient, ILocationApiClient
    {

        public LocationApiClient(HttpClient httpClient) : base(httpClient)
        {
        }
    }
}
