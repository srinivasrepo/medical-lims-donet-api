using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;

using System.Text;
using System.Xml.Serialization;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using System.Reflection;
using System.Data;
using System.Web.Script.Serialization;
using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.UtilUploads;

namespace MedicalLIMSApi.Core.CommonMethods
{
    public class CommonStaticMethods
    {

        public static string Serialize<T>(T t)
        {
            string XMLString = string.Empty;

            if (t == null)
                return XMLString;

            XmlSerializer x = null;

            try
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    x = new XmlSerializer(typeof(T));

                    x.Serialize(stringwriter, t);

                    XMLString = stringwriter.ToString();
                }
            }
            catch
            {
                XMLString = string.Empty;
            }
            finally
            {
                x = null;
            }

            return XMLString;
        }

        public static string Splices(string source, int start, int end)
        {
            if (end < 0) // Keep this for negative end support
            {
                end = source.Length + end;
            }

            int len = end - start;               // Calculate length

            return source.Substring(start, len); // Return Substring of length
        }

        public static T Deserialize<T>(string xmlString)
        {
            int startIndex = default(int);
            int lastIndex = default(int);
            string spliceXmlString = string.Empty;

            if (xmlString.IndexOf("<jsonTableInfo>") > -1)
            {
                startIndex = xmlString.IndexOf("<jsonTableInfo>");
                lastIndex = xmlString.IndexOf("</jsonTableInfo>");

                int len = xmlString.Length;

                spliceXmlString = Splices(xmlString, startIndex, lastIndex + 16);

                xmlString = xmlString.Replace(spliceXmlString, string.Empty);

                int subStartIndex = spliceXmlString.IndexOf('[');
                int subLastIndex = spliceXmlString.IndexOf(']');

                spliceXmlString = Splices(spliceXmlString, subStartIndex, subLastIndex + 1);

            }


            T target = default(T);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlString);
            XmlNodeReader xNodeReader = new XmlNodeReader(xDoc.DocumentElement);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            target = (T)xmlSerializer.Deserialize(xNodeReader);

            if (!string.IsNullOrEmpty(spliceXmlString))
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(spliceXmlString);

                var tableTypeInfo = target.GetType().GetProperty("jsonTableInfo");
                tableTypeInfo.SetValue(target, dt);
            }

            return target;

        }

        public string decryptQueryString(string strQueryString)
        {
            return Decrypt(strQueryString, "!#$a54?3");
        }

        public static string Encrypt(string strQueryString)
        {
            return Encrypt(strQueryString, "!#$a54?3");
        }

        public static T Decrypt<T>(string strQueryString)
        {
            T target = default(T);
            try
            {
                target = (T)Convert.ChangeType(Decrypt(strQueryString, "!#$a54?3"), typeof(T));
            }
            catch
            {
                if (!string.IsNullOrEmpty(strQueryString) && strQueryString != "undefined" && strQueryString != "null")
                    target = (T)Convert.ChangeType(strQueryString, typeof(T));
            }

            return target;

            //return Decrypt(strQueryString, "!#$a54?3");
        }

        protected static string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            byte[] key = { };
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };

            stringToDecrypt = stringToDecrypt.Replace(" ", "+");

            byte[] inputByteArray = new byte[stringToDecrypt.Length];
            try
            {
                key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();

                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();

                Encoding encoding = Encoding.UTF8; return encoding.GetString(ms.ToArray());
            }

            catch (System.Exception ex)
            {
                if (ex.Message == "Invalid length for a Base-64 char array.")
                {
                    stringToDecrypt = stringToDecrypt + "+";
                    return Decrypt(stringToDecrypt, sEncryptionKey);
                }
                else
                    throw ex;
            }
        }

        protected static string Encrypt(string stringToEncrypt, string sEncryptionKey)
        {
            byte[] key = { };
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };

            byte[] inputByteArray; //Convert.ToByte(stringToEncrypt.Length)

            try
            {
                key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();

                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray()).Replace("+", " ");
            }

            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DbInfo GetLookUpDBData(string code, string Condition, string purpose, bool IsRs232Mode)
        {
            DbInfo obj = new DbInfo();
            obj.PlantFilter = true;

            switch (code)
            {
                case "ALLACTIVEUSERS":
                    obj.ViewName = "lims.vwActiveUser";
                    obj.ID = "USER_ROLE_ID";
                    obj.Name = "USER_NAME";
                    obj.Code = "USER_CODE";
                    obj.PlantFilter = true;
                    break;

                case "StatusMaster":
                    obj.ViewName = "dbo.STATUS_MASTER";
                    obj.ID = "STATUS_ID";
                    obj.Name = "STATUS";
                    obj.Code = "STATUS_CODE";
                    obj.PlantFilter = false;
                    break;

                case "ALLPRODUCTS":
                    obj.ViewName = "vwLkpActProducts";
                    obj.ID = "PROD_ID";
                    obj.Name = "PRODUCT_NAME";
                    obj.Code = "PRODUCT_CODE";
                    obj.PlantFilter = false;
                    break;

                case "GetAllMaterials":
                    obj.ViewName = "vwLKPMaterialsAll";
                    obj.ID = "MAT_ID";
                    obj.Name = "NAME";
                    obj.Code = "CODEALIAS";
                    obj.PlantFilter = false;
                    break;

                case "GetActiveEquipments":
                    obj.ViewName = "vwLKPActiveEquipments";
                    obj.ID = "EQUIPMENT_ID";
                    obj.Name = "TITLE";
                    obj.Code = "EQP_CODE";
                    obj.ExtColName = "TYPE_CODE";
                    break;

                case "GetSpecifications":
                    obj.ViewName = "vwPlantSpecificationMasterData";
                    obj.ID = "SPECIFICATION_ID";
                    obj.Name = "SPECIFICATION";
                    obj.Code = "SPEC_NUMBER";
                    obj.ExtColName = "MAT_CODE";
                    break;

                case "INVBATCHES":
                    obj.ViewName = "dbo.vwINVBatchNumbers";
                    obj.ID = "INV_ID";
                    obj.Name = "STATUS";
                    obj.Code = "INV_BATCHNUM";
                    obj.ExtCondition = "REM_QTY > 0";
                    break;

                case "GetInvalidations":
                    obj.ViewName = "analysis.vwLKPGetInvalidations";
                    obj.ID = "InvalidationID";
                    obj.Name = "InvalidationCode";
                    obj.Code = "InvalidationNumber";
                    break;

                case "PlantMaterials":
                    obj.ViewName = "vwLKPMaterialsPlantWise";
                    obj.ID = "MAT_ID";
                    obj.Name = "NAME";
                    obj.Code = "CODEALIAS";
                    break;

                case "SearchPlantMaterials":
                    obj.ViewName = "vwSearchPlantMatLookup";
                    obj.ID = "MAT_ID";
                    obj.Name = "MAT_NAME";
                    obj.Code = "CODEALIAS";
                    break;

                case "GetPlantWiseProd":
                    obj.ViewName = "vwLkpProductsPlantWise";
                    obj.ID = "PROD_ID";
                    obj.Name = "PRODUCT_NAME";
                    obj.Code = "PRODUCT_CODE";
                    break;

                case "IndicatorDetails":
                    obj.ViewName = "solmgmt.vwLKPIndicators";
                    obj.ID = "IndicatorID";
                    obj.Name = "Status";
                    obj.Code = "IndicatorCode";
                    break;

                case "CalibrationReference":
                    obj.ViewName = "lims.vwLKPCalibrationReference";
                    obj.ID = "ReportID";
                    obj.Name = "Status";
                    obj.Code = "ReportNumber";
                    obj.ExtColName = "CalibPramID";
                    break;

                case "UserControlDetail":
                    obj.ViewName = "dbo.vwUserControlDetail";
                    obj.ID = "USER_ID";
                    obj.Name = "MODULE_NAME";
                    obj.Code = "FULL_NAME";
                    obj.ExtColName = "ROLE_NAME";
                    obj.PlantFilter = false;
                    break;

                case "GetSolutionIndex":
                    obj.ViewName = "solmgmt.vwLKPGetSolutionIndex";
                    obj.ID = "IndexID";
                    obj.Name = "Material";
                    obj.Code = "CodeAlies";
                    break;

                case "GetQCUsers":
                    obj.ViewName = "analysis.vwQCUsers";
                    obj.ID = "UserRoleID";
                    obj.Name = "FullName";
                    obj.Code = "UserCode";
                    obj.ExtColName = "UserID";
                    obj.PlantFilter = true;
                    break;

                case "GetCategoryItems":
                    obj.ViewName = "dbo.vwCategoryItems";
                    obj.ID = "CatItemID";
                    obj.Name = "CatItem";
                    obj.Code = "CatItemCode";
                    obj.PlantFilter = false;
                    break;

                case "SampleActivityInfo":
                    obj.ViewName = "samplan.vwSampleActivityInfo";
                    obj.ID = "ID";
                    obj.Name = "TestTitle";
                    obj.Code = "ArNumber";
                    obj.ExtColName = "UserName";
                    obj.PlantFilter = false;
                    break;

                case "SamplingActivityInfo":
                    obj.ViewName = "samplan.vwSamplingActivityInfo";
                    obj.ID = "ID";
                    obj.Name = "UserName";
                    obj.Code = "ArNumber";
                    obj.ExtColName = "SioCode";
                    obj.PlantFilter = true;
                    break;

                case "InvalidationActivityInfo":
                    obj.ViewName = "samplan.vwInvalidationActivityInfo";
                    obj.ID = "ID";
                    obj.Name = "Name";
                    obj.Code = "Code";
                    obj.PlantFilter = false;
                    break;

                case "GetSamplePlans":
                    obj.ViewName = "samplan.vwGetSamplePlans";
                    obj.ID = "PlanID";
                    obj.Name = "Status";
                    obj.Code = "RequestCode";
                    break;

                case "GetSampleNumbers":
                    obj.ViewName = "vwSampleBuildMaster";
                    obj.ID = "SAMPLE_ID";
                    obj.Name = "MAT_NAME";
                    obj.Code = "SAMPLE_NUM";
                    obj.PlantFilter = true;
                    break;

                case "GetLOTBatcheNumbers":
                    obj.ViewName = "vwSamMatInvID";
                    obj.ID = "INV_ID";
                    obj.Name = "NAME";
                    obj.Code = "INV_BATCHNUM";
                    obj.ExtColName = "BLOCK_NAME";
                    obj.PlantFilter = true;
                    break;

                case "GetParameterName":
                    obj.ViewName = "analysis.vwSpecTestInfo";
                    obj.ID = "SpecTestID";
                    obj.Name = "TestName";
                    obj.Code = "SrNum";
                    obj.PlantFilter = false;
                    break;

                case "GetARNumbers":
                    obj.ViewName = "vwSAMAnalysisARNumbers";
                    obj.ID = "AR_ID";
                    obj.Name = "AR_NUM";
                    obj.Code = "SIO_CODE";
                    break;

                case "GetProjects":
                    obj.ViewName = "PROJECT_MASTER";
                    obj.ID = "PROJECT_ID";
                    obj.Name = "PROJECT_NAME";
                    obj.Code = "PROJECT_CODE";
                    obj.PlantFilter = false;
                    break;

                case "GetSpecificatioinTests":
                    obj.ViewName = "spec.vwLKPTestOrParamters";
                    obj.ID = "SpecCatID";
                    obj.Name = "TestTitle";
                    obj.Code = "SrNum";
                    obj.PlantFilter = false;
                    break;

                case "GetOosTests":
                    obj.ViewName = "TEST_MASTER";
                    obj.ID = "TEST_ID";
                    obj.Code = "TEST_CODE";
                    obj.Name = "TEST_TITLE";
                    obj.PlantFilter = false;
                    break;

                case "GetOOSBatchNumbers":
                    obj.ViewName = "vwOOSBatchNumbers";
                    obj.ID = "INV_ID";
                    obj.Name = "INV_BATCHNUM";
                    obj.Code = "MATNAME";
                    break;

                case "GetSpecificationSearch":
                    obj.ViewName = "dbo.vwSpecificationSearch";
                    obj.ID = "SPECIFICATION_ID";
                    obj.Name = "NAME";
                    obj.Code = "SPEC_NUMBER";
                    break;

                case "ChangeUserPlanTestDetails":
                    obj.ViewName = "samplan.vwChangeUserPlanTestDetails";
                    obj.ID = "UserRoleID";
                    obj.Name = "UserName";
                    obj.Code = "UserCode";
                    obj.PlantFilter = false;
                    break;

                case "GetInventoryDetails":
                    obj.ViewName = "lims.vwLKPQCInventory";
                    obj.ID = "InvID";
                    obj.Name = "BatchNumber";
                    obj.Code = "MaterialName";
                    obj.ExtColName = "MatID";
                    break;

                case "PlanSamplingInfo":
                    obj.ViewName = "samplan.vwPlanSamplingInfo";
                    obj.ID = "ID";
                    obj.Name = "UserName";
                    obj.Code = "ArNumber";
                    obj.PlantFilter = false;
                    break;

                case "LabInventoryBatch":
                    obj.ViewName = "lims.vwQCInventoryPacks";
                    obj.ID = "PackInvID";
                    obj.Name = "ReservedQty";
                    obj.Code = "BatchNumber";
                    //obj.ExtColName = "CAT_ITEM_CODE";
                    obj.ExtColName = "RemQty";
                    break;

                case "ActiveTechniques":
                    obj.ViewName = "aqual.vwLKPActiveTechniques";
                    obj.ID = "MASTER_TESTID";
                    obj.Name = "TEST_NAME";
                    obj.Code = "TEST_CODE";
                    obj.PlantFilter = false;
                    break;

                case "ArNumbers":
                    obj.ViewName = "aqual.vwLKPArNumbers";
                    obj.ID = "ArID";
                    obj.Name = "ArNumber";
                    obj.Code = "BatchNumber";
                    obj.PlantFilter = true;
                    break;

                case "DestructionCode":
                    obj.ViewName = "samdest.vwLKPDestructionCode";
                    obj.ID = "DestructionID";
                    obj.Name = "DestSource";
                    obj.Code = "RequestCode";
                    obj.PlantFilter = true;
                    break;

                case "WetAndInstrumentationInfo":
                    obj.ViewName = "samplan.vwWetAndInstrumentationInfo";
                    obj.ID = "ID";
                    obj.ExtColName = "TestTitle";
                    obj.Name = "UserName";
                    obj.Code = "ArNumber";
                    obj.PlantFilter = true;
                    break;

                case "InchargeInvalidationActivities":
                    obj.ViewName = "samplan.vwInvalidationActivityInfoInchargeLevel";
                    obj.ID = "ID";
                    obj.Name = "UserName";
                    obj.Code = "Name";
                    obj.ExtColName = "EntitySource";
                    obj.PlantFilter = true;
                    break;

                case "ChangeUserPlanAndTest":
                    obj.ViewName = "samplan.vwChangeUserPlanAnaTests";
                    obj.ID = "UserRoleID";
                    obj.Name = "UserName";
                    obj.Code = "UserCode";
                    //obj.PlantFilter = false;
                    break;

                case "ChangeUserPlanOOSTest":
                    obj.ViewName = "samplan.vwChangeUserPlanOOSTests";
                    obj.ID = "UserRoleID";
                    obj.Name = "UserName";
                    obj.Code = "UserCode";
                    break;

                case "GetComponents":
                    obj.ViewName = "root.ConditionalComponents";
                    obj.ID = "ComponentID";
                    obj.Code = "ComponentCode";
                    obj.Name = "Component";
                    obj.PlantFilter = false;
                    break;

                case "GetVolSolutionIndexMaterials":
                    obj.ViewName = "solmgmt.vwLKPVolSolutionIndexMaterials";
                    obj.ID = "MaterialID";
                    obj.Code = "MaterialCode";
                    obj.Name = "MaterialName";
                    break;

                case "GetTestSolIndexMaterials":
                    obj.ViewName = "solmgmt.vwLKPTestSolIndexMaterials";
                    obj.ID = "MaterialID";
                    obj.Code = "MaterialCode";
                    obj.Name = "MaterialName";
                    break;

                case "GetActHplcGCClol":
                    obj.ViewName = "VWLkpActHplcGCClol";
                    obj.ID = "EQUIPMENT_ID";
                    obj.Code = "EQP_USER_CODE";
                    obj.Name = "TITLE";
                    break;

                case "GetActiveInstruments":
                    obj.ViewName = "vwActiveInstruments";
                    obj.ID = "EQUIPMENT_ID";
                    obj.Code = "EQP_USER_CODE";
                    obj.Name = "TITLE";
                    break;

                case "GetAdditionalTest":
                    obj.ViewName = "vwAnaTest";
                    obj.ID = "TEST_ID";
                    obj.Code = "TEST_CODE";
                    obj.Name = "TEST_TITLE";
                    obj.PlantFilter = false;
                    break;

                case "GetDeviationRequest":
                    obj.ViewName = "vwDeviationRequest";
                    obj.ID = "DEV_ID";
                    obj.Name = "MODULE_NAME";
                    obj.Code = "DEV_NUMBER";
                    obj.PlantFilter = true;
                    break;

                case "GetPlannedChange":
                    obj.ViewName = "vwPlannedChange";
                    obj.ID = "PC_ID";
                    obj.Name = "MODULE_NAME";
                    obj.Code = "PC_NUMBER";
                    obj.PlantFilter = true;
                    break;

                case "GetChangeControl":
                    obj.ViewName = "vwChangeControl";
                    obj.ID = "CCR_ID";
                    obj.Name = "MODULE_NAME";
                    obj.Code = "CCR_NUMBER";
                    obj.PlantFilter = true;
                    break;

                case "GetInitProcSearch":
                    obj.ViewName = "vwInitProcSearch";
                    obj.ID = "INIT_PROCREQID";
                    obj.Name = "NAME";
                    obj.Code = "INIT_PROCREQ_CODE";
                    obj.PlantFilter = true;
                    break;

                case "GetExportCAPA":
                    obj.ViewName = "vwExportCAPAItems";
                    obj.ID = "CAPAID";
                    obj.Name = "CAPA_SOURCE";
                    obj.Code = "CAPA_NUMBER";
                    obj.ExtColName = "SOURCE_REFERENCE";
                    obj.PlantFilter = true;
                    break;

                case "GetSpecTestCategories":
                    obj.ViewName = "vwSpecTestCategories";
                    obj.ID = "CATEGORY_ID";
                    obj.Name = "CAT_NAME";
                    obj.Code = "ROW_ID";
                    obj.PlantFilter = false;
                    break;

                case "GetSpecSubCategories":
                    obj.ViewName = "vwSpecSubCategories";
                    obj.ID = "SUB_CATID";
                    obj.Name = "CATEGORY";
                    obj.Code = "SUB_CATEGORY";
                    obj.PlantFilter = false;
                    break;

                case "GetAnaTest":
                    obj.ViewName = "vwAnaTest";
                    obj.ID = "TEST_ID";
                    obj.Name = "TEST_TITLE";
                    obj.Code = "TEST_CODE";
                    obj.PlantFilter = false;
                    break;

                case "GetSamARNumbers":
                    obj.ViewName = "analysis.vwLKPGetARNumbers";
                    obj.ID = "ArID";
                    obj.Name = "ArNumber";
                    obj.Code = "Code";
                    obj.ExtColName = "InvID";
                    break;

                case "GetGroupTests":
                    obj.ViewName = "analysis.vwLKPGroupTests";
                    obj.ID = "SamAnaTestID";
                    obj.Name = "TestTitle";
                    obj.Code = "SrNum";
                    obj.PlantFilter = false;
                    break;

                case "GetSDMSDetails":
                    obj.ViewName = "analysis.vwLKPGetSDMSDetails";
                    obj.ID = "SdmsID";
                    if (IsRs232Mode)
                    {
                        obj.Name = "Code";
                        obj.Code = "ReportTitle";
                        obj.ExtColName = "SamAnaTestID";
                    }
                    else
                    {
                        obj.Name = "ReportTitle";
                        obj.Code = "Code";
                        obj.ExtColName = "PrepRunNo";
                    }

                    obj.PlantFilter = false;
                    break;

                case "GetSDMSDetailsImpurities":
                    obj.ViewName = "analysis.vwLKPGetSDMSDetails";
                    obj.ID = "SdmsID";
                    obj.Name = "ReportTitle";
                    obj.Code = "Code";
                    obj.ExtColName = "PrepRunNo";
                    obj.PlantFilter = false;
                    break;

                case "StandardTestProc":
                    obj.ViewName = "rawdata.vwTemplates";
                    obj.ID = "TEMPLATE_ID";
                    obj.Name = "TITLE";
                    obj.Code = "TEMPLATE_CODE_VERSION";
                    obj.PlantFilter = false;
                    break;

                case "GetTests":
                    obj.ViewName = "vwTests";
                    obj.ID = "TEST_ID";
                    obj.Code = "TEST_CODE";
                    obj.Name = "TEST_TITLE";
                    obj.PlantFilter = false;
                    break;

                case "GetCalibrationParameters":
                    obj.ViewName = "calib.vwLKPCalibrationParameters";
                    obj.ID = "CalibParamID";
                    obj.Code = "RequestCode";
                    obj.Name = "Title";
                    break;

                case "GetCalibrationParameterSets":
                    obj.ViewName = "calib.vwCalibrationParameterSets";
                    obj.ID = "CalibParamID";
                    obj.Name = "Status";
                    obj.Code = "RequestCode";
                    break;

                case "GetInstrumentCalibration":
                    obj.ViewName = "calib.vwLkpInstrumentCalibration";
                    obj.ID = "ID";
                    obj.Name = "Status";
                    obj.Code = "ReferenceNumber";
                    break;

                case "AnalysisDataReview":
                    obj.ViewName = "analysis.vwLkpAnalysisDataReview";
                    obj.ID = "ID";
                    obj.Name = "Title";
                    obj.Code = "Code";
                    break;

                case "CalibrationDataReview":
                    obj.ViewName = "analysis.vwLkpCalibrationDataReview";
                    obj.ID = "ID";
                    obj.Name = "Title";
                    obj.Code = "Code";
                    break;

                case "GetManufacturers":
                    obj.ViewName = "vwManufacturerVendors";
                    obj.ID = "MFG_ID";
                    obj.Name = "VEN_NAME";
                    obj.Code = "VEN_CODE";
                    break;

                case "GetMBBatchNumbers":
                    obj.ViewName = "analysis.vwLKPGetMobilePhaseBatchNumbers";
                    obj.ID = "MobilePhaseID";
                    obj.Name = "BatchNumber";
                    obj.Code = "MobilePhaseCode";
                    obj.ExtColName = "UseBeforeDate";
                    break;

                case "GetPlantCustomers":
                    obj.ViewName = "dbo.vwLKPACTPlantCustomers";
                    obj.ID = "CUSTOMER_ID";
                    obj.Name = "CUSTOMER_NAME";
                    obj.Code = "CUST_CODE";
                    obj.PlantFilter = true;
                    break;

                case "MobilePhaseProduct":
                    obj.ViewName = "vwPlantWiseProductAndStages";
                    obj.ID = "STAGE_ID";
                    obj.Name = "PRODUCT_NAME";
                    obj.Code = "PRODUCT_CODE";
                    obj.ExtColName = "STAGE";
                    break;

                case "SearchMobilePhase":
                    obj.ViewName = "solmgmt.vwLKPSearchMobilePhase";
                    obj.ID = "MobilePhaseID";
                    obj.Code = "MobilePhaseCode";
                    obj.Name = "Status";
                    break;

                case "SearchVolumetricSolution":
                    obj.ViewName = "solmgmt.vwLKPSearchVolumetricSolution";
                    obj.ID = "SolutionID";
                    obj.Code = "SolutionCode";
                    obj.Name = "Status";
                    break;

                case "SearchCalibrationSolution":
                    obj.ViewName = "solmgmt.vwLKPSearchCalibrationSolution";
                    obj.ID = "StockSolutionID";
                    obj.Code = "RequestCode";
                    obj.Name = "Status";
                    break;

                case "SearchRinsingSolutions":
                    obj.ViewName = "solmgmt.vwLKPSearchRinsingSolutions";
                    obj.ID = "SolutionID";
                    obj.Name = "Status";
                    obj.Code = "RequestCode";
                    break;

                case "SolutionFormulae":
                    obj.ViewName = "root.SolMgmtFormulae";
                    obj.ID = "FormulaID";
                    obj.Name = "FormulaTitle";
                    obj.Code = "FormulaDef";
                    obj.PlantFilter = false;
                    break;

                case "DMSMappedDocuments":
                    obj.ViewName = "VWMappedDocumentsByEntity";
                    obj.ID = "DOCT_TRACK_ID";
                    obj.Name = "DOCT_NAME";
                    obj.Code = "DOCT_NUM";
                    break;

                case "GetSearchCloseShit":
                    obj.ViewName = "samplan.vwLKPSearchCloseShit";
                    obj.ID = "ShiftID";
                    obj.Code = "RequestCode";
                    obj.Name = "Status";
                    break;

                case "GetAnalysisActInstruments":
                    obj.ViewName = "vwAnalysisActInstruments";
                    obj.ID = "EQUIPMENT_ID";
                    obj.Code = "EQP_USER_CODE";
                    obj.Name = "TITLE";
                    break;

                case "GetBatchesForSendSample":
                    obj.ViewName = "lims.vwLkpBatchesForSendSample";
                    obj.ID = "InvID";
                    obj.Code = "BatchNumber";
                    obj.Name = "Material";
                    break;

                case "GetDREntityReferenceNumbers":
                    obj.ViewName = "analysis.vwLKPGetDREntityReferenceNumbers";
                    obj.ID = "RequestID";
                    obj.Name = "RequestType";
                    obj.Code = "RequestCode";
                    break;

                case "GetAQInwardNumbers":
                    obj.ViewName = "aqual.vwLKPGetAQInwardsNumbers";
                    obj.ID = "ID";
                    obj.Code = "Code";
                    obj.Name = "Status";
                    break;

                case "GetAllUsers":
                    obj.ViewName = "lims.vwLKPGetUser";
                    obj.ID = "UserID";
                    obj.Code = "UserCode";
                    obj.Name = "UserName";
                    break;

                case "GetSampleAnalysisSearchArNumber":
                    obj.ViewName = "lims.vwSampleAnalysisSearch";
                    obj.ID = "AR_ID";
                    obj.Code = "SioCode";
                    obj.Name = "AR_NUM";
                    break;

                case "DataReviewActivityInfo":
                    obj.ViewName = "samplan.vwDataReviewActivityInfo";
                    obj.ID = "ID";
                    obj.Code = "Code";
                    obj.Name = "Name";
                    obj.PlantFilter = false;
                    break;

                case "DataReviewActivityInfoInchargeLevel":
                    obj.ViewName = "samplan.vwDataReviewActivityInfoInchargeLevel";
                    obj.ID = "ID";
                    obj.Code = "Code";
                    obj.Name = "UserName";
                    obj.ExtColName = "Name";
                    break;

                case "OOSActivityInfo":
                    obj.ViewName = "samplan.vwOOSActivityInfo";
                    obj.ID = "ID";
                    obj.Code = "Name";
                    obj.Name = "Code";
                    obj.PlantFilter = false;
                    break;

                case "OOSActivityInfoInchargeLevel":
                    obj.ViewName = "samplan.vwOOSActivityInfoInchargeLevel";
                    obj.ID = "ID";
                    obj.Code = "Name";
                    obj.Name = "UserName";
                    obj.ExtColName = "Code";
                    break;


                case "CalibActivityInfo":
                    obj.ViewName = "samplan.vwCalibActivityInfo";
                    obj.ID = "ID";
                    obj.Code = "Code";
                    obj.Name = "Name";
                    obj.ExtColName = "Type";
                    obj.PlantFilter = false;
                    break;

                case "CalibActivityInfoInchargeLevel":
                    obj.ViewName = "samplan.vwCalibActivityInfoInchargeLevel";
                    obj.ID = "ID";
                    obj.Code = "Code";
                    obj.Name = "UserName";
                    obj.ExtColName = "Name";
                    break;

                case "GetQCInstruments":
                    obj.ViewName = "dbo.vwLKPQCInstruments";
                    obj.ID = "EQUIPMENT_ID";
                    obj.Name = "EQUIPMENT_NAME";
                    obj.Code = "EQUIPMENT_NUMBER";
                    break;

                case "SearchAnalystQualification":
                    obj.ViewName = "aqual.vwLKPSearchAnalystQualification";
                    obj.ID = "QualificationID";
                    obj.Code = "RequestCode";
                    obj.Name = "UserName";
                    break;

                case "WRKREFSampleMaterials":
                    obj.ViewName = "vwWORKRefSampleOWMaterials";
                    obj.ID = "MAT_ID";
                    obj.Name = "NAME";
                    obj.Code = "SAM_MATCODE";
                    break;

                case "WorkRefPacks":
                    obj.ViewName = "lkpWorkRefPacks";
                    obj.ID = "REF_SAMPACK_ID";
                    obj.Name = "PackRemQty";
                    obj.Code = "STD_BATCH_NUMBER";
                    break;

                case "GetAllCategoryItems":
                    obj.ViewName = "root.vwAllCategoryItems";
                    obj.ID = "CatItemID";
                    obj.Code = "CatItemCode";
                    obj.Name = "CatItem";
                    obj.PlantFilter = false;
                    break;

                case "GetAllEquipmentsInstruments":
                    obj.ViewName = "dbo.vwAllEquipmentsInstruments";
                    obj.ID = "EQUIPMENT_ID";
                    obj.Code = "EQP_USER_CODE";
                    obj.Name = "TITLE";
                    break;

                case "GetMaintenanceSchedule":
                    obj.ViewName = "calib.vwMaintenanceSchedule";
                    obj.ID = "SCHEDULE_ID";
                    obj.Code = "SCHEDULE_NUMBER";
                    obj.Name = "ScheduleType";
                    break;

                case "GetMaintenanceEquipments":
                    obj.ViewName = "calib.vwMaintenanceEquipments";
                    obj.ID = "EqpMaintID";
                    obj.Code = "EquipmentUserCode";
                    obj.Name = "EqpTitle";
                    break;

                case "GetRefEqpOthOccupancyDetails":
                    obj.ViewName = "lims.vwLkpRefOccupancyDetails";
                    obj.ID = "ID";
                    obj.Code = "Code";
                    obj.Name = "Name";
                    obj.ExtColName = "EntityRefNumber";
                    break;
            }

            return obj;
        }



        public static UserDetailsLists getUserList<T>(string param, List<T> list)
        {
            var lst = new UserDetailsLists();

            List<SingleBO> usrRoleIDList = list.Select((x, index) => new SingleBO { ID = Convert.ToInt32(x.GetType().GetProperty(param).GetValue(list[index])) }).ToList();
            usrRoleIDList = usrRoleIDList.GroupBy(x => x.ID, (key, group) => group.First()).Where(x => x.ID > default(int)).ToList();

            if (usrRoleIDList.Count > 0)
                lst = CommonStaticMethods.PostApiConnectionData<UserDetailsLists>("GetDetails", usrRoleIDList);

            return lst;
        }

        public static UserDetailsLists getUserList<T, T1>(string param, List<T> list, string extParam = "", List<T1> extList = default(List<T1>))
        {
            var lst = new UserDetailsLists();

            List<SingleBO> usrRoleIDList, usrRoleIDList1 = new List<SingleBO>();
            usrRoleIDList = CommonStaticMethods.getUserRoleIDList<T>(list, param);
            if (extList.Count > 0)
            {
                usrRoleIDList1 = CommonStaticMethods.getUserRoleIDList<T1>(extList, extParam);
                usrRoleIDList = usrRoleIDList.Concat(usrRoleIDList1).ToList();
                usrRoleIDList = usrRoleIDList.GroupBy(x => x.ID, (key, group) => group.First()).Where(x => x.ID > default(int)).ToList();
            }

            if (usrRoleIDList.Count > 0)
                lst = CommonStaticMethods.PostApiConnectionData<UserDetailsLists>("GetDetails", usrRoleIDList);

            return lst;
        }

        private static List<SingleBO> getUserRoleIDList<T>(List<T> list, string param)
        {
            List<SingleBO> usrRoleIDList = list.Select((x, index) => new SingleBO { ID = Convert.ToInt32(x.GetType().GetProperty(param).GetValue(list[index])) }).ToList();
            return usrRoleIDList.GroupBy(x => x.ID, (key, group) => group.First()).Where(x => x.ID > default(int)).ToList();
        }

        public static T GetApiConnectionData<T>(string RouteString, string baseUrl = "CommonApiUrl")
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings[baseUrl]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage resp = client.GetAsync(RouteString).Result;

            return ChangeObjectType<T>(resp);
        }

        public static T PostApiConnectionData<T>(string RouteString, object obj, string baseUrl = "CommonApiUrl")
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings[baseUrl]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage resp = client.PostAsJsonAsync(RouteString, obj).Result;

            return ChangeObjectType<T>(resp);
        }

        public static T ChangeObjectType<T>(HttpResponseMessage resp)
        {
            object val = default(object);

            if (resp.IsSuccessStatusCode)
                val = resp.Content.ReadAsAsync<T>().Result;

            return (T)Convert.ChangeType(val, typeof(T));
        }

        public static DocumentCountList getDocumentCount<T>(string param, List<T> genericList, UploadFileBO obj)
        {
            var lst = new DocumentCountList();

            obj.Lst = genericList.Select((x, index) => new SingleBO { ID = Convert.ToInt32(x.GetType().GetProperty(param).GetValue(genericList[index])) }).ToList();

            if (obj.Lst.Count > 0)
                lst = CommonStaticMethods.PostApiConnectionData<DocumentCountList>("HaveDocuments", obj, "DMS_URL");

            return lst;
        }

        public static string ConvertType(object o, string dataType)
        {

            if (o == default(object))
                return string.Empty;

            switch (dataType)
            {
                case "DateTime":
                case "System.Nullable`1[[System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]":
                    return Convert.ToDateTime(o).ToString("dd-MMM-yyyy HH:mm");
                default:
                    return Convert.ToString(o);
            }


            //return Convert.ChangeType(o, t);
        }

        public static void GetPlaceHolder(string entityCode, GetARDSPlaceholderData lst)
        {
            foreach (var item in lst.PlaceholderList)
            {
                if (GetObject(entityCode, lst) != null)
                {
                    PropertyInfo T1 = GetObject(entityCode, lst).GetType().GetProperty(item.ColumnName);
                    if (T1 != null)
                        item.PlaceholderValue = CommonStaticMethods.ConvertType(T1.GetValue(GetObject(entityCode, lst), null), T1.PropertyType.FullName);
                }
            }

        }

        private static object GetObject(string entityCode, GetARDSPlaceholderData lst)
        {
            if (entityCode == "QCSAMPASYS" || entityCode == "SPEC_VALID" || entityCode == "OOSPROC")
                return lst.SampleAnalysisValues;
            else if (entityCode == "ENGGMNT" || entityCode == "CALIB_VALIDATION")
                return lst.CalibrationValues;
            return new object();

        }

        public static void PrepareARDSTables(ReportDetails obj)
        {
            DataTable dt = new DataTable();
            obj.TablePlaceholderLst = new AsposeReportList();
            if (obj.ChemicalList != null && obj.ChemicalList.Count > 0)
            {
                AsposeReport item = new AsposeReport();
                item.PlaceHolder = "@#Chemical_Details#@";
                item.Table = ToDataTable(obj.ChemicalList);
                obj.TablePlaceholderLst.Add(item);
            }
            if(obj.RefChemicalList != null && obj.RefChemicalList.Count > 0)
            {
                AsposeReport item = new AsposeReport();
                item.PlaceHolder = "@#Ref_Chemical_Details#@";
                item.Table = ToDataTable(obj.RefChemicalList);
                obj.TablePlaceholderLst.Add(item);
            }

            //foreach (var resItem in obj.ResultSets)
            //{
            //    AsposeReport item = new AsposeReport();
            //    item.PlaceHolder = "@#" + resItem.Title + "#@";
            //    item.Table = new DataTable();
            //    foreach (var preItm in obj.Preparations)
            //    {
            //        dt = new DataTable();
            //        DataView dv = new DataView(obj.ImpurityData);
            //        string condition = string.Format("ResultSetID = {0} AND PreparationID = {1}", resItem.ResultSetID, preItm.PreparationID);
            //        dv.RowFilter = condition;
            //        dt = dv.ToTable();
            //        dt.Columns.Remove("ResultSetID");
            //        dt.Columns.Remove("PreparationID");

            //        DataRow newRow = dt.NewRow();
            //        newRow[0] = preItm.PreparationName;
            //        dt.Rows.InsertAt(newRow, 0);
            //        if (item.Table.Columns.Count == 0)
            //        {
            //            foreach (DataColumn column in dt.Columns)
            //                item.Table.Columns.Add(column.ColumnName);
            //        }
            //        foreach (DataRow dr in dt.Rows)
            //            item.Table.Rows.Add(dr.ItemArray);
            //    }
            //    obj.TablePlaceholderLst.Add(item);
            //}

            foreach (var resItem in obj.TablePlaceholders)
            {
                AsposeReport item = new AsposeReport();
                item.PlaceHolder = resItem.PlaceHolder;
                item.Table = new DataTable();

                dt = new DataTable();
                DataView dv = new DataView(obj.ImpurityData);
                string condition = string.Format("DetailID = {0} ", resItem.DetailID);
                dv.RowFilter = condition;
                dt = dv.ToTable();
                dt.Columns.Remove("DetailID");

                if (item.Table.Columns.Count == 0)
                {
                    foreach (DataColumn column in dt.Columns)
                        item.Table.Columns.Add(column.ColumnName);
                }
                foreach (DataRow dr in dt.Rows)
                    item.Table.Rows.Add(dr.ItemArray);
                obj.TablePlaceholderLst.Add(item);
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = CommonStaticMethods.ConvertType(Props[i].GetValue(item, null), Props[i].PropertyType.FullName);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static string ConvertToFriendlyDecimal(decimal? val)
        {
            bool gotPrecision = false;
            string val1 = Convert.ToString(val);

            string retValue = string.Empty;

            if (val1.IndexOf('.') > 0)
            {
                int position = 0;
                string[] parts = val1.Split(new char[] { '.' });
                string precision = string.Empty;

                char[] charArray = parts[1].ToCharArray();

                Array.Reverse(charArray);

                for (int i = 0; i < charArray.Length; i++)
                {
                    if (charArray[i] != '0')
                    {
                        position = i;
                        gotPrecision = true;
                        break;
                    }
                }

                if (gotPrecision)
                    position = (parts[1].Length - position);

                if (position > 0)
                    precision = parts[1].Substring(0, position);

                if (!string.IsNullOrEmpty(precision))
                    retValue = string.Format("{0}.{1}", parts[0], precision);
                else
                    retValue = parts[0];
            }
            else
                retValue = val1;

            return retValue;
        }

        public static bool TryTruncateDecimal(object initialDecimal, out decimal finalDecimal)
        {
            finalDecimal = default(decimal);
            try
            {
                finalDecimal = TruncateDecimal(initialDecimal);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static Decimal TruncateDecimal(object initialDecimal)
        {
            if (initialDecimal == null)
                return 0;
            decimal finalDecimal = default(decimal);
            Decimal.TryParse(initialDecimal.ToString(), out finalDecimal);
            if (finalDecimal != default(decimal))
            {
                int value = 5;
                int decimalPoint = 1;
                for (int i = value; i > 0; i--)
                    decimalPoint = decimalPoint * 10;

                finalDecimal = Math.Truncate(finalDecimal * decimalPoint) / decimalPoint;
            }
            else
                finalDecimal = Math.Truncate(finalDecimal);
            return finalDecimal;
        }

        public static string ToJson<T>(T obj)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(obj);
        }

        public static string GetSymbol(string value)
        {
            switch (value)
            {
                case "&#8451;":
                    return "℃";
            }

            return value;
        }
    }
}