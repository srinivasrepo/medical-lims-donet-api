using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Reports;
using MedicalLIMSApi.Infrastructure.ReportDAL;
using MedicalLIMSApi.Web.Reports.AppCode;
using MedicalLIMSApi.Web.Utilities;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;

namespace MedicalLIMSApi.Web.Reports
{
    public partial class rptViewer : CommonMethods
    {
        string versionCode = string.Empty;
        int requestID = default(int);
        int userID = default(int);
        int ardsExecID = default(int);
        int dmsID = default(int);
        string[] token = null;
        string rptFileName = string.Empty;
        string fileTitle = string.Empty;
        string rptXSD = string.Empty;
        string reportLogo = "UserFiles/srinivas_life_img.drawio.svg";
        DataSet ds = new DataSet();
        string retVal = string.Empty;
        string waterMark = string.Empty;
        string hasInvalidationRecods = string.Empty;
        DataSet dsSubReport = null;
        ReportsBO rptInfo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //TO GET THE ID AND CHECK IF IT IS A VALID REQUEST
            SetInputValues();

            if (string.IsNullOrEmpty(versionCode))
            {
                ltErr.Text = "Invalid report values; unable to generate report";
                return ;
            }

            rptInfo = ReportDAL.GetReportInfo(requestID, versionCode);

            List<ReportParameter> lprm = new List<ReportParameter>();
            rvRequestReport.LocalReport.DataSources.Clear();
            rvRequestReport.LocalReport.EnableExternalImages = true;
            rvRequestReport.ProcessingMode = ProcessingMode.Local;

            GetReportConfiguration();

            DataSet ds = AppDateTimeUtil.SetDSDateFormat(ReportDAL.GetReportContent(rptInfo, requestID, ardsExecID, default(short), default(byte)), AppDateTimeUtil.FormatTypes.DateWithTime, rptInfo);

            if (rptInfo.EntityReportCode == "ARDS_VOL_SOLUTION")
            {
                waterMark = Convert.ToString(ds.Tables["tblSolutionBasicInfo"].Rows[0]["WaterMark"]);
                hasInvalidationRecods = Convert.ToString(ds.Tables["tblSolutionBasicInfo"].Rows[0]["HasInvalidRecods"]);
            }

            else if (rptInfo.EntityReportCode == "SAMPINOUT_ATR" || rptInfo.EntityReportCode == "ENGGMT_ATR" || rptInfo.EntityReportCode == "OOS_LIMS")
                ds = GetCustomizeDataSet(ds, rptInfo.EntityReportCode);
            else if (rptInfo.EntityReportCode == "SPECVALIDATION")
                ds = BuidSpecTests(ds);

            BindReport(ds, lprm);

            //BIND REPORT BASIC PARAMETERS
            BindReportBasicInfo(lprm);

            try
            {

                GeneratePDF(ds);

            }
            catch (Exception ex) { }
 
        }

        private void GetReportConfiguration()
        {
            foreach (ReportParametersBO bo in rptInfo.ParametersList)
            {

                if (bo.ParameterName == "UserID" && bo.ParameterType && bo.ParameterDirection.ToUpper() == "IN")
                    bo.ParameterValue = userID;

                foreach (var key in Request.QueryString.AllKeys)
                {
                    if (bo.ParameterName == key && bo.ParameterType && bo.ParameterDirection.ToUpper() == "IN")
                    {
                        string value = string.Empty;
                        try
                        {
                            value = Request.QueryString[key];
                        }
                        catch
                        {
                            ltErr.Text = "Invalid report values; unable to generate report";
                            rptInfo = null;
                            return;
                        }
                        AssignParameterValue(bo, value);
                    }
                }
            }
        }

        private void BindReport(DataSet ds, List<ReportParameter> lprm, string path = "")
        {
            string rpt = string.Empty;
            if (rptInfo.HasSubReport)
                rvRequestReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessingResults);

            if (string.IsNullOrEmpty(path))
                rpt = string.Format("~/Reports/ReportRdlcs/{0}/{1}", rptInfo.EntityCode, rptInfo.RdlcName);
            else rpt = path;

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
                        if (rptInfo.TablesList.Exists(x => x.TableOrder == tableCount && (x.SourceType == "MAIN" || x.SourceType == "BOTH")))
                            rvRequestReport.LocalReport.DataSources.Add(new ReportDataSource(string.Format("{0}{1}", rptInfo.DataSourceName, dt.TableName), dt));
                        if (rptInfo.TablesList.Exists(x => x.TableOrder == tableCount && (x.SourceType == "SUB" || x.SourceType == "BOTH")))
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

            rvRequestReport.LocalReport.SetParameters(lprm);
        }

        private void AssignParametersToReport(List<ReportParameter> lprm, ReportParametersBO parameter)
        {
            switch (parameter.DataType)
            {
                case SqlDbType.Int:
                case SqlDbType.TinyInt:
                case SqlDbType.SmallInt:
                    lprm.Add(new ReportParameter(parameter.ParameterName, Convert.ToString(parameter.ParameterValue)));
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
                    lprm.Add(new ReportParameter(parameter.ParameterName, Convert.ToString(parameter.ParameterValue)));
                    break;
                case SqlDbType.Image:
                    lprm.Add(new ReportParameter(parameter.ParameterName, new Uri(Server.MapPath(parameter.Path + Convert.ToString(parameter.ParameterValue))).AbsoluteUri));
                    break;
                default:
                    lprm.Add(new ReportParameter(parameter.ParameterName, Convert.ToString(parameter.ParameterValue)));
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
                    bo.ParameterValue = ConvertTodateTime(value);
                    break;
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                    bo.ParameterValue = ConvertToDecimal(value);
                    break;
                case SqlDbType.Bit:
                    bo.ParameterValue = ConvertToBoolean(value);
                    break;
                default:
                    bo.ParameterValue = Convert.ToString(value);
                    break;
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
                path = "need to work";

            lprm.Add(new ReportParameter("prmImagePath", path));
            lprm.Add(new ReportParameter("prmFootNote", "This is an auto generated and electronically signed Document / Record"));
            //lprm.Add(new ReportParameter("prmUnitName", rptInfo.IsCorporate ? "Corporate" : Convert.ToString(Session[SessionVariables.PLANTNAME])));
            //lprm.Add(new ReportParameter("prmPrintedBy", Convert.ToString(Session[SessionVariables.USER_NAME])));
            //lprm.Add(new ReportParameter("prmPrintedOn", AppDateTimeUtil.SetDateFormatWithTime(DateTime.Now)));

            lprm.Add(new ReportParameter("prmCompanyName", System.Configuration.ConfigurationManager.AppSettings["companyname"]));
            lprm.Add(new ReportParameter("prmFormatNo", rptInfo.FormateNumber));
            lprm.Add(new ReportParameter("prmVersionNo", string.IsNullOrEmpty(rptInfo.VersionNumber) ? string.Empty : rptInfo.VersionNumber));
            lprm.Add(new ReportParameter("prmEffectiveDate", string.IsNullOrEmpty(rptInfo.EffectiveDate) ? string.Empty : rptInfo.EffectiveDate));
            lprm.Add(new ReportParameter("prmHeading", string.IsNullOrEmpty(rptInfo.Title) ? string.Empty : rptInfo.Title));

            rvRequestReport.LocalReport.SetParameters(lprm);
        }

        //GENERATE PDF
        void GeneratePDF(DataSet ds)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] ReportBytes = this.rvRequestReport.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            if (rptInfo.IsFinalStatusReached == true && dmsID > default(int) && rptInfo.EntityReportCode != "CHEMICAL_USAGE_LOG" && rptInfo.EntityReportCode != "QC_INV_DISTRIBUTION")
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();
                dmsObj.EntActID = requestID;
                dmsObj.EntityCode = rptInfo.EntityCode;
                dmsObj.DmsID = dmsID;
                dmsObj.Buffer = ReportBytes;
                dmsObj.AppCode = "MEDICAL_LIMS";
                dmsObj.LoginID = Convert.ToString(token[11]);
                dmsObj.PlantCode = Convert.ToString(token[6]);
                dmsObj.DeptCode = Convert.ToString(token[3]);
                dmsObj.RoleName = Convert.ToString(token[12]);
                dmsObj.WaterMark = waterMark;
                if (hasInvalidationRecods == "YES")
                    dmsObj.InvalidBuffer = GenerateVolInvalidationReport(ds);

                retVal = FileUploadUtility.UploadReportToDMS(dmsObj);
            }
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

        //GET THE ID AND REPORT TYPES FROM DATABASE
        void SetInputValues()
        {

            if (Request.QueryString["requestType"] != null)
                versionCode = Request.QueryString["requestType"];

            if (Request.QueryString["id"] != null)
            {
                requestID = ReadQS<int>("id");

                if (requestID == default(int))
                    int.TryParse(Request.QueryString["id"], out requestID);
            }

            if (Request.QueryString["ardsExecID"] != null)
            {
                ardsExecID = ReadQS<int>("ardsExecID");

                if (ardsExecID == default(int))
                    int.TryParse(Request.QueryString["ardsExecID"], out ardsExecID);
            }

            if (Request.QueryString["dmsID"] != null)
            {
                dmsID = ReadQS<int>("dmsID");

                if (dmsID == default(int))
                    int.TryParse(Request.QueryString["dmsID"], out dmsID);
            }

            if (Request.QueryString["token"] != null)
            {
                string encToken = Request.QueryString["token"];
                encToken = encToken.Substring(1, (encToken.Length - 2));
                CommonStaticMethods objEncrypt = new CommonStaticMethods();
                token = objEncrypt.decryptQueryString(encToken).Split('|');
                int.TryParse(token[0], out userID);
                //byte.TryParse(token[1], out plantID);

                //if (requestID == default(int))
                //    int.TryParse(Request.QueryString["id"], out requestID);
            }

        }

        DataSet GetCustomizeDataSet(DataSet ds, string entityRptCode)
        {
            if (entityRptCode == "SAMPINOUT_ATR" || entityRptCode  == "ENGGMT_ATR")
                ds = BuidATRTests(ds);
            else if (entityRptCode == "OOS_LIMS")
            {
                if (ds.Tables[0].Columns.Contains("RESULT_TO"))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["RESULT"] = FormatTestResult(dr["RESULT"], dr["RESULT_TO"], dr["TEST_METHOD_TYPE"],
                                                dr["SYMBOL_CODE"], dr["UTF_VALUE"], dr["UNIT_SYMBOL"]);
                    }

                    foreach (DataRow dr in ds.Tables[3].Rows)
                    {
                        dr["RESULT"] = FormatTestResult(dr["RESULT"], dr["RESULT_TO"], dr["TEST_METHOD_TYPE"],
                                                dr["SYMBOL_CODE"], dr["UTF_VALUE"], dr["UNIT_SYMBOL"]);
                    }
                }
            }
            return ds;
        }

        DataSet BuidATRTests(DataSet ds)
        {
            DataTable dtATR = new DataTable("tblATRResult");
            List<int> lstProcessed = new List<int>();
            List<int> lstsub = new List<int>();
            dtATR.Columns.Add("SRNO");
            dtATR.Columns.Add("TEST_CATEGORYID");
            dtATR.Columns.Add("TEST");
            dtATR.Columns.Add("RESULT");
            dtATR.Columns.Add("SPEC_DESC");
            dtATR.Columns.Add("TEST_SUBCATID");
            dtATR.Columns.Add("IS_CATEGORY");

            int categoryID = default(int);
            int subCateID = default(int);
            int srno = default(int);
            int substrno = default(int);
            int subsr = default(int);
            int specTestID = default(int);
            string catName = string.Empty;
            string subCatName = string.Empty;
            int rrtID = default(int);

            DataView cat = ds.Tables["tblATRResult"].DefaultView;
            cat.RowFilter = " TEST_CATEGORYID is not null";

            if (cat.ToTable().Rows.Count > 0)
            {
                foreach (DataRow pr in ds.Tables["tblATRResult"].Rows)
                {
                    categoryID = 0;
                    int.TryParse(Convert.ToString(pr["TEST_CATEGORYID"]), out categoryID);
                    int.TryParse(Convert.ToString(pr["RRT_ID"]), out rrtID);
                    catName = Convert.ToString(pr["CATEGORY"]);
                    if (categoryID == 0 && rrtID == 0)
                    {
                        int.TryParse(Convert.ToString(pr["SPEC_TEST_ID"]), out specTestID);
                        DataRow val = dtATR.NewRow();
                        val["SRNO"] = string.Format("{0}", srno + 1);
                        val["TEST"] = pr["TEST"];

                        val["RESULT"] = FormatTestResult(pr["FINAL_RESULT"], pr["FINAL_RESULT_TO"], pr["TEST_METHOD_TYPE"],
                                pr["SYMBOL_CODE"], pr["UTF_VALUE"], pr["UNIT_SYMBOL"], pr["CorrValue"], pr["CorrectedType"]);

                        val["SPEC_DESC"] = pr["SPEC_DESC"];
                        val["IS_CATEGORY"] = "0";
                        dtATR.Rows.Add(val);
                        srno++;
                        DataView drv = ds.Tables["tblATRResult"].DefaultView;
                        drv.RowFilter = string.Format(" RRT_ID IS NOT NULL AND SPEC_TEST_ID={0}", specTestID);
                        drv.Sort = "RRT_ID ASC";
                        subsr = 0;
                        foreach (DataRow dls in drv.ToTable().Rows)
                        {
                            subsr++;
                            DataRow drSubR = dtATR.NewRow();
                            drSubR["SRNO"] = string.Format("{0}.{1}", srno, subsr);
                            drSubR["TEST"] = dls["TEST"];
                            drSubR["RESULT"] = FormatTestResult(dls["FINAL_RESULT"], dls["FINAL_RESULT_TO"], dls["TEST_METHOD_TYPE"],
                                dls["SYMBOL_CODE"], dls["UTF_VALUE"], dls["UNIT_SYMBOL"], dls["CorrValue"], dls["CorrectedType"]);

                            drSubR["SPEC_DESC"] = dls["SPEC_DESC"];
                            drSubR["IS_CATEGORY"] = "0";
                            dtATR.Rows.Add(drSubR);
                        }
                    }
                    else if (rrtID == 0)
                    {
                        if (!lstProcessed.Exists(delegate (int match) { return (match == categoryID); }))
                        {
                            srno++;
                            lstProcessed.Add(categoryID);
                            DataRow drCategory = dtATR.NewRow();
                            //DataView dcv = ds.Tables[2].DefaultView;
                            //dcv.RowFilter = string.Format("CATEGORY_ID={0}", categoryID);
                            drCategory["SRNO"] = srno;
                            drCategory["TEST"] = catName;
                            drCategory["RESULT"] = "";
                            drCategory["SPEC_DESC"] = "";
                            drCategory["IS_CATEGORY"] = "1";

                            dtATR.Rows.Add(drCategory);
                            lstsub = new List<int>();
                            DataView dv = ds.Tables["tblATRResult"].DefaultView;
                            dv.RowFilter = string.Format("TEST_CATEGORYID={0}", categoryID);
                            dv.Sort = "RRT_ID,TEST_METHOD_ID ASC";
                            substrno = 0;
                            foreach (DataRow dr1 in dv.ToTable().Rows)
                            {
                                int.TryParse(Convert.ToString(dr1["TEST_SUB_CATEGORYID"]), out subCateID);

                                if (subCateID > 0)
                                {
                                    if (!lstsub.Exists(delegate (int match) { return (match == subCateID); }))
                                    {
                                        substrno++;
                                        lstsub.Add(subCateID);

                                        DataRow drSub = dtATR.NewRow();
                                        drSub["SRNO"] = string.Format("{0}.{1}", srno, substrno);
                                        drSub["TEST"] = Convert.ToString(dr1["SUB_CATEGORY"]);
                                        drSub["RESULT"] = "";
                                        drSub["SPEC_DESC"] = "";
                                        drSub["IS_CATEGORY"] = "1";
                                        dtATR.Rows.Add(drSub);

                                        DataView dl = dv;
                                        dl.RowFilter = string.Format("TEST_SUB_CATEGORYID = {0}", subCateID);
                                        dl.Sort = "RRT_ID,TEST_METHOD_ID ASC";
                                        subsr = 0;
                                        foreach (DataRow dls in dl.ToTable().Rows)
                                        {
                                            subsr++;
                                            DataRow drSubR = dtATR.NewRow();
                                            drSubR["SRNO"] = string.Format("{0}.{1}.{2}", srno, substrno, subsr);
                                            drSubR["TEST"] = dls["TEST"];
                                            drSubR["RESULT"] = FormatTestResult(dls["FINAL_RESULT"], dls["FINAL_RESULT_TO"], dls["TEST_METHOD_TYPE"],
                                                dls["SYMBOL_CODE"], dls["UTF_VALUE"], dls["UNIT_SYMBOL"], dls["CorrValue"], dls["CorrectedType"]);

                                            drSubR["SPEC_DESC"] = dls["SPEC_DESC"];
                                            drSubR["IS_CATEGORY"] = "0";
                                            dtATR.Rows.Add(drSubR);
                                        }
                                    }
                                }
                                else
                                {
                                    substrno++;
                                    DataRow drc = dtATR.NewRow();
                                    drc["SRNO"] = string.Format("{0}.{1}", srno, substrno);
                                    drc["TEST"] = dr1["TEST"];
                                    drc["RESULT"] = FormatTestResult(dr1["FINAL_RESULT"], dr1["FINAL_RESULT_TO"], dr1["TEST_METHOD_TYPE"],
                                        dr1["SYMBOL_CODE"], dr1["UTF_VALUE"], dr1["UNIT_SYMBOL"], dr1["CorrValue"], dr1["CorrectedType"]);

                                    drc["SPEC_DESC"] = dr1["SPEC_DESC"];
                                    drc["IS_CATEGORY"] = "0";
                                    dtATR.Rows.Add(drc);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DataRow pr in ds.Tables["tblATRResult"].Rows)
                {
                    DataRow val = dtATR.NewRow();
                    int.TryParse(Convert.ToString(pr["RRT_ID"]), out rrtID);

                    if (rrtID == default(int))
                    {
                        int.TryParse(Convert.ToString(pr["SPEC_TEST_ID"]), out specTestID);
                        val["SRNO"] = string.Format("{0}", srno + 1);
                        val["TEST"] = pr["TEST"];
                        val["RESULT"] = FormatTestResult(pr["FINAL_RESULT"], pr["FINAL_RESULT_TO"], pr["TEST_METHOD_TYPE"],
                                        pr["SYMBOL_CODE"], pr["UTF_VALUE"], pr["UNIT_SYMBOL"], pr["CorrValue"], pr["CorrectedType"]);

                        val["SPEC_DESC"] = pr["SPEC_DESC"];
                        val["IS_CATEGORY"] = "0";
                        dtATR.Rows.Add(val);

                        DataView drv = ds.Tables["tblATRResult"].DefaultView;
                        drv.RowFilter = string.Format(" RRT_ID IS NOT NULL AND SPEC_TEST_ID={0}", specTestID);
                        drv.Sort = "RRT_ID ASC";
                        srno++;
                        subsr = 0;
                        foreach (DataRow dls in drv.ToTable().Rows)
                        {
                            subsr++;
                            DataRow drSubR = dtATR.NewRow();
                            drSubR["SRNO"] = string.Format("{0}.{1}", srno, subsr);
                            drSubR["TEST"] = dls["TEST"];
                            drSubR["RESULT"] = FormatTestResult(dls["FINAL_RESULT"], dls["FINAL_RESULT_TO"], dls["TEST_METHOD_TYPE"],
                                        dls["SYMBOL_CODE"], dls["UTF_VALUE"], dls["UNIT_SYMBOL"], dls["CorrValue"], dls["CorrectedType"]);

                            drSubR["SPEC_DESC"] = dls["SPEC_DESC"];
                            drSubR["IS_CATEGORY"] = "0";
                            dtATR.Rows.Add(drSubR);
                        }
                    }
                }
            }

            DataSet ds1 = new DataSet();
            ds1.Tables.Add(ds.Tables[0].Copy());
            ds1.Tables.Add(dtATR.Copy());
            if (ds.Tables.Count > 2)
                ds1.Tables.Add(ds.Tables[2].Copy());
            //ds.Tables.RemoveAt(1);
            //ds.Tables.Add(dtATR);

            return ds1;
        }

        string FormatTestResult(object resultFrom, object resultTo, object methodType, object symbolCode, object UTFValue, object unitSymbol, object correctedValue = null, object correctedType = null)
        {
            string result = string.Empty;
            string symbol = Convert.ToString(symbolCode).ToUpper();
            int UTF = default(int);

            if (!string.IsNullOrEmpty(symbol))
            {
                int.TryParse(Convert.ToString(UTFValue), out UTF);
                if (UTF < 1)
                    throw new ApplicationException("UTF Value should not be empty for code " + symbol);

                if (symbol == "CENTIGRADE")
                    symbol = char.ConvertFromUtf32(UTF) + "C";
                else if (symbol == "FAHRENHEIT")
                    symbol = char.ConvertFromUtf32(UTF) + "F";
                else if (symbol == "INDEGREES")
                    symbol = "in" + char.ConvertFromUtf32(UTF);
                else if (symbol == "KGSQURE")
                    symbol = "kg/cm" + char.ConvertFromUtf32(UTF);
                else if (symbol == "GMSQURE")
                    symbol = "gm/cm" + char.ConvertFromUtf32(UTF);
                else if (symbol == "CMSQURE")
                    symbol = "cm" + char.ConvertFromUtf32(UTF);
                else
                    symbol = char.ConvertFromUtf32(UTF);
            }
            else if (!string.IsNullOrEmpty(Convert.ToString(unitSymbol)))
                symbol = Convert.ToString(unitSymbol);
            if (!string.IsNullOrEmpty(Convert.ToString(correctedValue)))
                result = string.Format("{0} ({1})", Convert.ToString(correctedType), correctedValue);
            else if (!string.IsNullOrEmpty(Convert.ToString(resultTo)))
                result = string.Format("{0} {1} to {2} {1}", resultFrom, symbol, resultTo);
            else
                result = string.Format("{0} {1}", resultFrom, symbol);

            return result;
        }

        byte[] GenerateVolInvalidationReport(DataSet ds)
        {
            byte[] buffer = null;
            string path = string.Format("~/Reports/ReportRdlcs/{0}/{1}", rptInfo.EntityCode, "ARDS_VOL_SOLU_INVALID_" + rptInfo.VersionNo + ".rdlc");
            List<ReportParameter> lprm = new List<ReportParameter>();
            rvRequestReport.LocalReport.DataSources.Clear();
            rvRequestReport.LocalReport.EnableExternalImages = true;
            rvRequestReport.ProcessingMode = ProcessingMode.Local;

            BindReport(ds, lprm, path);

            //BIND REPORT BASIC PARAMETERS
            BindReportBasicInfo(lprm);

            try
            {

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                buffer = this.rvRequestReport.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            }
            catch (Exception ex) { }
            return buffer;
        }

        DataSet BuidSpecTests(DataSet ds)
        {
            DataTable dtATR = new DataTable("tblTests");

            List<int> lstProcessed = new List<int>();
            List<int> lstsub = new List<int>();
            dtATR.Columns.Add("SRNO");
            dtATR.Columns.Add("SPEC_TEST_ID");
            dtATR.Columns.Add("TEST");
            dtATR.Columns.Add("RESULT");
            dtATR.Columns.Add("SPEC_DESC");
            dtATR.Columns.Add("VAL_RESULT");
            dtATR.Columns.Add("VALIDATED_BY");
            dtATR.Columns.Add("VALIDATED_ON");

            int categoryID = default(int);
            int subCateID = default(int);
            int srno = default(int);
            int substrno = default(int);
            int subsr = default(int);

            if (ds.Tables["tblCategories"].Rows.Count > 0)
            {
                foreach (DataRow pr in ds.Tables["tblTests"].Rows)
                {
                    categoryID = 0;
                    int.TryParse(Convert.ToString(pr["TEST_CATEGORYID"]), out categoryID);
                    if (categoryID == 0)
                    {
                        DataRow val = dtATR.NewRow();
                        val["SRNO"] = string.Format("{0}", srno + 1);
                        val["TEST"] = pr["TEST"];
                        val["SPEC_TEST_ID"] = pr["SPEC_TEST_ID"];
                        //val["RESULT"] = pr["RESULT"];
                        val["RESULT"] = FormatTestResult(pr["LOWER_LIMIT"], pr["UPPER_LIMIT"], pr["TEST_METHOD_TYPE"],
                                        pr["SYMBOL_CODE"], pr["UTF_VALUE"], pr["UNIT_SYMBOL"]);

                        val["SPEC_DESC"] = pr["SPEC_DESC"];
                        val["VAL_RESULT"] = pr["VAL_RESULT"];
                        val["VALIDATED_BY"] = pr["VALIDATED_BY"];
                        val["VALIDATED_ON"] = pr["VALIDATED_ON"];
                        dtATR.Rows.Add(val);
                        srno++;
                    }
                    else
                    {
                        if (!lstProcessed.Exists(delegate (int match) { return (match == categoryID); }))
                        {
                            srno++;
                            lstProcessed.Add(categoryID);
                            DataRow drCategory = dtATR.NewRow();
                            DataView dcv = ds.Tables[2].DefaultView;
                            dcv.RowFilter = string.Format("CATEGORY_ID={0}", categoryID);
                            drCategory["SRNO"] = srno;
                            drCategory["TEST"] = dcv.ToTable().Rows[0]["CAT_NAME"];
                            drCategory["SPEC_TEST_ID"] = "";
                            drCategory["RESULT"] = "";
                            drCategory["SPEC_DESC"] = "";
                            drCategory["VALIDATED_BY"] = "";
                            drCategory["VALIDATED_ON"] = "";
                            drCategory["VAL_RESULT"] = "";
                            dtATR.Rows.Add(drCategory);
                            lstsub = new List<int>();
                            DataView dv = ds.Tables["tblTests"].DefaultView;
                            dv.RowFilter = string.Format("TEST_CATEGORYID={0}", categoryID);
                            dv.Sort = "ROW_ID ASC";
                            substrno = 0;
                            foreach (DataRow dr1 in dv.ToTable().Rows)
                            {
                                int.TryParse(Convert.ToString(dr1["TEST_SUB_CATEGORYID"]), out subCateID);
                                if (subCateID > 0)
                                {
                                    if (!lstsub.Exists(delegate (int match) { return (match == subCateID); }))
                                    {
                                        substrno++;
                                        lstsub.Add(subCateID);

                                        DataRow drSub = dtATR.NewRow();
                                        DataView dvs = ds.Tables[3].DefaultView;
                                        dvs.RowFilter = string.Format("SUB_CATID = {0}", subCateID);
                                        drSub["SRNO"] = string.Format("{0}.{1}", srno, substrno);
                                        drSub["TEST"] = dvs.ToTable().Rows[0]["SUB_CATEGORY"];
                                        drSub["SPEC_TEST_ID"] = "";
                                        drSub["RESULT"] = "";
                                        drSub["SPEC_DESC"] = "";
                                        drCategory["VALIDATED_BY"] = "";
                                        drCategory["VALIDATED_ON"] = "";
                                        drCategory["VAL_RESULT"] = "";
                                        dtATR.Rows.Add(drSub);

                                        DataView dl = dv;
                                        dl.RowFilter = string.Format("TEST_SUB_CATEGORYID = {0}", subCateID);
                                        dl.Sort = "ROW_ID ASC";
                                        subsr = 0;
                                        foreach (DataRow dls in dl.ToTable().Rows)
                                        {
                                            subsr++;
                                            DataRow drSubR = dtATR.NewRow();
                                            drSubR["SRNO"] = string.Format("{0}.{1}.{2}", srno, substrno, subsr);
                                            drSubR["TEST"] = dls["TEST"];
                                            drSubR["SPEC_TEST_ID"] = dls["SPEC_TEST_ID"];
                                            //drSubR["RESULT"] = Convert.ToString(dls["RESULT"]);
                                            drSubR["RESULT"] = FormatTestResult(dls["LOWER_LIMIT"], dls["UPPER_LIMIT"], dls["TEST_METHOD_TYPE"],
                                        dls["SYMBOL_CODE"], dls["UTF_VALUE"], dls["UNIT_SYMBOL"]);

                                            drSubR["SPEC_DESC"] = dls["SPEC_DESC"];
                                            drSubR["VAL_RESULT"] = dls["VAL_RESULT"];
                                            drSubR["VALIDATED_BY"] = dls["VALIDATED_BY"];
                                            drSubR["VALIDATED_ON"] = dls["VALIDATED_ON"];
                                            dtATR.Rows.Add(drSubR);
                                        }
                                    }
                                }
                                else
                                {
                                    substrno++;
                                    DataRow drc = dtATR.NewRow();
                                    drc["SRNO"] = string.Format("{0}.{1}", srno, substrno);
                                    drc["TEST"] = dr1["TEST"];
                                    drc["SPEC_TEST_ID"] = dr1["SPEC_TEST_ID"];
                                    //drc["RESULT"] = dr1["RESULT"];
                                    drc["RESULT"] = FormatTestResult(dr1["LOWER_LIMIT"], dr1["UPPER_LIMIT"], dr1["TEST_METHOD_TYPE"],
                                        dr1["SYMBOL_CODE"], dr1["UTF_VALUE"], dr1["UNIT_SYMBOL"]);

                                    drc["SPEC_DESC"] = dr1["SPEC_DESC"];
                                    drc["VAL_RESULT"] = dr1["VAL_RESULT"];
                                    drc["VALIDATED_BY"] = dr1["VALIDATED_BY"];
                                    drc["VALIDATED_ON"] = dr1["VALIDATED_ON"];
                                    dtATR.Rows.Add(drc);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DataRow pr in ds.Tables["tblTests"].Rows)
                {
                    DataRow val = dtATR.NewRow();
                    val["SRNO"] = string.Format("{0}", srno + 1);
                    val["TEST"] = pr["TEST"];
                    val["SPEC_TEST_ID"] = pr["SPEC_TEST_ID"];
                    //val["RESULT"] = pr["RESULT"];
                    val["RESULT"] = FormatTestResult(pr["LOWER_LIMIT"], pr["UPPER_LIMIT"], pr["TEST_METHOD_TYPE"],
                                        pr["SYMBOL_CODE"], pr["UTF_VALUE"], pr["UNIT_SYMBOL"]);

                    val["SPEC_DESC"] = pr["SPEC_DESC"];
                    val["VAL_RESULT"] = pr["VAL_RESULT"];
                    val["VALIDATED_BY"] = pr["VALIDATED_BY"];
                    val["VALIDATED_ON"] = pr["VALIDATED_ON"];
                    dtATR.Rows.Add(val);
                    srno++;
                }
            }

            if (ds.Tables["tblValidations"].Columns.Count > 0)
            {
                foreach (DataRow dr in ds.Tables["tblValidations"].Rows)
                {
                    dr["RESULT"] = FormatTestResult(dr["RESULT"], dr["RESULT_TO"], dr["TEST_METHOD_TYPE"],
                                            dr["SYMBOL_CODE"], dr["UTF_VALUE"], dr["UNIT_SYMBOL"]);
                }
            }

            DataSet ds1 = new DataSet();
            ds1.Tables.Add(ds.Tables[0].Copy());
            ds1.Tables.Add(dtATR.Copy());
            ds1.Tables.Add(ds.Tables["tblValidations"].Copy());

            return ds1;
        }
    }
}