using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Payroll.Dal.Interfaces
{
    public interface IDbContext
    {
        SqlConnection CreateConnection();
    }
}
