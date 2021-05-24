using Payroll.Bll.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Payroll.Dal.TableParameters
{
    public class EmployeeTableType
    {
        DataTable _table;
        public EmployeeTableType()
        {
            _table = new DataTable("EmployeeTableType");
            BuildTable();
        }
        private void BuildTable()
        {
            _table.Columns.Add("Id", typeof(int));
            _table.Columns.Add("JobGroup", typeof(string));
        }
        public TableValuedParameter AddRows(IEnumerable<EmployeeModel> employees)
        {
            foreach (var employee in employees)
            {
                var row = _table.NewRow();
                row["Id"] = employee.Id;
                row["JobGroup"] = employee.JobGroup;
                _table.Rows.Add(row);
            }
            _table.AcceptChanges();

            return new TableValuedParameter(_table);
        }
    }
}
