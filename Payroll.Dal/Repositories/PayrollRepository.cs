using Dapper;
using Payroll.Bll.Core.Interfaces;
using Payroll.Bll.Core.Models.Payroll;
using Payroll.Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Dal.Repositories
{
    public class PayrollRepository: IPayrollRepository
    {
        private readonly IDbContext _dbContext;

        public PayrollRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertPayrollAsync(PayrollModel payroll, int reportId)
        {
            string query = @"INSERT INTO Pay(EmployeeId, ReportId, DateWorked, HoursWorked) VALUES (@EmployeeId, @ReportId, @DateWorked, @HoursWorked)";

            using (IDbConnection db = _dbContext.CreateConnection())
            {
                await db.ExecuteAsync(query, new
                {
                    EmployeeId = payroll.employeeId,
                    reportId,
                    DateWorked = payroll.date,
                    HoursWorked = payroll.hoursWorked
                });
            }
        }

        public async Task InsertPayrollsAsync(IEnumerable<PayrollModel> payrolls, int reportId)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("EmployeeId", typeof(int));
            table.Columns.Add("ReportId", typeof(int));
            table.Columns.Add("DateWorked", typeof(DateTime));
            table.Columns.Add("HoursWorked", typeof(float));

            foreach (var payroll in payrolls)
            {
                var row = table.NewRow();
                row["EmployeeId"] = payroll.employeeId;
                row["ReportId"] = reportId;
                row["DateWorked"] = payroll.date;
                row["HoursWorked"] = payroll.hoursWorked;
                table.Rows.Add(row);
            }

            table.AcceptChanges();

            using (SqlConnection connection = _dbContext.CreateConnection())
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "Pay";
                    bulkCopy.WriteToServer(table);
                }
            }
        }

        public async Task<IEnumerable<PayModel>> GetAllPaymentsAsync()
        {
            string query = @"SELECT p.EmployeeId, e.JobGroup, p.DateWorked as date, p.HoursWorked
                FROM Employee e 
                INNER JOIN Pay p
                ON e.Id = p.EmployeeId
                ORDER BY p.EmployeeId, DateWorked ASC";

            using (IDbConnection db = _dbContext.CreateConnection())
            {
                return await db.QueryAsync<PayModel>(query);
            }
        }
    }
}
