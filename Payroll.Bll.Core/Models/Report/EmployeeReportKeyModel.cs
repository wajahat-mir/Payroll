using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Bll.Core.Models.Report
{
    public class EmployeeReportKeyModel : IEquatable<EmployeeReportKeyModel>
    {
        public int EmployeeId { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }

        public override int GetHashCode()
        {
            return EmployeeId.GetHashCode() + startDate.GetHashCode() + endDate.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as EmployeeReportKeyModel);
        }
        public bool Equals(EmployeeReportKeyModel obj)
        {
            return (this.EmployeeId == obj.EmployeeId && String.Equals(this.startDate, obj.startDate) && String.Equals(this.endDate, obj.endDate));
        }
    }
}
