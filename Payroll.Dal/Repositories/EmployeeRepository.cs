using Dapper;
using Payroll.Bll.Core.Interfaces;
using Payroll.Bll.Core.Models.Employee;
using Payroll.Dal.Interfaces;
using Payroll.Dal.TableParameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Dal.Repositories
{
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly IDbContext _dbContext;

        public EmployeeRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddEmployeesAsync(IEnumerable<EmployeeModel> employees)
        {
            var distinctEmployees = employees.Distinct();

            var tableParameter = new EmployeeTableType();
            var employeesTableData =  tableParameter.AddRows(distinctEmployees);

            string query = @"MERGE Employee AS tar
                            USING @employeeTable AS src
                                ON (tar.Id = src.Id)
                            WHEN NOT MATCHED BY TARGET THEN
                                INSERT INTO Employee(Id, JobGroup) VALUES (@src.Id, @src.JobGroup)";

            using (IDbConnection db = _dbContext.CreateConnection())
            {
                await db.ExecuteAsync(query, new { employeeTable = employeesTableData });
            }
        }
    }
}
