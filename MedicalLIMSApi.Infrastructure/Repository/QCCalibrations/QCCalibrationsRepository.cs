using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.QCCalibrations;
using MedicalLIMSApi.Core.Interface.QCCalibrations;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.QCCalibrations
{
    public class QCCalibrationsRepository : IQCCalibrations
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public TransResultApprovals ManageCalibrationHeadersInfo(QCCalibrationsHeadersInfoBO obj, TransResults trn)
        {
            var lst = new TransResultApprovals();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspManageCalibrationParameterSets");

            if (obj.CalibParamID > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibParamID", obj.CalibParamID);

            ctx.AddInParameter<int>(cmd, "@InstrumentTypeID", obj.InstrumentTypeID);

            ctx.AddInParameter<string>(cmd, "@Title", obj.Title);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@InstrUserCodes", obj.InstrUserCodes);

            if (obj.InstrumentList.Count > default(int))
                ctx.AddInParameter<string>(cmd, "@InstrumentTypeXML", obj.InstrumentTypeXML);

            if (obj.UploadFiles.Count > 0)
                ctx.AddInParameter<string>(cmd, "@UploadFilesXML", obj.XMLUpload);

            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            if (!string.IsNullOrEmpty(obj.ManualReferenceNumber))
                ctx.AddInParameter<string>(cmd, "@ManualRefNumber", obj.ManualReferenceNumber);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TransResultApprovals>(reader);
                foreach (var rr in rrRes)
                    lst = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetQCCalibrationsHeadersInfoBO GetCalibrationHeaderDetails(int calibParamID, TransResults tr)
        {
            var lst = new GetQCCalibrationsHeadersInfoBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspGetCalibrationHeaderDetails");

            ctx.AddInParameter<int>(cmd, "@CalibParamID", calibParamID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetQCCalibrationsHeadersInfoBO>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                reader.NextResult();

                var rrResTrn = ((IObjectContextAdapter)context).ObjectContext.Translate<TransResultApprovals>(reader);
                foreach (var rr in rrResTrn)
                    lst.Trn = rr;

                reader.NextResult();

                lst.SelectedInstList = new SIngleBOList();
                var rrResInstr = ((IObjectContextAdapter)context).ObjectContext.Translate<SingleBO>(reader);
                foreach (var rr in rrResInstr)
                    lst.SelectedInstList.Add(rr);

                reader.NextResult();

                lst.InstrumentTypeIDList = new List<InstrumentCategoryItemsBO>();
                var rrInstr = ((IObjectContextAdapter)context).ObjectContext.Translate<InstrumentCategoryItemsBO>(reader);
                foreach (var rr in rrInstr)
                    lst.InstrumentTypeIDList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string AddNewSpecCategory(AddNewSpecCategoryBO obj, TransResults trn)
        {
            var retVal = default(int);
            string retCode = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "uspSPECNewCategory");
            ctx.AddInParameter<string>(cmd, "@CAT_NAME", obj.Category);
            ctx.AddInParameter<int>(cmd, "@SPEC_ID", obj.CalibParamID);
            ctx.AddInParameter<int>(cmd, "@USER_ID", trn.UserID);
            ctx.AddInParameter<short>(cmd, "@PLANT_ID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@MODULE_CODE", trn.DeptCode);
            ctx.AddInParameter<string>(cmd, "@EntityCode", "CAL_PARAM_SET");
            ctx.AddOutParameter(cmd, "@RETVAL", System.Data.DbType.Int16, 2);
            cmd.ExecuteNonQuery();
            retVal = Convert.ToInt32(ctx.GetOutputParameterValue(cmd, "@RETVAL"));
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            retCode = retVal == -2 ? "ALREADY_EXIST" : "SUCCESS";
            return retCode;
        }

        public string AddNewSpecSubCategory(AddNewSpecCategoryBO obj, TransResults trn)
        {
            var retVal = default(int);
            string retCode = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "uspSPECNewSubCategory");

            ctx.AddInParameter<int>(cmd, "@CATEGORY_ID", obj.CategoryID);
            ctx.AddInParameter<string>(cmd, "@SUB_CAT_NAME", obj.SubCategory);
            ctx.AddInParameter<int>(cmd, "@SPEC_ID", obj.CalibParamID);
            ctx.AddInParameter<int>(cmd, "@USER_ID", trn.UserID);
            ctx.AddInParameter<short>(cmd, "@PLANT_ID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@MODULE_CODE", trn.DeptCode);
            ctx.AddInParameter<string>(cmd, "@EntityCode", "CAL_PARAM_SET");
            ctx.AddOutParameter(cmd, "@RETVAL", System.Data.DbType.Int16, 2);
            cmd.ExecuteNonQuery();
            retVal = Convert.ToInt32(ctx.GetOutputParameterValue(cmd, "@RETVAL"));
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            retCode = retVal == -2 ? "ALREADY_EXIST" : "SUCCESS";
            return retCode;
        }

        public QCGetSingleTestMethod QCUpdateSingleTestMethodInstrumentData(ManageQCSingleTestMethod obj, TransResults trn)
        {
            var lst = new QCGetSingleTestMethod();
            int retVal = default(int);

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "uspQCUpdateSingleTestMethodInstrumentData");

            ctx.AddInParameter<int>(cmd, "@SPECID", obj.CalibParamID);
            ctx.AddInParameter<int>(cmd, "@TEST_ID", obj.TestID);
            ctx.AddInParameter<string>(cmd, "@MODULE_CODE", trn.DeptCode);
            ctx.AddInParameter<short>(cmd, "@PLANT_ID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@SPECLIMIT", obj.SpecLimit);

            if (obj.List.Count > default(int))
                ctx.AddInParameter<string>(cmd, "@DES_RES_XML", obj.DescResultXML);

            ctx.AddInParameter<string>(cmd, "@TEST_METHOD_TYPE", obj.LimitType);

            //if (obj.LowerLimit > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@LOWER_LIMIT", obj.LowerLimit);
            //if (obj.UpperLimit > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@UPPER_LIMIT", obj.UpperLimit);
            ctx.AddInParameter<bool>(cmd, "@IsLowerLimitApp", obj.IsLowerLimitApp);
            ctx.AddInParameter<bool>(cmd, "@IsUpperLimitApp", obj.IsUpperLimitApp);
            ctx.AddInParameter<int>(cmd, "@CREATED_BY", trn.UserID);

            if (obj.CategoryID > default(int))
                ctx.AddInParameter<int>(cmd, "@CATEGORY_ID", obj.CategoryID);
            if (obj.SubCategoryID > default(int))
                ctx.AddInParameter<int>(cmd, "@SUBCATEGORY_ID", obj.SubCategoryID);

            ctx.AddInParameter<string>(cmd, "@EntityCode", "CAL_PARAM_SET");
            ctx.AddOutParameter(cmd, "@RETVAL", System.Data.DbType.Int32, 4);
            ctx.AddInOutParameter<int>(cmd, "@SPEC_TEST_ID", obj.SpecTestID, 4, System.Data.DbType.Int32);
            ctx.AddInOutParameter<string>(cmd, "@INITIATION_TIME", obj.InitTime, 25, System.Data.DbType.String);
            cmd.ExecuteNonQuery();
            retVal = Convert.ToInt32(ctx.GetOutputParameterValue(cmd, "@RETVAL"));
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            lst.ReturnFlag = retVal > -1 ? "SUCCESS" : retVal == -2 ? "DUP_PARAMER" : "ERROR";
            lst.InitTime = ctx.GetOutputParameterValue(cmd, "@INITIATION_TIME");
            return lst;
        }

        public GetCalibrationTestsBOList GetCalibrationTests(int calibParamID, int specID)
        {
            var lst = new GetCalibrationTestsBOList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspGetCalibrationTests");

            if (calibParamID > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibParamID", calibParamID);

            if (specID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecID", specID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetCalibrationTestsBO>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetCalibrationsTestDetailsBO GetCalibrationTestDetailsByID(int specCatID)
        {

            var lst = new GetCalibrationsTestDetailsBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspGetCalibrationTestDetailsByID");

            ctx.AddInParameter<int>(cmd, "@SpecCatID", specCatID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetCalibrationsTestDetailsBO>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                reader.NextResult();

                var rrResList = ((IObjectContextAdapter)context).ObjectContext.Translate<CalibrationResultBO>(reader);
                lst.List = new CalibrationResultBOList();
                foreach (var rr in rrResList)
                    lst.List.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public CalibrationResultBOList GetTestResultByID(int specCatID)
        {
            var lst = new CalibrationResultBOList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspGetCalibrationTestResults");

            ctx.AddInParameter<int>(cmd, "@SpecCatID", specCatID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResList = ((IObjectContextAdapter)context).ObjectContext.Translate<CalibrationResultBO>(reader);

                foreach (var rr in rrResList)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public void QCSPECDeleteTestMethods(QCSPECDeleteTestMethodsBO obj, TransResults trn)
        {
            var retVal = default(int);

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "uspQCSPECDeleteTestMethods");

            ctx.AddInParameter<int>(cmd, "@USERID", trn.UserID);
            ctx.AddInParameter<int>(cmd, "@PLANT_ID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@MODULE_CODE", trn.DeptCode);

            if (obj.SpecTestID > default(int))
                ctx.AddInParameter<int>(cmd, "@SPEC_TEST_ID", obj.SpecTestID);

            ctx.AddInParameter<int>(cmd, "@SPECIFICATIONID", obj.CalibParamID);

            if (obj.CategoryID > default(int))
                ctx.AddInParameter<int>(cmd, "@CATEGORYID", obj.CategoryID);

            if (obj.SubCategoryID > default(int))
                ctx.AddInParameter<int>(cmd, "@SUB_CATEGORYID", obj.SubCategoryID);

            ctx.AddOutParameter(cmd, "@RETVAL", System.Data.DbType.Int16, 4);
            ctx.AddInOutParameter<string>(cmd, "@INITIATION_TIME", obj.InitTime, 25, System.Data.DbType.String);
            ctx.AddInParameter<string>(cmd, "@EntityCode", "CAL_PARAM_SET");

            cmd.ExecuteNonQuery();

            trn.InitTime = ctx.GetOutputParameterValue(cmd, "@INITIATION_TIME");
            retVal = Convert.ToInt32(ctx.GetOutputParameterValue(cmd, "@RETVAL"));
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            trn.ResultFlag = (retVal == -2) ? "DUP" : (retVal == -1) ? "AT_ONE_TEST" : "OK";
        }

        public ViewQCCalibrationDetailsBO ViewCalibrationDetailsByID(int calibParamID, TransResults tr)
        {
            var lst = new ViewQCCalibrationDetailsBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspGetCalibrationDetailsForView");
            ctx.AddInParameter<int>(cmd, "@CalibParamID", calibParamID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrHeaderInfo = ((IObjectContextAdapter)context).ObjectContext.Translate<GetQCCalibrationsHeadersInfoBO>(reader);

                lst.HeadersInfo = new GetQCCalibrationsHeadersInfoBO();

                foreach (var rr in rrHeaderInfo)
                    lst.HeadersInfo = rr;

                reader.NextResult();

                var rrResList = ((IObjectContextAdapter)context).ObjectContext.Translate<GetCalibrationTestsBO>(reader);

                lst.List = new GetCalibrationTestsBOList();

                foreach (var rr in rrResList)
                    lst.List.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public SearchResults<SearchQCCalibrationResultBO> SearchQCCalibrations(SearchQCCalibrationsBO obj, TransResults trn)
        {
            var lst = new SearchResults<SearchQCCalibrationResultBO>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspSearchCalibrationParameterSets");

            if (obj.InstrumentType > default(int))
                ctx.AddInParameter<int>(cmd, "@InstrumentType", obj.InstrumentType);
            if (obj.CalibrationID > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibrationID", obj.CalibrationID);
            if (obj.InstrumentID > default(int))
                ctx.AddInParameter<int>(cmd, "@InstrumentID", obj.InstrumentID);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (!string.IsNullOrEmpty(obj.Title))
                ctx.AddInParameter<string>(cmd, "@Title", obj.Title);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.CalibrationIDTo > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibrationIDTo", obj.CalibrationIDTo);
            if (obj.CalibrationIDFrom > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibrationIDFrom", obj.CalibrationIDFrom);
            if (obj.InitiatedOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@InitiatedOn",obj.InitiatedOn);
            if (obj.InitiatedBy > default(int))
                ctx.AddInParameter<int>(cmd, "@InitiatedBy", obj.InitiatedBy);

            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@RoleID", trn.RoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResultTotal = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var rr in rrResultTotal)
                    lst.TotalNumberOfRows = rr;

                reader.NextResult();

                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<SearchQCCalibrationResultBO>(reader);
                var list = new List<SearchQCCalibrationResultBO>();
                foreach (var rr in rrResult)
                    list.Add(rr);

                lst.SearchList = list;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public TransResultApprovals ManageAssignSTPGroupTest(ManageAssignSTPGroupTestDetails obj, TransResults trn)
        {
            var lst = new TransResultApprovals();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspQCCalibrationManageGroupTechAndAssignSTP");

            if (obj.CalibParamID > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibParamID", obj.CalibParamID);
            if (obj.SpecID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecID", obj.SpecID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);

            if (obj.TemplateID > default(int))
                ctx.AddInParameter<int>(cmd, "@TestTemplateID", obj.TemplateID);

            ctx.AddInParameter<string>(cmd, "@TestXML", obj.XMLTest);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResTrn = ((IObjectContextAdapter)context).ObjectContext.Translate<TransResultApprovals>(reader);

                foreach (var rr in rrResTrn)
                    lst = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetIntrumentsBOList GetInstrumentsByType(string equipCode, short plantID)
        {
            var lst = new GetIntrumentsBOList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspGetInstrumentsByType");

            ctx.AddInParameter<string>(cmd, "@InstrumentTypeCode", equipCode);
            ctx.AddInParameter<short>(cmd, "@PlantID", plantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResList = ((IObjectContextAdapter)context).ObjectContext.Translate<GetIntrumentsBO>(reader);

                foreach (var rr in rrResList)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public TransResultApprovals NewVersionCalibParamSet(int calibrationParamID, TransResults trn)
        {

            var lst = new TransResultApprovals();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspNewVersionCalibrationParamSet");

            ctx.AddInParameter<int>(cmd, "@CalibParamID", calibrationParamID);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TransResultApprovals>(reader);
                foreach (var rr in rrRes)
                    lst = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string CalibrationChangeStatus(CommentsBO obj, TransResults trn)
        {
            var retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspInsertCommentsForChangeStatus");

            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<string>(cmd, "@Comments", obj.Comment);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddOutParameter(cmd, "@RetMsg", DbType.String, 25);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public SpecTestList GetTestByID(TestBo obj)
        {
            SpecTestList lst = new SpecTestList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetTestByID");
            if (obj.SpecID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecID", obj.SpecID);
            if (obj.CalibID > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibParamID", obj.CalibID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SpecTest>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public ManagePlantForCalibrationParametersResult GetPlantForCalibrationParameters(ManagePlantForCalibrationParameters obj, TransResults trn)
        {
            var lst = new ManagePlantForCalibrationParametersResult();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspManagePlantForCalibrationParameters");

            ctx.AddInParameter<int>(cmd, "@CalibParamID", obj.CalibParamID);
            if (!string.IsNullOrEmpty(obj.PlantXml))
                ctx.AddInParameter<string>(cmd, "@PlantXml", obj.PlantXml);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                lst.GetPlantForCalibrationParameters = new List<GetPlantForCalibrationParameters>();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetPlantForCalibrationParameters>(reader);
                foreach (var rr in rrRes)
                    lst.GetPlantForCalibrationParameters.Add(rr);

                reader.NextResult();

                if (obj.Type == "SAVE")
                {
                    var returnFlag = ((IObjectContextAdapter)context).ObjectContext.Translate<string>(reader);
                    foreach (var rr in returnFlag)
                        lst.ReturnFlag = rr;
                }

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }
        public AssignInstrumentList AssignInstruments(AssignInstrumentDetailsBO obj, TransResults trn)
        {
            AssignInstrumentList lst = new AssignInstrumentList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspManageCalibrationParamterInstruments");

            ctx.AddInParameter<int>(cmd, "@CalibParamID", obj.CalibParamID);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            if (obj.InstrumentTypeID > default(int))
                ctx.AddInParameter<int?>(cmd, "@InstrumentTypeID", obj.InstrumentTypeID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            if (obj.List != null && obj.List.Count > default(int))
                ctx.AddInParameter<string>(cmd, "@InstrumentXml", obj.InstrumentXml);


            using (var reader = cmd.ExecuteReader())
            {
                lst.List = new InstrumentList();
                var rrList = ((IObjectContextAdapter)context).ObjectContext.Translate<InstrumentData>(reader);
                foreach (var r in rrList)
                    lst.List.Add(r);

                reader.NextResult();
                if (reader.HasRows)
                {
                    var rrInstr = ((IObjectContextAdapter)context).ObjectContext.Translate<string>(reader);

                    foreach (var rr in rrInstr)
                        lst.ReturnFlag = rr;
                }

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public CALIBManageARDS CALIBManageARDS(ManageArdsDocuments obj, TransResults trn)
        {
            CALIBManageARDS caliObj = new CALIBManageARDS();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspCALIBManageARDS");

            ctx.AddInParameter<int>(cmd, "@CalibParamID", obj.CalibParamID);
            if (obj.DocTrackID > default(int))
                ctx.AddInParameter<int>(cmd, "@DocTrackID", obj.DocTrackID);
            if (!string.IsNullOrEmpty(obj.Mode))
                ctx.AddInParameter<string>(cmd, "@Mode", obj.Mode);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddOutParameter(cmd, "@ReturnFlag", System.Data.DbType.String, 20);
            using (var reader = cmd.ExecuteReader())
            {
                caliObj.GetArdsDocuments = new List<GetArdsDocuments>();

                var rrList = ((IObjectContextAdapter)context).ObjectContext.Translate<GetArdsDocuments>(reader);
                foreach (var r in rrList)
                    caliObj.GetArdsDocuments.Add(r);
            }
            caliObj.ReturnFlag = ctx.GetOutputParameterValue(cmd, "@ReturnFlag");

            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return caliObj;
        }

        public RecordActionDetails CloneCalibrationParamSet(int calibParamID, TransResults tr)
        {
            RecordActionDetails act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd,"calib.uspCloneCalibrationParamSet");

            ctx.AddInParameter<int>(cmd, "@CalibParamID", calibParamID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using(var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    act = rr;
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return act;
        }

    }
}
