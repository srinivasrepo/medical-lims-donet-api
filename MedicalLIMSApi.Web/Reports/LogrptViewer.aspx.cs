using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Reports;
using MedicalLIMSApi.Infrastructure.ReportDAL;
using MedicalLIMSApi.Web.Reports.AppCode;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MedicalLIMSApi.Web.Reports
{
    public partial class LogrptViewer : CommonMethods
    {
        CommonStaticMethods objEncrypt = new CommonStaticMethods();
        string entityRptCode = string.Empty;
        string extraRptCode = string.Empty;
        string rptFileName = string.Empty;
        DataSet dsSubReport = null;
        ReportsBO rptInfo = null;
        int userID = default(int);
        byte plantID = default(byte);
        short year = default(short);
        byte month = default(byte);
        string reportLogo = "UserFiles/srinivas_life_img.drawio.svg";

        protected void Page_Load(object sender, EventArgs e)
        {
            //TO GET THE ID AND CHECK IF IT IS A VALID REQUEST
            SetInputValues();

            if (string.IsNullOrEmpty(entityRptCode))
            {
                ltErr.Text = "Invalid report values; unable to generate report";
                return;
            }

            //GET THE REPORT CONFIGURATION
            rptInfo = ReportDAL.GetLogReportInfo(entityRptCode, 1, extraRptCode);
            rvRequestReport.LocalReport.DataSources.Clear();
            List<ReportParameter> lprm = new List<ReportParameter>();
            rvRequestReport.LocalReport.DataSources.Clear();
            rvRequestReport.LocalReport.EnableExternalImages = true;

            //READING OF QUERYSTRING KEYS AND ASSIGNING VALUES TO DB PARAMETERS
            GetReportConfiguration();

            if (rptInfo == null)
                return;

            //GETTING THE DATASOURCES
            DataSet ds = AppDateTimeUtil.SetDSDateFormat(ReportDAL.GetReportContent(rptInfo, default(int), default(int), year, month), AppDateTimeUtil.FormatTypes.DateWithTime, rptInfo);


            BindReport(ds, lprm);

            //BIND REPORT BASIC PARAMETERS
            BindReportBasicInfo(lprm);

            //GENERATE PDF
            GeneratePDF();

            rvRequestReport.LocalReport.Refresh();
        }

        //READING OF QUERYSTRING KEYS
        private void GetReportConfiguration()
        {
            var plantBO = rptInfo.ParametersList.FirstOrDefault(x => (x.ParameterName == "PlantID" || x.ParameterName == "PLANT_ID") && x.ParameterDirection == "IN");
            if (plantBO != null)
                plantBO.ParameterValue = plantID;

            var userBO = rptInfo.ParametersList.FirstOrDefault(x => x.ParameterName == "UserID" && x.ParameterDirection == "IN");
            if (userBO != null)
                userBO.ParameterValue = userID;

            foreach (var key in Request.QueryString.AllKeys)
            {
                ReportParametersBO bo = rptInfo.ParametersList.FirstOrDefault(x => x.ParameterName == key && x.ParameterDirection == "IN");
                if (bo != null) { AssignParameterValue(bo, GetParameterValue(key)); };
            }
        }


        private string GetParameterValue(string key)
        {
            string value = string.Empty;
            try
            {
                value = objEncrypt.decryptQueryString(Request.QueryString[key]);
            }
            catch
            {
                value = Request.QueryString[key];
            }
            return value;
        }

        //ASSIGN QUERYSTRING VALUES TO DB PARAMETERS 
        private void AssignParameterValue(ReportParametersBO bo, string value)
        {
            switch (bo.DataType)
            {
                case SqlDbType.Int:
                case SqlDbType.TinyInt:
                case SqlDbType.SmallInt:
                    bo.ParameterValue = ConvertToInt(value);
                    break;
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                    bo.ParameterValue = AppDateTimeUtil.ConvertToDateTimeFromString(value);
                    break;
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                    bo.ParameterValue = ConvertToDecimal(value);
                    break;
                case SqlDbType.Bit:
                    bo.ParameterValue = ConvertToBoolean(value);
                    break;
                default:
                    bo.ParameterValue = value.ToString();
                    break;
            }
        }

        //ASSIGNING DATASOURCE TO REPORT
        private void BindReport(DataSet ds, List<ReportParameter> lprm)
        {
            if (rptInfo.HasSubReport)
                rvRequestReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessingResults);

            string rpt = string.Format("~/Reports/ReportRdlcs/{0}/{1}", rptInfo.EntityCode, rptInfo.RdlcName);

            rvRequestReport.ProcessingMode = ProcessingMode.Local;

            rvRequestReport.LocalReport.ReportPath = Server.MapPath(rpt);
            if (ds != null)
            {
                int tableCount = 0;

                if (rptInfo.HasSubReport)
                    dsSubReport = new DataSet();

                foreach (DataTable dt in ds.Tables)
                {
                    if (dt != null)
                    {
                        if (rptInfo.TablesList.Exists(x => x.TableOrder == tableCount && (x.SourceType != "SUB" && x.SourceType != "PRM")))
                            rvRequestReport.LocalReport.DataSources.Add(new ReportDataSource(string.Format("{0}{1}", rptInfo.DataSourceName, dt.TableName), dt));
                        if (rptInfo.TablesList.Exists(x => x.TableOrder == tableCount && (x.SourceType != "MAIN" && x.SourceType != "PRM")))
                            dsSubReport.Tables.Add(dt.Copy());
                    }
                    tableCount = tableCount + 1;
                }
            }

            foreach (ReportParametersBO parameter in rptInfo.ParametersList)
            {
                if (parameter.ParameterDirection.ToUpper() == "RPT" || (parameter.ParameterDirection.ToUpper() == "OUT"))
                    AssignParametersToReport(lprm, parameter);
            }
        }

        //ASSIGN PARAMETERS TO REPORT DEPENDING ON THEIR DATATYPE
        private void AssignParametersToReport(List<ReportParameter> lprm, ReportParametersBO parameter)
        {
            switch (parameter.DataType)
            {
                case SqlDbType.Int:
                case SqlDbType.TinyInt:
                case SqlDbType.SmallInt:
                    lprm.Add(new ReportParameter(parameter.ParameterName, parameter.ParameterValue.ToString()));
                    break;
                case SqlDbType.Date:
                    lprm.Add(new ReportParameter(parameter.ParameterName, AppDateTimeUtil.SetDateFormat(parameter.ParameterValue)));
                    break;
                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                    lprm.Add(new ReportParameter(parameter.ParameterName, AppDateTimeUtil.SetDateFormatWithTime(parameter.ParameterValue)));
                    break;
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                    lprm.Add(new ReportParameter(parameter.ParameterName, Convert.ToString(ConvertToDecimal(parameter.ParameterValue))));
                    break;
                case SqlDbType.Bit:
                    lprm.Add(new ReportParameter(parameter.ParameterName, parameter.ParameterValue.ToString()));
                    break;
                default:
                    lprm.Add(new ReportParameter(parameter.ParameterName, parameter.ParameterValue.ToString()));
                    break;
            }
        }

        void LocalReport_SubreportProcessingResults(object sender, SubreportProcessingEventArgs e)
        {
            foreach (DataTable dt in dsSubReport.Tables)
            {
                e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(string.Format("{0}{1}", rptInfo.DataSourceName, dt.TableName), dt));
            }
        }


        #region Common Methods

        //GET THE ID AND REPORT TYPES FROM DATABASE
        void SetInputValues()
        {
            if (Request.QueryString["entityRptCode"] != null)
                entityRptCode = Request.QueryString["entityRptCode"];
            if (Request.QueryString["ex"] != null)
                extraRptCode = Request.QueryString["ex"];
            if (Request.QueryString["year"] != null)
                year = Convert.ToInt16(Request.QueryString["year"]);
            if (Request.QueryString["month"] != null)
                month = Convert.ToByte(Request.QueryString["month"]);
            if (Request.QueryString["token"] != null)
            {
                string encToken = Request.QueryString["token"];
                encToken = encToken.Substring(1, (encToken.Length - 2));
                CommonStaticMethods objEncrypt = new CommonStaticMethods();
                string[] token = objEncrypt.decryptQueryString(encToken).Split('|');
                int.TryParse(token[0], out userID);
                byte.TryParse(token[1], out plantID);

                //if (requestID == default(int))
                //    int.TryParse(Request.QueryString["id"], out requestID);
            }
        }

        //BIND BASIC PARAMETERS TO REPORT
        void BindReportBasicInfo(List<ReportParameter> lprm)
        {
            string path = "";
            string AppPath = Request.ApplicationPath;

            if (AppPath == "/")
                path = Request.Url.AbsoluteUri.Replace(Request.RawUrl, AppPath + reportLogo);
            else
                path = Request.Url.AbsoluteUri.Replace(Request.RawUrl, AppPath + "/" + reportLogo);


            if (string.IsNullOrEmpty(path))
                path = "Need to work";

            lprm.Add(new ReportParameter("prmImagePath", path));
            lprm.Add(new ReportParameter("prmFootNote", "This is an auto generated and electronically signed Document / Record"));
            lprm.Add(new ReportParameter("prmUnitName", rptInfo.IsCorporate ? "Corporate" : Convert.ToString(rptInfo.PlantName)));
            //lprm.Add(new ReportParameter("prmPrintedBy", Convert.ToString(Session[SessionVariables.USER_NAME])));
            //lprm.Add(new ReportParameter("prmPrintedOn", AppDateTimeUtil.SetDateFormatWithTime(DateTime.Now)));

            lprm.Add(new ReportParameter("prmCompanyName", System.Configuration.ConfigurationManager.AppSettings["companyname"]));
            lprm.Add(new ReportParameter("prmFormatNo", rptInfo.FormateNumber));
            lprm.Add(new ReportParameter("prmVersionNo", string.IsNullOrEmpty(rptInfo.VersionNumber) ? string.Empty : rptInfo.VersionNumber));
            lprm.Add(new ReportParameter("prmEffectiveDate", string.IsNullOrEmpty(rptInfo.EffectiveDate) ? string.Empty : rptInfo.EffectiveDate));
            lprm.Add(new ReportParameter("prmHeading", string.IsNullOrEmpty(rptInfo.Title) ? string.Empty : rptInfo.Title));

            try
            {
                rvRequestReport.LocalReport.SetParameters(lprm);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //GENERATE PDF
        void GeneratePDF()
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] ReportBytes = this.rvRequestReport.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            rptFileName = rptInfo.ReportName;
            WriteResponse(ReportBytes);
        }

        //TO DOWNLOAD PDF TO FILE SYSTEM
        void WriteResponse(byte[] buffer)
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", buffer.Length.ToString());
            Response.AddHeader("content-disposition", string.Format(@"inline; filename=""{0}""", string.Format(rptInfo.Title + "{0}", ".pdf")));
            Response.BinaryWrite(buffer);
            Response.Flush();
            Response.End();
        }

        //string FormatTestResult(object resultFrom, object resultTo, object methodType, object symbolCode, object UTFValue, object unitSymbol)
        //{
        //    string result = string.Empty;
        //    string symbol = Convert.ToString(symbolCode).ToUpper();
        //    int UTF = default(int);

        //    if (!string.IsNullOrEmpty(symbol))
        //    {
        //        int.TryParse(Convert.ToString(UTFValue), out UTF);
        //        if (UTF < 1)
        //            throw new ApplicationException("UTF Value should not be empty for code " + symbol);

        //        if (symbol == "CENTIGRADE")
        //            symbol = char.ConvertFromUtf32(UTF) + "C";
        //        else if (symbol == "FAHRENHEIT")
        //            symbol = char.ConvertFromUtf32(UTF) + "F";
        //        else if (symbol == "INDEGREES")
        //            symbol = "in" + char.ConvertFromUtf32(UTF);
        //        else if (symbol == "KGSQURE")
        //            symbol = "kg/cm" + char.ConvertFromUtf32(UTF);
        //        else if (symbol == "GMSQURE")
        //            symbol = "gm/cm" + char.ConvertFromUtf32(UTF);
        //        else if (symbol == "CMSQURE")
        //            symbol = "cm" + char.ConvertFromUtf32(UTF);
        //        else
        //            symbol = char.ConvertFromUtf32(UTF);
        //    }
        //    else if (!string.IsNullOrEmpty(Convert.ToString(unitSymbol)))
        //        symbol = Convert.ToString(unitSymbol);

        //    if (!string.IsNullOrEmpty(Convert.ToString(resultTo)))
        //        result = string.Format("{0} {1} to {2} {1}", resultFrom, symbol, resultTo);
        //    else if (!string.IsNullOrEmpty(Convert.ToString(resultFrom)))
        //        result = string.Format("{0} {1}", resultFrom, symbol);

        //    return result;
        //}

        //DataSet GetCustomizeDataSet(DataSet ds, string entityRptCode)
        //{
        //    if (entityRptCode == "MATIN_LOG")
        //    {
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            if (dr["SPECIFIC_GRAVITY_QTY"] != DBNull.Value)
        //            {
        //                string qty = string.Format("{0} {1}", Common.CommonMethods.ConvertToFriendlyDecimal(dr["ITEM_QUANTITY"]), dr["MAT_UOM"]);
        //                dr["ITEM_QUANTITY"] = qty;
        //                string specificGravityUOM = dr["MAT_UOM_CODE"].ToString() == "KG" ? "Lt" : "Kg";
        //                dr["MAT_UOM"] = "(" + dr["SPECIFIC_GRAVITY_QTY"] + " " + specificGravityUOM + ")";
        //            }

        //            if (Convert.ToString(dr["AR_NUM"]) != "N / A")
        //                dr["AR_NUM"] = MEDICALLIMSBAL.Generics.AppGenerics.GetCommaSplittedARNumber(Convert.ToString(dr["AR_NUM"]), MEDICALLIMSBAL.Generics.AppGenerics.LinkOrTest.Test);
        //        }
        //    }
        //    else if (entityRptCode == "STOCK_BALANCE")
        //    {
        //        string Condition = string.Empty;
        //        decimal val = default(decimal);

        //        if (ds != null)
        //        {
        //            ds.Tables[0].Columns.Add("TOTAL_QTY");
        //            ds.Tables[0].Columns.Add("APP_QTY");
        //            ds.Tables[0].Columns.Add("REJ_QTY");
        //            ds.Tables[0].Columns.Add("QRNTYN_QTY");

        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                int matID = default(int);
        //                int.TryParse(dr["MATID"].ToString(), out matID);

        //                var Result = ds.Tables[1].AsEnumerable().Where(r => r.Field<Int32>("MATID") == matID)
        //                        .Sum(x => Convert.ToDecimal(x["BAL_QTY"]));

        //                if (TryTruncateDecimal(Result.ToString(), out val))
        //                    dr["TOTAL_QTY"] = val;
        //                else
        //                    dr["TOTAL_QTY"] = 0;

        //                Result = ds.Tables[1].AsEnumerable().Where(r => r.Field<Int32>("MATID") == matID)
        //                        .Where(r => r.Field<string>("STATUS_CODE") == "APP")
        //                        .Sum(x => Convert.ToDecimal(x["BAL_QTY"]));

        //                if (TryTruncateDecimal(Result, out val))
        //                    dr["APP_QTY"] = val;
        //                else
        //                    dr["APP_QTY"] = 0;

        //                Result = ds.Tables[1].AsEnumerable().Where(r => r.Field<Int32>("MATID") == matID)
        //                        .Where(r => r.Field<string>("STATUS_CODE") == "REJ")
        //                        .Sum(x => Convert.ToDecimal(x["BAL_QTY"]));

        //                if (TryTruncateDecimal(Result, out val))
        //                    dr["REJ_QTY"] = val;
        //                else
        //                    dr["REJ_QTY"] = 0;

        //                Result = ds.Tables[1].AsEnumerable().Where(r => r.Field<Int32>("MATID") == matID)
        //                        .Where(r => r.Field<string>("STATUS_CODE") != "REJ")
        //                        .Where(r => r.Field<string>("STATUS_CODE") != "APP")
        //                        .Sum(x => Convert.ToDecimal(x["BAL_QTY"]));

        //                if (TryTruncateDecimal(Result, out val))
        //                    dr["QRNTYN_QTY"] = val;
        //                else
        //                    dr["QRNTYN_QTY"] = 0;
        //            }
        //        }
        //    }
        //    return ds;
        //}

        #endregion
    }
}