using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.QCCalibrations;

namespace MedicalLIMSApi.Core.Interface.QCCalibrations
{
    public interface IQCCalibrations
    {
        TransResultApprovals ManageCalibrationHeadersInfo(QCCalibrationsHeadersInfoBO obj, TransResults trn);

        GetQCCalibrationsHeadersInfoBO GetCalibrationHeaderDetails(int calibParamID, TransResults tr);

        string AddNewSpecCategory(AddNewSpecCategoryBO obj, TransResults trn);

        string AddNewSpecSubCategory(AddNewSpecCategoryBO obj, TransResults trn);

        QCGetSingleTestMethod QCUpdateSingleTestMethodInstrumentData(ManageQCSingleTestMethod obj, TransResults trn);

        GetCalibrationTestsBOList GetCalibrationTests(int calibParamID, int specID);

        GetCalibrationsTestDetailsBO GetCalibrationTestDetailsByID(int specCatID);

        CalibrationResultBOList GetTestResultByID(int specCatID);

        void QCSPECDeleteTestMethods(QCSPECDeleteTestMethodsBO obj, TransResults trn);

        ViewQCCalibrationDetailsBO ViewCalibrationDetailsByID(int calibParamID, TransResults tr);

        SearchResults<SearchQCCalibrationResultBO> SearchQCCalibrations(SearchQCCalibrationsBO obj, TransResults trn);

        TransResultApprovals ManageAssignSTPGroupTest(ManageAssignSTPGroupTestDetails obj, TransResults trn);

        GetIntrumentsBOList GetInstrumentsByType(string equipCode, short plantID);

        TransResultApprovals NewVersionCalibParamSet(int calibrationParamID, TransResults trn);

        string CalibrationChangeStatus(CommentsBO obj, TransResults trn);

        SpecTestList GetTestByID(TestBo obj);

        AssignInstrumentList AssignInstruments(AssignInstrumentDetailsBO obj, TransResults trn);

        ManagePlantForCalibrationParametersResult GetPlantForCalibrationParameters(ManagePlantForCalibrationParameters obj, TransResults tr);

        CALIBManageARDS  CALIBManageARDS(ManageArdsDocuments obj, TransResults tr);

        RecordActionDetails CloneCalibrationParamSet(int calibParamID, TransResults tr);
    }
}
