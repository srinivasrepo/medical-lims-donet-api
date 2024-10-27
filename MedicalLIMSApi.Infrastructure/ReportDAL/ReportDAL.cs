using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Reports;
using MedicalLIMSApi.Core.Entities.UtilUploads;
using MedicalLIMSApi.Infrastructure.Context;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.ReportDAL
{
    public class ReportDAL
    {
        public static ReportsBO GetReportInfo(int requestID, string versionCode)
        {
            ReportsBO obj = new ReportsBO();
            obj.LogType = "RPT";
            ReportParametersBO parameterBO = null;
            ReportDataSourceTableNamesBO tableNamesBO = null;
            ReportFormatableColumnsBO formatableColumnsBO = null;

            SqlDatabase db = new SqlDatabase(DBInfo.GetInstance().ConnectionString);

            using (DbCommand cmd = db.GetStoredProcCommand("lims.uspRPTGetReportInfo"))
            {
                db.AddInParameter(cmd, "@EntActID", DbType.Int32, requestID);
                db.AddInParameter(cmd, "@VersionCode", DbType.String, versionCode);

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        obj.EntityCode = Convert.ToString(reader["ENTITY_CODE"]);
                        obj.FileName = Convert.ToString(reader["RPT_FILE_NAME"]);
                        obj.RequestCode = Convert.ToString(reader["REQ_CODE"]);
                        obj.ReportName = Convert.ToString(reader["REPORT_NAME"]);
                        obj.PlantCode = Convert.ToString(reader["PLANTCODE"]);
                        if (reader["PLANTID"] != DBNull.Value)
                            obj.PlantID = Convert.ToInt16(reader["PLANTID"]);
                        obj.ReqCodeForReport = Convert.ToString(reader["REPORT_CODE"]);
                        obj.PlantName = Convert.ToString(reader["PLANTNAME"]);
                        obj.EntityReportCode = Convert.ToString(reader["ENTITY_RPT_CODE"]);
                        obj.IsFinalStatusReached = Convert.ToBoolean(reader["IsFinalStatusReached"]);
                        obj.VersionNo = Convert.ToString(reader["VersionNo"]);
                    }
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            obj.RdlcName = Convert.ToString(reader["RDLC_NAME"]);
                            obj.DataSourceName = Convert.ToString(reader["DATASOURCE_NAME"]);
                            obj.StoredProcedure = Convert.ToString(reader["STORED_PROCEDURE_NAME"]);
                            obj.HasSubReport = reader["HAS_SUB_REPORT"] == DBNull.Value ? default(bool) : Convert.ToBoolean(reader["HAS_SUB_REPORT"]);
                            obj.IsCorporate = Convert.ToBoolean(reader["IS_CORPORATE"]);
                        }
                    }
                    if (reader.NextResult())
                    {
                        obj.ParametersList = new ReportParametersList();
                        while (reader.Read())
                        {
                            parameterBO = new ReportParametersBO();
                            parameterBO.ParameterName = Convert.ToString(reader["PARAMETER_NAME"]);
                            parameterBO.ParameterDirection = Convert.ToString(reader["PARAMETER_DIRECTION"]);
                            parameterBO.ParameterLength = reader["PARAMETER_LENGTH"] != DBNull.Value ? Convert.ToInt32(reader["PARAMETER_LENGTH"]) : default(int);
                            parameterBO.ParameterType = reader["PARAMETER_TYPE"] == DBNull.Value ? default(bool) : Convert.ToBoolean(reader["PARAMETER_TYPE"]);
                            if (reader["DATA_TYPE"] != DBNull.Value)
                                parameterBO.DataType = (SqlDbType)Enum.Parse(typeof(SqlDbType), Convert.ToString(reader["DATA_TYPE"]), true);
                            parameterBO.IsNullable = reader["IS_NULLABLE"] == DBNull.Value ? default(bool) : Convert.ToBoolean(reader["IS_NULLABLE"]);
                            parameterBO.DBColumnName = Convert.ToString(reader["COLUMN_NAME"]);
                            parameterBO.TableOrder = reader["TABLE_ORDER"] != DBNull.Value ? Convert.ToByte(reader["TABLE_ORDER"]) : default(byte);
                            parameterBO.Path = Convert.ToString(reader["IMAGE_PATH"]);
                            obj.ParametersList.Add(parameterBO);
                        }
                    }
                    if (reader.NextResult())
                    {
                        obj.TablesList = new ReportDataSourceTableNamesList();
                        while (reader.Read())
                        {
                            tableNamesBO = new ReportDataSourceTableNamesBO();
                            tableNamesBO.TableName = Convert.ToString(reader["TABLE_NAME"]);
                            tableNamesBO.TableOrder = reader["TABLE_ORDER"] == DBNull.Value ? default(byte) : Convert.ToByte(reader["TABLE_ORDER"]);
                            tableNamesBO.SourceType = Convert.ToString(reader["SOURCE_TYPE"]);
                            obj.TablesList.Add(tableNamesBO);
                        }
                    }
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            obj.VersionNumber = Convert.ToString(reader["VERSION_NUMBER"]);
                            obj.FormateNumber = Convert.ToString(reader["FORMAT_NUMBER"]);
                            obj.EffectiveDate = Convert.ToString(reader["EFFECTIVE_DATE"]);
                            obj.Title = Convert.ToString(reader["TITLE"]);
                        }
                    }
                    if (reader.NextResult())
                    {
                        obj.FormatableColumnsList = new ReportFormatableColumnsList();
                        while (reader.Read())
                        {
                            formatableColumnsBO = new ReportFormatableColumnsBO();
                            formatableColumnsBO.TableName = Convert.ToString(reader["TABLE_NAME"]);
                            formatableColumnsBO.ColumnName = Convert.ToString(reader["COLUMN_NAME"]);
                            if (reader["DATA_TYPE"] != DBNull.Value)
                                formatableColumnsBO.DataType = (SqlDbType)Enum.Parse(typeof(SqlDbType), Convert.ToString(reader["DATA_TYPE"]), true);
                            obj.FormatableColumnsList.Add(formatableColumnsBO);
                        }
                    }
                }
            }
            return obj;
        }

        public static DataSet GetReportContent(ReportsBO bo, int requestID, int ardsExecID, short year, byte month)
        {
            DataSet ds = null;
            int i = default(int);
            SqlDatabase db = new SqlDatabase(DBInfo.GetInstance().ConnectionString);
            using (DbCommand cmd = db.GetStoredProcCommand(bo.StoredProcedure))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (requestID > default(int))
                    db.AddInParameter(cmd, "@ID", DbType.Int32, requestID);
                if (ardsExecID > default(int))
                    db.AddInParameter(cmd, "@ArdsExecID", DbType.Int32, ardsExecID);
                if (year > default(short))
                    db.AddInParameter(cmd, "@Year", DbType.Int16, year);
                if (month > default(byte))
                    db.AddInParameter(cmd, "@Month", DbType.Byte, month);

                if (bo.ParametersList != null)
                {
                    foreach (ReportParametersBO parameter in bo.ParametersList)
                    {
                        if (parameter.ParameterDirection.ToUpper() == "IN")
                            AddInParameter(db, cmd, parameter);
                        if (parameter.ParameterDirection.ToUpper() == "OUT")
                            db.AddOutParameter(cmd, "@" + parameter.ParameterName, parameter.DataType, parameter.ParameterLength);
                    }
                }
                ds = db.ExecuteDataSet(cmd);
                foreach (ReportDataSourceTableNamesBO tblName in bo.TablesList)
                {
                    if (i < ds.Tables.Count)
                    {
                        ds.Tables[i].TableName = tblName.TableName;
                        i++;
                    }
                }
                foreach (ReportParametersBO parameter in bo.ParametersList)
                {
                    if (parameter.ParameterDirection.ToUpper() == "OUT" && parameter.ParameterType)
                        ReadOutParameter(db, cmd, parameter);
                    else if (parameter.ParameterDirection.ToUpper() == "RPT")
                    {
                        if (ds.Tables[parameter.TableOrder].Rows.Count > 0)
                            AssignParameterValue(parameter, ds.Tables[parameter.TableOrder].Rows[0][parameter.DBColumnName]);
                        else
                            AssignParameterValue(parameter, null);
                    }
                }

            }
            return ds;
        }

        internal static void AssignParameterValue(ReportParametersBO parameter, object value)
        {
            parameter.IsNullValue = value == DBNull.Value ? true : false;
            switch (parameter.DataType)
            {
                case SqlDbType.Int:
                case SqlDbType.TinyInt:
                case SqlDbType.SmallInt:
                    parameter.ParameterValue = value != DBNull.Value ? Convert.ToInt32(value) : default(int);
                    break;
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                    parameter.ParameterValue = value != DBNull.Value ? Convert.ToDateTime(value) : DateTime.MinValue;
                    break;
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                    parameter.ParameterValue = value != DBNull.Value ? Convert.ToDecimal(value) : default(decimal);
                    break;
                case SqlDbType.Bit:
                    parameter.ParameterValue = value != DBNull.Value ? Convert.ToBoolean(value) : default(bool);
                    break;
                default:
                    parameter.ParameterValue = Convert.ToString(value);
                    break;
            }
        }
        //TO READ OUTPUT PARAMETERS AND ASSIGN VALUES  DEPENDING ON THEIR DATATYPES
        internal static void ReadOutParameter(SqlDatabase db, DbCommand cmd, ReportParametersBO parameter)
        {
            switch (parameter.DataType)
            {
                case SqlDbType.Int:
                case SqlDbType.TinyInt:
                case SqlDbType.SmallInt:
                    parameter.ParameterValue = (db.GetParameterValue(cmd, "@" + parameter.ParameterName) != DBNull.Value) ? Convert.ToInt32(db.GetParameterValue(cmd, "@" + parameter.ParameterName)) : default(int);
                    break;
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                    parameter.ParameterValue = (db.GetParameterValue(cmd, "@" + parameter.ParameterName) != DBNull.Value) ? Convert.ToDateTime(db.GetParameterValue(cmd, "@" + parameter.ParameterName)) : default(DateTime);
                    break;
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                    parameter.ParameterValue = (db.GetParameterValue(cmd, "@" + parameter.ParameterName) != DBNull.Value) ? Convert.ToDecimal(db.GetParameterValue(cmd, "@" + parameter.ParameterName)) : default(decimal);
                    break;
                case SqlDbType.Bit:
                    parameter.ParameterValue = (db.GetParameterValue(cmd, "@" + parameter.ParameterName) != DBNull.Value) ? Convert.ToBoolean(db.GetParameterValue(cmd, "@" + parameter.ParameterName)) : default(bool);
                    break;
                default:
                    parameter.ParameterValue = Convert.ToString(db.GetParameterValue(cmd, "@" + parameter.ParameterName));
                    break;
            }
        }
        //TO ADD INPUT PARAMETERS DEPENDING ON THEIR DATATYPES
        internal static void AddInParameter(SqlDatabase db, DbCommand cmd, ReportParametersBO parameter)
        {
            switch (parameter.DataType)
            {
                case SqlDbType.Int:
                case SqlDbType.TinyInt:
                case SqlDbType.SmallInt:
                    if (!parameter.IsNullable || Convert.ToInt32(parameter.ParameterValue) > default(int))
                        db.AddInParameter(cmd, "@" + parameter.ParameterName, parameter.DataType, parameter.ParameterValue);
                    break;
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                    if (!parameter.IsNullable || Convert.ToDateTime(parameter.ParameterValue) > default(DateTime))
                        db.AddInParameter(cmd, "@" + parameter.ParameterName, parameter.DataType, parameter.ParameterValue);
                    break;
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                    if (!parameter.IsNullable || Convert.ToDecimal(parameter.ParameterValue) > default(decimal))
                        db.AddInParameter(cmd, "@" + parameter.ParameterName, parameter.DataType, parameter.ParameterValue);
                    break;
                case SqlDbType.Bit:
                    if (!parameter.IsNullable || !string.IsNullOrEmpty(Convert.ToString(parameter.ParameterValue)))
                        db.AddInParameter(cmd, "@" + parameter.ParameterName, parameter.DataType, parameter.ParameterValue);
                    break;
                default:
                    if (!parameter.IsNullable || !string.IsNullOrEmpty(Convert.ToString(parameter.ParameterValue)))
                        db.AddInParameter(cmd, "@" + parameter.ParameterName, parameter.DataType, parameter.ParameterValue);
                    break;
            }
        }

        //TO SAVE REPORT INFO IN LOG 
        public void SaveReportInfoInLog(int requestID, string entityRptCode, string fileName, string rptGenMode, string requestCode, short plantID)
        {
            SqlDatabase db = new SqlDatabase(DBInfo.GetInstance().ConnectionString);

            using (DbCommand cmd = db.GetStoredProcCommand("uspRPTSaveRptInfoInLog"))
            {
                db.AddInParameter(cmd, "@ENT_ACT_ID", DbType.Int32, requestID);
                db.AddInParameter(cmd, "@ENTITY_RPTCODE", DbType.String, entityRptCode);
                db.AddInParameter(cmd, "@FILENAME", DbType.String, fileName);
                db.AddInParameter(cmd, "@GENERATED_FROM", DbType.String, rptGenMode);
                db.AddInParameter(cmd, "@REQ_CODE", DbType.String, requestCode);
                db.AddInParameter(cmd, "@PLANTID", DbType.Int16, plantID);

                db.ExecuteNonQuery(cmd);
            }
        }

        public static ReportsBO GetLogReportInfo(string entityRptCode, short plantID, string extraRptCode)
        {
            ReportsBO obj = new ReportsBO();
            obj.LogType = "LOG";
            ReportParametersBO parameterBO = null;
            ReportDataSourceTableNamesBO tableNamesBO = null;
            ReportFormatableColumnsBO formatableColumnsBO = null;

            SqlDatabase db = new SqlDatabase(DBInfo.GetInstance().ConnectionString);

            using (DbCommand cmd = db.GetStoredProcCommand("uspRPTGetLogReportInfo"))
            {
                db.AddInParameter(cmd, "@ENTITY_RPTCODE", DbType.String, entityRptCode);
                db.AddInParameter(cmd, "@PLANT_ID", DbType.Int16, plantID);
                if (!string.IsNullOrEmpty(extraRptCode))
                    db.AddInParameter(cmd, "@EXT_CODE", DbType.String, extraRptCode);
                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        obj.EntityCode = Convert.ToString(reader["ENTITY_CODE"]);
                        obj.RdlcName = Convert.ToString(reader["RDLC_NAME"]);
                        obj.DataSourceName = Convert.ToString(reader["DATASOURCE_NAME"]);
                        obj.StoredProcedure = Convert.ToString(reader["STORED_PROCEDURE_NAME"]);
                        obj.HasSubReport = reader["HAS_SUB_REPORT"] == DBNull.Value ? default(bool) : Convert.ToBoolean(reader["HAS_SUB_REPORT"]);
                        obj.IsCorporate = Convert.ToBoolean(reader["IS_CORPORATE"]);
                    }

                    if (reader.NextResult())
                    {
                        obj.ParametersList = new ReportParametersList();
                        while (reader.Read())
                        {
                            parameterBO = new ReportParametersBO();
                            parameterBO.ParameterName = Convert.ToString(reader["PARAMETER_NAME"]);
                            parameterBO.ParameterDirection = Convert.ToString(reader["PARAMETER_DIRECTION"]);
                            parameterBO.ParameterLength = reader["PARAMETER_LENGTH"] != DBNull.Value ? Convert.ToInt32(reader["PARAMETER_LENGTH"]) : default(int);
                            parameterBO.ParameterType = reader["PARAMETER_TYPE"] == DBNull.Value ? default(bool) : Convert.ToBoolean(reader["PARAMETER_TYPE"]);
                            if (reader["DATA_TYPE"] != DBNull.Value)
                                parameterBO.DataType = (SqlDbType)Enum.Parse(typeof(SqlDbType), Convert.ToString(reader["DATA_TYPE"]), true);
                            parameterBO.IsNullable = reader["IS_NULLABLE"] == DBNull.Value ? default(bool) : Convert.ToBoolean(reader["IS_NULLABLE"]);
                            parameterBO.DBColumnName = Convert.ToString(reader["COLUMN_NAME"]);
                            parameterBO.TableOrder = reader["TABLE_ORDER"] != DBNull.Value ? Convert.ToByte(reader["TABLE_ORDER"]) : default(byte);
                            obj.ParametersList.Add(parameterBO);
                        }
                    }
                    if (reader.NextResult())
                    {
                        obj.TablesList = new ReportDataSourceTableNamesList();
                        while (reader.Read())
                        {
                            tableNamesBO = new ReportDataSourceTableNamesBO();
                            tableNamesBO.TableName = Convert.ToString(reader["TABLE_NAME"]);
                            tableNamesBO.TableOrder = reader["TABLE_ORDER"] == DBNull.Value ? default(byte) : Convert.ToByte(reader["TABLE_ORDER"]);
                            tableNamesBO.SourceType = Convert.ToString(reader["SOURCE_TYPE"]);
                            obj.TablesList.Add(tableNamesBO);
                        }
                    }
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            obj.FormateNumber = Convert.ToString(reader["FORMAT_NUMBER"]);
                            obj.VersionNumber = Convert.ToString(reader["VERSION_NUMBER"]);
                            obj.EffectiveDate = Convert.ToString(reader["EFFECTIVE_DATE"]);
                            obj.Title = Convert.ToString(reader["TITLE"]);
                        }
                    }
                    if (reader.NextResult())
                    {
                        obj.FormatableColumnsList = new ReportFormatableColumnsList();
                        while (reader.Read())
                        {
                            formatableColumnsBO = new ReportFormatableColumnsBO();
                            formatableColumnsBO.TableName = Convert.ToString(reader["TABLE_NAME"]);
                            formatableColumnsBO.ColumnName = Convert.ToString(reader["COLUMN_NAME"]);
                            if (reader["DATA_TYPE"] != DBNull.Value)
                                formatableColumnsBO.DataType = (SqlDbType)Enum.Parse(typeof(SqlDbType), Convert.ToString(reader["DATA_TYPE"]), true);
                            obj.FormatableColumnsList.Add(formatableColumnsBO);
                        }
                    }
                }
            }
            return obj;
        }

        public static string UploadReportDMSIDs(string dmsXml, TransResults tr)
        {
            TrainingContext context = new TrainingContext();
            DBHelper ctx = new DBHelper();
            string retCode = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspUpdateDMSIDinReportContainer");
            ctx.AddInParameter<string>(cmd, "@DmsXml", dmsXml);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@RetMsg", DbType.String, 25);
            cmd.ExecuteNonQuery();
            retCode = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retCode;
        }

        public static ReportDetails GetCalibrationReportDetails(ArdsBO obj, TransResults tr)
        {
            TrainingContext context = new TrainingContext();
            DBHelper ctx = new DBHelper();
            ReportDetails data = new ReportDetails();
            int? reportID = default(int);

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspGetCalibrationReportDetails");

            ctx.AddInParameter<int>(cmd, "@ArdsExecID", obj.ArdsExecID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            if (obj.InvalidationID > default(int))
                ctx.AddInParameter<int?>(cmd, "@InvalidationID", obj.InvalidationID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrID = ((IObjectContextAdapter)context).ObjectContext.Translate<int?>(reader);
                foreach (var rr in rrID)
                    reportID = rr;

                reader.NextResult();

                var rrData = ((IObjectContextAdapter)context).ObjectContext.Translate<ReportDetails>(reader);
                foreach (var rr in rrData)
                    data = rr;
                data.ReportID = reportID;

                data.PlaceHolders = new GetARDSPlaceholderData();

                reader.NextResult();
                data.PlaceHolders.PlaceholderList = new PlaceholderList();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<PlaceholderData>(reader);
                foreach (var rr in rrLst)
                    data.PlaceHolders.PlaceholderList.Add(rr);

                reader.NextResult();

                if (obj.EntityCode == "QCSAMPASYS" || obj.EntityCode == "SPEC_VALID" || obj.EntityCode == "OOSPROC")
                {
                    var rrVal = ((IObjectContextAdapter)context).ObjectContext.Translate<SampleAnalysisPlaceholderValues>(reader);
                    data.PlaceHolders.SampleAnalysisValues = new SampleAnalysisPlaceholderValues();
                    foreach (var rr in rrVal)
                        data.PlaceHolders.SampleAnalysisValues = rr;
                }
                else if (obj.EntityCode == "ENGGMNT" || obj.EntityCode == "CALIB_VALIDATION")
                {
                    var rrVal = ((IObjectContextAdapter)context).ObjectContext.Translate<CalibrationPlaceholderValues>(reader);
                    data.PlaceHolders.CalibrationValues = new CalibrationPlaceholderValues();
                    foreach (var rr in rrVal)
                        data.PlaceHolders.CalibrationValues = rr;
                }
                reader.NextResult();

                var rrChe = ((IObjectContextAdapter)context).ObjectContext.Translate<ChemicalBO>(reader);
                data.ChemicalList = new ChemicalList();
                foreach (var rr in rrChe)
                    data.ChemicalList.Add(rr);
                reader.NextResult();

                var rrRefChe = ((IObjectContextAdapter)context).ObjectContext.Translate<ChemicalBO>(reader);
                data.RefChemicalList = new ChemicalList();
                foreach (var rr in rrRefChe)
                    data.RefChemicalList.Add(rr);

                reader.NextResult();
                var rrTable = ((IObjectContextAdapter)context).ObjectContext.Translate<TablePlaceHolders>(reader);
                data.TablePlaceholders = new TablePlaceholdersList();
                foreach (var rr in rrTable)
                    data.TablePlaceholders.Add(rr);

                //reader.NextResult();

                //var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ResultSetBO>(reader);
                //data.ResultSets = new ResultSetList();
                //foreach (var rr in rrRes)
                //    data.ResultSets.Add(rr);

                //reader.NextResult();

                //var rrPre = ((IObjectContextAdapter)context).ObjectContext.Translate<PreparationBO>(reader);
                //data.Preparations = new PreparationList();
                //foreach (var rr in rrPre)
                //    data.Preparations.Add(rr);

                reader.NextResult();

                DataTable dt = new DataTable();
                dt.Load(reader);
                data.ImpurityData = dt;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return data;
        }

        public static string UpdateArdsPlaceholders(UpdateARDSPlaceholder obj, TransResults tr)
        {
            TrainingContext context = new TrainingContext();
            DBHelper ctx = new DBHelper();
            string retCode = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspUpdateARDSPlaceholderValues");

            ctx.AddInParameter<string>(cmd, "@PlaceholderXml", obj.PlaceholderXml);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntActID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@RetCode", System.Data.DbType.String, 25);
            if (!string.IsNullOrEmpty(obj.Type))
                ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            cmd.ExecuteNonQuery();
            retCode = Convert.ToString(ctx.GetOutputParameterValue(cmd, "@RetCode"));
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retCode;
        }
    }
}
