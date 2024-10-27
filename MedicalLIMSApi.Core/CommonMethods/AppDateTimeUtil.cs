using MedicalLIMSApi.Core.Entities.Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;

namespace MedicalLIMSApi.Core.CommonMethods
{
    public class AppDateTimeUtil
    {
        #region Convert Object To Required DateTime String Formats

        public static string SetDateFormat(object dt)
        {
            return GetDateTimeString(dt, DefaultErrorText, FormatTypes.Date);
        }

        public static string SetDateFormat(object dt, string ErrorText)
        {
            return GetDateTimeString(dt, ErrorText, FormatTypes.Date);
        }

        public static string SetDateFormatWithTime(object dt)
        {
            return GetDateTimeString(dt, DefaultErrorText, FormatTypes.DateWithTime);
        }

        public static string SetDateFormatWithTime(object dt, string ErrorText)
        {
            return GetDateTimeString(dt, ErrorText, FormatTypes.DateWithTime);
        }

        public static string SetDateFormatWithFullTime(object dt)
        {
            return SetDateFormatWithFullTime(dt, DefaultErrorText);
        }

        public static string SetDateFormatWithFullTime(object dt, string ErrorText)
        {
            return GetDateTimeString(dt, ErrorText, FormatTypes.DateWithFullTime);
        }

        private static string GetDateTimeString(object dt, string ErrorText, FormatTypes formatType)
        {
            string dateFormat = GetConfigDateFormat(formatType);

            string formatedDateString = ErrorText;

            try
            {
                DateTime ConvertDate = Convert.ToDateTime(Convert.ToString(dt));
                if (ConvertDate != DateTime.MinValue)
                    formatedDateString = ConvertDate.ToString(dateFormat);
            }
            catch { }

            return formatedDateString;
        }

        #endregion

        #region Get Date/Time/Both Formats

        public static DateTime ConvertToDateTimeFromString(string input)
        {
            try
            {
                input = FormatInputDateTimeString(input);
                return DateTime.ParseExact(input.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch { return DateTime.MinValue; }
        }

        public static bool ConvertToDateTimeTryParse(string input, out DateTime dt)
        {
            dt = DateTime.MinValue;
            try
            {
                dt = ConvertToDateTimeFromString(input);
                return (dt != DateTime.MinValue);
            }
            catch { return false; }
        }

        private static string[] formats
        {
            get
            {
                if (USformats.Contains(AppDateFormat))
                    return USformats;
                else
                    return Localformats;
            }
        }

        private static string[] USformats = new string[]
        {
            "MM/dd/yyyy hh:mm:ss tt",
            "MM/dd/yyyy hh:mm:sstt",
            "MM/dd/yyyy hh:mm tt",
            "MM/dd/yyyy hh:mmtt",
            "MM/dd/yyyy HH:mm:ss",
            "MM/dd/yyyy HH:mm",
            "MM/dd/yyyy"
        };

        private static string[] Localformats = new string[]
        {
            "dd/MM/yyyy hh:mm:ss tt",
            "dd/MM/yyyy hh:mm:sstt",
            "dd/MM/yyyy hh:mm tt",
            "dd/MM/yyyy hh:mmtt",
            "dd/MM/yyyy HH:mm:ss",
            "dd/MM/yyyy HH:mm",
            "dd/MM/yyyy"
        };

        private static string FormatInputDateTimeString(string input)
        {
            string formattedStr = string.Empty;

            string[] arr = input.Split(' ');
            string[] strDate = arr[0].Split('/');

            foreach (string s in strDate)
            {
                if (s.Length < 2)
                    formattedStr += formattedStr == string.Empty ? "0" + s : "/0" + s;
                else
                    formattedStr += formattedStr == string.Empty ? s : "/" + s;
            }

            if (arr.Length > 1)
            {
                formattedStr = formattedStr + " ";
                string[] strTime = arr[1].Split(':');

                for (int t = 0; t < 2; t++)
                {
                    if (strTime[t].Length < 2)
                        formattedStr += t == 0 ? "0" + strTime[t] : ":0" + strTime[t];
                    else
                        formattedStr += t == 0 ? strTime[t] : ":" + strTime[t];
                }
            }

            if (arr.Length > 2)
            {
                formattedStr += (arr[2].ToUpper() == "AM" || arr[2].ToUpper() == "PM") ? " " + arr[2].ToUpper() : string.Empty;
            }

            return formattedStr;
        }

        #endregion

        private static string GetConfigDateFormat(FormatTypes formatType)
        {
            string dateFormat = string.Empty;
            switch (formatType)
            {
                case FormatTypes.Date:
                    dateFormat = AppDateFormat;
                    break;
                case FormatTypes.DateWithTime:
                    dateFormat = AppDateTimeFormat;
                    break;
                case FormatTypes.DateWithFullTime:
                    dateFormat = AppFullDateTimeFormat;
                    break;
                default: break;
            }
            return dateFormat;
        }

        public static string DefaultErrorText
        {
            get
            {
                return "N / A";
            }
        }

        //public static string ScriptDateFormat
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["APPSCRIPTDATEFORMAT"];
        //    }
        //}

        public static string AppDateFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["APPDATEFORMAT"];
            }
        }

        public static string AppDateTimeFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["APPDATETIMEFORMAT"];
            }
        }

        public static string AppFullDateTimeFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["APPFULLDATETIMEFORMAT"];
            }
        }

        public enum FormatTypes
        {
            Date = 1,
            DateWithTime = 2,
            DateWithFullTime = 3
        }


        public static DataSet SetDSDateFormat(DataSet ds, FormatTypes FormatType, ReportsBO obj)
        {
            return SetDSDateFormat(ds, DefaultErrorText, FormatType, obj);
        }

        public static DataSet SetDSDateFormat(DataSet ds, string ErrorText, FormatTypes FormatType, ReportsBO obj)
        {
            foreach (DataTable dt in ds.Tables)
            {
                SetDTDateFormat(dt, ErrorText, FormatType, obj);
            }

            return ds;
        }

        public static DataTable SetDTDateFormat(DataTable dt, string ErrorText, FormatTypes FormatType, ReportsBO obj)
        {
            List<ReportFormatableColumnsBO> formatList = null;
            if (obj != null && obj.FormatableColumnsList != null)
                formatList = obj.FormatableColumnsList.FindAll(delegate (ReportFormatableColumnsBO t) { return (t.TableName == dt.TableName); });
            List<string> lstColNames = new List<string>();
            List<string> lstDecimalColNames = new List<string>();
            FormatTypes fType = FormatType;
            ReportFormatableColumnsBO formatCol = null;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].DataType == typeof(DateTime))
                {
                    dt.Columns.Add(dt.Columns[i].ColumnName + "__New", typeof(string));
                    lstColNames.Add(dt.Columns[i].ColumnName);
                }
                else if (dt.Columns[i].DataType == typeof(decimal))
                {
                    formatCol = null;
                    dt.Columns.Add(dt.Columns[i].ColumnName + "__New", typeof(string));
                    lstDecimalColNames.Add(dt.Columns[i].ColumnName);
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                foreach (string col in lstColNames)
                {
                    fType = FormatType;
                    if (formatList != null)
                    {
                        formatCol = null;
                        formatCol = formatList.Find(delegate (ReportFormatableColumnsBO t) { return (t.ColumnName == col); });
                        if (formatCol != null)
                            fType = formatCol.DataType == SqlDbType.Date ? FormatTypes.Date : FormatType;
                    }
                    switch (fType)
                    {
                        case FormatTypes.Date:
                            row[col + "__New"] = SetDateFormat(row[col], ErrorText);
                            break;
                        case FormatTypes.DateWithTime:
                            row[col + "__New"] = SetDateFormatWithTime(row[col], ErrorText);
                            break;
                        case FormatTypes.DateWithFullTime:
                            row[col + "__New"] = SetDateFormatWithFullTime(row[col], ErrorText);
                            break;
                        default: break;
                    }
                }

                foreach (string col in lstDecimalColNames)
                {
                    formatCol = null;
                    if (formatList != null)
                        formatCol = formatList.Find(delegate (ReportFormatableColumnsBO t) { return (t.ColumnName == col); });

                    if (formatCol != null)
                        row[col + "__New"] = row[col] == DBNull.Value ? "0" : Convert.ToString(CommonStaticMethods.ConvertToFriendlyDecimal(Convert.ToDecimal(row[col])));
                    else
                        row[col + "__New"] = row[col] == DBNull.Value ? "N / A" : Convert.ToString(CommonStaticMethods.ConvertToFriendlyDecimal(Convert.ToDecimal(row[col])));
                }
            }

            foreach (string col in lstColNames)
            {
                dt.Columns.Remove(col);
                dt.Columns[col + "__New"].ColumnName = col;
            }

            foreach (string col in lstDecimalColNames)
            {
                dt.Columns.Remove(col);

                dt.Columns[col + "__New"].ColumnName = col;
            }
            return dt;
        }


    }
}
