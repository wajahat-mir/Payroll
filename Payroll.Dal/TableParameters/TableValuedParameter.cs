using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Payroll.Dal.TableParameters
{
    public class TableValuedParameter : SqlMapper.ICustomQueryParameter
    {
        private DataTable _dataTable;
        private string _typeName;

        public TableValuedParameter(DataTable dataTable)
        {
            _typeName = dataTable.TableName;
            _dataTable = dataTable;
        }

        public void AddParameter(IDbCommand command, string name)
        {
            var parameter = (SqlParameter)command.CreateParameter();
            parameter.ParameterName = name;
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.Value = _dataTable;
            parameter.TypeName = _dataTable.TableName;

            command.Parameters.Add(parameter);
        }

        public int RowCount()
        {
            return _dataTable.Rows.Count;
        }
    }
}

