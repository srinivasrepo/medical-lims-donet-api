using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Entities.SampleAnalysis;
using MedicalLIMSApi.Core.Interface.SampleAnalysis;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.SampleAnalysis
{
    public class SampleAnalysisRepository : ISampleAnalysis
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public SearchResults<AnalysisSearchResult> SearchSampleAnalysis(SearchSampleAnalysisBO obj, TransResults tr)
        {
            var lst = new SearchResults<AnalysisSearchResult>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSAMANASearchAnalysis");
            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            if (obj.CatID > default(int))
                ctx.AddInParameter<int>(cmd, "@CatID", obj.CatID);
            if (obj.SubcatID > default(int))
                ctx.AddInParameter<int>(cmd, "@SubCatID", obj.SubcatID);
            if (obj.MatID > default(int))
                ctx.AddInParameter<int>(cmd, "@MatID", obj.MatID);
            if (obj.SampleID > default(int))
                ctx.AddInParameter<int>(cmd, "@SampleID", obj.SampleID);
            if (obj.ProductID > default(int))
                ctx.AddInParameter(cmd, "@ProductID", obj.ProductID);
            if (obj.StageID > default(int))
                ctx.AddInParameter(cmd, "@StageID", obj.StageID);
            if (obj.BatchID > default(int))
                ctx.AddInParameter<int>(cmd, "@BatchID", obj.BatchID);
            if (obj.AnalysisTypeID > default(int))
                ctx.AddInParameter<int>(cmd, "@AnaTypeID", obj.AnalysisTypeID);
            if (obj.ARID > default(int))
                ctx.AddInParameter<int>(cmd, "@ARID", obj.ARID);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (obj.PlantAreaID > default(int))
                ctx.AddInParameter<int>(cmd, "@PlantAreaID", obj.PlantAreaID);
            if (obj.ProjectID > default(int))
                ctx.AddInParameter<int>(cmd, "@ProjectID", obj.ProjectID);
            if (obj.DateFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateFrom", obj.DateFrom);
            if (obj.DateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateTo", obj.DateTo);
            if (obj.MoleclueType > default(byte))
                ctx.AddInParameter<byte>(cmd, "@MoleclueType", obj.MoleclueType);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@RoleID", tr.RoleID);
            ctx.AddInParameter<string>(cmd, "@DeptCode", tr.DeptCode);
            using (var reader = cmd.ExecuteReader())
            {
                var rrCount = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var rr in rrCount)
                    lst.TotalNumberOfRows = rr;

                reader.NextResult();

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<AnalysisSearchResult>(reader);
                List<AnalysisSearchResult> item = new List<AnalysisSearchResult>();
                foreach (var rr in rrRes)
                    item.Add(rr);
                lst.SearchList = item;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public AnalysisHeader GetAnalysisHeaderInfo(int sioID, int userRoleID)
        {
            AnalysisHeader obj = new AnalysisHeader();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSAMANAGetHeaderInfo");
            ctx.AddInParameter<int>(cmd, "@SioID", sioID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", userRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<AnalysisHeader>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();

                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                obj.Act = new RecordActionDetails();
                foreach (var rr in rrAct)
                    obj.Act = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public AnalysisTypeList GetAnalysisTypes()
        {
            AnalysisTypeList lst = new AnalysisTypeList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "uspQCGetAnalysisTypes");

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<AnalysisType>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public BlockList GetBlocks(GetBolckList obj, short plantID)
        {
            BlockList lst = new BlockList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspGetBlockByPlantID");
            ctx.AddInParameter<short>(cmd, "@PlantID", plantID);
            if (!string.IsNullOrEmpty(obj.DeptCode))
                ctx.AddInParameter<string>(cmd, "@DeptCode", obj.DeptCode);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);


            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<Block>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public SupplierCOADetails GetSupplierCOADetails(int sioID, int userRoleID)
        {
            SupplierCOADetails obj = new SupplierCOADetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSAMANACUSTGetSampleDetailsByID");
            ctx.AddInParameter<int>(cmd, "@SioID", sioID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", userRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SupplierCOADetails>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();

                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                obj.Act = new RecordActionDetails();
                foreach (var rr in rrAct)
                    obj.Act = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public RecordActionDetails ManageSupplierCOADetails(SupplierCOADetails obj, TransResults tr)
        {
            RecordActionDetails act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSAMANACUSTManageCOA");
            ctx.AddInParameter<int>(cmd, "@SioID", obj.SioID);
            ctx.AddInParameter<string>(cmd, "@SampleResult", obj.SampleResult);
            ctx.AddInParameter<string>(cmd, "@SampledBy", obj.SampledBy);
            ctx.AddInParameter<string>(cmd, "@SampleRemarks", obj.Remarks);
            ctx.AddInParameter<string>(cmd, "@DateType", obj.SupRetestExpiryTypeValue);
            ctx.AddInParameter<DateTime?>(cmd, "@Date", obj.SupRetestExpiryDate);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            if (!string.IsNullOrEmpty(obj.StorageCondition))
                ctx.AddInParameter<string>(cmd, "@SampleStorage", obj.StorageCondition);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return act;
        }

        public GetSpecificationsBOList GetAnalysisSpecifications(int entityActID, string entityCode)
        {
            var lst = new GetSpecificationsBOList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetSpecsBySIOID");
            ctx.AddInParameter<int>(cmd, "@EntityActID", entityActID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", entityCode);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSpecificationsBO>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public MangeSampleAnalysisResult MangeSampleAnalysis(MangeSampleAnalysis obj, TransResults tr)
        {
            MangeSampleAnalysisResult lst = new MangeSampleAnalysisResult();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspManageSampling");

            ctx.AddInParameter<int>(cmd, "@SIOID", obj.SioID);
            ctx.AddInParameter<string>(cmd, "@REMARKS", obj.Deviation);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@SAMPLE_APPLICABILITY", obj.SampleCollected);
            if (obj.NoofContainers > default(short))
                ctx.AddInParameter<short>(cmd, "@PACKS_SAMPLED", obj.NoofContainers);
            if (!string.IsNullOrEmpty(obj.SampledBy))
                ctx.AddInParameter<string>(cmd, "@SAMPLED_BY", obj.SampledBy);
            if (obj.CompositeSampleQty > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@COMPSAM_QTY", obj.CompositeSampleQty);
            if (obj.Uom > default(int))
                ctx.AddInParameter<int>(cmd, "@UOM", obj.Uom);
            if (obj.QtyAnalysis > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@SAMPLE_QTY", obj.QtyAnalysis);
            if (obj.ReserveSampleQty > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@RESERVE_SAMACTQTY", obj.ReserveSampleQty);
            if (obj.ReserveSampleUom > default(int))
                ctx.AddInParameter<int>(cmd, "@RESERVE_SAMACTQTY_UOM", obj.ReserveSampleUom);
            if (obj.SampleReceviedOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@SAMPLED_ON", obj.SampleReceviedOn);
            if (obj.SamplerID > default(int))
                ctx.AddInParameter<int>(cmd, "@SamplerID", obj.SamplerID);
            if (!string.IsNullOrEmpty(obj.UPLOAD_DOCT))
                ctx.AddInParameter<string>(cmd, "@UPLOAD_DOCT", obj.UPLOAD_DOCT);
            if (!string.IsNullOrEmpty(obj.QtyFrom))
                ctx.AddInParameter<string>(cmd, "@STB_QTYFROM", obj.QtyFrom);
            if (!string.IsNullOrEmpty(obj.SampleStorageTemp))
                ctx.AddInParameter<string>(cmd, "@STORAGE_CONDITION", obj.SampleStorageTemp);
            if (!string.IsNullOrEmpty(obj.SamplingPoint))
                ctx.AddInParameter<string>(cmd, "@SamplingPoint", obj.SamplingPoint);
            ctx.AddOutParameter(cmd, "@ReturnFlag", System.Data.DbType.String, 25);
            ctx.AddInOutParameter<string>(cmd, "@INITIATION_TIME", obj.InitTime, 25, System.Data.DbType.String);
            ctx.AddOutParameter(cmd, "@Status", System.Data.DbType.String, 50);

            if (!string.IsNullOrEmpty(obj.OtherListXML))
                ctx.AddInParameter<string>(cmd, "@OtherListXML", obj.OtherListXML);
            if (!string.IsNullOrEmpty(obj.ChecklistTypeCode))
                ctx.AddInParameter<string>(cmd, "@ChecklistCategory", obj.ChecklistTypeCode);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    lst.RptList = new UploadReportList();
                    var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<UploadReports>(reader);
                    foreach (var rr in rrRes)
                        lst.RptList.Add(rr);
                }
            }

            lst.ReturnFlag = ctx.GetOutputParameterValue(cmd, "@ReturnFlag");
            lst.InitTime = ctx.GetOutputParameterValue(cmd, "@INITIATION_TIME");
            lst.Status = ctx.GetOutputParameterValue(cmd, "@Status");
            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return lst;
        }

        public GetAssignedDocsBySpecIDList GetAssignedDocsBySpecID(GetArdsAssignDoc obj)
        {
            GetAssignedDocsBySpecIDList lst = new GetAssignedDocsBySpecIDList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspARDSGetAssignedDocsBySpecID");

            ctx.AddInParameter<int>(cmd, "@EntActID", obj.EntActID);
            ctx.AddInParameter<string>(cmd, "@SourceCode", obj.SourceCode);
            if (obj.SpecID > default(int))
                ctx.AddInParameter<int>(cmd, "@SPECID", obj.SpecID);
            if (obj.CalibParamID > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibParamID", obj.CalibParamID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetAssignedDocsBySpecID>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public ARDSGetAssignedDocsList ARDSGetAssignedDocs(int entActID, string sourceCode)
        {
            ARDSGetAssignedDocsList lst = new ARDSGetAssignedDocsList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspARDSGetAssignedDocs");

            ctx.AddInParameter<int>(cmd, "@EntActID", entActID);
            ctx.AddInParameter<string>(cmd, "@SourceCode", sourceCode);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ARDSGetAssignedDocs>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public TransResultApprovals ARDSManageRequest(ARDSManageRequest obj, TransResults tr)
        {
            var lst = new TransResultApprovals();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspARDSManageRequest");

            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@SpecID", obj.SpecID);
            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActID);
            if (obj.TrackID > default(int))
                ctx.AddInParameter<int>(cmd, "@TrackID", obj.TrackID);
            if (!string.IsNullOrEmpty(obj.DocPath))
                ctx.AddInParameter<string>(cmd, "@DocPath", obj.DocPath);
            ctx.AddInParameter<string>(cmd, "@AnalysisMode", obj.AnalysisMode);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (!string.IsNullOrEmpty(obj.ExtraneousAnalysis))
                ctx.AddInParameter<string>(cmd, "@ExtraneousMatterApplicable", obj.ExtraneousAnalysis);
            if (obj.List != null && obj.List.Count > 0)
                ctx.AddInParameter<string>(cmd, "@ExtraneousTestsXml", obj.ExtraneousTestsXml);
            if (!string.IsNullOrEmpty(obj.ContainerWiseAnalysisApp))
                ctx.AddInParameter<string>(cmd, "@ContaierWiseAnalysisApp", obj.ContainerWiseAnalysisApp);
            if (obj.DMSID > default(int))
                ctx.AddInParameter<int>(cmd, "@DMSID", obj.DMSID);
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

        public GetSamplingInfo GetSamplingInfo(int sioID, int userID)
        {
            GetSamplingInfo lst = new GetSamplingInfo();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetSamplingInfo");

            ctx.AddInParameter<int>(cmd, "@SioID", sioID);
            ctx.AddInParameter<int>(cmd, "@UserID", userID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSamplingInfo>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public string ARDSDiscardPrintRequest(DiscardPrintrequestBO obj, TransResults trn)
        {
            string retVal = string.Empty;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspARDSDiscardPrintRequest");
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@EnityActID", obj.EntActID);
            ctx.AddInParameter<int>(cmd, "@REQ_DOCID", obj.ReqDocID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<string>(cmd, "@RefNumber", obj.RefNumber);
            ctx.AddOutParameter(cmd, "@ReturnFlag", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@ReturnFlag");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public SampleContainerDetails GetIssuedContainerDetails(GetSamplePack obj)
        {
            SampleContainerDetails samObj = new SampleContainerDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "qrct.uspGetContainersForEntity");
            ctx.AddInParameter<string>(cmd, "@PurposeCode", obj.PurposeCode);
            ctx.AddInParameter<int>(cmd, "@EntityMatID", obj.SioID);
            if (obj.SecUomID > default(int))
                ctx.AddInParameter<int>(cmd, "@SecUOMID", obj.SecUomID);
            if (obj.ReqQuantity > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@ReqQuantity", obj.ReqQuantity);

            using (var reader = cmd.ExecuteReader())
            {
                SamplePackList lst = new SamplePackList();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SamplePacks>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                reader.NextResult();
                var rrObj = ((IObjectContextAdapter)context).ObjectContext.Translate<SampleContainerDetails>(reader);
                foreach (var rr in rrObj)
                    samObj = rr;
                samObj.Lst = lst;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return samObj;
        }

        public string ManageSamplePacks(ManageSamplePack obj, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "qrct.uspSaveContainerSelections");
            ctx.AddInParameter<string>(cmd, "@PurposeCode", obj.PurposeCode);
            ctx.AddInParameter<int>(cmd, "@EntMatID", obj.SioID);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<string>(cmd, "@ModuleCode", tr.DeptCode);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            if (obj.SecUomID > default(int))
                ctx.AddInParameter<int>(cmd, "@SecUomId", obj.SecUomID);
            ctx.AddStructuredInParameter(cmd, "@InterfaceType", obj.Lst);
            ctx.AddOutParameter(cmd, "@RetMsg", System.Data.DbType.String, 256);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public GetAnalysisTests GetAnalysisTest(int entityActID, string entityCode, int userID)
        {
            GetAnalysisTests obj = new GetAnalysisTests();
            var lst = new GetAnalysisTestBOList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetAnalysisTests");
            ctx.AddInParameter<int>(cmd, "@EntityActID", entityActID);
            ctx.AddInParameter<string>(cmd, "@SourceCode", entityCode);
            ctx.AddInParameter<int>(cmd, "@UserID", userID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetAnalysisTestBO>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                reader.NextResult();

                var rrData = ((IObjectContextAdapter)context).ObjectContext.Translate<GetAnalysisTests>(reader);
                foreach (var rr in rrData)
                    obj = rr;
                obj.Lst = lst;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }
        public SampleTestInfo GetSampleTestInfo(GetSampleAnaTestBO objSam)
        {
            SampleTestInfo obj = new SampleTestInfo();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.GetSamTestInfo");
            ctx.AddInParameter<int>(cmd, "@SamAnaTestID", objSam.SampleAnaTestID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", objSam.EntityCode);
            if (!string.IsNullOrEmpty(objSam.EntityCode))
                ctx.AddInParameter<string>(cmd, "@SourceCode", objSam.SourceCode);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SampleTestInfo>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();

                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<TestResult>(reader);
                obj.Lst = new TestResultList();
                foreach (var rr in rrLst)
                    obj.Lst.Add(rr);

                reader.NextResult();

                obj.TestResult = new TestResultValues();
                var rrTest = ((IObjectContextAdapter)context).ObjectContext.Translate<TestResultValues>(reader);
                foreach (var rr in rrTest)
                    obj.TestResult = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public TestResultValues GetResultStatus(TestValues obj)
        {
            TestResultValues data = new TestResultValues();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSAMANACheckResultStatus");
            ctx.AddInParameter<int>(cmd, "@SamAnaTestID", obj.sampleAnaTestID);
            ctx.AddInParameter<decimal>(cmd, "@RESULT", obj.Result);
            //if (obj.ResultTo > default(decimal))
            ctx.AddInParameter<decimal>(cmd, "@RESULT_TO", obj.ResultTo);
            if (!string.IsNullOrEmpty(obj.SourceCode))
                ctx.AddInParameter<string>(cmd, "@SourceCode", obj.SourceCode);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TestResultValues>(reader);
                foreach (var rr in rrRes)
                    data = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return data;
        }

        public TestRetVal UpdateTestResults(UpdTestResults obj, TransResults tr)
        {
            TestRetVal data = new TestRetVal();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspUpdateTestResults");
            ctx.AddInParameter<int>(cmd, "@SamAnaTestID", obj.SampleAnaTestID);
            if (!string.IsNullOrEmpty(obj.Result))
                ctx.AddInParameter<string>(cmd, "@Result", obj.Result);
            ctx.AddInParameter<string>(cmd, "@PassOrFail", obj.PassOrFail);
            if (!string.IsNullOrEmpty(obj.ResultTo))
                ctx.AddInParameter<string>(cmd, "@ResultTo", obj.ResultTo);
            if (!string.IsNullOrEmpty(obj.ResultType))
                ctx.AddInParameter<string>(cmd, "@ResultType", obj.ResultType);
            ctx.AddInParameter<bool>(cmd, "@SendOOS", obj.SendOss);
            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            if (!string.IsNullOrEmpty(obj.TestInitTime))
                ctx.AddInParameter<string>(cmd, "@TestInitTime", obj.TestInitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (!string.IsNullOrEmpty(obj.SpecPassOrFail))
                ctx.AddInParameter<string>(cmd, "@SpecPassOrFail", obj.SpecPassOrFail);
            ctx.AddInParameter<string>(cmd, "@TypeCode", obj.TypeCode);
            if (!string.IsNullOrEmpty(obj.CorrValue))
                ctx.AddInParameter<string>(cmd, "@CorrValue", obj.CorrValue);
            if (!string.IsNullOrEmpty(obj.Justification))
                ctx.AddInParameter<string>(cmd, "@Justification", obj.Justification);
            if (obj.NewSampleRefID > default(int))
                ctx.AddInParameter<int>(cmd, "@NewSampleRefID", obj.NewSampleRefID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TestRetVal>(reader);
                foreach (var rr in rrRes)
                    data = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return data;
        }

        public MangeSampleAnalysisResult SaveAnalysis(AnalysisRemarks obj, TransResults tr)
        {
            MangeSampleAnalysisResult data = new MangeSampleAnalysisResult();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSaveAnalysis");
            ctx.AddInParameter<int>(cmd, "@SIOID", obj.SioID);
            ctx.AddInParameter<string>(cmd, "@REMARKS", obj.Remarks);
            if (!string.IsNullOrEmpty(obj.specPrecautions))
                ctx.AddInParameter<string>(cmd, "@SPL_PRECAUTION", obj.specPrecautions);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (!string.IsNullOrEmpty(obj.SourceCode))
                ctx.AddInParameter<string>(cmd, "@SourceCode", obj.SourceCode);
            if (obj.ContainerAnaID > default(int))
                ctx.AddInParameter<int>(cmd, "@ContainerAnaID", obj.ContainerAnaID);
            ctx.AddInOutParameter<string>(cmd, "@INITIATION_TIME", obj.InitTime, 25, System.Data.DbType.String);
            ctx.AddOutParameter(cmd, "@ReturnFlag", System.Data.DbType.String, 25);
            if (!string.IsNullOrEmpty(obj.AnalysisStatus))
                ctx.AddInOutParameter<string>(cmd, "@AnalysisStatus", obj.AnalysisStatus, 10, DbType.String);
            else
                ctx.AddOutParameter(cmd, "@AnalysisStatus", DbType.String, 10);
            //using (var reader = cmd.ExecuteReader())
            //{
            //    var rrUsr = ((IObjectContextAdapter)context).ObjectContext.Translate<MangeSampleAnalysisResult>(reader);
            //    foreach (var rr in rrUsr)
            //        data = rr;

            //    reader.NextResult();

            //    var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SingleBO>(reader);
            //    data.List = new SIngleBOList();
            //    foreach (var rr in rrRes)
            //        data.List.Add(rr);
            //}
            cmd.ExecuteNonQuery();
            data.InitTime = ctx.GetOutputParameterValue(cmd, "@INITIATION_TIME");
            data.ReturnFlag = ctx.GetOutputParameterValue(cmd, "@ReturnFlag");
            data.AnalysisStatus = ctx.GetOutputParameterValue(cmd, "@AnalysisStatus");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return data;
        }

        public string RaiseDeviation(Deviation obj, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspDCRiseDeviation");
            ctx.AddInParameter<string>(cmd, "@DCActionCode", obj.DcActionCode);
            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<string>(cmd, "@RefCode", obj.RefCode);
            if (!string.IsNullOrEmpty(obj.Remarks))
                ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);
            if (!string.IsNullOrEmpty(obj.NumList))
                ctx.AddInParameter<string>(cmd, "@DevNumbers", obj.NumList);
            ctx.AddInParameter<string>(cmd, "@DevType", obj.DevType);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@RetMsg", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return retMsg;
        }

        public GetInstrumentTitlesBOList GetInstrumentsForTest(GetInstrumentsForTestBO obj)
        {
            var lst = new GetInstrumentTitlesBOList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetInstrumentsForTest");
            ctx.AddInParameter<int>(cmd, "@SamAnaTestID", obj.SamAnalTestID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetInstrumentTitlesBO>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetEQPUGetEqpTypeCode GetEQPUGetEqpTypeCode(int eqpID)
        {
            var lst = new GetEQPUGetEqpTypeCode();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "uspEQPUGetEqpTypeCode");
            ctx.AddInParameter<int>(cmd, "@EQP_ID", eqpID);
            ctx.AddOutParameter(cmd, "@ACT_TYPE_CODE", System.Data.DbType.String, 50);
            ctx.AddOutParameter(cmd, "@TYPE_CODE", System.Data.DbType.String, 50);
            cmd.ExecuteNonQuery();
            lst.ActTypeCode = ctx.GetOutputParameterValue(cmd, "@ACT_TYPE_CODE");
            lst.TypeCode = ctx.GetOutputParameterValue(cmd, "@TYPE_CODE");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return lst;
        }

        public int EQPUGetCumulativeCount(int columnID)
        {
            int count = default(int);

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "uspEQPUGetCumulativeCount");
            ctx.AddInParameter<int>(cmd, "@COLUMN_ID", columnID);
            ctx.AddOutParameter(cmd, "@COUNT", System.Data.DbType.Int32, 4);
            cmd.ExecuteNonQuery();
            count = Convert.ToInt32(ctx.GetOutputParameterValue(cmd, "@COUNT"));
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return count;
        }

        public GetOccupancyData GetTestInstruments(GetInstrumentsForTestBO objDetails)
        {
            GetOccupancyData obj = new GetOccupancyData();
            var lst = new GetTestInstrumentsBOList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetTestInstruments");
            ctx.AddInParameter<int>(cmd, "@SamAnaTestID", objDetails.SamAnalTestID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", objDetails.EntityCode);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetTestInstrumentsBO>(reader);

                foreach (var rr in rrRes)
                    lst.Add(rr);

                reader.NextResult();

                var rrData = ((IObjectContextAdapter)context).ObjectContext.Translate<GetOccupancyData>(reader);
                foreach (var rr in rrData)
                    obj = rr;

                obj.Lst = lst;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public GetTestInstrumentDetails InsertNUpdateInstruments(InsertNUpdateInstrumentsBO obj, TransResults trn)
        {
            var lst = new GetTestInstrumentDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspInsertNUpdateInstruments");

            if (obj.SamAnaInstID > default(int))
                ctx.AddInParameter<int>(cmd, "@SamAnaInstID", obj.SamAnaInstID);
            if (obj.InstrumentTitleID > default(int))
                ctx.AddInParameter<int>(cmd, "@InstID", obj.InstrumentTitleID);


            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<int>(cmd, "@SamAnaTestID", obj.SamAnaTestID);
            ctx.AddInParameter<int>(cmd, "@UserID", trn.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<bool>(cmd, "@IsOOSReq", obj.OccupancyRequired);
            ctx.AddInParameter<string>(cmd, "@OccCode", obj.OccupancyType);

            if (obj.DateFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@StartTime", obj.DateFrom);
            if (obj.DateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@EndTime", obj.DateTo);
            if (!string.IsNullOrEmpty(obj.Remarks))
                ctx.AddInParameter<string>(cmd, "@InstRemarks", obj.Remarks);
            if (obj.ColumnID > default(int))
                ctx.AddInParameter<int>(cmd, "@RefColID", obj.ColumnID);
            if (!string.IsNullOrEmpty(obj.DataSeqFile))
                ctx.AddInParameter<string>(cmd, "@DataSQFile", obj.DataSeqFile);
            ctx.AddInParameter<string>(cmd, "@TestInitTime", obj.TestInitTime);
            if (obj.List != null && obj.List.Count > 0)
                ctx.AddInParameter<string>(cmd, "@ColumnsXML", obj.ColumnXml);
            if (obj.RefEqpOccID > default(int))
                ctx.AddInParameter<int>(cmd, "@RefEqpOccID", obj.RefEqpOccID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetTestInstrumentDetails>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetTestInstrumentDetails DeleteSpecInstruments(DeleteInstrumentBO obj, TransResults trn)
        {
            var lst = new GetTestInstrumentDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSADeleteSpecInstruments");
            ctx.AddInParameter<int>(cmd, "@SamAnaTestID", obj.SamInstrID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<int>(cmd, "@USERID", trn.UserID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);

            ctx.AddInParameter<string>(cmd, "@TestInitTime", obj.InitTime);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetTestInstrumentDetails>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetAnaOccuInstrumentsBO GetAnaInstOccDetails(GetInstrumentDetailsByIDBO obj)
        {
            var lst = new GetAnaOccuInstrumentsBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetAnaInstOccDetails");
            if (obj.SamAnaInstID > default(int))
                ctx.AddInParameter<int>(cmd, "@SamAnaTestInstID", obj.SamAnaInstID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (obj.CalibMainID > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibMainID", obj.CalibMainID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetAnaOccuInstrumentsBO>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                reader.NextResult();
                lst.lst = new ColumnsList();
                var rrlst = ((IObjectContextAdapter)context).ObjectContext.Translate<ManageColumnsBO>(reader);
                foreach (var rr in rrlst)
                    lst.lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetRRTValuesBO ManageSampleRRTValuesBO(GetSampleRRTValuesBO obj, TransResults trn)
        {
            var lst = new GetRRTValuesBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspManageRRT");

            if (obj.RRtID > default(int))
                ctx.AddInParameter<int>(cmd, "@RRTID", obj.RRtID);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<int>(cmd, "@SAM_ANA_TST_ID", obj.SamTestID);
            ctx.AddInParameter<string>(cmd, "@TYPE", obj.Type);
            if (!string.IsNullOrEmpty(obj.TestDesc))
                ctx.AddInParameter<string>(cmd, "@TEST_DESC", obj.TestDesc);
            if (!string.IsNullOrEmpty(obj.Result))
                ctx.AddInParameter<string>(cmd, "@RESULT", obj.Result);
            if (!string.IsNullOrEmpty(obj.AcceptenceCriteria))
                ctx.AddInParameter<string>(cmd, "@ACCEPTANCE_CRITERIA", obj.AcceptenceCriteria);

            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@TestInitTime", obj.InitTime);

            using (var reader = cmd.ExecuteReader())
            {

                var rrResTrn = ((IObjectContextAdapter)context).ObjectContext.Translate<GetTestInstrumentDetails>(reader);

                lst.Trn = new GetTestInstrumentDetails();

                foreach (var rr in rrResTrn)
                    lst.Trn = rr;

                reader.NextResult();

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSampleTestRRTValuesBO>(reader);

                lst.List = new GetSampleTestRRTValuesBOList();

                foreach (var rr in rrRes)
                    lst.List.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetARDSInputDetails GetARDSInputs(int samAnaTestID, string sourceCode, TransResults tr)
        {
            GetARDSInputDetails obj = new GetARDSInputDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspGetARDSInputs");
            ctx.AddInParameter<int>(cmd, "@ARDSSourceRefKey", samAnaTestID);
            ctx.AddInParameter<string>(cmd, "@ARDSSourceCode", sourceCode);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetARDSInputDetails>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();

                var rrTab = ((IObjectContextAdapter)context).ObjectContext.Translate<TabBo>(reader);
                obj.TabList = new TabList();
                foreach (var rr in rrTab)
                    obj.TabList.Add(rr);

                reader.NextResult();

                var rrSec = ((IObjectContextAdapter)context).ObjectContext.Translate<ARDSSection>(reader);
                obj.SectionList = new SectionList();
                foreach (var rr in rrSec)
                    obj.SectionList.Add(rr);

                reader.NextResult();

                var rrSecDet = ((IObjectContextAdapter)context).ObjectContext.Translate<SectionData>(reader);
                obj.SectionDetailsList = new SectionDetailList();
                foreach (var rr in rrSecDet)
                    obj.SectionDetailsList.Add(rr);

                reader.NextResult();

                var rrInvDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<InvalidationBO>(reader);

                obj.InvList = new InvalidationBOList();
                foreach (var rr in rrInvDetails)
                    obj.InvList.Add(rr);

                reader.NextResult();

                var rrSolventsDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<Solvents>(reader);

                obj.SolList = new SolventsList();
                foreach (var rr in rrSolventsDetails)
                    obj.SolList.Add(rr);

                reader.NextResult();

                obj.ReviewList = new ArdsReviewList();
                var rrReviewList = ((IObjectContextAdapter)context).ObjectContext.Translate<ArdsReviewBO>(reader);
                foreach (var rr in rrReviewList)
                    obj.ReviewList.Add(rr);

                reader.NextResult();

                obj.DynamicValueLst = new List<DynamicValues>();
                var rrDyncLst = ((IObjectContextAdapter)context).ObjectContext.Translate<DynamicValues>(reader);
                foreach (var rr in rrDyncLst)
                    obj.DynamicValueLst.Add(rr);

                reader.NextResult();

                obj.TableResultLst = new TableResultSetsList();
                var rrTableLst = ((IObjectContextAdapter)context).ObjectContext.Translate<GetTableResultSets>(reader);
                foreach (var rr in rrTableLst)
                    obj.TableResultLst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public GetSavedInputData SaveInputValues(InputValues obj, TransResults tr)
        {
            GetSavedInputData data = new GetSavedInputData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspSaveInputValue");
            ctx.AddInParameter<int>(cmd, "@ARDSSourceRefKey", obj.SamAnaTestID);
            ctx.AddInParameter<string>(cmd, "@ARDSSourceCode", obj.ArdsSourceCode);
            ctx.AddInParameter<int>(cmd, "@DetailID", obj.DetailID);
            ctx.AddInParameter<string>(cmd, "@Value", obj.Value);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            if (obj.IsFormulaEval)
                ctx.AddInParameter<bool>(cmd, "@isFormulaEval", obj.IsFormulaEval);
            if (!string.IsNullOrEmpty(obj.ActValue))
                ctx.AddInParameter<string>(cmd, "@ActValue", obj.ActValue);
            if (!string.IsNullOrEmpty(obj.ImpurityXML))
                ctx.AddInParameter<string>(cmd, "@ImpurityXML", obj.ImpurityXML);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSavedInputData>(reader);
                foreach (var rr in rrRes)
                    data = rr;

                reader.NextResult();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<EffectedInputFileds>(reader);
                data.Lst = new EffectedInputFiledLists();
                foreach (var rr in rrLst)
                    data.Lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return data;
        }

        public AdditionalTestList GetAdditionalTest(AdditionalTestBODetails obj)
        {
            AdditionalTestList lst = new AdditionalTestList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSAMANAGetAdditionalTests");
            ctx.AddInParameter<int>(cmd, "@SamAnaID", obj.SamAnaID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<AdditionalTest>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public GetAddTests ManageAdditionalTest(MngAdditionalTest obj, TransResults tr)
        {
            GetAddTests data = new GetAddTests();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspManageAdditionalTest");
            ctx.AddInParameter<int>(cmd, "@SamAnaID", obj.SamAnaID);
            ctx.AddInParameter<int>(cmd, "@TestID", obj.TestID);
            ctx.AddInParameter<string>(cmd, "@SpeLimit", obj.SpecLimit);
            ctx.AddInParameter<string>(cmd, "@Result", obj.Result);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetAddTests>(reader);
                foreach (var rr in rrRes)
                    data = rr;

                reader.NextResult();

                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<AdditionalTest>(reader);
                data.Lst = new AdditionalTestList();
                foreach (var rr in rrLst)
                    data.Lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return data;
        }

        public GetAddTests DeleteAdditionalTest(int addTestID, TransResults tr)
        {
            GetAddTests data = new GetAddTests();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspDeleteAdditionalTest");
            ctx.AddInParameter<int>(cmd, "@AddTestID", addTestID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetAddTests>(reader);
                foreach (var rr in rrRes)
                    data = rr;

                reader.NextResult();

                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<AdditionalTest>(reader);
                data.Lst = new AdditionalTestList();
                foreach (var rr in rrLst)
                    data.Lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return data;
        }

        public string GetDeviationDescription(string entityCode, string dcActionCode)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspDCGetActionDescriptions");
            ctx.AddInParameter<string>(cmd, "@EnitiyCode", entityCode);
            ctx.AddInParameter<string>(cmd, "@DcAction", dcActionCode);
            ctx.AddOutParameter(cmd, "@Description", System.Data.DbType.String, 250);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@Description");
            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return retVal;
        }

        public MangeSampleAnalysisResult UpdateFinalRemarks(UpdFinalRemarks obj, TransResults tr)
        {
            MangeSampleAnalysisResult data = new MangeSampleAnalysisResult();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspUpdateFinalRemarks");
            ctx.AddInParameter<int>(cmd, "@SAMANAID", obj.SamAnaID);
            ctx.AddInParameter<string>(cmd, "@REMARKS", obj.Remarks);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);

            ctx.AddInOutParameter<string>(cmd, "@INITIATION_TIME", obj.InitTime, 25, System.Data.DbType.String);
            ctx.AddOutParameter(cmd, "@RETVAL", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            data.InitTime = ctx.GetOutputParameterValue(cmd, "@INITIATION_TIME");
            data.ReturnFlag = ctx.GetOutputParameterValue(cmd, "@RETVAL");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return data;
        }

        public ExecuteFormula GetFormulaDependentDetails(GetFormualDetails obj, TransResults tr)
        {
            ExecuteFormula data = new ExecuteFormula();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspGetFormulaDependentInputs");
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@ARDSSourceRefKey", obj.SamAnaTestID);
            ctx.AddInParameter<string>(cmd, "@ARDSSourceCode", obj.SourceCode);
            ctx.AddInParameter<int>(cmd, "@DetailID", obj.DetailID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ExecuteFormula>(reader);
                foreach (var rr in rrRes)
                    data = rr;

                reader.NextResult();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<FormulaDepenDetails>(reader);
                data.Lst = new FormulaDependentList();
                foreach (var rr in rrLst)
                    data.Lst.Add(rr);

                reader.NextResult();
                var impLst = ((IObjectContextAdapter)context).ObjectContext.Translate<FormulaDepenDetails>(reader);
                data.ImpurityLst = new FormulaDependentList();
                foreach (var rr in impLst)
                    data.ImpurityLst.Add(rr);

                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return data;
        }

        public GetConfData ConfirmEArds(ConfirmRawData obj, TransResults tr)
        {
            GetConfData act = new GetConfData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspConfirmData");
            ctx.AddInParameter<int>(cmd, "@ARDSSourceRefKey", obj.SamAnaTestID);
            ctx.AddInParameter<string>(cmd, "@ARDSSourceCode", obj.SourceCode);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetConfData>(reader);
                foreach (var rr in rrRes)
                    act = rr;

                //reader.NextResult();

                //var rrUsr = ((IObjectContextAdapter)context).ObjectContext.Translate<GetConfData>(reader);
                //foreach (var rr in rrUsr)
                //{
                //    act.PlantCode = rr.PlantCode;
                //    act.DeptCode = rr.DeptCode;
                //    act.RoleName = rr.RoleName;
                //    act.LoginID = rr.LoginID;
                //    act.EntityCode = rr.EntityCode;
                //}

                //reader.NextResult();

                //var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<SingleBO>(reader);
                //act.List = new SIngleBOList();
                //foreach (var rr in rrLst)
                //    act.List.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return act;
        }

        public GetIncludeExcludeTestBO ManageIncludeExcludeTest(IncludeExcludeTestBOItems obj, TransResults trn)
        {
            var lst = new GetIncludeExcludeTestBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspManageIncludeExcludeTest");

            ctx.AddInParameter<string>(cmd, "@XMLSamAnaTestID", obj.XMLString);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@UserID", trn.UserID);
            if (!string.IsNullOrEmpty(obj.SourceCode))
                ctx.AddInParameter<string>(cmd, "@SourceCode", obj.SourceCode);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<string>(reader);
                foreach (var rr in rrRes)
                    lst.ReturnFlag = rr;

                reader.NextResult();

                var rrResList = ((IObjectContextAdapter)context).ObjectContext.Translate<IncludeExcludeTestBO>(reader);
                lst.List = new IncludeExcludeTestBOList();
                foreach (var rr in rrResList)
                    lst.List.Add(rr);
            }
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return lst;
        }

        public SDMSInputValuesBOList GetSDMSDataBySamAnaTestID(int samAnaTestID)
        {
            var lst = new SDMSInputValuesBOList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetSDMSData");

            ctx.AddInParameter<int>(cmd, "@SamAnaTestID", samAnaTestID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SDMSInputValuesBO>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetSDMSInputDetailsBO ManageSDMSInputDetails(ManageSDMSInputDetailsBO obj, TransResults trn)
        {
            var lst = new GetSDMSInputDetailsBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspSaveSDMSMappingDetails");

            ctx.AddInParameter<int>(cmd, "@ARDSSourceRefKey", obj.SamAnaTestID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@ARDSSourceCode", obj.Source);
            ctx.AddInParameter<string>(cmd, "@DetailsXml", obj.XMLValues);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResTrn = ((IObjectContextAdapter)context).ObjectContext.Translate<TransResultApprovals>(reader);

                lst.Trn = new TransResultApprovals();

                foreach (var rr in rrResTrn)
                    lst.Trn = rr;

                reader.NextResult();

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ManageSDMSInputValuesBO>(reader);
                lst.List = new ManageSDMSInputValuesBOList();

                foreach (var rr in rrRes)
                    lst.List.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }


        public TestDetails GetMappingInfo(GetMappingInfo obj)
        {
            TestDetails data = new TestDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetMappingInfo");

            ctx.AddInParameter<int>(cmd, "@CurrSamAnaTestID", obj.CurrentSamAnaTestID);
            if (obj.SamAnaTestID > default(int))
                ctx.AddInParameter<int>(cmd, "@SamAnaTestID", obj.SamAnaTestID);
            if (obj.SdmsID > default(int))
                ctx.AddInParameter<int>(cmd, "@SdmsID", obj.SdmsID);

            ctx.AddInParameter<bool>(cmd, "@IsGetARDSExecDetails", obj.IsGetARDSExecDetails);

            using (var reader = cmd.ExecuteReader())
            {
                if (obj.IsGetARDSExecDetails)
                {
                    var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TestDetails>(reader);
                    foreach (var rr in rrRes)
                        data = rr;

                    reader.NextResult();
                }

                var rrData = ((IObjectContextAdapter)context).ObjectContext.Translate<string>(reader);
                foreach (var rr in rrData)
                    data.DataProcessed = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return data;
        }

        public GetAnalysisTypesList GetAnalysisTypes(int matCatID)
        {
            var list = new GetAnalysisTypesList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetAnalysisTypes");

            ctx.AddInParameter<int>(cmd, "@MatCatID", matCatID);

            using (var reader = cmd.ExecuteReader())
            {
                var lst = ((IObjectContextAdapter)context).ObjectContext.Translate<GetAnalysisTypes>(reader);
                foreach (var rr in lst)
                    list.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return list;
        }

        public GetSampleSourcesList GetSampleSources()
        {
            var list = new GetSampleSourcesList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetSampleSources");

            using (var reader = cmd.ExecuteReader())
            {
                var lst = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSampleSources>(reader);
                foreach (var rr in lst)
                    list.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return list;
        }

        public ManageUpdateFormulaResultFlagBO ManageIsFinalFormula(ManageIsFinalFormulaBO obj, TransResults trn)
        {
            var lst = new ManageUpdateFormulaResultFlagBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspUpdateFormulaResultFlag");

            if (obj.SamAnaTestID > default(int))
                ctx.AddInParameter<int>(cmd, "@ARDSSourceRefKey", obj.SamAnaTestID);


            ctx.AddInParameter<int>(cmd, "@DetailID", obj.DetailID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            ctx.AddInParameter<string>(cmd, "@ARDSSourceCode", obj.ARDSSourceCode);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@Formula", obj.Formula);
            if (obj.SpecTestID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecTestID", obj.SpecTestID);
            if (obj.ImpurityValueID > default(int))
                ctx.AddInParameter<int>(cmd, "@ImpurityValueID", obj.ImpurityValueID);


            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ManageUpdateFormulaResultFlagBO>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public ContainerWiseMaterialsResults ContainerWiseMaterials(ContainerWiseMaterials obj, TransResults trn)
        {
            var conObj = new ContainerWiseMaterialsResults();
            conObj.List = new SearchResults<GetContainerWiseMaterials>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspContainerWiseMaterials");
            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            if (obj.ContainerWiseMatID > default(int))
                ctx.AddInParameter<int>(cmd, "@ContainerWiseMatID", obj.ContainerWiseMatID);
            if (obj.MaterialID > default(int))
                ctx.AddInParameter<int>(cmd, "@MaterialID", obj.MaterialID);
            if (obj.AnalysisTypeID > default(int))
                ctx.AddInParameter<int>(cmd, "@AnalysisTypeID", obj.AnalysisTypeID);
            if (!string.IsNullOrEmpty(obj.SampleSourceCode))
                ctx.AddInParameter<string>(cmd, "@SampleSourceCode", obj.SampleSourceCode);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            ctx.AddOutParameter(cmd, "@ResultFlag", DbType.String, 15);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    var rrCount = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                    foreach (var rr in rrCount)
                        conObj.List.TotalNumberOfRows = rr;

                    reader.NextResult();

                    List<GetContainerWiseMaterials> item = new List<GetContainerWiseMaterials>();
                    var lst = ((IObjectContextAdapter)context).ObjectContext.Translate<GetContainerWiseMaterials>(reader);
                    foreach (var rr in lst)
                        item.Add(rr);

                    conObj.List.SearchList = item;
                }
                cmd.Connection.Close();
            }

            conObj.ResultFlag = ctx.GetOutputParameterValue(cmd, "@ResultFlag");
            ctx.CloseConnection(context);

            return conObj;
        }

        public GetTestByCategory GetTestByCategory(TestCatBO obj)
        {
            GetTestByCategory data = new GetTestByCategory();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetTestByCatID");
            ctx.AddInParameter<int>(cmd, "@CatID", obj.CatID);
            ctx.AddInParameter<int>(cmd, "@DetailID", obj.DetailID);
            ctx.AddInParameter<int>(cmd, "@ARDSSourceRefKey", obj.ARDSSourceRefKey);
            ctx.AddInParameter<string>(cmd, "@ARDSSourceCode", obj.ARDSSourceCode);
            if (obj.SubCatID > default(int))
                ctx.AddInParameter<int>(cmd, "@SubCatID", obj.SubCatID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrObj = ((IObjectContextAdapter)context).ObjectContext.Translate<GetTestByCategory>(reader);
                foreach (var rr in rrObj)
                    data = rr;
                reader.NextResult();
                data.TestList = new GetAnalysisTestBOList();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetAnalysisTestBO>(reader);
                foreach (var rr in rrRes)
                    data.TestList.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return data;
        }


        public GetContainerAnalysisDetails GetContainerWiseAnalysis(int sioID)
        {
            GetContainerAnalysisDetails obj = new GetContainerAnalysisDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspContinerWiseAnalysisDetails");
            ctx.AddInParameter<int>(cmd, "@SioID", sioID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetContainerAnalysisDetails>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();

                var rrPack = ((IObjectContextAdapter)context).ObjectContext.Translate<PackDetails>(reader);
                obj.PackList = new List<PackDetails>();
                foreach (var rr in rrPack)
                    obj.PackList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public RecordActionDetails SaveContainerArdsDetails(SaveContainerArdsDetails obj, TransResults tr)
        {
            RecordActionDetails act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspInsertContainerWiseAnalysis");
            ctx.AddInParameter<int>(cmd, "@SpecID", obj.SpecID);
            ctx.AddInParameter<string>(cmd, "@AnalysisMode", obj.ArdsMode);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            if (obj.TestID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecCatID", obj.TestID);
            ctx.AddInParameter<string>(cmd, "@PackXml", obj.PackXml);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@SioID", obj.SioID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return act;
        }

        public SamplerDetails GetSamplerDetails(int sioID)
        {
            SamplerDetails obj = new SamplerDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.GetSamplerDetails");
            ctx.AddInParameter<int>(cmd, "@SioID", sioID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SamplerDetails>(reader);
                foreach (var rr in rrRes)
                    obj = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public TestRetVal SendTestForReview(SendForReview obj, TransResults tr)
        {
            TestRetVal act = new TestRetVal();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSendTestForReview");
            if (obj.ArdsExecID > 0)
                ctx.AddInParameter<int>(cmd, "@ArdsExecID", obj.ArdsExecID);
            if (obj.Lst != null && obj.Lst.Count > 0)
                ctx.AddInParameter<string>(cmd, "@PacklistXml", obj.Packlist);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<string>(cmd, "@TestInitTime", obj.TestInitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TestRetVal>(reader);
                foreach (var rr in rrRes)
                    act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return act;
        }

        public ArdsReviewBO AddArdsReviewComment(ManageArdsReview obj, TransResults tr)
        {
            ArdsReviewBO retObj = new ArdsReviewBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspAddARDSReviewInfo");

            ctx.AddInParameter<int>(cmd, "@ArdsExecID", obj.ArdsExecID);
            ctx.AddInParameter<string>(cmd, "@ARDSSourceCode", obj.ArdsSourceCode);
            ctx.AddInParameter<int>(cmd, "@TabID", obj.TabID);
            ctx.AddInParameter<string>(cmd, "@Comment", obj.Commnet);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ArdsReviewBO>(reader);
                foreach (var rr in rrRes)
                    retObj = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return retObj;
        }

        public RecordActionDetails SkipPacksFromAnalysis(SkipPacksBO obj, TransResults tr)
        {
            RecordActionDetails act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSkipPacksFromContainerAnalysis");

            ctx.AddInParameter<int>(cmd, "@SioID", obj.sioID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@PacksXml", obj.PackXml);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return act;
        }

        public ManageUpdateFormulaResultFlagBO SkipInpurFieldFromExecution(ManageIsFinalFormulaBO obj, TransResults tr)
        {
            var lst = new ManageUpdateFormulaResultFlagBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspSkipInputFieldFromExecution");

            ctx.AddInParameter<int>(cmd, "@ARDSSourceRefKey", obj.SamAnaTestID);
            ctx.AddInParameter<int>(cmd, "@DetailID", obj.DetailID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<string>(cmd, "@ARDSSourceCode", obj.ARDSSourceCode);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            if (!string.IsNullOrEmpty(obj.Comments))
                ctx.AddInParameter<string>(cmd, "@Comments", obj.Comments);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ManageUpdateFormulaResultFlagBO>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                reader.NextResult();
                lst.Lst = new SIngleBOList();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<SingleBO>(reader);
                foreach (var rr in rrLst)
                    lst.Lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public List<PacksSendToReview> GetPackTestsForSendToReview(int sioID, int specCatID)
        {
            List<PacksSendToReview> lst = new List<PacksSendToReview>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetPackTestsForSendToReview");

            ctx.AddInParameter<int>(cmd, "@SioID", sioID);
            ctx.AddInParameter<int>(cmd, "@SpecCatID", specCatID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<PacksSendToReview>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public SectionDetailList GetSTPCommonDataforMapping(int entActID)
        {
            SectionDetailList lst = new SectionDetailList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspGetSTPCommonDataforMapping");

            ctx.AddInParameter<int>(cmd, "@ArdsExecID", entActID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SectionData>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string ManageSTPCommonDataMapping(ManageSTPCommonData obj, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspManageSTPCommonData");

            ctx.AddInParameter<int>(cmd, "@ArdsExecID", obj.ArdsExecID);
            ctx.AddInParameter<int>(cmd, "@SourceArdsExecID", obj.SourceArdsExecID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);

            ctx.AddOutParameter(cmd, "@RetCode", DbType.String, 25);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return retMsg;
        }

        public string ManageImpurityBasicInfo(ManageImpurityBasicInfoBO obj, TransResults trn)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspManageImpurity");

            if (obj.ImpurityID > default(int))
                ctx.AddInParameter<int>(cmd, "@ImpurityID", obj.ImpurityID);

            ctx.AddInParameter<int>(cmd, "@ArdsExecID", obj.ArdsExecID);
            ctx.AddInParameter<string>(cmd, "@ImpurityName", obj.ImpurityName);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddOutParameter(cmd, "@RetMsg", DbType.String, 25);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public GetUnknownImpuritiesBO GetUnknownImpurities(int ardsExecID)
        {
            var lst = new GetUnknownImpuritiesBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspGetUnknownImpurities");

            ctx.AddInParameter<int>(cmd, "@ArdsExecID", ardsExecID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResImp = ((IObjectContextAdapter)context).ObjectContext.Translate<ImpurityBO>(reader);

                lst.ImpList = new ImpurityBOList();

                foreach (var rr in rrResImp)
                    lst.ImpList.Add(rr);

                reader.NextResult();

                var rrResInj = ((IObjectContextAdapter)context).ObjectContext.Translate<InjectionsBO>(reader);

                lst.InjList = new InjectionsBOList();

                foreach (var rr in rrResInj)
                    lst.InjList.Add(rr);

                reader.NextResult();

                var rrResUnData = ((IObjectContextAdapter)context).ObjectContext.Translate<UnknownImpuritiesDataBO>(reader);

                lst.UnKIList = new UnknownImpuritiesDataBOList();

                foreach (var rr in rrResUnData)
                    lst.UnKIList.Add(rr);

                reader.NextResult();
                var rrInp = ((IObjectContextAdapter)context).ObjectContext.Translate<InputTypeBO>(reader);
                lst.InputTypeList = new InputTypeList();
                foreach (var rr in rrInp)
                    lst.InputTypeList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string ManageImpuritySDMSDetails(string XMLString, TransResults trn)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspManageUnknownImpurities");

            if (!string.IsNullOrEmpty(XMLString))
                ctx.AddInParameter<string>(cmd, "@ImpuritiesXml", XMLString);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            ctx.AddOutParameter(cmd, "@RetMsg", DbType.String, 25);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public GetMulitpleValueExecBO ManageMutipleFormulaValue(ManageMultFormulaValueBO obj, TransResults trn)
        {
            GetMulitpleValueExecBO retObj = new GetMulitpleValueExecBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspSaveMulitiInputValues");

            ctx.AddInParameter<int>(cmd, "@ARDSSourceRefKey", obj.SamAnaTestID);
            ctx.AddInParameter<string>(cmd, "@ARDSSourceCode", obj.ArdsSourceCode);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@ImpurityXML", obj.FormulaXML);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetMulitpleValueExecBO>(reader);
                foreach (var rr in rrRes)
                    retObj = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return retObj;

        }

        public DynamicFormulaExecBO GetDynamicFormulaInfo(GetFormualDetails obj, TransResults tr)
        {
            DynamicFormulaExecBO retObj = new DynamicFormulaExecBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspGetDynamicFormulaDependentInputs");

            ctx.AddInParameter<int>(cmd, "@ARDSSourceRefKey", obj.SamAnaTestID);
            ctx.AddInParameter<string>(cmd, "@ARDSSourceCode", obj.SourceCode);
            ctx.AddInParameter<int>(cmd, "@DetailID", obj.DetailID);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<DynamicFormulaExecBO>(reader);
                foreach (var rr in rrRes)
                    retObj = rr;

                reader.NextResult();

                retObj.FormulaLst = new DynamicFormulaLst();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<DynamicFormulaBO>(reader);
                foreach (var rr in rrLst)
                    retObj.FormulaLst.Add(rr);

                reader.NextResult();

                retObj.ImpList = new ImpurityDetailsList();
                var rrImp = ((IObjectContextAdapter)context).ObjectContext.Translate<ImpurityDetailsBO>(reader);
                foreach (var rr in rrImp)
                    retObj.ImpList.Add(rr);


                reader.NextResult();

                retObj.FormulaDepenLst = new FormulaDependentList();
                var rrFml = ((IObjectContextAdapter)context).ObjectContext.Translate<FormulaDepenDetails>(reader);
                foreach (var rr in rrFml)
                    retObj.FormulaDepenLst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return retObj;
        }

        public FormulaDependentList GetDyncFormulaDependentData(GetFormualDetails obj)
        {
            FormulaDependentList lst = new FormulaDependentList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspDyncFormulaDependentValues");

            ctx.AddInParameter<int>(cmd, "@ARDSSourceRefKey", obj.SamAnaTestID);
            ctx.AddInParameter<string>(cmd, "@ARDSSourceCode", obj.SourceCode);
            ctx.AddInParameter<int>(cmd, "@DetailID", obj.DetailID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<FormulaDepenDetails>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public ExecuteFormula ExecuteDynamicFormulaData(PostExecDynamicFormula obj, TransResults tr)
        {
            ExecuteFormula retObj = new ExecuteFormula();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspDyncFormulaExectionDetails");

            ctx.AddInParameter<int>(cmd, "@ImpValueID", obj.ImpurityValueID);
            ctx.AddInParameter<string>(cmd, "@DependentXml", obj.ValueXml);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ExecuteFormula>(reader);
                foreach (var rr in rrRes)
                    retObj = rr;

                reader.NextResult();

                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<FormulaDepenDetails>(reader);
                retObj.Lst = new FormulaDependentList();
                foreach (var rr in rrLst)
                    retObj.Lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return retObj;
        }

        public string DeleteImpurity(int impurityID, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspDeleteImpurity");

            ctx.AddInParameter<int>(cmd, "@ImpurityID", impurityID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddOutParameter(cmd, "@RetMsg", DbType.String, 25);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public TestRetVal SwitchARDSMode(SwitchArdsMode obj, TransResults tr)
        {
            TestRetVal retObj = new TestRetVal();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSwitchARDSMode");

            ctx.AddInParameter<int>(cmd, "@ArdsExecID", obj.ArdsExecID);
            ctx.AddInParameter<string>(cmd, "@ArdsMode", obj.ArdsMode);
            ctx.AddInParameter<string>(cmd, "@TestInitTime", obj.TestInitTime);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (!string.IsNullOrEmpty(obj.SourceCode))
                ctx.AddInParameter<string>(cmd, "@SourceCode", obj.SourceCode);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TestRetVal>(reader);
                foreach (var rr in rrRes)
                    retObj = rr;
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return retObj;
        }

        public ManageUpdateFormulaResultFlagBO SkipUnknownImpurities(ManageIsFinalFormulaBO obj)
        {
            ManageUpdateFormulaResultFlagBO retObj = new ManageUpdateFormulaResultFlagBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspSkipUnknownImpurities");

            ctx.AddInParameter<int>(cmd, "@ImpurityValueID", obj.ImpurityValueID);
            if (!string.IsNullOrEmpty(obj.Type))
                ctx.AddInParameter<string>(cmd, "@ValueMode", obj.Type);
            if (!string.IsNullOrEmpty(obj.Comments))
                ctx.AddInParameter<string>(cmd, "@Comments", obj.Comments);
            ctx.AddOutParameter(cmd, "@ReturnFlag", DbType.String, 25);
            using (var reader = cmd.ExecuteReader())
            {
                retObj.Lst = new SIngleBOList();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SingleBO>(reader);
                foreach (var rr in rrRes)
                    retObj.Lst.Add(rr);
            }
            retObj.ReturnFlag = ctx.GetOutputParameterValue(cmd, "@ReturnFlag");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retObj;
        }

        public ColumnsList ManageColumnInfo(ManageColumnsBO obj, TransResults tr)
        {
            ColumnsList lst = new ColumnsList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "uspEQPOTHManageColumnInfo");

            ctx.AddInParameter<int>(cmd, "@EpqOccID", obj.EpqOccID);
            if (obj.ColumnInjectionID > default(int))
                ctx.AddInParameter<int>(cmd, "@ColumnInjectionID", obj.ColumnInjectionID);
            ctx.AddInParameter<int>(cmd, "@ColumnID", obj.ColumnID);
            ctx.AddInParameter<short?>(cmd, "@NoOfInjections", obj.NoOfInjections);
            ctx.AddInParameter<int?>(cmd, "@CumulativeInj", obj.CumulativeInj);
            ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);
            ctx.AddInParameter<string>(cmd, "@DataSeqFile", obj.DataSeqFile);
            ctx.AddInParameter<bool>(cmd, "@isFromNew", true);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<int>(cmd, "@EnityActID", obj.EnityActID);
            ctx.AddInParameter<string>(cmd, "@RefNo", obj.RefNo);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ManageColumnsBO>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public ColumnsList DeleteColumnInfo(ManageColumnsBO obj, TransResults tr)
        {
            ColumnsList lst = new ColumnsList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "uspEQPOTHDeleteColumnData");

            ctx.AddInParameter<int>(cmd, "@ColumnInjectionID", obj.ColumnInjectionID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<int>(cmd, "@EnityActID", obj.EnityActID);
            ctx.AddInParameter<string>(cmd, "@RefNo", obj.RefNo);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ManageColumnsBO>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public string ConfirmImpMapping(ConfirmImpMapping obj, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspConfirmImpurityMapping");

            ctx.AddInParameter<int>(cmd, "@ArdsExecID", obj.ArdsExecID);
            ctx.AddInParameter<string>(cmd, "@MappingType", obj.MappingType);
            ctx.AddInParameter<bool>(cmd, "@IsConfirm", obj.IsConfirm);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@RetMsg", DbType.String, 25);

            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return retMsg;
        }

        public GetTableResultSetExec GetTableResultSetExecution(int ardsExecID, int resultSetID)
        {
            GetTableResultSetExec obj = new GetTableResultSetExec();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspGetTableResultSetExecution");

            ctx.AddInParameter<int>(cmd, "@ArdsExecID", ardsExecID);
            ctx.AddInParameter<int>(cmd, "@ResultSetID", resultSetID);
            ctx.AddOutParameter(cmd, "@RetFlag", DbType.String, 25);

            using (var reader = cmd.ExecuteReader())
            {
                var rrTb1 = ((IObjectContextAdapter)context).ObjectContext.Translate<TableResultExecBO>(reader);
                obj.Table1 = new TableResultExecList();
                foreach (var rr in rrTb1)
                    obj.Table1.Add(rr);

                reader.NextResult();
                obj.Table2 = new TableResultExecList();
                var rrTb2 = ((IObjectContextAdapter)context).ObjectContext.Translate<TableResultExecBO>(reader);
                foreach (var rr in rrTb2)
                    obj.Table2.Add(rr);
            }
            obj.ResultFlag = ctx.GetOutputParameterValue(cmd, "@RetFlag");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return obj;
        }

        public GetAnaOccuInstrumentsBO GetRefEqpOthInfo(int refEqpOccID)
        {
            GetAnaOccuInstrumentsBO obj = new GetAnaOccuInstrumentsBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetReferenceEqpOccpancyInfo");
            ctx.AddInParameter<int>(cmd, "@EqpOthOccID", refEqpOccID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetAnaOccuInstrumentsBO>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();

                obj.lst = new ColumnsList();
                var rrCols = ((IObjectContextAdapter)context).ObjectContext.Translate<ManageColumnsBO>(reader);
                foreach (var rr in rrCols)
                    obj.lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public GetSDMSDataDetailsBO GetSDMSData(ManageSDMSDataBO obj, TransResults trn)
        {
            var lst = new GetSDMSDataDetailsBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetManageSDMSData");
            ctx.AddInParameter<int>(cmd, "@SamAnaTestID", obj.SamAnaTestID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);

            if (!string.IsNullOrEmpty(obj.XMLList))
                ctx.AddInParameter<string>(cmd, "@SDMSXML", obj.XMLList);

            using (var reader = cmd.ExecuteReader())
            {
                if (obj.Type != "GET")
                {
                    var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSDMSDataDetailsBO>(reader);
                    foreach (var rr in rrResult)
                        lst = rr;

                    reader.NextResult();
                }

                var rrCols = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSDMSDataBO>(reader);
                lst.List = new GetSDMSDataBOList();
                foreach (var rr in rrCols)
                    lst.List.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetTestInstrumentDetails InvalidInstOccupancy(DeleteInstrumentBO obj, TransResults tr)
        {
            GetTestInstrumentDetails retObj = new GetTestInstrumentDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspInvalidateAddedInstOccupancy");

            ctx.AddInParameter<int>(cmd, "@ArdsInstID", obj.SamInstrID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<string>(cmd, "@TestInitTime", obj.InitTime);
            if (!string.IsNullOrEmpty(obj.Remarks))
                ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using(var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetTestInstrumentDetails>(reader);
                foreach (var rr in rrRes)
                    retObj = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return retObj;
        }

        public CategoryMasterList GetAnalysisChecklistCategories()
        {
            CategoryMasterList lst = new CategoryMasterList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetChecklistCategories");

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<CategoryMaster>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public TestRetVal ChangeAnalysisTestType(ChangeTestType obj, TransResults tr)
        {
            TestRetVal retObj = new TestRetVal();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSAMANAChangeAnalysisTestType");
            ctx.AddInParameter<int>(cmd, "@ArdsExecID", obj.ArdsExecID);
            ctx.AddInParameter<string>(cmd, "@TestInitTime", obj.TestInitTime);
            ctx.AddInParameter<string>(cmd, "@TestType", obj.TestType);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using(var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TestRetVal>(reader);
                foreach (var rr in rrRes)
                    retObj = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return retObj;
        }
    }
}
