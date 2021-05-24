using Dapper;
using Payroll.Bll.Core.Interfaces;
using Payroll.Bll.Core.Models.Report;
using Payroll.Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Dal.Repositories
{
    public class ReportRepository: IReportRepository
    {
        private readonly IDbContext _dbContext;
        public ReportRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReportModel> GetReportByIdAsync(int reportId)
        {
            string query = @"SELECT Id FROM Report WHERE Id = @ReportId";

            using (IDbConnection db = _dbContext.CreateConnection())
            {
                db.Open();
                return await db.QueryFirstOrDefaultAsync<ReportModel>(query, new { reportId });
                
            }
        }

        public async Task CreateReportAsync(int reportId)
        {
            string query = @"INSERT INTO Report (Id) VALUES(@ReportId)";

            using (IDbConnection db = _dbContext.CreateConnection())
            {
                db.Open();
                await db.ExecuteAsync(query, new { reportId });
            }
        }
    }
}
