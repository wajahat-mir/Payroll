using Microsoft.Extensions.Options;
using Payroll.Bll.Core.Models.AppSettings;
using Payroll.Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Dal.Adaptors
{
    public class DbContext: IDbContext
    {
        private readonly IOptionsMonitor<AppConfiguration> _configuration;

        public DbContext(IOptionsMonitor<AppConfiguration> configuration)
        {
            _configuration = configuration;
        }
        public SqlConnection CreateConnection()
        {
            var connString = _configuration.CurrentValue.ConnectionStrings.DbConnectionString;
            return new SqlConnection(connString);
        }
    }
}
