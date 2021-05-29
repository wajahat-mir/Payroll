using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Dal.Interfaces
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string endpoint, IEnumerable<KeyValuePair<string, object>> parameters = null, IEnumerable<KeyValuePair<string, object>> headers = null);
        Task<T> PostAsync<T>(string endpoint, dynamic body, IEnumerable<KeyValuePair<string, object>> parameters = null, IEnumerable<KeyValuePair<string, object>> headers = null);
        Task<T> PutAsync<T>(string endpoint, dynamic body, IEnumerable<KeyValuePair<string, object>> parameters = null, IEnumerable<KeyValuePair<string, object>> headers = null);
        Task<T> DeleteAsync<T>(string endpoint);
    }

    public interface ILocationApiClient : IApiClient
    { }
}
