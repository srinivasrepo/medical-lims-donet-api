using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SampleAnalysis;
using MedicalLIMSApi.Core.Entities.UtilUploads;
using MedicalLIMSApi.Core.Interface.SampleAnalysis;
using MedicalLIMSApi.Web.App_Start;
using MedicalLIMSApi.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.SampleAnalysis
{
    [LIMSAuthorization]
    public class SampleAnalysisController : ApiController
    {
        ISampleAnalysis db;

        public SampleAnalysisController(ISampleAnalysis db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("SearchSampleAnalysis")]
        public SearchResults<AnalysisSearchResult> SearchSampleAnalysis(SearchSampleAnalysisBO obj)
        {
            return db.SearchSampleAnalysis(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetAnalysisHeaderInfo")]
        public AnalysisHeader GetAnalysisHeaderInfo(string encSioID)
        {
            int sioID = default(int);
            sioID = CommonStaticMethods.Decrypt<int>(encSioID);
            return db.GetAnalysisHeaderInfo(sioID, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserRoleID);
        }

        [HttpGet]
        [Route("GetAnalysisTypes")]
        public AnalysisTypeList GetAnalysisTypes()
        {
            return db.GetAnalysisTypes();
        }

        [HttpPost]
        [Route("GetBlockByPlantID")]
        public BlockList GetBlocks(GetBolckList obj)
        {
            return db.GetBlocks(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID);
        }

        [HttpGet]
        [Route("GetSupplierCOADetails")]
        public SupplierCOADetails GetSupplierCOADetails(string encSioID)
        {
            int sioID = default(int);
            sioID = CommonStaticMethods.Decrypt<int>(encSioID);
            return db.GetSupplierCOADetails(sioID, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserRoleID);
        }

        [HttpPost]
        [Route("ManageSupplierCOADetails")]
        public RecordActionDetails ManageSupplierCOADetails(SupplierCOADetails obj)
        {
            TransResults tr = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            SingleBO siObj = new SingleBO();
            UploadFileBO upObj = new UploadFileBO();
            RecordActionDetails retObj = new RecordActionDetails();
            upObj.AppCode = tr.ApplicationType;
            upObj.EntityCode = "QCSAMPASYS";
            upObj.Section = "COASAMANA";
            siObj.ID = obj.SamAnaID;
            upObj.Lst = new SIngleBOList();
            upObj.Lst.Add(siObj);

            DocumentCountList docList = CommonStaticMethods.PostApiConnectionData<DocumentCountList>("HaveDocuments", upObj, "DMS_URL");
            if (docList == null || docList.Count == 0 || !(docList[0].NoOfFiles > default(int)))
            {
                retObj.ReturnFlag = "UPLOAD";
                return retObj;
            }

            return db.ManageSupplierCOADetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetAnalysisSpecifications")]
        public GetSpecificationsBOList GetAnalysisSpecifications(string encEntityActID, string entityCode)
        {
            return db.GetAnalysisSpecifications(CommonStaticMethods.Decrypt<int>(encEntityActID), entityCode);
        }

        [HttpPost]
        [Route("MangeSampleAnalysis")]
        public MangeSampleAnalysisResult MangeSampleAnagysis(MangeSampleAnalysis obj)
        {
            MangeSampleAnalysisResult data = new MangeSampleAnalysisResult();
            TransResults tr = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            data = db.MangeSampleAnalysis(obj, tr);

            if (data.ReturnFlag == "OK" && data.RptList != null && data.RptList.Count > 0)
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();
                dmsObj.EntActID = obj.SioID;
                dmsObj.EntityCode = "QCSAMPASYS";
                dmsObj.ReferenceNumber = obj.ReferenceNumber;
                dmsObj.List = data.RptList;
                data.ReturnFlag = FileUploadUtility.UploadReportInfoToDMS(dmsObj);
                if (data.ReturnFlag == "SUCCESS")
                    data.ReturnFlag = "OK";
            }

            if (obj.SamplerID > default(int) && data.ReturnFlag == "OK")
            {
                try
                {
                    SamplerDetails samObj = new SamplerDetails();
                    samObj = db.GetSamplerDetails(obj.SioID);

                    if (string.IsNullOrEmpty(samObj.SampledBy) || samObj.ReviewedBy == null || samObj.ReviewedBy == default(int))
                        return data;

                    samObj.PlantID = tr.PlantID;
                    samObj.DeptID = tr.DeptID;
                    samObj.SioID = obj.SioID;

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["EFORMATS_API_URL"]);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage resp = client.PostAsJsonAsync("InsertSampleToolCleaning", samObj).Result;

                    data.ReturnFlag = resp.Content.ReadAsAsync<string>().Result;

                }
                catch (Exception ex)
                {

                }
            }

            return data;
        }

        [HttpPost]
        [Route("GetAssignedDocsBySpecID")]
        public GetAssignedDocsBySpecIDList GetAssignedDocsBySpecID(GetArdsAssignDoc obj)
        {
            return db.GetAssignedDocsBySpecID(obj);
        }

        [HttpGet]
        [Route("ARDSGetAssignedDocs")]
        public ARDSGetAssignedDocsList ARDSGetAssignedDocs(string encEntActID, string sourceCode)
        {
            return db.ARDSGetAssignedDocs(CommonStaticMethods.Decrypt<int>(encEntActID), sourceCode);
        }

        [HttpPost]
        [Route("ARDSManageRequest")]
        public TransResultApprovals ARDSManageRequest(ARDSManageRequest obj)
        {
            return db.ARDSManageRequest(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetSamplingInfo")]
        public GetSamplingInfo GetSamplingInfo(string encSioID)
        {
            return db.GetSamplingInfo(CommonStaticMethods.Decrypt<int>(encSioID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserID);
        }

        [HttpPost]
        [Route("ARDSDiscardPrintRequest")]
        public string ARDSDiscardPrintRequest(DiscardPrintrequestBO obj)
        {
            return db.ARDSDiscardPrintRequest(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetIssuedContainerDetails")]
        public SampleContainerDetails GetIssuedContainerDetails(GetSamplePack obj)
        {
            return db.GetIssuedContainerDetails(obj);
        }

        [HttpPost]
        [Route("ManageSamplePacks")]
        public string ManageSamplePacks(ManageSamplePack obj)
        {
            return db.ManageSamplePacks(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetAnalysisTestBySioID")]
        public GetAnalysisTests GetAnalysisTest(string encEntityActID, string entityCode)
        {
            GetAnalysisTests obj = new GetAnalysisTests();
            DocumentCountList lst = new DocumentCountList();
            obj = db.GetAnalysisTest(CommonStaticMethods.Decrypt<int>(encEntityActID), entityCode, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserID);

            //UploadFileBO upObj = new UploadFileBO();
            //upObj.AppCode = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().ApplicationType;
            //upObj.EntityCode = entityCode == "CONT_WISE_ANA" ? "QCSAMPASYS" : (entityCode == "OOS_HYPOTEST" || entityCode == "OOS_TEST") ? "OOSPROC" : entityCode;
            //upObj.Section = "TSTDOCS";
            //lst = CommonStaticMethods.getDocumentCount<GetAnalysisTestBO>("SamAnaTestID", obj.Lst, upObj);
            //foreach(var item in lst)
            //{
            //    for (int i = 0; i < obj.Lst.Count; i++)
            //    {
            //        if (item.ID == obj.Lst[i].SamAnaTestID && item.NoOfFiles > default(int))
            //        {
            //            obj.Lst[i].HasDocuments = true;
            //            break;
            //        }
            //        else if (item.ID == obj.Lst[i].SamAnaTestID && item.NoOfFiles == default(int))
            //        {
            //            obj.Lst[i].HasDocuments = false;
            //            break;
            //        }
            //    }
            //}
            return obj;
        }

        [HttpPost]
        [Route("GetSampleTestInfo")]
        public SampleTestInfo GetSampleTestInfo(GetSampleAnaTestBO obj)
        {
            return db.GetSampleTestInfo(obj);
        }

        [HttpPost]
        [Route("GetResultStatus")]
        public TestResultValues GetResultStatus(TestValues obj)
        {
            return db.GetResultStatus(obj);
        }

        [HttpPost]
        [Route("UpdateTestResults")]
        public TestRetVal UpdateTestResults(UpdTestResults obj)
        {
            return db.UpdateTestResults(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SaveAnalysis")]
        public MangeSampleAnalysisResult SaveAnalysis(AnalysisRemarks obj)
        {
            MangeSampleAnalysisResult retObj = new MangeSampleAnalysisResult();
            TransResults tr = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            retObj = db.SaveAnalysis(obj, tr);

            //if (retObj.ReturnFlag == "OK" && retObj.List != null && retObj.List.Count > 0)
            //{
            //    UploadMergeFile updObj = new UploadMergeFile();
            //    updObj.EntActID = obj.SioID;
            //    updObj.EntityCode = retObj.EntityCode;
            //    updObj.PlantCode = retObj.PlantCode;
            //    updObj.SectionCode = "RAWDATA_SDMS";
            //    updObj.DeptCode = retObj.DeptCode;
            //    updObj.LoginID = retObj.LoginID;
            //    updObj.Role = retObj.RoleName;
            //    updObj.AppCode = tr.ApplicationType;
            //    updObj.List = retObj.List;
            //    updObj.InsertSection = obj.EntityCode;
            //    updObj.ReferenceNumber = obj.ReferenceNumber;
            //    if (obj.SourceCode == "CONT_WISE_ANA")
            //        updObj.FileName = string.Format("ContainerWiseRawData_{0}", obj.ReferenceNumber);
            //    else
            //        updObj.FileName = string.Format("MergedRawData_{0}", obj.ReferenceNumber);


            //    string retMsg = CommonStaticMethods.PostApiConnectionData<string>("MergeSDMSFiles", updObj, "DMS_URL");
            //    if (retMsg == "Success")
            //        retObj.ReturnFlag = "OK";
            //}

            return retObj;
        }

        [HttpPost]
        [Route("RaiseDeviation")]
        public string RaiseDeviation(Deviation obj)
        {
            return db.RaiseDeviation(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetInstrumentsForTest")]
        public GetInstrumentTitlesBOList GetInstrumentsForTest(GetInstrumentsForTestBO obj)
        {
            return db.GetInstrumentsForTest(obj);
        }

        [HttpGet]
        [Route("GetEQPUGetEqpTypeCode")]
        public GetEQPUGetEqpTypeCode GetEQPUGetEqpTypeCode(int eqpID)
        {
            return db.GetEQPUGetEqpTypeCode(eqpID);
        }

        [HttpGet]
        [Route("GetCumulativeCount")]
        public int EQPUGetCumulativeCount(int columnID)
        {
            return db.EQPUGetCumulativeCount(columnID);
        }

        [HttpPost]
        [Route("GetTestInstruments")]
        public GetOccupancyData GetTestInstruments(GetInstrumentsForTestBO obj)
        {
            return db.GetTestInstruments(obj);
        }

        [HttpPost]
        [Route("InsertNUpdateInstruments")]
        public GetTestInstrumentDetails InsertNUpdateInstruments(InsertNUpdateInstrumentsBO obj)
        {
            return db.InsertNUpdateInstruments(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("DeleteInstrumnetDetailsByID")]
        public GetTestInstrumentDetails DeleteSpecInstruments(DeleteInstrumentBO obj)
        {
            return db.DeleteSpecInstruments(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetInstrumnetDetailsByID")]
        public GetAnaOccuInstrumentsBO GetAnaInstOccDetails(GetInstrumentDetailsByIDBO obj)
        {
            return db.GetAnaInstOccDetails(obj);
        }

        [HttpPost]
        [Route("ManageTestSampleRRTValues")]
        public GetRRTValuesBO ManageSampleRRTValuesBO(GetSampleRRTValuesBO obj)
        {
            return db.ManageSampleRRTValuesBO(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetARDSInputs")]
        public GetARDSInputDetails GetARDSInputs(string encSamAnaTestID, string sourceCode)
        {
            GetARDSInputDetails obj = new GetARDSInputDetails();
            obj = db.GetARDSInputs(CommonStaticMethods.Decrypt<int>(encSamAnaTestID), sourceCode, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            if (obj.IsNewReq && obj.EntityCode != "SPEC_VALID" && obj.EntityCode != "CALIB_VALIDATION")
            {
                ArdsBO arObj = new ArdsBO();
                arObj.ArdsExecID = CommonStaticMethods.Decrypt<int>(encSamAnaTestID);
                arObj.EntityCode = obj.EntityCode;
                GeneratePlaceHolders(arObj);

                obj = db.GetARDSInputs(CommonStaticMethods.Decrypt<int>(encSamAnaTestID), sourceCode, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
            }
            return obj;
        }

        [HttpPost]
        [Route("SaveInputValues")]
        public GetSavedInputData SaveInputValues(InputValues obj)
        {
            if (obj.ImpurityValues != null && obj.ImpurityValues.Count > 0)
                obj.ImpurityXML = CommonStaticMethods.Serialize<FormulaDependentList>(obj.ImpurityValues);
            return db.SaveInputValues(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetAdditionalTest")]
        public AdditionalTestList GetAdditionalTest(AdditionalTestBODetails obj)
        {
            return db.GetAdditionalTest(obj);
        }

        [HttpPost]
        [Route("ManageAdditionalTest")]
        public GetAddTests ManageAdditionalTest(MngAdditionalTest obj)
        {
            return db.ManageAdditionalTest(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("DeleteAdditionalTest")]
        public GetAddTests DeleteAdditionalTest(int addTestID)
        {
            return db.DeleteAdditionalTest(addTestID, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetDeviationDescription")]
        public string GetDeviationDescription(string entityCode, string dcActionCode)
        {
            return db.GetDeviationDescription(entityCode, dcActionCode);
        }

        [HttpPost]
        [Route("UpdateFinalRemarks")]
        public MangeSampleAnalysisResult UpdateFinalRemarks(UpdFinalRemarks obj)
        {
            return db.UpdateFinalRemarks(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetFormulaDependentDetails")]
        public ExecuteFormula GetFormulaDependentDetails(GetFormualDetails obj)
        {
            return db.GetFormulaDependentDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ExecuteFormula")]
        public string ExecuteFormula(GetFormualDetails obj)
        {
            ExecuteFormula data = new ExecuteFormula();
            data = db.GetFormulaDependentDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            return CommonStaticMethods.ConvertToFriendlyDecimal(Convert.ToDecimal(CommonStaticMethods.PostApiConnectionData<string>("ExecuteFormula", data)));
        }

        [HttpPost]
        [Route("ExecuteMultipleFormulas")]
        public FormulaDependentList ExecuteMultipleFormulas(GetFormualDetails obj)
        {
            ExecuteFormula data = new ExecuteFormula();
            data = db.GetFormulaDependentDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            foreach (FormulaDepenDetails item in data.ImpurityLst)
            {
                data.Formula = item.CalcFormula;
                if (item.Value != "ND" && item.Value != "DR")
                    item.Value = CommonStaticMethods.ConvertToFriendlyDecimal(Convert.ToDecimal(CommonStaticMethods.PostApiConnectionData<string>("ExecuteFormula", data)));
                item.ItemOrder = 1;
                item.DetailID = obj.DetailID;
                item.InputCode = data.Code;
                item.ActualValue = item.Value;
            }

            return data.ImpurityLst;

        }

        [HttpPost]
        [Route("ExecuteSystemFormulas")]
        public GetSavedInputData ExecuteSystemFormulas(GetFormualDetails obj)
        {
            ExecuteFormula data = new ExecuteFormula();
            GetSavedInputData output = new GetSavedInputData();
            data = db.GetFormulaDependentDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            InputValues iv = new InputValues();
            iv.ArdsSourceCode = obj.SourceCode;
            iv.SamAnaTestID = obj.SamAnaTestID;
            iv.DetailID = obj.DetailID;
            iv.InitTime = obj.InitTime;
            TableResultExecList lst = new TableResultExecList(); ;
            TableResultExecBO objimp = null;
            if (data.ImpurityLst != null && data.ImpurityLst.Count > 0 && data.ResultFlag == "OK")
            {

                foreach (FormulaDepenDetails item in data.ImpurityLst)
                {
                    objimp = new TableResultExecBO();
                    data.Formula = item.CalcFormula;
                    if (item.Value != "ND" && item.Value != "DR" && !string.IsNullOrEmpty(data.Formula))
                        item.Value = CommonStaticMethods.ConvertToFriendlyDecimal(Convert.ToDecimal(CommonStaticMethods.PostApiConnectionData<string>("ExecuteFormula", data)));
                    else
                        item.Value = "0";
                    item.ItemOrder = 1;
                    item.DetailID = obj.DetailID;
                    item.InputCode = data.Code;
                    item.ActualValue = item.Value;

                    objimp.DetailID = obj.DetailID;
                    objimp.Code = item.InputCode;
                    objimp.SysCode = item.InputCode;
                    objimp.Description = item.InputDescription;
                    objimp.CalcFormula = item.CalcFormula;
                    objimp.InputValue = item.Value;
                    objimp.ImpurityValueID = item.ImpurityValueID;
                    objimp.Type = "REG";
                    lst.Add(objimp);


                }
                iv.ImpurityXML = CommonStaticMethods.Serialize<FormulaDependentList>(data.ImpurityLst);
                iv.Value = data.ImpurityLst[0].Value;
                iv.ActValue = iv.Value;
            }
            else if (data.ResultFlag == "OK")
            {
                if (!string.IsNullOrEmpty(data.Formula))
                    iv.Value = CommonStaticMethods.ConvertToFriendlyDecimal(Convert.ToDecimal(CommonStaticMethods.PostApiConnectionData<string>("ExecuteFormula", data)));
                else
                    iv.Value = "0";
                iv.ActValue = data.Value;
                objimp = new TableResultExecBO();
                objimp.DetailID = obj.DetailID;
                objimp.Code = data.Code;
                objimp.SysCode = data.Code;
                objimp.CalcFormula = data.Formula;
                objimp.InputValue = iv.Value;
                objimp.Description = data.Description;
                objimp.Type = "REG";
                lst.Add(objimp);
            }

            if (data.ResultFlag == "OK")
            {
                output = SaveInputValues(iv);
                output.SysFormulas = lst;
            }
            else
                output.ReturnFlag = data.ResultFlag;
            return output;

        }


        private FormulaDependentList GetDependentValues(List<FormulaDepenDetails> lst)
        {
            FormulaDependentList dependentFormlas = new FormulaDependentList();

            foreach (FormulaDepenDetails itemS in lst)
                dependentFormlas.Add(itemS);

            return dependentFormlas;
        }


        [HttpPost]
        [Route("ConfirmEArds")]
        public GetConfData ConfirmEArds(ConfirmRawData obj)
        {
            GetConfData retObj = new GetConfData();
            TransResults tr = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            retObj = db.ConfirmEArds(obj, tr);
            //if (retObj.ReturnFlag == "OK" && retObj.List != null && retObj.List.Count > 0)
            //{
            //    UploadMergeFile updObj = new UploadMergeFile();
            //    updObj.EntActID = obj.SamAnaTestID;
            //    updObj.EntityCode = retObj.EntityCode;
            //    updObj.PlantCode = retObj.PlantCode;
            //    updObj.SectionCode = "RAWDATA_SDMS";
            //    updObj.DeptCode = retObj.DeptCode;
            //    updObj.LoginID = retObj.LoginID;
            //    updObj.Role = retObj.RoleName;
            //    updObj.AppCode = tr.ApplicationType;
            //    updObj.List = retObj.List;
            //    updObj.InsertSection = "TSTDOCS";
            //    updObj.ReferenceNumber = retObj.ReferenceNumber;
            //    updObj.FileName = string.Format("MergedRawData_{0}", obj.SamAnaTestID);


            //    string retMsg = CommonStaticMethods.PostApiConnectionData<string>("MergeSDMSFiles", updObj, "DMS_URL");
            //    if (retMsg == "Success")
            //        retObj.ReturnFlag = "OK";
            //}
            return retObj;
        }

        [HttpPost]
        [Route("ManageIncludeExcludeTest")]
        public GetIncludeExcludeTestBO ManageIncludeExcludeTest(IncludeExcludeTestBOItems obj)
        {
            return db.ManageIncludeExcludeTest(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetSDMSDataBySamAnaTestID")]
        public SDMSInputValuesBOList GetSDMSDataBySamAnaTestID(string encSamAnaTestID)
        {
            SDMSInputValuesBOList lst = new SDMSInputValuesBOList();
            lst = db.GetSDMSDataBySamAnaTestID(CommonStaticMethods.Decrypt<int>(encSamAnaTestID));

            try
            {
                foreach (var item in lst)
                {
                    item.obj = new SDMSDetailsBO();
                    item.obj = CommonStaticMethods.Deserialize<SDMSDetailsBO>(item.XMLData);
                }
            }
            catch (Exception ex) { }

            return lst;
        }

        [HttpPost]
        [Route("ManageSDMSInputDetails")]
        public GetSDMSInputDetailsBO ManageSDMSInputDetails(ManageSDMSInputDetailsBO obj)
        {
            return db.ManageSDMSInputDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }




        [HttpPost]
        [Route("GetMappingInfo")]
        public TestDetails GetMappingInfo(GetMappingInfo obj)
        {

            var lst = new TestDetails();
            lst = db.GetMappingInfo(obj);
            return lst;
        }

        [HttpGet]
        [Route("GetAnalysisTypes")]
        public GetAnalysisTypesList GetAnalysisTypes(int matCatID)
        {
            return db.GetAnalysisTypes(matCatID);
        }

        [HttpGet]
        [Route("GetSampleSources")]
        public GetSampleSourcesList GetSampleSources()
        {
            return db.GetSampleSources();
        }

        [HttpPost]
        [Route("ContainerWiseMaterials")]
        public ContainerWiseMaterialsResults ContainerWiseMaterials(ContainerWiseMaterials obj)
        {
            return db.ContainerWiseMaterials(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageIsFinalFormula")]
        public ManageUpdateFormulaResultFlagBO ManageIsFinalFormula(ManageIsFinalFormulaBO obj)
        {
            return db.ManageIsFinalFormula(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetTestByCategory")]
        public GetTestByCategory GetTestByCategory(TestCatBO obj)
        {
            return db.GetTestByCategory(obj);
        }

        [HttpGet]
        [Route("GetContainerWiseAnalysis")]
        public GetContainerAnalysisDetails GetContainerWiseAnalysis(string encSioID)
        {
            return db.GetContainerWiseAnalysis(CommonStaticMethods.Decrypt<int>(encSioID));
        }

        [HttpPost]
        [Route("SaveContainerArdsDetails")]
        public RecordActionDetails SaveContainerArdsDetails(SaveContainerArdsDetails obj)
        {
            return db.SaveContainerArdsDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SendTestForReview")]
        public TestRetVal SendTestForReview(SendForReview obj)
        {
            return db.SendTestForReview(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("ViewARDSMasterDocument")]
        public string ViewARDSMasterDocument(int documentID)
        {
            string path = string.Empty;
            path = CommonStaticMethods.GetApiConnectionData<string>(string.Format("ViewARDSMasterDocument?AppCode={0}&DocumentID={1}", MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().ApplicationType, documentID), "DMS_URL");
            return path;
        }

        [HttpGet]
        [Route("ViewARDSPrintDocument")]
        public string ViewARDSPrintDocument(int dmsID, string plantOrgCode)
        {
            string path = string.Empty;
            path = CommonStaticMethods.GetApiConnectionData<string>(string.Format("ViewARDSPrintDocument?AppCode={0}&DMSId={1}&plantOrgCode={2}", MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().ApplicationType, dmsID, plantOrgCode), "DMS_URL");
            return path;
        }

        [HttpPost]
        [Route("AddArdsReviewComment")]
        public ArdsReviewBO AddArdsReviewComment(ManageArdsReview obj)
        {
            return db.AddArdsReviewComment(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SkipPacksFromAnalysis")]
        public RecordActionDetails SkipPacksFromAnalysis(SkipPacksBO obj)
        {
            return db.SkipPacksFromAnalysis(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SkipInpurFieldFromExecution")]
        public ManageUpdateFormulaResultFlagBO SkipInpurFieldFromExecution(ManageIsFinalFormulaBO obj)
        {
            return db.SkipInpurFieldFromExecution(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetPackTestsForSendToReview")]
        public List<PacksSendToReview> GetPackTestsForSendToReview(string encSioID, int specCatID)
        {
            return db.GetPackTestsForSendToReview(CommonStaticMethods.Decrypt<int>(encSioID), specCatID);
        }

        [HttpGet]
        [Route("GetSTPCommonDataforMapping")]
        public SectionDetailList GetSTPCommonDataforMapping(string encEntActID)
        {
            return db.GetSTPCommonDataforMapping(CommonStaticMethods.Decrypt<int>(encEntActID));
        }

        [HttpPost]
        [Route("ManageSTPCommonData")]
        public string ManageSTPCommonDataMapping(ManageSTPCommonData obj)
        {
            return db.ManageSTPCommonDataMapping(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }


        [HttpPost]
        [Route("ManageImpurityBasicInfo")]
        public string ManageImpurityBasicInfo(ManageImpurityBasicInfoBO obj)
        {
            return db.ManageImpurityBasicInfo(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetUnknownImpurities")]
        public GetUnknownImpuritiesBO GetUnknownImpurities(int ardsExecID)
        {
            return db.GetUnknownImpurities(ardsExecID);
        }

        [HttpPost]
        [Route("ManageImpuritySDMSDetails")]
        public string ManageImpuritySDMSDetails(ManageUnKnownImpSDMSDetailsBOList list)
        {
            string XMLString = string.Empty;

            if (list.Count > 0)
                XMLString = CommonStaticMethods.Serialize(list);

            return db.ManageImpuritySDMSDetails(XMLString, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageMultipleFormulaValues")]
        public GetMulitpleValueExecBO ManageMutipleFormulaValue(ManageMultFormulaValueBO obj)
        {
            return db.ManageMutipleFormulaValue(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetDynamicFormulaInfo")]
        public DynamicFormulaExecBO GetDynamicFormulaInfo(GetFormualDetails obj)
        {
            return db.GetDynamicFormulaInfo(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetDyncFormulaDependentData")]
        public FormulaDependentList GetDyncFormulaDependentData(GetFormualDetails obj)
        {
            return db.GetDyncFormulaDependentData(obj);
        }

        [HttpPost]
        [Route("ExecuteDynamicFormulaData")]
        public string ExecuteDynamicFormulaData(PostExecDynamicFormula obj)
        {
            ExecuteFormula data = new ExecuteFormula();
            data = db.ExecuteDynamicFormulaData(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
            return CommonStaticMethods.ConvertToFriendlyDecimal(Convert.ToDecimal(CommonStaticMethods.PostApiConnectionData<string>("ExecuteFormula", data)));
        }

        [HttpGet]
        [Route("DeleteImpurity")]
        public string DeleteImpurity(int impurityID)
        {
            return db.DeleteImpurity(impurityID, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }


        private string GeneratePlaceHolders(ArdsBO obj)
        {
            ReportDetails rptObj = new ReportDetails();
            rptObj = FileUploadUtility.GetCalibrationReportDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            rptObj.LoginID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().LoginID;

            CommonStaticMethods.GetPlaceHolder(obj.EntityCode, rptObj.PlaceHolders);

            return UpdateArdsPlaceholders(Convert.ToString(obj.ArdsExecID), obj.EntityCode, rptObj.PlaceHolders.PlaceholderList);
        }
        string UpdateArdsPlaceholders(string encEntActID, string entityCode, PlaceholderList lst)
        {
            UpdateARDSPlaceholder obj = new UpdateARDSPlaceholder();
            obj.EncEntActID = encEntActID;
            obj.EntityCode = entityCode;
            obj.Lst = lst;
            obj.Type = "SYSTEM_VALUE";
            return FileUploadUtility.UpdateArdsPlaceholders(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SwitchARDSMode")]
        public TestRetVal SwitchARDSMode(SwitchArdsMode obj)
        {
            return db.SwitchARDSMode(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SkipUnknownImpurities")]
        public ManageUpdateFormulaResultFlagBO SkipUnknownImpurities(ManageIsFinalFormulaBO obj)
        {
            return db.SkipUnknownImpurities(obj);
        }

        [HttpPost]
        [Route("ManageColumnInfo")]
        public ColumnsList ManageColumnInfo(ManageColumnsBO obj)
        {
            return db.ManageColumnInfo(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("DeleteColumnInfo")]
        public ColumnsList DeleteColumnInfo(ManageColumnsBO obj)
        {
            return db.DeleteColumnInfo(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ConfirmImpMapping")]
        public string ConfirmImpMapping(ConfirmImpMapping obj)
        {
            return db.ConfirmImpMapping(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetTableResultSetExecution")]
        public GetTableResultSetExec GetTableResultSetExecution(int ardsExecID, int resultSetID)
        {
            return db.GetTableResultSetExecution(ardsExecID, resultSetID);
        }

        [HttpGet]
        [Route("GetRefEqpOthInfo")]
        public GetAnaOccuInstrumentsBO GetRefEqpOthInfo(int refEqpOccID)
        {
            return db.GetRefEqpOthInfo(refEqpOccID);
        }

        [HttpPost]
        [Route("GetSDMSDataDetails")]
        public GetSDMSDataDetailsBO GetSDMSData(ManageSDMSDataBO obj)
        {
            TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();

            GetSDMSDataDetailsBO lst = new GetSDMSDataDetailsBO();

            lst = db.GetSDMSData(obj, trn);

            if (lst.ResultFlag == "SUCCESS" && obj.Type != "GET")
            {

                SDMSFileBO fileBO = new SDMSFileBO();
                fileBO.EntityActID = obj.SamAnaTestID;
                fileBO.AppCode = trn.ApplicationType;
                fileBO.PlantCode = trn.PlantCode;
                fileBO.DeparmentCode = trn.DeptCode;
                fileBO.ReferenceNumber = string.IsNullOrEmpty(obj.ReferenceNumber) ? lst.ReferenceNumber : obj.ReferenceNumber;
                fileBO.LoginID = trn.LoginID;
                fileBO.Role = trn.RoleName;
                fileBO.EntityCode = obj.EntityCode;
                fileBO.SectionCode = "TSTDOCS";
                fileBO.XMLSDMSID = obj.XMLList;
                fileBO.Type = obj.Type;
                fileBO.Remarks = obj.Remarks;

                var retCode = CommonStaticMethods.PostApiConnectionData<string>("InvalidSDMSDataProcessFile", fileBO, "DMS_URL");
            }

            return lst;
        }

        [HttpPost]
        [Route("InvalidInstOccupancy")]
        public GetTestInstrumentDetails InvalidInstOccupancy(DeleteInstrumentBO obj)
        {
            return db.InvalidInstOccupancy(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetAnalysisChecklistCategories")]
        public CategoryMasterList GetAnalysisChecklistCategories()
        {
            return db.GetAnalysisChecklistCategories();
        }

        [HttpPost]
        [Route("ChangeAnalysisTestType")]
        public TestRetVal ChangeAnalysisTestType(ChangeTestType obj)
        {
            return db.ChangeAnalysisTestType(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }
    }
}
