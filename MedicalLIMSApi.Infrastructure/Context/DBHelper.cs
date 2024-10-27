using MedicalLIMSApi.Core.Entities.SampleAnalysis;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace MedicalLIMSApi.Infrastructure.Context
{
    public class DBHelper
    {
        public void PrepareProcedure(DbCommand cmd, string procedure)
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedure;
        }

        public void AddInParameter<T>(DbCommand cmd, string parameterName, T parameterValue)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.Value = parameterValue;
            cmd.Parameters.Add(param);
        }

        public void AddStructuredInParameter(DbCommand cmd, string parameterName, InventoryPackIssueList parameterValue)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.Value = parameterValue;
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);
        }

        public void AddInOutParameter<T>(DbCommand cmd, string parameterName, T parameterValue, int size, DbType parameterType)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.Value = parameterValue;
            param.Direction = ParameterDirection.InputOutput;
            param.DbType = parameterType;
            param.Size = size;
            cmd.Parameters.Add(param);
        }

        public void AddOutParameter(DbCommand cmd, string parameterName, DbType parameterType, int size)
        {
            SqlParameter result = new SqlParameter();
            result.ParameterName = parameterName;
            result.Direction = ParameterDirection.Output;
            result.Size = size;
            result.DbType = parameterType;
            cmd.Parameters.Add(result);
        }

        public string GetOutputParameterValue(DbCommand cmd, string parameterName)
        {
            string result = string.Empty;
            result = cmd.Parameters[parameterName].Value.ToString();
            return result;
        }

        public DbCommand PrepareCommand(TrainingContext context)
        {
            CloseConnection(context);
            context.Database.Initialize(force: false);
            var cmd = context.Database.Connection.CreateCommand();
            context.Database.Connection.Open();
            return cmd;
        }

        public void CloseConnection(TrainingContext context)
        {
            context.Database.Connection.Close();
        }
    }
}
