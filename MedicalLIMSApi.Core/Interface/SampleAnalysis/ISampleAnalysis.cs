using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SampleAnalysis;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Interface.SampleAnalysis
{
    public interface ISampleAnalysis
    {
        SearchResults<AnalysisSearchResult> SearchSampleAnalysis(SearchSampleAnalysisBO obj, TransResults tr);

        AnalysisHeader GetAnalysisHeaderInfo(int sioID, int userRoleID);

        AnalysisTypeList GetAnalysisTypes();

        BlockList GetBlocks(GetBolckList obj, short plantID);

        SupplierCOADetails GetSupplierCOADetails(int sioID, int userRoleID);

        RecordActionDetails ManageSupplierCOADetails(SupplierCOADetails obj, TransResults tr);

        MangeSampleAnalysisResult MangeSampleAnalysis(MangeSampleAnalysis obj, TransResults tr);

        GetSpecificationsBOList GetAnalysisSpecifications(int entityActID, string entityCode);

        GetAssignedDocsBySpecIDList GetAssignedDocsBySpecID(GetArdsAssignDoc obj);

        ARDSGetAssignedDocsList ARDSGetAssignedDocs(int entAct, string sourceCode);

        TransResultApprovals ARDSManageRequest(ARDSManageRequest obj, TransResults tr);

        string ARDSDiscardPrintRequest(DiscardPrintrequestBO obj, TransResults trn);

        GetSamplingInfo GetSamplingInfo(int sioID, int userID);

        SampleContainerDetails GetIssuedContainerDetails(GetSamplePack obj);

        string ManageSamplePacks(ManageSamplePack obj, TransResults tr);

        GetAnalysisTests GetAnalysisTest(int entityActID, string entityCode, int userID);

        SampleTestInfo GetSampleTestInfo(GetSampleAnaTestBO obj);

        TestResultValues GetResultStatus(TestValues obj);

        TestRetVal UpdateTestResults(UpdTestResults obj, TransResults tr);

        MangeSampleAnalysisResult SaveAnalysis(AnalysisRemarks obj, TransResults tr);

        string RaiseDeviation(Deviation obj, TransResults tr);

        GetInstrumentTitlesBOList GetInstrumentsForTest(GetInstrumentsForTestBO obj);

        GetEQPUGetEqpTypeCode GetEQPUGetEqpTypeCode(int eqpID);

        int EQPUGetCumulativeCount(int columnID);

        GetOccupancyData GetTestInstruments(GetInstrumentsForTestBO obj);

        GetTestInstrumentDetails InsertNUpdateInstruments(InsertNUpdateInstrumentsBO obj, TransResults trn);

        GetTestInstrumentDetails DeleteSpecInstruments(DeleteInstrumentBO obj, TransResults trn);

        GetAnaOccuInstrumentsBO GetAnaInstOccDetails(GetInstrumentDetailsByIDBO obj);

        GetRRTValuesBO ManageSampleRRTValuesBO(GetSampleRRTValuesBO obj, TransResults trn);

        GetARDSInputDetails GetARDSInputs(int samAnaID, string sourceCode, TransResults tr);

        GetSavedInputData SaveInputValues(InputValues obj, TransResults tr);

        AdditionalTestList GetAdditionalTest(AdditionalTestBODetails obj);

        GetAddTests ManageAdditionalTest(MngAdditionalTest obj, TransResults tr);

        GetAddTests DeleteAdditionalTest(int addTestID, TransResults tr);

        string GetDeviationDescription(string entityCode, string dcActionCode);

        MangeSampleAnalysisResult UpdateFinalRemarks(UpdFinalRemarks obj, TransResults tr);

        ExecuteFormula GetFormulaDependentDetails(GetFormualDetails obj, TransResults tr);

        GetConfData ConfirmEArds(ConfirmRawData obj, TransResults tr);

        GetIncludeExcludeTestBO ManageIncludeExcludeTest(IncludeExcludeTestBOItems obj, TransResults trn);

        SDMSInputValuesBOList GetSDMSDataBySamAnaTestID(int samAnaTestID);

        GetSDMSInputDetailsBO ManageSDMSInputDetails(ManageSDMSInputDetailsBO obj, TransResults trn);

        TestDetails GetMappingInfo(GetMappingInfo obj);

        GetAnalysisTypesList GetAnalysisTypes(int matCatID);

        GetSampleSourcesList GetSampleSources();

        ContainerWiseMaterialsResults ContainerWiseMaterials(ContainerWiseMaterials obj, TransResults trn);

        ManageUpdateFormulaResultFlagBO ManageIsFinalFormula(ManageIsFinalFormulaBO obj, TransResults trn);

        GetTestByCategory GetTestByCategory(TestCatBO obj);

        GetContainerAnalysisDetails GetContainerWiseAnalysis(int sioID);

        RecordActionDetails SaveContainerArdsDetails(SaveContainerArdsDetails obj, TransResults tr);

        SamplerDetails GetSamplerDetails(int sioID);

        TestRetVal SendTestForReview(SendForReview obj, TransResults tr);

        ArdsReviewBO AddArdsReviewComment(ManageArdsReview obj, TransResults tr);

        RecordActionDetails SkipPacksFromAnalysis(SkipPacksBO obj, TransResults tr);

        ManageUpdateFormulaResultFlagBO SkipInpurFieldFromExecution(ManageIsFinalFormulaBO obj, TransResults tr);

        List<PacksSendToReview> GetPackTestsForSendToReview(int sioID, int specCatID);

        SectionDetailList GetSTPCommonDataforMapping(int entActID);

        string ManageSTPCommonDataMapping(ManageSTPCommonData obj, TransResults tr);

        string ManageImpurityBasicInfo(ManageImpurityBasicInfoBO obj, TransResults trn);

        GetUnknownImpuritiesBO GetUnknownImpurities(int ardsExecID);

        string ManageImpuritySDMSDetails(string XMLString, TransResults trn);

        GetMulitpleValueExecBO ManageMutipleFormulaValue(ManageMultFormulaValueBO obj, TransResults trn);

        DynamicFormulaExecBO GetDynamicFormulaInfo(GetFormualDetails obj, TransResults tr);

        FormulaDependentList GetDyncFormulaDependentData(GetFormualDetails obj);

        ExecuteFormula ExecuteDynamicFormulaData(PostExecDynamicFormula obj, TransResults tr);

        string DeleteImpurity(int impurityID, TransResults tr);

        TestRetVal SwitchARDSMode(SwitchArdsMode obj, TransResults tr);

        ManageUpdateFormulaResultFlagBO SkipUnknownImpurities(ManageIsFinalFormulaBO obj);

        ColumnsList ManageColumnInfo(ManageColumnsBO obj, TransResults tr);

        ColumnsList DeleteColumnInfo(ManageColumnsBO obj, TransResults tr);

        string ConfirmImpMapping(ConfirmImpMapping obj, TransResults tr);

        GetTableResultSetExec GetTableResultSetExecution(int ardsExecID, int resultSetID);

        GetAnaOccuInstrumentsBO GetRefEqpOthInfo(int refEqpOccID);

        GetSDMSDataDetailsBO GetSDMSData(ManageSDMSDataBO obj, TransResults trn);

        GetTestInstrumentDetails InvalidInstOccupancy(DeleteInstrumentBO obj, TransResults tr);

        CategoryMasterList GetAnalysisChecklistCategories();

        TestRetVal ChangeAnalysisTestType(ChangeTestType obj, TransResults tr);
    }
}
