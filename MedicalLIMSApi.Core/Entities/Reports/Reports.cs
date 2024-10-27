using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace MedicalLIMSApi.Core.Entities.Reports
{

    public class ReportsBO
    {
        public string EntityCode { get; set; }

        public string FileName { get; set; }

        public string RequestCode { get; set; }

        public string ReportName { get; set; }

        public string ReqCodeForReport { get; set; }

        public short PlantID { get; set; }

        public string PlantCode { get; set; }

        public string PlantName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string RdlcName { get; set; }

        public string DataSourceName { get; set; }

        public string StoredProcedure { get; set; }

        public bool HasSubReport { get; set; }

        public bool IsCorporate { get; set; }

        public string FormateNumber { get; set; }

        public string VersionNumber { get; set; }

        public string EffectiveDate { get; set; }

        public string Title { get; set; }

        public ReportParametersList ParametersList { get; set; }

        public ReportDataSourceTableNamesList TablesList { get; set; }

        public ReportFormatableColumnsList FormatableColumnsList { get; set; }

        public string LogType { get; set; }

        public string EntityReportCode { get; set; }

        public bool ? IsFinalStatusReached { get; set; }

        public string VersionNo { get; set; }
    }

    public class ReportParametersBO
    {
        public string ParameterName { get; set; }

        public string ParameterStringValue { get; set; }

        public object ParameterValue { get; set; }

        public string ParameterDirection { get; set; }

        public bool ParameterType { get; set; }

        public SqlDbType DataType { get; set; }

        public int ParameterLength { get; set; }

        public bool IsNullable { get; set; }

        public byte TableOrder { get; set; }

        public string DBColumnName { get; set; }

        public string Path { get; set; }

        public bool IsNullValue { get; set; }
    }

    public class ReportDataSourceTableNamesBO
    {
        public string TableName { get; set; }

        public byte TableOrder { get; set; }

        public string SourceType { get; set; }
    }

    public class ReportFormatableColumnsBO
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public SqlDbType DataType { get; set; }
    }

    public class ReportParametersList : List<ReportParametersBO> { }

    public class ReportDataSourceTableNamesList : List<ReportDataSourceTableNamesBO> { }

    public class ReportFormatableColumnsList : List<ReportFormatableColumnsBO> { }

    public class ReportSearchResult
    {
        public int EntityActID { get; set; }

        public string EncEntityActID { get { return CommonStaticMethods.Encrypt(EntityActID.ToString()); } }

        public string VersionCode { get; set; }

        public string RequstCode { get; set; }

        public DateTime ReportDate { get; set; }

        public string ReportName { get; set; }

        public string MaterialName { get; set; }

    }

    public class ReportSearchBO : PagerBO
    {

        public string ReportCode { get; set; }

        public string RefCode { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public short PlantID { get; set; }

        public int MatID { get; set; }
    }
    public class EntityReports
    {
        public string EntityRPTCode { get; set; }

        public string ReportName { get; set; }

        public string ReportType { get; set; }
    }
    public class EntityReportsList : List<EntityReports> { }


}
