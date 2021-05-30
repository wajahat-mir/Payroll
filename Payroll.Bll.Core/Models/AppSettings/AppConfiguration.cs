using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Bll.Core.Models.AppSettings
{
    public class AppConfiguration
    {
        public ConnectionStringsSettings ConnectionStrings { get; set; }
        public IReadOnlyCollection<ApiKey> ApiKey { get; set; }

    }
}
